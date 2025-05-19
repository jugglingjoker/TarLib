using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace TarLib.States {

    public abstract class IntStyle {
        public abstract int Value { get; }

        public static implicit operator IntStyle(int value) {
            return new StaticIntStyle(value);
        }
    }

    public class StaticIntStyle : IntStyle {
        public StaticIntStyle(int value) {
            Value = value;
        }

        public override int Value { get; }
    }

    public struct MenuBlockStyleRule : IEquatable<MenuBlockStyleRule> {
        // TODO: Give margin and padding the same treatment as border size
        public MarginStyle? Margin { get; set; }
        public PaddingStyle? Padding { get; set; }
        public ContentDirectionStyle? ContentDirection { get; set; }
        public AlignStyle? ContentHorizontalAlign { get; set; }
        public AlignStyle? ContentVerticalAlign { get; set; }
        public SizeStyle? WidthStyle { get; set; }
        public SizeStyle? HeightStyle { get; set; }
        public IntStyle Width { get; set; }
        public IntStyle Height { get; set; }
        public ScrollStyle? HorizontalScroll { get; set; }
        public ScrollStyle? VerticalScroll { get; set; }
        public string FontId { get; set; }
        public Color? FontColor { get; set; }
        public Color? PlaceholderFontColor { get; set; }
        public Color? BackgroundColor { get; set; }
        public BorderColorStyle? BorderColor { get; set; }

        public string TextBefore { get; set; }
        public string TextAfter { get; set; }

        public BorderSizeStyle BorderSize {
            get => new BorderSizeStyle(BorderTop ?? 0, BorderLeft ?? 0, BorderBottom ?? 0, BorderRight ?? 0);
            set {
                BorderLeft = value.Left;
                BorderTop = value.Top;
                BorderBottom = value.Bottom;
                BorderRight = value.Right;
            }
        }

        public int? BorderLeft { get; set; }
        public int? BorderRight { get; set; }
        public int? BorderTop { get; set; }
        public int? BorderBottom { get; set; }

        public bool Equals(MenuBlockStyleRule rule) {
            return EqualityComparer<MarginStyle?>.Default.Equals(Margin, rule.Margin) &&
                   EqualityComparer<PaddingStyle?>.Default.Equals(Padding, rule.Padding) &&
                   ContentDirection == rule.ContentDirection &&
                   ContentHorizontalAlign == rule.ContentHorizontalAlign &&
                   ContentVerticalAlign == rule.ContentVerticalAlign &&
                   WidthStyle == rule.WidthStyle &&
                   HeightStyle == rule.HeightStyle &&
                   Width == rule.Width &&
                   Height == rule.Height &&
                   HorizontalScroll == rule.HorizontalScroll &&
                   VerticalScroll == rule.VerticalScroll &&
                   FontId == rule.FontId &&
                   EqualityComparer<Color?>.Default.Equals(FontColor, rule.FontColor) &&
                   EqualityComparer<Color?>.Default.Equals(PlaceholderFontColor, rule.PlaceholderFontColor) &&
                   EqualityComparer<Color?>.Default.Equals(BackgroundColor, rule.BackgroundColor) &&
                   EqualityComparer<BorderColorStyle?>.Default.Equals(BorderColor, rule.BorderColor) &&
                   EqualityComparer<BorderSizeStyle>.Default.Equals(BorderSize, rule.BorderSize) &&
                   BorderLeft == rule.BorderLeft &&
                   BorderRight == rule.BorderRight &&
                   BorderTop == rule.BorderTop &&
                   BorderBottom == rule.BorderBottom &&
                   TextBefore == rule.TextBefore &&
                   TextAfter == rule.TextAfter;
        }

        public override int GetHashCode() {
            HashCode hash = new HashCode();
            hash.Add(Margin);
            hash.Add(Padding);
            hash.Add(ContentDirection);
            hash.Add(ContentHorizontalAlign);
            hash.Add(ContentVerticalAlign);
            hash.Add(WidthStyle);
            hash.Add(HeightStyle);
            hash.Add(Width);
            hash.Add(Height);
            hash.Add(HorizontalScroll);
            hash.Add(VerticalScroll);
            hash.Add(FontId);
            hash.Add(FontColor);
            hash.Add(PlaceholderFontColor);
            hash.Add(BackgroundColor);
            hash.Add(BorderColor);
            hash.Add(BorderSize);
            hash.Add(BorderLeft);
            hash.Add(BorderRight);
            hash.Add(BorderTop);
            hash.Add(BorderBottom);
            hash.Add(TextBefore);
            hash.Add(TextAfter);
            return hash.ToHashCode();
        }

        public static MenuBlockStyleRule operator +(MenuBlockStyleRule left, MenuBlockStyleRule right) {
            return new MenuBlockStyleRule() {
                Margin = left.Margin ?? right.Margin,
                Padding = left.Padding ?? right.Padding,
                ContentDirection = left.ContentDirection ?? right.ContentDirection,
                ContentHorizontalAlign = left.ContentHorizontalAlign ?? right.ContentHorizontalAlign,
                ContentVerticalAlign = left.ContentVerticalAlign ?? right.ContentVerticalAlign,
                WidthStyle = left.WidthStyle ?? right.WidthStyle,
                HeightStyle = left.HeightStyle ?? right.HeightStyle,
                Width = left.Width ?? right.Width,
                Height = left.Height ?? right.Height,
                HorizontalScroll = left.HorizontalScroll ?? right.HorizontalScroll,
                VerticalScroll = left.VerticalScroll ?? right.VerticalScroll,
                FontId = left.FontId ?? right.FontId,
                FontColor = left.FontColor ?? right.FontColor,
                PlaceholderFontColor = left.PlaceholderFontColor ?? right.PlaceholderFontColor,
                BackgroundColor = left.BackgroundColor ?? right.BackgroundColor,
                BorderColor = left.BorderColor ?? right.BorderColor,

                BorderLeft = left.BorderLeft ?? right.BorderLeft,
                BorderRight = left.BorderRight ?? right.BorderRight,
                BorderTop = left.BorderTop ?? right.BorderTop,
                BorderBottom = left.BorderBottom ?? right.BorderBottom,

                TextBefore = left.TextBefore ?? right.TextBefore,
                TextAfter = left.TextAfter ?? right.TextAfter,
            };
        }
    }
}
