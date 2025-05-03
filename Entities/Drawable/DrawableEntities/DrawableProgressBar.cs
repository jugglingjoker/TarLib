using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TarLib.Extensions;
using TarLib.Graphics;
using TarLib.States;

namespace TarLib.Entities.Drawable {
    public abstract class DrawableProgressBar : IDrawableEntity {
        private BackgroundTexture background;
        private ProgressTexture progress;

        public DrawableProgressBar() {
            background = new BackgroundTexture(this);
            progress = new ProgressTexture(this);
        }

        public abstract float ProgressComplete { get; }

        public abstract float DrawWidth { get; }
        public abstract float DrawHeight { get; }
        public abstract Vector2 DrawPosition { get; }

        public abstract Texture2D BackgroundDrawTexture { get; }
        public abstract Texture2D BoxDrawTexture { get; }

        public abstract Color EmptyBarColor { get; }
        public abstract Color BoxBorderColor { get; }
        public abstract Color ProgressBarColor { get; }
        public abstract BorderSizeStyle BoxBorderSize { get; }

        public virtual bool IsVisible { get; }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 positionOffset = default, float startDepth = 0, float endDepth = 1) {
            if(IsVisible) {
                spriteBatch.Draw(background, positionOffset, startDepth, endDepth);
                spriteBatch.Draw(progress, positionOffset, startDepth, endDepth);
            }
        }

        public class BackgroundTexture : IDrawableBox {
            private DrawableProgressBar progressBar;

            public BackgroundTexture(DrawableProgressBar progressBar) {
                this.progressBar = progressBar;
            }

            public bool BoxDrawVisible => true;
            public Texture2D BoxDrawTexture => progressBar.BackgroundDrawTexture;
            public Vector2 BoxDrawPosition => progressBar.DrawPosition;
            public float BoxDrawHeight => progressBar.DrawHeight;
            public float BoxDrawWidth => progressBar.DrawWidth;
            public Color BoxColor => progressBar.EmptyBarColor;
            public float BoxDrawDepth => 0.5f;
            public Color BoxBorderColor => progressBar.BoxBorderColor;
            public BorderSizeStyle BoxBorderSize => progressBar.BoxBorderSize;
            public float BoxBorderDrawDepth => 0.33f;
        }

        public class ProgressTexture : IDrawableBox {
            private DrawableProgressBar progressBar;

            public ProgressTexture(DrawableProgressBar progressBar) {
                this.progressBar = progressBar;
            }

            public bool BoxDrawVisible => true;
            public Texture2D BoxDrawTexture => progressBar.BoxDrawTexture;
            public Vector2 BoxDrawPosition => progressBar.DrawPosition + progressBar.background.BoxBorderSize.Position;
            public float BoxDrawWidth => (progressBar.DrawWidth - progressBar.background.BoxBorderSize.TotalHorizontal) * progressBar.ProgressComplete ;
            public float BoxDrawHeight => progressBar.DrawHeight - progressBar.background.BoxBorderSize.TotalVertical;
            public Color BoxColor => progressBar.ProgressBarColor;
            public float BoxDrawDepth => 0.66f;
            public Color BoxBorderColor => Color.Transparent;
            public BorderSizeStyle BoxBorderSize => 0;
            public float BoxBorderDrawDepth => 0.75f;
        }
    }
}
