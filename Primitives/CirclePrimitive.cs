using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using TarLib.Extensions;

namespace TarLib.Primitives {
    public struct CirclePrimitive : IPrimitive {
        public Vector2 Center { get; set; }
        public float Radius { get; set; }

        public float Diameter => Radius * 2;
        public float Circumference => Diameter * MathHelper.Pi;
        public float Area => Radius * Radius * MathHelper.Pi;

        public CirclePrimitive(Vector2 center, float radius) {
            Center = center;
            Radius = radius;
        }

        public CirclePrimitive Rotate(float rotation) {
            return RotateAroundPoint(Vector2.Zero, rotation);
        }

        public CirclePrimitive RotateAroundPoint(Vector2 point, float rotation) {
            Center = Vector2Extensions.AngleToVector2((point - Center).ToAngle() + rotation) * Center.DistanceTo(point);
            return this;
        }

        public CirclePrimitive Translate(Vector2 offset) {
            Center += offset;
            return this;
        }

        public static CirclePrimitive operator +(CirclePrimitive circle, Vector2 offset) {
            return circle.Translate(offset);
        }

        public static CirclePrimitive operator -(CirclePrimitive circle, Vector2 offset) {
            return circle.Translate(-1 * offset);
        }

        public bool DoesIntersect(Vector2 point) {
            return point.DistanceTo(Center) < Radius;
        }

        public bool DoesIntersect(IPrimitive primitive) {
            if(primitive is CirclePrimitive circle) {
                return circle.Center.DistanceTo(Center) <= circle.Radius + Radius;
            } else {
                return primitive.DoesIntersect(this); // use their method instead
            }
        }

        public float DistanceTo(Vector2 point) {
            return Math.Max(0, point.DistanceTo(Center) - Radius);
        }

        public float DistanceTo(IPrimitive primitive) {
            return Math.Max(0, primitive.DistanceTo(Center) - Radius);
        }

        public List<Vector2> GetIntersectPoints(IPrimitive primitive) {
            if (primitive is CirclePrimitive circle) {
                var points = new List<Vector2>();
                var dist = Center.DistanceTo(circle.Center);
                if (dist <= Radius + circle.Radius && dist >= Math.Abs(Radius - circle.Radius) && (dist != 0 || Center != circle.Center)) {
                    var a = (Radius * Radius - circle.Radius * circle.Radius + dist * dist) / (2 * dist);
                    var h = (float)Math.Sqrt(Radius * Radius - a * a);
                    var center = Center + a * (circle.Center - Center) / dist;

                    points.Add(new Vector2(
                        center.X + h * (circle.Center.Y - Center.Y) / dist,
                        center.Y - h * (circle.Center.X - Center.X) / dist
                        ));
                    if (h != 0) {
                        points.Add(new Vector2(
                            center.X - h * (circle.Center.Y - Center.Y) / dist,
                            center.Y + h * (circle.Center.X - Center.X) / dist
                            ));
                    }
                }
                return points;
            } else {
                return primitive.GetIntersectPoints(this);
            }
        }

    }
}
