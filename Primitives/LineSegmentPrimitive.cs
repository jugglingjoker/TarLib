using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TarLib.Extensions;

namespace TarLib.Primitives {
    public struct LineSegmentPrimitive : IPrimitive {
        private LinePrimitive line;
        public Vector2 Point1 {
            get => line.Origin;
            set => line.Origin = value;
        }
        private Vector2 _Point2;
        public Vector2 Point2 {
            get => _Point2;
            set {
                _Point2 = value;
                line.Angle = (Point2 - Point1).ToAngle();
            }
        }
        public float Length => Point1.DistanceTo(Point2);
        public Vector2 Center => (Point1 + Point2) / 2;
        public float Angle => line.Angle;

        public LineSegmentPrimitive(Vector2 point1, Vector2 point2) {
            line = LinePrimitive.FromPoints(point1, point2);
            _Point2 = point2;
        }

        public LineSegmentPrimitive Translate(Vector2 offset) {
            line.Translate(offset);
            _Point2 += offset;
            return this;
        }

        public LineSegmentPrimitive Rotate(float rotation) {
            return RotateAroundPoint(Vector2.Zero, rotation);
        }

        public LineSegmentPrimitive RotateAroundPoint(Vector2 point, float rotation) {
            line.RotateAroundPoint(point, rotation);
            _Point2 = Vector2Extensions.AngleToVector2((point - _Point2).ToAngle() + rotation) * _Point2.DistanceTo(point);
            return this;
        }

        public bool DoesIntersect(Vector2 point) {
            if (line.DoesIntersect(point)) {
                bool x = Point1.X > Point2.X ? (point.X <= Point1.X && point.X >= Point2.X) : (point.X >= Point1.X && point.X <= Point2.X);
                bool y = Point1.Y > Point2.Y ? (point.Y <= Point1.Y && point.Y >= Point2.Y) : (point.Y >= Point1.Y && point.Y <= Point2.Y);
                return x && y;
            } else {
                return false;
            }
        }

        public bool DoesIntersect(IPrimitive primitive) {
            return primitive.DoesIntersect(Point1) || primitive.DoesIntersect(Point2) || GetIntersectPoints(primitive).Count > 0;
        }

        public float DistanceTo(Vector2 point) {
            var otherLine = new LinePrimitive(point, Angle + MathHelper.PiOver2);
            foreach (var intersect in GetIntersectPoints(otherLine)) {
                return point.DistanceTo(intersect);
            }
            return Math.Min(point.DistanceTo(Point1), point.DistanceTo(Point2));
        }

        public float DistanceTo(IPrimitive primitive) {
            throw new NotImplementedException();
        }

        public List<Vector2> GetIntersectPoints(IPrimitive primitive) {
            var segment = this;
            var points = line.GetIntersectPoints(primitive);

            return line.GetIntersectPoints(primitive).Where(intersect => segment.DoesIntersect(intersect)).ToList();
        }

        
    }
}
