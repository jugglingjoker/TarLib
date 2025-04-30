using Microsoft.Xna.Framework;

namespace TarLib.Entities.Interactable {
    public interface IDraggableEntityMagnet {
        IDraggableEntity Entity { get; }
        Vector2 Position { get; }
    }
}
