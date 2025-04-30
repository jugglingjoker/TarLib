using Microsoft.Xna.Framework;
using TarLib.Primitives;
using TarLib.States;

namespace TarLib.Entities.Interactable.Primitives {

    public abstract class SimplePrimitiveInteractableEntity<TPrimitive> : SimpleInteractableEntity
        where TPrimitive : IPrimitive {

        public abstract TPrimitive Hotzone { get; }
        public abstract float MaxDistance { get; }

        protected SimplePrimitiveInteractableEntity(IGameStateInteractableView view) : base(view) {

        }

        public override bool IsAt(Vector2 position) {
            return Hotzone.DistanceTo(position) <= MaxDistance;
        }

        public override bool IsBetween(RectanglePrimitive selection) {
            return Hotzone.DoesIntersect(selection);
        }
    }
}
