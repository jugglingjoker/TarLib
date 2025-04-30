using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TarLib.Extensions;

namespace TarLib.Primitives {
    public struct LineRayPrimitive : IPrimitive {
        private LinePrimitive line;

        public Vector2 Origin {
            get => line.Origin;
            set => line.Origin = value;
        }
        public float Angle {
            get => line.Angle;
            set => line.Angle = value;
        }

        public LineRayPrimitive(Vector2 origin, float angle) {
            line = new LinePrimitive(origin, angle);
        }

        public LineRayPrimitive Translate(Vector2 offset) {
            Origin += offset;
            return this;
        }

        public LineRayPrimitive Rotate(float rotation) {
            return RotateAroundPoint(Origin, rotation);
        }

        public LineRayPrimitive RotateAroundPoint(Vector2 point, float rotation) {
            line.RotateAroundPoint(point, rotation);
            return this;
        }

        public float DistanceTo(Vector2 point) {
            throw new NotImplementedException();
        }

        public float DistanceTo(IPrimitive primitive) {
            throw new NotImplementedException();
        }

        public bool DoesIntersect(Vector2 point) {
            if (line.DoesIntersect(point)) {
                return Math.Abs(Origin.AngleTo(point) - Angle) == 0; // TODO: Make configurable
            } else {
                return false;
            }
        }

        public bool DoesIntersect(IPrimitive primitive) {
            return primitive.DoesIntersect(Origin) || GetIntersectPoints(primitive).Count > 0;
        }

        public List<Vector2> GetIntersectPoints(IPrimitive primitive) {
            var ray = this;
            return line.GetIntersectPoints(primitive).Where(intersect => ray.DoesIntersect(intersect)).OrderBy(point => point.DistanceTo(ray.Origin)).ToList();
        }

        public static LineRayPrimitive FromPoints(Vector2 origin, Vector2 pointOnRay) {
            return FromAngle(origin, origin.AngleTo(pointOnRay));
        }

        public static LineRayPrimitive FromAngle(Vector2 origin, float angle) {
            return new LineRayPrimitive(origin, angle);
        }
    }
}
