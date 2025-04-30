using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TarLib.Input;

namespace TarLib.States {
    public interface IGameStateView {
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime, float startDepth, float endDepth);

        void LoadContent();
        void UnloadContent();

        void Start();
        void End();

        /*
        event EventHandler<MouseMoveEventArgs> OnMouseMove;
        event EventHandler<MouseClickEventArgs> OnMouseClickStart;
        event EventHandler<MouseClickEventArgs> OnMouseClickEnd;
        event EventHandler<MouseScrollWheelChangeEventArgs> OnMouseScrollWheelChange;
        */

        void MouseMove(MouseMoveEventArgs args);
        void MouseClickStart(MouseClickEventArgs args);
        void MouseClickEnd(MouseClickEventArgs args);
        void MouseScrollWheelChange(MouseScrollWheelChangeEventArgs args);
        void KeyPressStart(KeyPressEventArgs args);
        void KeyPressEnd(KeyPressEventArgs args);
    }
}
