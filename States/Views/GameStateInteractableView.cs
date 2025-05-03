using Microsoft.Xna.Framework;
using TarLib.Input;
using System;
using TarLib.Entities.Interactable;
using TarLib.Primitives;
using TarLib.Entities.Drawable;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using TarLib.Extensions;
using Microsoft.Xna.Framework.Input;
using TarLib.Graphics;

namespace TarLib.States {

    public interface IGameStateInteractableView {
        public MultipleInteractableEntities DragTarget { get; }
        public MultipleInteractableEntities HoverTarget { get; }
        public MultipleInteractableEntities SelectTarget { get; }
        public MultipleInteractableEntities SelectingTarget { get; }
    }

    public abstract class GameStateInteractableView<TGameState> : GameStateView<TGameState>, IGameStateInteractableView
        where TGameState : IGameState {

        public readonly object objectLock = new();

        private ObservableVariable<MultipleInteractableEntities> dragTarget = new();
        private ObservableVariable<MultipleInteractableEntities> hoverTarget = new();
        private ObservableVariable<MultipleInteractableEntities> selectTarget = new();
        private ObservableVariable<MultipleInteractableEntities> selectingTarget = new();
        private ObservableVariable<MultipleInteractableEntities> clickTarget = new();
        private TimeSpan? lastClickTime;
        private bool isDoubleClick;

        public InteractableCollection Interactables { get; }

        public MultipleInteractableEntities DragTarget {
            get => dragTarget.Value;
            set => dragTarget.Value = value;
        }

        public MultipleInteractableEntities HoverTarget {
            get => hoverTarget.Value;
            set => hoverTarget.Value = value;
        }

        public MultipleInteractableEntities SelectTarget {
            get => selectTarget.Value;
            set => selectTarget.Value = value;
        }

        public MultipleInteractableEntities SelectingTarget {
            get => selectingTarget.Value;
            set => selectingTarget.Value = value;
        }

        public MultipleInteractableEntities ClickTarget {
            get => clickTarget.Value;
            set => clickTarget.Value = value;
        }

        public event EventHandler<(IDraggableEntity entity, Vector2 position)> OnDragStart;
        public event EventHandler<(IDraggableEntity entity, Vector2 position)> OnDrag;
        public event EventHandler<(IDraggableEntity entity, Vector2 position)> OnDragEnd;

        public event EventHandler<(MultipleInteractableEntities oldValue, MultipleInteractableEntities newValue)> OnDragTargetChange {
            add {
                lock (objectLock) {
                    dragTarget.OnChange += value;
                }
            }
            remove {
                lock (objectLock) {
                    dragTarget.OnChange -= value;
                }
            }
        }

        public event EventHandler<(MultipleInteractableEntities oldValue, MultipleInteractableEntities newValue)> OnHoverTargetChange {
            add {
                lock (objectLock) {
                    hoverTarget.OnChange += value;
                }
            }
            remove {
                lock (objectLock) {
                    hoverTarget.OnChange -= value;
                }
            }
        }

        public event EventHandler<(MultipleInteractableEntities oldValue, MultipleInteractableEntities newValue)> OnSelectTargetChange {
            add {
                lock (objectLock) {
                    selectTarget.OnChange += value;
                }
            }
            remove {
                lock (objectLock) {
                    selectTarget.OnChange -= value;
                }
            }
        }

        public event EventHandler<(MultipleInteractableEntities oldValue, MultipleInteractableEntities newValue)> OnSelectingTargetChange {
            add {
                lock (objectLock) {
                    selectingTarget.OnChange += value;
                }
            }
            remove {
                lock (objectLock) {
                    selectingTarget.OnChange -= value;
                }
            }
        }

        public event EventHandler<(MultipleInteractableEntities oldValue, MultipleInteractableEntities newValue)> OnClickTargetChange {
            add {
                lock (objectLock) {
                    clickTarget.OnChange += value;
                }
            }
            remove {
                lock (objectLock) {
                    clickTarget.OnChange -= value;
                }
            }
        }

        public virtual MouseButton SelectMouseButton => MouseButton.LeftButton;
        public virtual MouseButton DragMouseButton => MouseButton.LeftButton;
        public virtual double DoubleClickDuration => 0.5;

        public SelectingTargetInteractable SelectingInteractable { get; }

        protected GameStateInteractableView(TGameState state) : base(state) {
            SelectingInteractable = new SelectingTargetInteractable(this);

            Interactables = new InteractableCollection();
            Interactables.Add(SelectingInteractable);

            OnMouseMove += GameStateInteractableView_OnMouseMove;
            OnMouseClickStart += GameStateInteractableView_OnMouseClickStart;
            OnMouseClickEnd += GameStateInteractableView_OnMouseClickEnd;
        }

        protected virtual Vector2 DrawPositionToRealPosition(Vector2 position) {
            return position;
        }

        protected virtual Vector2 RealPositionToDrawPosition(Vector2 position) {
            return position;
        }

        private void GameStateInteractableView_OnMouseClickStart(object sender, MouseClickEventArgs e) {
            var translatedPosition = DrawPositionToRealPosition(e.MousePosition.ToVector2());

            // Hover action
            var hoverEntity = new MultipleInteractableEntities(Interactables.GetAt(translatedPosition));
            if (SelectTarget?.Contains(hoverEntity) ?? false) {
                HoverTarget = new MultipleInteractableEntities(SelectTarget);
            } else {
                HoverTarget = hoverEntity.Interactables.Count > 0 && hoverEntity.CanBeSelected ? hoverEntity : null;
            }

            // Select action
            if (HoverTarget != null && e.MouseButton == SelectMouseButton) {
                if (State.BaseGame.Input.KeysDown.Contains(Keys.LeftShift) || State.BaseGame.Input.KeysDown.Contains(Keys.RightShift)) {
                    if (SelectTarget != null) {
                        if (SelectTarget.Contains(hoverEntity)) {
                            RemoveSelectTarget(hoverEntity);
                        } else if(SelectTarget.CanBeMultiSelected && HoverTarget.CanBeMultiSelected) {
                            AddSelectTarget(hoverEntity);
                        }
                    } else {
                        SetSelectTarget(HoverTarget);
                    }
                } else {
                    SetSelectTarget(HoverTarget);
                }
            }

            // Click action
            ClickTarget = null;
            if (hoverEntity is IClickableEntity clickable) {
                ClickTarget = hoverEntity;
                if (lastClickTime != null && (e.GameTime.TotalGameTime - lastClickTime.Value).TotalSeconds < DoubleClickDuration) {
                    clickable.MouseDoubleClickStart(e);
                    isDoubleClick = true;
                } else {
                    clickable.MouseClickStart(e);
                }
                lastClickTime = e.GameTime.TotalGameTime;
            }

            // Drag action
            if (e.MouseButton == DragMouseButton) {
                DragTarget = HoverTarget ?? new MultipleInteractableEntities(SelectingInteractable);
                DragTarget.DragStart(translatedPosition);
                OnDragStart?.Invoke(this, (DragTarget, translatedPosition));
            }
        }

        public void AddSelectTarget(params IInteractableEntity[] targets) {
            var newSelect = new MultipleInteractableEntities(SelectTarget);
            var multiSelect = new MultipleInteractableEntities(targets);
            newSelect.Add(multiSelect);
            SelectTarget = newSelect;
            multiSelect.AfterAddedToSelectTarget(new SelectTargetEventArgs(SelectTarget));
        }

        public void RemoveSelectTarget(params IInteractableEntity[] targets) {
            var newSelect = new MultipleInteractableEntities(SelectTarget);
            var multiSelect = new MultipleInteractableEntities(targets);
            newSelect.Remove(multiSelect);
            SelectTarget = newSelect;
            multiSelect.AfterRemovedFromSelectTarget(new SelectTargetEventArgs(SelectTarget));
        }

        public void SetSelectTarget(params IInteractableEntity[] targets) {
            SelectTarget = new MultipleInteractableEntities(targets);
            SelectTarget.AfterAddedToSelectTarget(new SelectTargetEventArgs(SelectTarget));
        }

        private void GameStateInteractableView_OnMouseMove(object sender, MouseMoveEventArgs e) {
            var translatedPosition = DrawPositionToRealPosition(e.MousePosition.ToVector2());

            if (DragTarget != null) {
                var magnets = new List<IDraggableEntityMagnet>();
                foreach (var dragTarget in DragTarget.Draggables.Where(dragTarget => dragTarget.Magnets != null)) {
                    magnets.AddRange(dragTarget.Magnets.Where(magnet => magnet.Position.DistanceTo(translatedPosition) < 25).OrderBy(magnet => magnet.Position.DistanceTo(translatedPosition)));
                }

                DragTarget?.DragTo(translatedPosition, magnets);
                OnDrag?.Invoke(this, (DragTarget, translatedPosition));
            } else {
                var hoverEntity = new MultipleInteractableEntities(Interactables.GetAt(translatedPosition));
                if (SelectTarget?.Contains(hoverEntity) ?? false) {
                    HoverTarget = SelectTarget;
                } else if(hoverEntity.CanBeSelected) {
                    HoverTarget = hoverEntity;
                }
            }
        }

        private void GameStateInteractableView_OnMouseClickEnd(object sender, MouseClickEventArgs e) {
            var translatedPosition = DrawPositionToRealPosition(e.MousePosition.ToVector2());

            if (ClickTarget != null && ClickTarget is IClickableEntity clickable) {
                if(HoverTarget != null && HoverTarget.Contains(ClickTarget)) {
                    if (isDoubleClick) {
                        clickable.MouseDoubleClickEnd(e);
                        isDoubleClick = false;
                    } else {
                        clickable.MouseClickEnd(e);
                    }
                } else {
                    clickable.MouseClickCancel(e);
                }
            }

            if (DragTarget != null) {
                var magnets = new List<IDraggableEntityMagnet>();
                foreach (var dragTarget in DragTarget.Draggables.Where(dragTarget => dragTarget.Magnets != null)) {
                    magnets.AddRange(dragTarget.Magnets.Where(magnet => magnet.Position.DistanceTo(translatedPosition) < 25).OrderBy(magnet => magnet.Position.DistanceTo(translatedPosition)));
                }

                DragTarget.DragEnd(translatedPosition, magnets);
                OnDragEnd?.Invoke(this, (DragTarget, translatedPosition));
                DragTarget = null;
            }
        }

        public class SelectingTargetInteractable : IDrawableEntity, IDraggableEntity {

            public SelectingTargetInteractable(GameStateInteractableView<TGameState> view) {
                View = view;
                Texture = new TargetTexture(this);
            }

            public GameStateInteractableView<TGameState> View { get; }
            public Vector2 Start { get; private set; }
            public Vector2 End { get; private set; }
            public RectanglePrimitive Selection => RectanglePrimitive.FromPoints(View.RealPositionToDrawPosition(Start), View.RealPositionToDrawPosition(End));

            public float MinChangeX => float.MinValue;
            public float MaxChangeX => float.MaxValue;
            public float MinChangeY => float.MinValue;
            public float MaxChangeY => float.MaxValue;

            public bool CanBeSelected => false;
            public bool CanBeMultiSelected => false;

            public List<IDraggableEntityMagnet> Magnets => default;

            public Vector2 Size => (Start - End);
            public float DrawWidth => Math.Abs(Size.X);
            public float DrawHeight => Math.Abs(Size.Y);
            public Vector2 DrawPosition => Start;

            public IDrawableTexture Texture { get; }

            public void DragStart(Vector2 position) {
                Start = position; 
            }

            public void DragTo(Vector2 position, List<IDraggableEntityMagnet> magnets = default) {
                End = position;
                View.SelectingTarget = new MultipleInteractableEntities(View.Interactables.GetBetween(RectanglePrimitive.FromPoints(Start, position)).Where(interactable => interactable.CanBeSelected && interactable.CanBeMultiSelected).ToArray());
            }

            public void DragEnd(Vector2 position, List<IDraggableEntityMagnet> magnets = default) {
                if (View.State.BaseGame.Input.KeysDown.Contains(Keys.LeftShift) || View.State.BaseGame.Input.KeysDown.Contains(Keys.RightShift)) {
                    View.SelectTarget = new MultipleInteractableEntities(View.SelectTarget, View.SelectingTarget);
                } else {
                    View.SelectTarget = View.SelectingTarget;
                }
                View.SelectingTarget = null;
            }

            public bool IsAt(Vector2 position) {
                return false;
            }

            public bool IsBetween(RectanglePrimitive selection) {
                return false;
            }

            public IInteractableEntity GetElements(RectanglePrimitive? selection) {
                return null;
            }

            public void AfterAddedToSelectTarget(SelectTargetEventArgs e) {
                // do nothing
            }

            public void AfterRemovedFromSelectTarget(SelectTargetEventArgs e) {
                // do nothing
            }

            public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 positionOffset = default, float startDepth = 0, float endDepth = 1) {
                spriteBatch.Draw(Texture, positionOffset, startDepth, endDepth);
            }

            public class TargetTexture : IDrawableTexture {
                public TargetTexture(SelectingTargetInteractable interactable) {
                    Interactable = interactable;
                }

                public GameStateInteractableView<TGameState> View => Interactable.View;
                public SelectingTargetInteractable Interactable { get; }

                public Texture2D DrawTexture => View.State.BaseGame.Textures.Default;
                public Rectangle? DrawTextureFrame => null;
                public Vector2 DrawOrigin => DrawTexture.GetCenter();
                public float DrawRotation => 0;
                public Vector2 DrawScale => Interactable.Selection.Size; // TODO: Add scale factor
                public SpriteEffects DrawEffects => SpriteEffects.None;
                public Color DrawColor => new(1.0f, 0.0f, 0.0f, 0.1f); // TODO: Change to config
                public Vector2 DrawPosition => Interactable.Selection.Center;
                public float DrawDepth => 0f; // TODO: Change to config
                public bool DrawVisible => View.SelectingTarget != null; // TODO: Change to config
            }
        }
    }
}
