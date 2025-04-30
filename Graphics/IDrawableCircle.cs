using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TarLib.Graphics {

    public interface IDrawableCircle {
        bool CircleDrawVisible { get; }
        Texture2D CircleDrawTexture { get; }
        Vector2 CircleDrawCenter { get; }
        float CircleDrawRadius { get; }

        Color CircleColor { get; }
        float CircleDrawDepth { get; }

        Color CircleBorderColor { get; }
        float CircleBorderWidth { get; }
        float CircleBorderDrawDepth { get; }
    }
}
