using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TarLib.Input {
    public class MouseClickEventArgs : EventArgs {

        public bool IsAvailable { get; private set; }

        public Point MousePosition { get; }
        public MouseButton MouseButton { get; }
        public GameTime GameTime { get; }

        public MouseClickEventArgs(Point mousePosition, MouseButton mouseButton, GameTime gameTime) {
            MousePosition = mousePosition;
            MouseButton = mouseButton;
            GameTime = gameTime;
            IsAvailable = true;
        }

        public void FlagAsUsed() {
            IsAvailable = false;
        }

        public static implicit operator MouseClickEventArgs((Point mousePosition, MouseButton mouseButton, GameTime gameTime) clickEvent) {
            return new MouseClickEventArgs(clickEvent.mousePosition, clickEvent.mouseButton, clickEvent.gameTime);
        }
    }
}
