using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TarLib.Extensions {
    public static class Texture2DExtensions {

        // Draw drawable entity
        public static Vector2 GetCenter(this Texture2D texture) {
            return new Vector2(texture.Width, texture.Height) / 2;
        }
    }
}
