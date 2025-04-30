using Microsoft.Xna.Framework;
using System;

namespace TarLib.Input {
    public class MouseScrollWheelChangeEventArgs : EventArgs {
        public Point MousePosition { get; }
        public float ScrollWheelChange { get; }
        public GameTime GameTime { get; }

        public MouseScrollWheelChangeEventArgs(Point mousePosition, float scrollWheelChange, GameTime gameTime) {
            MousePosition = mousePosition;
            ScrollWheelChange = scrollWheelChange;
            GameTime = gameTime;
        }

        public static implicit operator MouseScrollWheelChangeEventArgs((Point mousePosition, float scrollWheelChange, GameTime gameTime) scrollWheelEvent) {
            return new MouseScrollWheelChangeEventArgs(scrollWheelEvent.mousePosition, scrollWheelEvent.scrollWheelChange, scrollWheelEvent.gameTime);
        }
    }
}
