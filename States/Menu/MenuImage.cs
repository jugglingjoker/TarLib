using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TarLib.States {
    public class MenuImage : MenuBlock {

        public string TextureId { get; set; }
        public Texture2D Texture => Menu.State.BaseGame.Textures[TextureId];

        private Rectangle? bounds;
        public virtual Rectangle? Bounds {
            get => bounds;
            set => bounds = value;
        }

        private Color color = Color.White;
        public virtual Color Color {
            get => color;
            set => color = value;
        }

        private float rotation;
        public virtual float Rotation {
            get => rotation;
            set => rotation = value;
        }

        private Vector2 scale = Vector2.One;
        public virtual Vector2 Scale {
            get => scale;
            set => scale = value;
        }

        private Vector2 origin = Vector2.Zero;
        public virtual Vector2 Origin {
            get => origin;
            set => origin = value;
        }

        private int? contentActualWidth;
        public override int ContentActualWidth {
            get {
                if(contentActualWidth == null) {
                    // TODO: Move to function
                    contentActualWidth = (int)((Bounds?.Width ?? Texture.Bounds.Width) * Scale.X);
                    contentActualHeight = (int)((Bounds?.Height ?? Texture.Bounds.Height) * Scale.Y);
                    NeedsRefresh = false;
                }
                return contentActualWidth.Value;
            }
        }

        private int? contentActualHeight;
        public override int ContentActualHeight {
            get {
                if(contentActualHeight == null) {
                    contentActualWidth = (int)((Bounds?.Width ?? Texture.Bounds.Width) * Scale.X);
                    contentActualHeight = (int)((Bounds?.Height ?? Texture.Bounds.Height) * Scale.Y);
                    NeedsRefresh = false;
                }
                return contentActualHeight.Value;
            }
        }

        protected override MenuBlockStyleTypeList StyleTypes => MenuBlockStyleType.Image;

        public MenuImage(
            string textureId,
            IGameMenu menu = default) : base(menu) {
            TextureId = textureId;
        }

        protected override void DrawContent(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position, float startDepth, float endDepth) {
            spriteBatch.Draw(
                texture: Texture,
                position: position,
                sourceRectangle: Bounds,
                color: Color,
                rotation: Rotation,
                origin: Origin,
                scale: Scale,
                effects: SpriteEffects.None,
                layerDepth: startDepth);
        }
    }
}
