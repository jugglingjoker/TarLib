using Microsoft.Xna.Framework;
using System;
using TarLib.Primitives;

namespace TarLib.Entities.Interactable {
    public interface IInteractableEntity {

        bool IsAt(Vector2 position);
        bool IsBetween(RectanglePrimitive selection);
        IInteractableEntity GetElements(RectanglePrimitive? selection = null);

        void AfterRemovedFromSelectTarget(SelectTargetEventArgs e);
        void AfterAddedToSelectTarget(SelectTargetEventArgs e);

        public float MinChangeX { get; }
        public float MaxChangeX { get; }
        public float MinChangeY { get; }
        public float MaxChangeY { get; }

        public bool CanBeSelected { get; }
        public bool CanBeMultiSelected { get; }
    }

    public class SelectTargetEventArgs : EventArgs {

        public IInteractableEntity SelectTarget { get; }

        public SelectTargetEventArgs(IInteractableEntity selectTarget) {
            SelectTarget = selectTarget;
        }
    }
}
