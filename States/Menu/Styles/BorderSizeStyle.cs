using Microsoft.Xna.Framework;

namespace TarLib.States {
    public struct BorderSizeStyle {
        public int Top { get; set; }
        public int Left { get; set; }
        public int Bottom { get; set; }
        public int Right { get; set; }
        public int TotalVertical => Top + Bottom;
        public int TotalHorizontal => Left + Right;
        public Vector2 Total => new Vector2(TotalHorizontal, TotalVertical);
        public Vector2 Position => new Vector2(Left, Top);

        public BorderSizeStyle(int top = default, int left = default, int bottom = default, int right = default) {
            Top = top;
            Left = left;
            Bottom = bottom;
            Right = right;
        }

        public static implicit operator BorderSizeStyle(int borderSize) {
            return new BorderSizeStyle(
                top: borderSize,
                left: borderSize,
                right: borderSize,
                bottom: borderSize);
        }

        public static implicit operator BorderSizeStyle((int top, int left, int bottom, int right) borderSizes) {
            return new BorderSizeStyle(
                top: borderSizes.top,
                left: borderSizes.left,
                right: borderSizes.right,
                bottom: borderSizes.bottom);
        }

        public static implicit operator BorderSizeStyle((int vertical, int horizontal) borderSizes) {
            return new BorderSizeStyle(
                top: borderSizes.vertical,
                bottom: borderSizes.vertical,
                left: borderSizes.horizontal,
                right: borderSizes.horizontal);
        }

        public static BorderSizeStyle operator / (BorderSizeStyle style, int divisor) {
            return new BorderSizeStyle(
                top: style.Top / divisor,
                bottom: style.Bottom / divisor,
                left: style.Left / divisor,
                right: style.Right / divisor);
        }

        public static BorderSizeStyle operator + (BorderSizeStyle style, int amount) {
            return new BorderSizeStyle(
                top: style.Top + amount,
                bottom: style.Bottom + amount,
                left: style.Left + amount,
                right: style.Right + amount);
        }
    }
}
