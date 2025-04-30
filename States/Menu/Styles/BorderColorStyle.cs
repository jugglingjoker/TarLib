using Microsoft.Xna.Framework;

namespace TarLib.States {
    public struct BorderColorStyle {
        public Color Top { get; set; }
        public Color Left { get; set; }
        public Color Bottom { get; set; }
        public Color Right { get; set; }

        public BorderColorStyle(Color top = default, Color left = default, Color bottom = default, Color right = default) {
            Top = top;
            Left = left;
            Bottom = bottom;
            Right = right;
        }

        public static implicit operator BorderColorStyle(Color color) {
            return new BorderColorStyle(
                top: color,
                left: color,
                right: color,
                bottom: color);
        }

        public static implicit operator BorderColorStyle((Color top, Color left, Color bottom, Color right) colors) {
            return new BorderColorStyle(
                top: colors.top,
                left: colors.left,
                right: colors.right,
                bottom: colors.bottom);
        }

        public static implicit operator BorderColorStyle((Color vertical, Color horizontal) colors) {
            return new BorderColorStyle(
                top: colors.vertical,
                bottom: colors.vertical,
                left: colors.horizontal,
                right: colors.horizontal);
        }
    }
}
