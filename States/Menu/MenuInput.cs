using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using TarLib.Input;
using Microsoft.Xna.Framework;
using TarLib.Entities.Drawable;
using TarLib.Extensions;
using TarLib.Graphics;

namespace TarLib.States {

    enum MenuInputType {

    }

    public abstract class MenuInput<TInput> : MenuText {
        private readonly ObservableVariable<TInput> value = new();
        private readonly ObservableVariable<int> cursorLocation = new();
        private string TemporaryString;

        public MenuInput(TInput initialValue, IGameMenu menu = null) : base(text: initialValue.ToString(), menu: menu) {
            var allKeys = KeysList.AllVisible.Concat(KeysList.AnyShift).Concat(KeysList.AnyControl).ToHashSet();
            allKeys.Add(Keys.Enter);
            allKeys.Add(Keys.Delete);
            allKeys.Add(Keys.Back);
            allKeys.Add(Keys.Left);
            allKeys.Add(Keys.Right);

            AllValidKeys = allKeys;

            OnKeyPressStart += MenuInput_OnKeyPressStart;
            OnClickStart += MenuInput_OnClickStart;

            IsDefault = false;
            InitialValue = initialValue;
            Value = initialValue;

            value.OnChange += StoredValue_OnChange;

            Cursor = new CursorTexture(this);

            DefaultStyle = new MenuBlockStyle() {
                Default = new MenuBlockStyleRule() {
                    Padding = (top: 5, left: 15, bottom: 5, right: 15),
                    BorderSize = (4, 2, 2, 2),
                    BorderColor = Color.Black,
                    PlaceholderFontColor = Color.LightGray,
                    BackgroundColor = Color.White,
                    Margin = 3
                },
                Error = new MenuBlockStyleRule() {
                    BorderColor = Color.Red,
                }
            } + DefaultStyle;
        }

        protected IReadOnlySet<Keys> AllValidKeys { get; }
        public TInput InitialValue { get; }
        public TInput DefaultValue { get; set; } = default;

        public TInput Value {
            get => value.Value ?? DefaultValue;
            set => this.value.Value = value;
        }

        public CursorTexture Cursor { get; }

        public int CursorLocation {
            get => cursorLocation.Value;
            set {
                cursorLocation.Value = MathHelper.Clamp(value, 0, Text.Length);
                ActualOffset = new Vector2(Font.MeasureString(Text.Substring(0, CursorLocation)).X, 0);
            }
        }

        public event EventHandler<(TInput oldValue, TInput newValue)> OnChange {
            add {
                lock (objectLock) {
                    this.value.OnChange += value;
                }
            }
            remove {
                lock (objectLock) {
                    this.value.OnChange -= value;
                }
            }
        }

        public event EventHandler<(int oldValue, int newValue)> OnCursorLocationChange {
            add {
                lock (objectLock) {
                    cursorLocation.OnChange += value;
                }
            }
            remove {
                lock (objectLock) {
                    cursorLocation.OnChange -= value;
                }
            }
        }

        public event EventHandler<(string input, int cursorLocation)> OnInvalidInput;

        public bool IsDefault { get; private set; }

        public override Color FontColor => (IsDefault ? ActiveStyleRule.PlaceholderFontColor : ActiveStyleRule.FontColor) ?? base.FontColor;

        protected override MenuBlockStyleTypeList StyleTypes => base.StyleTypes + MenuBlockStyleType.Input;

        public Vector2 ActualOffset { get; set; } = Vector2.Zero;

        public override bool CanUseLeftMouseButton => true;

        protected override void DrawContent(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position, float startDepth, float endDepth) {
            base.DrawContent(gameTime, spriteBatch, position, startDepth, endDepth);
            spriteBatch.Draw(Cursor, position, startDepth, endDepth);
        }

        private void MenuInput_OnClickStart(object sender, MouseClickEventArgs e) {
            if(HotZone != null) {
                var offsetX = e.MousePosition.X - (HotZone.Value.X + Padding.Left);

                if(offsetX <= 0) {
                    CursorLocation = 0;
                } else {
                    if(IsDefault) {
                        CursorLocation = 0;
                    } else {
                        var totalWidth = 0f;
                        for (int i = 0; i < Text.Length; i++) {
                            var letterWidth = Font.MeasureString(Text[i].ToString()).X;
                            if (totalWidth + letterWidth >= offsetX) {
                                var perc = (offsetX - totalWidth) / letterWidth;
                                CursorLocation = perc > 0.4f ? i + 1 : i;
                                return;
                            }
                            totalWidth += letterWidth + Font.Spacing;
                        }
                        CursorLocation = Text.Length;
                    }
                }
            }
        }

