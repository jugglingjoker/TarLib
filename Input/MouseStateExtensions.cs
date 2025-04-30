using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace TarLib.Input {
    public static class MouseStateExtensions {
        public static List<MouseButton> GetPressedButtons(this MouseState mouseState) {
            var pressedButtons = new List<MouseButton>();

            if (mouseState.LeftButton.HasFlag(ButtonState.Pressed)) {
                pressedButtons.Add(MouseButton.LeftButton);
            }
            if (mouseState.RightButton.HasFlag(ButtonState.Pressed)) {
                pressedButtons.Add(MouseButton.RightButton);
            }
            if (mouseState.MiddleButton.HasFlag(ButtonState.Pressed)) {
                pressedButtons.Add(MouseButton.MiddleButton);
            }
            if (mouseState.XButton1.HasFlag(ButtonState.Pressed)) {
                pressedButtons.Add(MouseButton.XButton1);
            }
            if (mouseState.XButton2.HasFlag(ButtonState.Pressed)) {
                pressedButtons.Add(MouseButton.XButton2);
            }

            return pressedButtons;
        }
    }
}
