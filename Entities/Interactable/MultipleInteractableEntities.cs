using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TarLib.Input;
using TarLib.Primitives;

namespace TarLib.Entities.Interactable {
    public class MultipleInteractableEntities : IDraggableEntity, IClickableEntity {

        private InteractableCollection interactables = new();
        public IReadOnlyCollection<IInteractableEntity> Interactables => interactables;

        private InteractableCollection<IDraggableEntity> draggables = new();
        public IReadOnlyCollection<IDraggableEntity> Draggables => draggables;

        private InteractableCollection<IClickableEntity> clickables = new();
        public IReadOnlyCollection<IClickableEntity> Clickables => clickables;

        private Vector2 dragStartPosition;
        public float MinChangeX => interactables.Max(interactable => interactable.MinChangeX);
        public float MaxChangeX => interactables.Min(interactable => interactable.MaxChangeX);
        public float MinChangeY => interactables.Max(interactable => interactable.MinChangeY);
        public float MaxChangeY => interactables.Min(interactable => interactable.MaxChangeY);

        public bool CanBeSelected => interactables.All(interactable => interactable.CanBeSelected);
        public bool CanBeMultiSelected => interactables.All(interactable => interactable.CanBeMultiSelected);

        public List<IDraggableEntityMagnet> Magnets => null;

        public MultipleInteractableEntities(params IInteractableEntity[] entities) {
            foreach(var entity in entities) {
                Add(entity);
            }
        }

        public void Add(IInteractableEntity entity) {
            if (entity is MultipleInteractableEntities multiEntities) {
                foreach (var interactable in multiEntities.Interactables) {
                    Add(interactable);
                }
            } else if(entity != null) {
                if(!interactables.Contains(entity)) {
                    interactables.Add(entity);
                    if (entity is IDraggableEntity draggable) {
                        draggables.Add(draggable);
                    }
                    if (entity is IClickableEntity clickable) {
                        clickables.Add(clickable);
                    }
                }
            }
        }

        public void Remove(IInteractableEntity entity) {
            if(entity is MultipleInteractableEntities multiEntities) {
                foreach(var interactable in multiEntities.Interactables) {
                    Remove(interactable);
                }
            } else if(entity != null) {
                interactables.Remove(entity);
                if (entity is IDraggableEntity draggable) {
                    draggables.Remove(draggable);
                }
                if (entity is IClickableEntity clickable) {
                    clickables.Remove(clickable);
                }
            }
        }

        public bool Contains(IInteractableEntity entity) {
            if (entity is MultipleInteractableEntities multipleEntities) {
                if (multipleEntities.Interactables.Count > 0) {
                    foreach (var multipleEntity in multipleEntities.Interactables) {
                        if (!Contains(multipleEntity)) {
                            return false;
                        }
                    }
                    return true;
                } else {
                    return false;
                }
            } else {
                return Interactables.Contains(entity);
            }
        }

        public override bool Equals(object obj) {
            if(obj is MultipleInteractableEntities multipleEntities) {
                if(multipleEntities.Interactables.Count == Interactables.Count) {
                    var diff = multipleEntities.Interactables.Except(Interactables);
                    if(!diff.Any()) {
                        return true;
                    }
                }
            }
            return base.Equals(obj);
        }

        public void DragStart(Vector2 position) {
            dragStartPosition = position;
            foreach(var entity in Draggables) {
                entity.DragStart(position);
            }
        }

        public void DragTo(Vector2 position, List<IDraggableEntityMagnet> magnets = default) {
            var change = position - dragStartPosition;
            var changeX = MathHelper.Clamp(change.X, MinChangeX, MaxChangeX);
            var changeY = MathHelper.Clamp(change.Y, MinChangeY, MaxChangeY);
            foreach (var entity in Draggables) {
                entity.DragTo(dragStartPosition + new Vector2(changeX, changeY), magnets.Where(magnet => magnet.Entity == entity).ToList());
            }
        }

        public void DragEnd(Vector2 position, List<IDraggableEntityMagnet> magnets = default) {
            foreach (var entity in Draggables) {
                entity.DragEnd(position, magnets.Where(magnet => magnet.Entity == entity).ToList());
            }
        }

        public bool IsAt(Vector2 position) {
            return true;
        }

        public bool IsBetween(RectanglePrimitive selection) {
            foreach(var entity in Interactables) {
                if(entity.IsBetween(selection)) {
                    return true;
                }
            }
            return false;
        }

        public IInteractableEntity GetElements(RectanglePrimitive? selection = null) {
            if(selection != null) {
                var elements = new MultipleInteractableEntities();
                foreach (var entity in Interactables) {
                    if (entity.IsBetween(selection.Value)) {
                        elements.Add(entity);
                    }
                }
                return elements;
            } else {
                return this;
            }
        }

        public IInteractableEntity GetElements() {
            return this;
        }

        public void MouseClickStart(MouseClickEventArgs e) {
            foreach (var entity in Clickables) {
                entity.MouseClickStart(e);
            }
        }

        public void MouseClickEnd(MouseClickEventArgs e) {
            foreach(var entity in Clickables) {
                entity.MouseClickEnd(e);
            }
        }

        public void MouseClickCancel(MouseClickEventArgs e) {
            foreach (var entity in Clickables) {
                entity.MouseClickCancel(e);
            }
        }

        public void MouseDoubleClickStart(MouseClickEventArgs e) {
            foreach (var entity in Clickables) {
                entity.MouseDoubleClickStart(e);
            }
        }

        public void MouseDoubleClickEnd(MouseClickEventArgs e) {
            foreach (var entity in Clickables) {
                entity.MouseDoubleClickEnd(e);
            }
        }

        public void AfterRemovedFromSelectTarget(SelectTargetEventArgs e) {
            foreach(var entity in Interactables) {
                entity.AfterRemovedFromSelectTarget(e);
            }
        }

        public void AfterAddedToSelectTarget(SelectTargetEventArgs e) {
            foreach (var entity in Interactables) {
                entity.AfterAddedToSelectTarget(e);
            }
        }
    }
}
