using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TarLib.States;

namespace TarLib.Graphics {
    public interface IDrawableBox {
        bool BoxDrawVisible { get; }

        Texture2D BoxDrawTexture { get; }
        Vector2 BoxDrawPosition { get; }
        float BoxDrawHeight { get; }
        float BoxDrawWidth { get; }

        Color BoxColor { get; }
        float BoxDrawDepth { get; }

        Color BoxBorderColor { get; }
        BorderSizeStyle BoxBorderSize { get; }
        float BoxBorderDrawDepth { get; }
    }
}
