using Microsoft.Xna.Framework;
using System;

namespace TarLib.Input {
    public class MouseMoveEventArgs : EventArgs {
        public Point MousePosition { get; }
        public GameTime GameTime { get; }

        public MouseMoveEventArgs(Point mousePosition, GameTime gameTime) {
            MousePosition = mousePosition;
            GameTime = gameTime;
        }

        public static implicit operator MouseMoveEventArgs((Point mousePosition, GameTime gameTime) e) {
            return new MouseMoveEventArgs(e.mousePosition, e.gameTime);
        }
    }
}
