using System;

namespace TarLib.Entities.Drawable {
    public abstract class DrawableEntityAnimation<TEntity>
        where TEntity : IDrawableEntityWithAnimation {
        private static Random Random;
        protected static double RandomMod => Random.NextDouble();

        public TEntity Entity { get; }
        public abstract int FrameX { get; }
        public abstract int FrameY { get; }

        public DrawableEntityAnimation(TEntity entity) {
            Entity = entity;
            if (Random == null) {
                Random = new Random();
            }
        }

        public abstract void Update(float elapsedTime);
        public abstract void Reset();
    }
}
