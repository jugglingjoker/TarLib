
using System;

namespace TarLib.Entities.Drawable {
    public abstract class RotatingEntityAnimation<TEntity> : DrawableEntityAnimation<TEntity>
        where TEntity : IDrawableEntityWithRotatingAnimation {

        public override int FrameX => Entity.DrawAnimationRotation;
        public RotatingEntityAnimation(TEntity entity) : base(entity) {

        }

        public override void Update(float elapsedTime) {
            // Do nothing
        }

        public override void Reset() {

        }
    }
}
