using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TarLib.Graphics {
    public interface IDrawableString {
        SpriteFont DrawFont { get; }
        string DrawText { get; }
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
