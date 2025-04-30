using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TarLib.Entities.Drawable;
using TarLib.Extensions;

namespace TarLib.Graphics {
    /*
    public struct StringDrawInstructions : IDrawableString {
        public SpriteFont DrawFont { get; }
        public string DrawText { get; }
        public Vector2 DrawPosition { get; }
        public Color DrawColor { get; }
        public float DrawRotation { get; }
        public Vector2 DrawOrigin { get; }
        public Vector2 DrawScale { get; }
        public SpriteEffects DrawEffects { get; }
        public float DrawDepth { get; }

        public bool DrawVisible => true;

        public StringDrawInstructions(SpriteFont font, string text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth) {
            DrawFont = font;
            DrawText = text;
            DrawPosition = position;
            DrawColor = color;
            DrawRotation = rotation;
            DrawOrigin = origin;
            DrawScale = scale;
            DrawEffects = effects;
            DrawDepth = layerDepth;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position, float startDepth = 0, float endDepth = 1) {
            spriteBatch.Draw(
                entity: this,
                position: position,
                startDepth: startDepth,
                endDepth: endDepth);
        }
    }
    */
}
