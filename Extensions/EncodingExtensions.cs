using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TarLib.Extensions {

    public static class EncodingExtensions {
        public const int INT_SIZE = 4;
        public const int FLOAT_SIZE = 4;
        public const int VECTOR2_SIZE = FLOAT_SIZE * 4;

        public static byte[] ToBytes(this int value) {
            return BitConverter.GetBytes(value);
        }

        public static int BytesToInt(this IEnumerable<byte> data) {
            if (data.Count() == INT_SIZE) {
                return BitConverter.ToInt32(data.ToArray());
            } else {
                throw new FormatException();
            }
        }

        public static byte[] ToBytes(this float value) {
            return BitConverter.GetBytes(value);
        }

        public static float BytesToFloat(this IEnumerable<byte> data) {
            if (data.Count() == FLOAT_SIZE) {
                return BitConverter.ToSingle(data.ToArray());
            } else {
                throw new FormatException();
            }
        }

        public static byte[] ToBytes(this Vector2 value) {
            return BitConverter.GetBytes(value.X).Concat(BitConverter.GetBytes(value.Y)).ToArray();
        }

        public static Vector2 BytesToVector2(this IEnumerable<byte> data) {
            if (data.Count() == VECTOR2_SIZE) {
                return new Vector2(
                    x: data.Take(FLOAT_SIZE).BytesToFloat(),
                    y: data.Skip(FLOAT_SIZE).Take(FLOAT_SIZE).BytesToFloat()
                );
            } else {
                throw new FormatException();
            }
        }

        public static byte[] ToBytes(this string value) {
            var bytes = value.Length.ToBytes();
            for (int i = 0; i < value.Length; i++) {
                bytes.Append((byte)value[i]);
            }
            return bytes;
        }

        public static string BytesToString(this IEnumerable<byte> data) {
            var sb = new StringBuilder();
            foreach (var charByte in data) {
                sb.Append((char)charByte);
            }
            return sb.ToString();
        }
    }
}
