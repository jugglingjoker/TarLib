using Microsoft.Xna.Framework;

namespace TarLib.States {
    public struct MarginStyle {
        public int Top { get; set; }
        public int Left { get; set; }
        public int Bottom { get; set; }
        public int Right { get; set; }
        public int TotalVertical => Top + Bottom;
        public int TotalHorizontal => Left + Right;
        public Vector2 Total => new Vector2(TotalHorizontal, TotalVertical);
        public Vector2 Position => new Vector2(Left, Top);

        public MarginStyle(int top = default, int left = default, int bottom = default, int right = default) {
            Top = top;
            Left = left;
            Bottom = bottom;
            Right = right;
        }

        public static implicit operator MarginStyle(int dimension) {
            return new MarginStyle(
                top: dimension,
                left: dimension,
                right: dimension,
                bottom: dimension);
        }

        public static implicit operator MarginStyle((int top, int left, int bottom, int right) dimensions) {
            return new MarginStyle(
                top: dimensions.top,
                left: dimensions.left,
                right: dimensions.right,
                bottom: dimensions.bottom);
        }

        public static implicit operator MarginStyle((int vertical, int horizontal) dimensions) {
            return new MarginStyle(
                top: dimensions.vertical,
                bottom: dimensions.vertical,
                left: dimensions.horizontal,
                right: dimensions.horizontal);
        }
    }
}
