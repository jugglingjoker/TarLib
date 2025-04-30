using Microsoft.Xna.Framework;
using TarLib.Input;

namespace TarLib.Entities.Interactable {
    public interface IClickableEntity : IInteractableEntity {
        void MouseClickStart(MouseClickEventArgs e);
        void MouseClickEnd(MouseClickEventArgs e);
        void MouseDoubleClickStart(MouseClickEventArgs e);
        void MouseDoubleClickEnd(MouseClickEventArgs e);
        void MouseClickCancel(MouseClickEventArgs e);
    }
}
