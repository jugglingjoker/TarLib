using Microsoft.Xna.Framework;
using System;

namespace TarLib.Extensions {
    public static class Vector3Extensions {

        public static float ToDisplacementLength(this Vector3 vector3) {
            return (new Vector2(vector3.X, vector3.Y)).Length();
        }

        public static float ToDisplacementAngle(this Vector3 vector3) {
            return (float)Math.Atan2(vector3.Y, vector3.X);
        }

        public static float ToInclinationAngle(this Vector3 vector3) {
            return (float)Math.Atan2(vector3.Z, ToDisplacementLength(vector3));
        }
    }
}
