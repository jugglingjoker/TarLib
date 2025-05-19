using Microsoft.Xna.Framework;
using TarLib.Input;

namespace TarLib.States {
    public interface IGameMenu {
        IGameState State { get; }

        IMenuBlock HoverTarget { get; }
        IMenuBlock SelectTarget { get; }
        IMenuBlock ClickTarget { get; }

        IMenuWindow ExclusiveWindow { get; }

        void MouseMove(MouseMoveEventArgs e);
        void MouseClickStart(MouseClickEventArgs e);
        void MouseClickEnd(MouseClickEventArgs e);
        void MouseScrollWheelChange(MouseScrollWheelChangeEventArgs e);
        void KeyPressStart(KeyPressEventArgs e);
        void KeyPressEnd(KeyPressEventArgs e);

        void Add(IMenuWindow window, string insertBefore = default, string insertAfter = default);
        void Remove(IMenuWindow window);
        void Clear();

        void Update(GameTime gameTime);
        void Draw(GameTime gameTime, float startDepth = 0, float endDepth = 1);
    }
}
