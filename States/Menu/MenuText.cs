using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TarLib.States {

    public interface IMenuTextSource {
        string Text { get; }
    }

    public class MenuText : MenuText<MenuText.TextSource> {
        public MenuText(string text, bool canWrapText = false, IGameMenu menu = null) : base(new TextSource(text), canWrapText, menu) {

        }

        public class TextSource : IMenuTextSource {
            public TextSource(string text) {
                Text = text;
            }
            public string Text { get; set; }
        }
    }

    public class MenuText<TMenuTextSource> : MenuBlock
        where TMenuTextSource : IMenuTextSource {

        protected static readonly object objectLock = new();

        private bool isTextChanged = false;
        private readonly List<string> textSegments = new();

        public TMenuTextSource Source { get; init; }

        private ObservableVariable<string> previousText = new();
        public string Text {
            get {
                previousText.Value = Source.Text;
                return previousText.Value;
            }
        }
        public string FullText => (ActiveStyleRule.TextBefore ?? "") + Text + (ActiveStyleRule.TextAfter ?? "");
        public bool CanWrapText { get; }

        private int contentActualWidth;
        public override int ContentActualWidth {
            get {
                if(textSegments.Count == 0) {
                    CalculateTextSegments();
                }
                return contentActualWidth;
            }
        }

        private int contentActualHeight;
        public override int ContentActualHeight {
            get {
                if (textSegments.Count == 0) {
                    CalculateTextSegments();
                }
                return contentActualHeight;
            }
        }

        public event EventHandler<(string oldText, string newText)> OnTextChange {
            add {
                lock (objectLock) {
                    previousText.OnChange += value;
                }
            }
            remove {
                lock (objectLock) {
                    previousText.OnChange -= value;
                }
            }
        }

        protected override MenuBlockStyleTypeList StyleTypes => MenuBlockStyleType.Text;

        public MenuText(
            TMenuTextSource source,
            bool canWrapText = false,
            IGameMenu menu = default) : base(menu) {

            Source = source;
            OnTextChange += MenuText_OnTextChange; 
            CanWrapText = canWrapText;            

            DefaultStyle = new MenuBlockStyleRule() {
                FontColor = Color.Black
            };
        }

        private void MenuText_OnTextChange(object sender, (string oldText, string newText) e) {
            isTextChanged = true;
        }

        private void CalculateTextSegments() {
            var text = FullText;
            textSegments.Clear();
            if (CanWrapText && ActiveStyleRule.WidthStyle == SizeStyle.Exact && ActiveStyleRule.Width != default) {
                string currentLine = default;
                foreach (var token in text.Split(" ")) {
                    if (currentLine == default) {
                        currentLine = token;
                    } else {
                        var testLine = currentLine + " " + token;
                        if (Font.MeasureString(testLine).X <= ActiveStyleRule.Width?.Value) {
                            currentLine = testLine;
                        } else {
                            textSegments.Add(currentLine);
                            currentLine = token;
                        }
                    }
                }
                if (currentLine != default) {
                    textSegments.Add(currentLine);
                }
            } else {
                if(text != null) {
                    textSegments.Add(text);
                }
            }
            contentActualWidth = textSegments.Max(textSegment => (int)Math.Ceiling(Font.MeasureString(textSegment).X));
            contentActualHeight = textSegments.Sum(textSegment => (int)Math.Ceiling(Font.MeasureString(textSegment).Y));
            NeedsRefresh = false;
        }

        protected override void DrawContent(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position, float startDepth, float endDepth) {
            var checkText = Text;
            if (isTextChanged) {
                CalculateTextSegments();
                isTextChanged = false;
            }

            var currentPosition = position;
            foreach(var textSegment in textSegments) {
                spriteBatch.DrawString(
                    spriteFont: Font,
                    text: textSegment,
                    position: currentPosition,
                    color: FontColor,
                    rotation: 0,
                    origin: Vector2.Zero,
                    scale: Vector2.One,
                    effects: SpriteEffects.None,
                    layerDepth: startDepth);
                currentPosition.Y += Font.MeasureString(textSegment).Y;
            }
        }
    }
}
