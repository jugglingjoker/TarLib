using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace TarLib.Input {
    public class InputManager {
        public TarGame Game { get; }

        private KeyboardState OldKeyboardState;
        public KeyboardState KeyboardState { get; protected set; }
        private MouseState OldMouseState;
        public MouseState MouseState { get; protected set; }

        public HashSet<Keys> KeysDown => KeyboardState.GetPressedKeys().ToHashSet();
        public HashSet<Keys> OldKeysDown => OldKeyboardState.GetPressedKeys().ToHashSet();
        public HashSet<Keys> StartingKeysDown => KeysDown.Except(OldKeysDown).ToHashSet();
        public HashSet<Keys> EndingKeysDown => OldKeysDown.Except(KeysDown).ToHashSet();

        public HashSet<MouseButton> MouseInputsDown => MouseState.GetPressedButtons().ToHashSet();
        public HashSet<MouseButton> OldMouseInputsDown => OldMouseState.GetPressedButtons().ToHashSet();
        public HashSet<MouseButton> StartingMouseInputsDown => MouseInputsDown.Except(OldMouseInputsDown).ToHashSet();
        public HashSet<MouseButton> EndingMouseInputsDown => OldMouseInputsDown.Except(MouseInputsDown).ToHashSet();

        public Point MousePosition => MouseState.Position;
        public bool IsMouseMoved => OldMouseState.Position != MouseState.Position;
        public bool IsMouseScrollWheelMoved => OldMouseState.ScrollWheelValue != MouseState.ScrollWheelValue;
        public int MouseScrollWheelChange => MouseState.ScrollWheelValue - OldMouseState.ScrollWheelValue;

        public InputManager(TarGame game) {
            Game = game;
        }

        public void Update() {
            OldKeyboardState = KeyboardState;
            KeyboardState = Keyboard.GetState();
            OldMouseState = MouseState;
            MouseState = Mouse.GetState();
        }
    }
}
