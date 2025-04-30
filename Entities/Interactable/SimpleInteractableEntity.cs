using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using TarLib.Entities.Drawable;
using TarLib.Extensions;
using TarLib.Input;
using TarLib.Primitives;
using TarLib.States;

namespace TarLib.Entities.Interactable { 
    public abstract class SimpleInteractableEntity : IDraggableEntity, IClickableEntity{
        public IGameStateInteractableView BaseView { get; }

        public Vector2 OriginalPosition { get; private set; }
        public Vector2 DragStartPosition { get; private set; }
        public abstract Vector2 Position { get; set; }

        public virtual float MinX => float.MinValue;
        public virtual float MaxX => float.MaxValue;
        public virtual float MinY => float.MinValue;
        public virtual float MaxY => float.MaxValue;
        public float MinChangeX => MinX - OriginalPosition.X;
        public float MaxChangeX => MaxX - OriginalPosition.X;
        public float MinChangeY => MinY - OriginalPosition.Y;
        public float MaxChangeY => MaxY - OriginalPosition.Y;

        public virtual bool CanBeSelected => true;
        public virtual bool CanBeMultiSelected => true;

        public virtual bool IsHoverTarget => BaseView.HoverTarget?.Contains(this) ?? false;
        public virtual bool IsDragTarget => BaseView.DragTarget?.Contains(this) ?? false;
        public virtual bool IsSelectTarget => BaseView.SelectTarget?.Contains(this) ?? false;
        public virtual bool IsSelectingTarget => BaseView.SelectingTarget?.Contains(this) ?? false;

        public virtual List<IDraggableEntityMagnet> Magnets => null;

        public SimpleInteractableEntity(IGameStateInteractableView view) {
            BaseView = view;
        }

        public virtual void DragStart(Vector2 position) {
            OriginalPosition = Position;
            DragStartPosition = position;
        }

        public virtual void DragTo(Vector2 position, List<IDraggableEntityMagnet> magnets = default) {
            if (magnets != default && magnets.Count > 0) {
                Position = magnets.First().Position;
            } else {
                var change = position - DragStartPosition;
                var changeX = MathHelper.Clamp(change.X, MinChangeX, MaxChangeX);
                var changeY = MathHelper.Clamp(change.Y, MinChangeY, MaxChangeY);
                Position = OriginalPosition + new Vector2(changeX, changeY);
            }
        }

        public virtual void DragEnd(Vector2 position, List<IDraggableEntityMagnet> magnets = default) { }
        public virtual void MouseClickStart(MouseClickEventArgs e) { }
        public virtual void MouseClickEnd(MouseClickEventArgs e) { }
        public virtual void MouseDoubleClickStart(MouseClickEventArgs e) { }
        public virtual void MouseDoubleClickEnd(MouseClickEventArgs e) { }
        public virtual void MouseClickCancel(MouseClickEventArgs e) { }
        public virtual void AfterRemovedFromSelectTarget(SelectTargetEventArgs e) { }
        public virtual void AfterAddedToSelectTarget(SelectTargetEventArgs e) { }

        public abstract bool IsAt(Vector2 position);
        public abstract bool IsBetween(RectanglePrimitive selection);
        public virtual IInteractableEntity GetElements(RectanglePrimitive? selection = null) => this;
    }
}
