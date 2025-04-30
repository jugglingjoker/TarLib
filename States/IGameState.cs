using Microsoft.Xna.Framework;
using System;
using TarLib.Input;

namespace TarLib.States {
    public interface IGameState {
        void LoadContent();
        void UnloadContent();
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);

        TarGame BaseGame { get; }
        IGameMenu Menu { get; }

        void MouseMove(MouseMoveEventArgs args);
        void MouseClickStart(MouseClickEventArgs args);
        void MouseClickEnd(MouseClickEventArgs args);
        void MouseScrollWheelChange(MouseScrollWheelChangeEventArgs args);
        void KeyPressStart(KeyPressEventArgs args);
        void KeyPressEnd(KeyPressEventArgs args);
    }
}
