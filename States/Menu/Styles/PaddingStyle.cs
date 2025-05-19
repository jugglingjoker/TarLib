using Microsoft.Xna.Framework;

namespace TarLib.States {
    public struct PaddingStyle {
        public int Top { get; }
        public int Left { get; }
        public int Bottom { get; }
        public int Right { get; }
        public int TotalVertical => Top + Bottom;
        public int TotalHorizontal => Left + Right;
        public Vector2 Total => new Vector2(TotalHorizontal, TotalVertical);
        public Vector2 Position => new Vector2(Left, Top);

        public PaddingStyle(int top = default, int left = default, int bottom = default, int right = default) {
            Top = top;
            Left = left;
            Bottom = bottom;
            Right = right;
        }

        public static implicit operator PaddingStyle(int dimension) {
            return new PaddingStyle(
                top: dimension,
                left: dimension,
                right: dimension,
                bottom: dimension);
        }

        public static implicit operator PaddingStyle((int top, int left, int bottom, int right) dimensions) {
            return new PaddingStyle(
                top: dimensions.top,
                left: dimensions.left,
                right: dimensions.right,
                bottom: dimensions.bottom);
        }

        public static implicit operator PaddingStyle((int vertical, int horizontal) dimensions) {
            return new PaddingStyle(
                top: dimensions.vertical,
                bottom: dimensions.vertical,
                left: dimensions.horizontal,
                right: dimensions.horizontal);
        }

        public static PaddingStyle operator + (PaddingStyle style, int amount) {
            return new PaddingStyle(
                top: style.Top + amount,
                bottom: style.Bottom + amount,
                left: style.Left + amount,
                right: style.Right + amount);
        }
    }
}
