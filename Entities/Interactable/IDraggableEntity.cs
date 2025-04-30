using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace TarLib.Entities.Interactable {

    public interface IDraggableEntity : IInteractableEntity {
        void DragStart(Vector2 position);
        void DragTo(Vector2 position, List<IDraggableEntityMagnet> magnets);
        void DragEnd(Vector2 position, List<IDraggableEntityMagnet> magnets);
        List<IDraggableEntityMagnet> Magnets { get; }
    }
}
