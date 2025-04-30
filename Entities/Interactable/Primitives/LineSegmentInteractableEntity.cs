using Microsoft.Xna.Framework;
using TarLib.Primitives;
using TarLib.States;

namespace TarLib.Entities.Interactable.Primitives {
    public abstract class LineSegmentInteractableEntity : SimplePrimitiveInteractableEntity<LineSegmentPrimitive> {
        public override LineSegmentPrimitive Hotzone => new LineSegmentPrimitive(Start, End);
        public abstract Vector2 Start { get; }
        public abstract Vector2 End { get; }

        protected LineSegmentInteractableEntity(IGameStateInteractableView view) : base(view) {

        }
    }
}
