using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TarLib.Entities.Drawable {

    public interface IDrawableLineTexture {
        Texture2D LineDrawTexture { get; }
        Vector2 LineStart { get; }
        Vector2 LineEnd { get; }
        Color LineColor { get; }
        float LineThickness { get; }
        float LineDrawDepth { get; }
        bool LineDrawVisible { get; }
    }
}