        private void StoredValue_OnChange(object sender, (TInput oldValue, TInput newValue) e) {
            Source.Text = (e.newValue ?? Value).ToString();
        }

        private void MenuInput_OnKeyPressStart(object sender, KeyPressEventArgs e) {
            var usableKeys = e.AvailableKeys.Intersect(AllValidKeys);
            var usableVisibleKeys = usableKeys.Intersect(KeysList.AllVisible);

            if (usableKeys.Any()) {
                if (usableKeys.Contains(Keys.Left)) {
                    CursorLocation--;
                } else if (usableKeys.Contains(Keys.Right)) {
                    CursorLocation++;
                }

                if (usableKeys.Contains(Keys.Delete)) {
                    DeleteCharAt(CursorLocation);
                }
                if (usableKeys.Contains(Keys.Back) && CursorLocation > 0) {
                    DeleteCharAt(CursorLocation - 1);
                    CursorLocation--;
                }
                foreach(var key in usableVisibleKeys) {
                    AddChar(key, Menu.State.BaseGame.Input.KeysDown.Intersect(KeysList.AnyShift).Any(), Menu.State.BaseGame.Input.KeyboardState.CapsLock);
                }
            }
            e.FlagKeysAsUsed(usableKeys);
        }

        protected virtual void AddChar(Keys character, bool anyShift, bool capsLock) {
            var input = character.ToActualString(anyShift, capsLock);
            var testInput = IsDefault ? input : (TemporaryString ?? Value.ToString()).Insert(CursorLocation, input);

            TInput translatedValue = TranslateInput(testInput);
            if (translatedValue != null) {
                TemporaryString = null;
                IsDefault = false;
                Value = translatedValue;
                CursorLocation += input.Length;
            } else if (IsValidTemporaryString(testInput)) {
                IsDefault = false;
                TemporaryString = testInput;
                CursorLocation += input.Length;
            } else {
                OnInvalidInput?.Invoke(this, (testInput, CursorLocation));
            }
        }

        protected virtual bool IsValidTemporaryString(string testInput) {
            return false;
        }

        protected abstract TInput TranslateInput(string testInput);

        protected virtual void DeleteCharAt(int index) {
            if(index < Text.Length && index >= 0) {
                var testInput = (TemporaryString ?? Value.ToString()).Remove(index, 1);

                if (testInput.Length == 0) {
                    TemporaryString = null;
                    IsDefault = true;
                    Value = DefaultValue;
                    CursorLocation = 0;
                } else if (IsValidTemporaryString(testInput)) {
                    TemporaryString = testInput;
                    IsDefault = false;
                } else {
                    TInput translatedValue = TranslateInput(testInput);
                    if (translatedValue != null) {
                        Value = translatedValue;
                        IsDefault = false;
                    } else {
                        OnInvalidInput?.Invoke(this, (testInput, CursorLocation));
                    }
                }
                
            }
        }

        public class CursorTexture : IDrawableTexture {
            private MenuInput<TInput> MenuInput { get; }

            public Texture2D DrawTexture => MenuInput.Menu.State.BaseGame.Textures.Default;
            public Rectangle? DrawTextureFrame => null;
            public Vector2 DrawOrigin => Vector2.Zero;
            public float DrawRotation => 0;
            public Vector2 DrawScale => new(2, MenuInput.ContentHeight); // TODO: define width
            public SpriteEffects DrawEffects => SpriteEffects.None;
            public Color DrawColor => Color.Black;
            public Vector2 DrawPosition => MenuInput.ActualOffset - new Vector2(1, 0); // TODO: define width / 2
            public float DrawDepth => 1f;
            public bool DrawVisible => MenuInput.Menu.SelectTarget == MenuInput;

            public CursorTexture(MenuInput<TInput> menuInput) {
                MenuInput = menuInput;
            }
        }
    }
}
