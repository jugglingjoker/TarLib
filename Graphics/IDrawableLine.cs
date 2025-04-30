using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TarLib.Graphics {
    public interface IDrawableLine {
        Texture2D LineDrawTexture { get; }
        Vector2 LineStart { get; }
        Vector2 LineEnd { get; }
        Color LineColor { get; }
        float LineThickness { get; }
        float LineDrawDepth { get; }
        bool LineDrawVisible { get; }
    }
}
