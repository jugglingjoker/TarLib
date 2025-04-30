using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TarLib.Entities.Drawable {

    public interface IDrawableEntity {

        float DrawWidth { get; }
        float DrawHeight { get; }
        Vector2 DrawPosition { get; }

        void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position = default, float startDepth = 0, float endDepth = 1);
    }
}
