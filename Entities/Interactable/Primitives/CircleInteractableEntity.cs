using TarLib.Primitives;
using TarLib.States;

namespace TarLib.Entities.Interactable.Primitives {
    public abstract class CircleInteractableEntity : SimplePrimitiveInteractableEntity<CirclePrimitive> {
        public override CirclePrimitive Hotzone => new CirclePrimitive(Position, Radius);
        public override float MaxDistance => 0;
        public abstract float Radius { get; }

        protected CircleInteractableEntity(IGameStateInteractableView view) : base(view) {

        }
    }
}
