using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TarLib.Entities.Drawable {
    public interface IDrawableTexture {
        Texture2D DrawTexture { get; }
        Rectangle? DrawTextureFrame { get; }
        Vector2 DrawOrigin { get; }
        float DrawRotation { get; }
        Vector2 DrawScale { get; }
        SpriteEffects DrawEffects { get; }
        Color DrawColor { get; }
        Vector2 DrawPosition { get; }
        float DrawDepth { get; }
        bool DrawVisible { get; }
    }
}
