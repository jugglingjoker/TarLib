using Microsoft.Xna.Framework;
using System;

namespace TarLib.Extensions {
    public static class Vector2Extensions {
        public static Vector2 AngleToVector2(float angle) {
            var wrappedAngle = MathHelper.WrapAngle(angle);

            if(wrappedAngle == 0) {
                return new Vector2(1, 0);
            } else if(wrappedAngle == MathHelper.PiOver2) {
                return new Vector2(0, 1);
            } else if(wrappedAngle == MathHelper.Pi) {
                return new Vector2(-1, 0);
            } else if(wrappedAngle == -1 * MathHelper.PiOver2) {
                return new Vector2(0, -1);
            } else {
                return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
            }
        }

        public static Vector2 ToNormalize(this Vector2 vector2) {
            if (vector2.X == 0 && vector2.Y == 0) {
                return vector2;
            }

            var newVector2 = vector2;
            newVector2.Normalize();
            return newVector2;
        }

        public static float ToAngle(this Vector2 vector2) {
            return (float)Math.Atan2(vector2.Y, vector2.X);
        }

        public static float DistanceTo(this Vector2 vector2, Vector2 point) {
            return (point - vector2).Length();
        }

        public static float AngleTo(this Vector2 vector2, Vector2 point) {
            return (point - vector2).ToAngle();
        }

        public static Vector2 Rotate(this Vector2 vector2, float angle) {
            return Vector2.Transform(vector2, Matrix.CreateRotationZ(angle));
        }

        public static Vector2 RoundTo(this Vector2 vector2, float nearest) {
            if(nearest != 0) {
                return new Vector2(
                    (float)Math.Round(vector2.X / nearest) * nearest,
                    (float)Math.Round(vector2.Y / nearest) * nearest);
            } else {
                throw new DivideByZeroException();
            }
        }

        public static Vector2 Center(this Vector2 start, Vector2 end) {
            return (end - start) / 2 + start;
        }

        public static (float Angle, float Time, Vector2 Point) NearestIntersect(this Vector2 center, float speed, Vector2 otherCenter, Vector2 otherVelocity) {
            var distance = center.DistanceTo(otherCenter);
            var direction = (otherCenter - center).ToAngle();
            var alpha = MathHelper.Pi + direction - otherVelocity.ToAngle();
            var otherSpeed = otherVelocity.Length();

            // destination not moving
            if (otherSpeed == 0 && speed > 0) {
                return ((otherCenter - center).ToAngle(), distance / speed, otherCenter);
            }

            // Equal speed and no intersection possible
            if (speed == otherSpeed && Math.Cos(alpha) < 0) {
                return (float.NaN, float.NaN, default);
            }

            var a = speed * speed - otherSpeed * otherSpeed;
            var b = 2 * distance * otherSpeed * (float)Math.Cos(alpha);
            var c = -1 * distance * distance;
            var disc = b * b - 4 * a * c;

            // no intersection possible
            if (disc < 0) {
                return (float.NaN, float.MaxValue, default);
            }

            var time1 = (float)(-b + Math.Sqrt(disc)) / (2 * a);
            var time2 = (float)(-b - Math.Sqrt(disc)) / (2 * a);

            if (time1 < 0 && time2 < 0) {
                return (float.NaN, float.MaxValue, default);
            } else {
                var time = time2 < 0 ? time1 : (time1 < 0 ? time2 : (time1 < time2 ? time1 : time2));
                var intersect = otherCenter + otherVelocity * time;
                return (
                    Angle: (intersect - center).ToAngle(),
                    Time: time,
                    Point: intersect);
            }
        }
    }

    public struct PreciseVector2 {
        public const uint DEFAULT_PRECISION = 4;

        private uint _Precision;

        private long _RawX;
        public double X {
            get => _RawX * Math.Pow(10, _Precision);
            set => _RawX = (long)(value / Math.Pow(10, _Precision));
        }

        private long _RawY;
        public double Y {
            get => _RawY * Math.Pow(10, _Precision);
            set => _RawY = (long)(value / Math.Pow(10, _Precision));
        }

        public PreciseVector2(double x, double y, uint precision = DEFAULT_PRECISION) {
            _Precision = precision;
            _RawX = (long)(x / Math.Pow(10, _Precision));
            _RawY = (long)(y / Math.Pow(10, _Precision));
        }

        public double Length() => Math.Sqrt(LengthSquared());
        public double LengthSquared() => X * X + Y * Y;

        public static implicit operator Vector2(PreciseVector2 input) {
            return new Vector2((float)input.X, (float)input.Y);
        }

        public static PreciseVector2 Zero => new PreciseVector2(0, 0);
        public static PreciseVector2 One => new PreciseVector2(1, 1);
    }
}
