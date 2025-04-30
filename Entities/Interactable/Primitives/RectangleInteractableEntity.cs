using Microsoft.Xna.Framework;
using TarLib.Primitives;
using TarLib.States;

namespace TarLib.Entities.Interactable.Primitives {
    public abstract class RectangleInteractableEntity : SimplePrimitiveInteractableEntity<RectanglePrimitive> {
        public override RectanglePrimitive Hotzone => new RectanglePrimitive(Position.X, Position.Y, Width, Height);
        public override float MaxDistance => 0;

        public abstract float Width { get; }
        public abstract float Height { get; }

        protected RectangleInteractableEntity(IGameStateInteractableView view) : base(view) {

        }
    }
}
