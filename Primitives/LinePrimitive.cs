using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using TarLib.Extensions;

namespace TarLib.Primitives {
    public struct LinePrimitive : IPrimitive {
        private Vector2 angleVector;
        private float angle;
        public float Angle {
            get => angle;
            set {
                angle = MathHelper.WrapAngle(value);
                angleVector = Vector2Extensions.AngleToVector2(angle);
            }
        }
        public Vector2 Origin { get; set; }

        public float Slope => angleVector.X != 0 ? angleVector.Y / angleVector.X : float.NaN;
        public float Intersect => !float.IsNaN(Slope) ? Origin.Y - Slope * Origin.X : float.NaN;
        public float InvertedSlope => angleVector.Y != 0 ? angleVector.X / angleVector.Y : float.NaN;
        public float InvertedIntersect => !float.IsNaN(InvertedSlope) ? Origin.X - InvertedSlope * Origin.Y : float.NaN;
        public bool IsParallel(LinePrimitive otherLine) => otherLine.Angle == Angle;

        public LinePrimitive(Vector2 origin, float angle) {
            Origin = origin;
            this.angle = angle;
            angleVector = Vector2Extensions.AngleToVector2(this.angle);
        }

        public float ValueAtX(float xVal) {
            return !float.IsNaN(Slope) ? Slope * xVal + Intersect : float.NaN;
        }

        public float ValueAtY(float yVal) {
            return !float.IsNaN(InvertedSlope) ? InvertedSlope * yVal + InvertedIntersect : float.NaN;
        }

        public bool DoesIntersect(Vector2 point) {
            var xVal = ValueAtY(point.Y);
            var yVal = ValueAtX(point.X);
            return (float.IsNaN(xVal) || Math.Abs(xVal - point.X) < 0.1f) && (float.IsNaN(yVal) || Math.Abs(yVal - point.Y) < 0.1f); // TODO: change to const, or configurable value
        }

        public bool DoesIntersect(IPrimitive primitive) {
            return GetIntersectPoints(primitive).Count > 0;
        }

        public float DistanceTo(Vector2 point) {
            var otherLine = new LinePrimitive(point, Angle + MathHelper.PiOver2);
            var intersects = GetIntersectPoints(otherLine);
            foreach(var intersect in intersects) {
                return point.DistanceTo(intersect);
            }
            return float.MaxValue;
        }

        public float DistanceTo(IPrimitive primitive) {
            throw new NotImplementedException();
        }

        public LinePrimitive Translate(Vector2 offset) {
            Origin += offset;
            return this;
        }

        public LinePrimitive Rotate(float rotation) {
            return RotateAroundPoint(Vector2.Zero, rotation);
        }

        public LinePrimitive RotateAroundPoint(Vector2 point, float rotation) {
            Origin = point + Vector2Extensions.AngleToVector2((point - Origin).ToAngle() + rotation) * Origin.DistanceTo(point);
            Angle += rotation;
            return this;
        }

        public List<Vector2> GetIntersectPoints(IPrimitive primitive) {
            var points = new List<Vector2>();
            if(primitive is LinePrimitive otherLine) {
                if(!otherLine.IsParallel(this)) {
                    if(float.IsNaN(Slope)) {
                        var x = ValueAtY(0);
                        var y = otherLine.ValueAtX(x);
                        points.Add(new Vector2(x, y));
                    } else if (float.IsNaN(otherLine.Slope)) {
                        var x = otherLine.ValueAtY(0);
                        var y = ValueAtX(x);
                        points.Add(new Vector2(x, y));
                    } else {
                        var x = (otherLine.Intersect - Intersect) / (Slope - otherLine.Slope);
                        var y = ValueAtX(x);
                        points.Add(new Vector2(x, y));
                    }
                } else {
                    
                }
            } else if(primitive is CirclePrimitive circle) {
                if(!float.IsNaN(Slope)) {
                    var a = Slope * Slope + 1;
                    var b = 2 * (Slope * Intersect - Slope * circle.Center.Y - circle.Center.X);
                    var c = circle.Center.X * circle.Center.X + circle.Center.Y * circle.Center.Y + Intersect * Intersect - circle.Radius * circle.Radius - 2 * Intersect * circle.Center.Y;
                    var disc = b * b - 4 * a * c;

                    if (disc >= 0) {
                        var x1 = (float)(-b + Math.Sqrt(disc)) / (2 * a);
                        var x2 = (float)(-b - Math.Sqrt(disc)) / (2 * a);
                        if (x1 != x2) {
                            points.Add(new Vector2(x2, ValueAtX(x2)));
                        }
                        points.Add(new Vector2(x1, ValueAtX(x1)));
                    }
                } else {
                    var a = InvertedSlope * InvertedSlope + 1;
                    var b = 2 * (InvertedSlope * InvertedIntersect - InvertedSlope * circle.Center.X - circle.Center.Y);
                    var c = circle.Center.X * circle.Center.X + circle.Center.Y * circle.Center.Y + InvertedIntersect * InvertedIntersect - circle.Radius * circle.Radius - 2 * InvertedIntersect * circle.Center.X;
                    var disc = b * b - 4 * a * c;

                    if (disc >= 0) {
                        var y1 = (float)(-b + Math.Sqrt(disc)) / (2 * a);
                        var y2 = (float)(-b - Math.Sqrt(disc)) / (2 * a);
                        if (y1 != y2) {
                            points.Add(new Vector2(ValueAtY(y2), y2));
                        }
                        points.Add(new Vector2(ValueAtY(y1), y1));
                    }
                }
            } else {
                points.AddRange(primitive.GetIntersectPoints(this));
            }
            return points;
        }

        public static LinePrimitive FromPoints(Vector2 point1, Vector2 point2) {
            return new LinePrimitive(point1, (point2 - point1).ToAngle());
        }

        
    }
}
