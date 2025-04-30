using Microsoft.Xna.Framework;

namespace TarLib.Entities.Interactable {
    public class SimpleDraggableMagnet : IDraggableEntityMagnet {
        public IDraggableEntity Entity { get; }
        public Vector2 Position { get; }

        public SimpleDraggableMagnet(IDraggableEntity entity, Vector2 position) {
            Entity = entity;
            Position = position;
        }
    }
}
