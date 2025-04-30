using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TarLib.Primitives {
    public struct RectanglePrimitive : IPrimitive, IPrimitiveWithCenter {
        private LineSegmentPrimitive topSegment;
        private LineSegmentPrimitive bottomSegment;
        private LineSegmentPrimitive leftSegment;
        private LineSegmentPrimitive rightSegment;

        public float X {
            get => topSegment.Point1.X;
            set {
                var width = Width;
                var height = Height;
                TopLeft = new(value, TopLeft.Y);
                BottomRight = new(value + width, TopLeft.Y + height);
            }
        }
        public float Y {
            get => topSegment.Point1.Y;
            set {
                var width = Width;
                var height = Height;
                TopLeft = new(TopLeft.X, value);
                BottomRight = new(TopLeft.X + width, value + height);
            }
        }
        
        public float Left {
            get => X;
            set {
                topSegment.Point1 = new(value, topSegment.Point1.Y);
                bottomSegment.Point1 = new(value, bottomSegment.Point1.Y);
                leftSegment.Point1 = topSegment.Point1;
                leftSegment.Point2 = bottomSegment.Point1;
            }
        }

        public float Right {
            get => X + Width;
            set {
                topSegment.Point2 = new(value, topSegment.Point2.Y);
                bottomSegment.Point2 = new(value, bottomSegment.Point2.Y);
                rightSegment.Point1 = topSegment.Point2;
                rightSegment.Point2 = bottomSegment.Point2;
            }
        }

        public float Top {
            get => Y;
            set {
                leftSegment.Point1 = new(leftSegment.Point1.X, value);
                rightSegment.Point1 = new(rightSegment.Point1.X, value);
                topSegment.Point1 = leftSegment.Point1;
                topSegment.Point2 = rightSegment.Point1;
            }
        }

        public float Bottom {
            get => Y + Height;
            set {
                leftSegment.Point2 = new(leftSegment.Point2.X, value);
                rightSegment.Point2 = new(rightSegment.Point2.X, value);
                bottomSegment.Point1 = leftSegment.Point2;
                bottomSegment.Point2 = rightSegment.Point2;
            }
        }

        public Vector2 TopLeft {
            get => new(Left, Top);
            set {
                Left = value.X;
                Top = value.Y;
            }
        }

        public Vector2 TopMiddle {
            get => topSegment.Center;
        }

        public Vector2 TopRight {
            get => new(Right, Top);
            set {
                Right = value.X;
                Top = value.Y;
            }
        }

        public Vector2 CenterLeft {
            get => leftSegment.Center;
        }

        public Vector2 CenterRight {
            get => rightSegment.Center;
        }

        public Vector2 BottomLeft {
            get => new(Left, Bottom);
            set {
                Left = value.X;
                Bottom = value.Y;
            }
        }

        public Vector2 BottomMiddle {
            get => bottomSegment.Center;
        }

        public Vector2 BottomRight {
            get => new(Right, Bottom);
            set {
                Right = value.X;
                Bottom = value.Y;
            }
        }

        public Vector2 Position {
            get => TopLeft;
            set {
                var change = value - Position;
                leftSegment.Point1 += change;
                leftSegment.Point2 += change;
                topSegment.Point1 += change;
                topSegment.Point2 += change;
                rightSegment.Point1 += change;
                rightSegment.Point2 += change;
                bottomSegment.Point1 += change;
                bottomSegment.Point2 += change;
            }
        }

        public float Width {
            get => topSegment.Length;
            set {
                topSegment.Point2 = new(topSegment.Point1.X + value, topSegment.Point1.Y);
                bottomSegment.Point2 = new(bottomSegment.Point1.X + value, bottomSegment.Point1.Y);
                rightSegment.Point1 = topSegment.Point2;
                rightSegment.Point2 = bottomSegment.Point2;
            }
        }
        
        public float Height {
            get => leftSegment.Length;
            set {
                leftSegment.Point2 = new(leftSegment.Point1.X, leftSegment.Point1.Y + value);
                rightSegment.Point2 = new(rightSegment.Point1.X, rightSegment.Point1.Y + value);
                bottomSegment.Point1 = leftSegment.Point2;
                bottomSegment.Point2 = rightSegment.Point2;
            }
        }

        public Vector2 Size => new(Width, Height);
        public Vector2 Center => new(X + Width / 2, Y + Height / 2);

        public RectanglePrimitive(float x, float y, float width, float height) {
            var _x = width > 0 ? x : x + width;
            var _y = height > 0 ? y : y + height;
            var _width = width > 0 ? width : width * -1;
            var _height = height > 0 ? height : height * -1;

            var _topLeft = new Vector2(_x, _y);
            var _topRight = new Vector2(_x + _width, _y);
            var _bottomLeft = new Vector2(_x, _y + _height);
            var _bottomRight = new Vector2(_x + _width, _y + _height);

            topSegment = new LineSegmentPrimitive(_topLeft, _topRight);
            bottomSegment = new LineSegmentPrimitive(_bottomLeft, _bottomRight);
            leftSegment = new LineSegmentPrimitive(_topLeft, _bottomLeft);
            rightSegment = new LineSegmentPrimitive(_topRight, _bottomRight);
        }

        public static RectanglePrimitive FromPoints(Vector2 topLeft, Vector2 bottomRight) {
            return new RectanglePrimitive(
                x: topLeft.X,
                y: topLeft.Y,
                width: bottomRight.X - topLeft.X,
                height: bottomRight.Y - topLeft.Y);
        }

        public static RectanglePrimitive FromPosition(Vector2 position, Vector2 size) {
            return new RectanglePrimitive(
                x: position.X,
                y: position.Y,
                width: size.X,
                height: size.Y);
        }

        public static implicit operator RectanglePrimitive(Rectangle rectangle) {
            return new RectanglePrimitive(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }

        public static explicit operator Rectangle(RectanglePrimitive rectangle) {
            return new Rectangle((int)rectangle.X, (int)rectangle.Y, (int)rectangle.Width, (int)rectangle.Height);
        }

        public static RectanglePrimitive operator +(RectanglePrimitive rectangle, Vector2 point) {
            return new RectanglePrimitive(rectangle.X + point.X, rectangle.Y + point.Y, rectangle.Width, rectangle.Height);
        }

        public bool DoesIntersect(Vector2 point) {
            return !(point.X < Left || point.X > Right || point.Y < Top || point.Y > Bottom);
        }

        public bool DoesIntersect(IPrimitive primitive) {
            if (primitive is CirclePrimitive circle && DoesIntersect(circle.Center)) {
                return true;
            } else if (primitive is RectanglePrimitive rectangle
                && (rectangle.DoesIntersect(TopLeft)
                    || rectangle.DoesIntersect(TopRight)
                    || rectangle.DoesIntersect(BottomLeft)
                    || rectangle.DoesIntersect(BottomRight)
                    || DoesIntersect(rectangle.TopLeft)
                    || DoesIntersect(rectangle.TopRight)
                    || DoesIntersect(rectangle.BottomLeft)
                    || DoesIntersect(rectangle.BottomRight))) {
                return true;
            } else if (primitive is LineSegmentPrimitive lineSegment && (DoesIntersect(lineSegment.Point1) || DoesIntersect(lineSegment.Point2))) {
                return true;
            }

            return topSegment.DoesIntersect(primitive) ||
                leftSegment.DoesIntersect(primitive) ||
                rightSegment.DoesIntersect(primitive) ||
                bottomSegment.DoesIntersect(primitive);
        }

        public float DistanceTo(Vector2 point) {
            if(DoesIntersect(point)) {
                return 0;
            } else {
                return (new float[] {
                    topSegment.DistanceTo(point),
                    bottomSegment.DistanceTo(point),
                    leftSegment.DistanceTo(point),
                    rightSegment.DistanceTo(point),
                }).Min();
            }
        }

        public float DistanceTo(IPrimitive primitive) {
            throw new NotImplementedException();
        }

        public List<Vector2> GetIntersectPoints(IPrimitive primitive) {
            return topSegment.GetIntersectPoints(primitive)
                .Concat(bottomSegment.GetIntersectPoints(primitive))
                .Concat(leftSegment.GetIntersectPoints(primitive))
                .Concat(rightSegment.GetIntersectPoints(primitive))
                .ToList();
        }
    }
}
