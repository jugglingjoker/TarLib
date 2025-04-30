using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TarLib.Entities.Drawable;
using TarLib.Extensions;

namespace TarLib.Graphics {
    /*
    public struct Texture2DDrawInstructions : IDrawableTexture {
        public Texture2D DrawTexture { get; }
        public Rectangle? DrawTextureFrame { get; }
        public Vector2 DrawPosition { get; }
        public Color DrawColor { get; }
        public float DrawRotation { get; }
        public Vector2 DrawOrigin { get; }
        public Vector2 DrawScale { get; }
        public SpriteEffects DrawEffects { get; }
        public float DrawDepth { get; }

        public bool DrawVisible => true;

        public Texture2DDrawInstructions(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth) {
            DrawTexture = texture;
            DrawPosition = position;
            DrawTextureFrame = sourceRectangle;
            DrawColor = color;
            DrawRotation = rotation;
            DrawOrigin = origin;
            DrawScale = scale;
            DrawEffects = effects;
            DrawDepth = layerDepth;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position = default, float startDepth = 0, float endDepth = 1) {
            spriteBatch.Draw(
                entity: this,
                position: default,
                startDepth: startDepth,
                endDepth: endDepth);
        }
    }
    */
}
