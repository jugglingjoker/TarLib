using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TarLib.Extensions;
using TarLib.States;

namespace TarLib.Entities.Drawable {

    public interface IDrawableCircleTexture {
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
