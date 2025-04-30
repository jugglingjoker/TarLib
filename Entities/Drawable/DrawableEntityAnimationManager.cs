using Microsoft.Xna.Framework;
using System;

namespace TarLib.Entities.Drawable {
    public abstract class DrawableEntityAnimationManager<TEntity, TStateTypesEnum>
        where TEntity : IDrawableEntityWithAnimation<TStateTypesEnum>
        where TStateTypesEnum : Enum {

        public TEntity Entity { get; }

        public DrawableEntityAnimation<TEntity> CurrentAnimation => this[Entity.State.Type] ?? DefaultAnimation;
        protected abstract DrawableEntityAnimation<TEntity> DefaultAnimation { get; }
        protected abstract DrawableEntityAnimation<TEntity> this[TStateTypesEnum state] { get; }

        public Rectangle? Bounds => new Rectangle(
            x: (CurrentAnimation?.FrameX ?? 0) * Entity.DrawAnimationFrameWidth,
            y: (CurrentAnimation?.FrameY ?? 0) * Entity.DrawAnimationFrameHeight,
            width: Entity.DrawAnimationFrameWidth,
            height: Entity.DrawAnimationFrameHeight);

        public DrawableEntityAnimationManager(TEntity entity) {
            Entity = entity;
            Entity.OnStateChange += OnEntityStateChange;
        }

        public void OnEntityStateChange(object sender, (IEntityState<TStateTypesEnum> oldState, IEntityState<TStateTypesEnum> newState) stateChangeData) {
            var oldAnimation = this[stateChangeData.oldState.Type];
            var newAnimation = this[stateChangeData.newState.Type];
            if (oldAnimation != newAnimation) {
                newAnimation?.Reset();
            }
        }

        public void Update(float elapsedTime) {
            CurrentAnimation.Update(elapsedTime);
        }
    }
}
