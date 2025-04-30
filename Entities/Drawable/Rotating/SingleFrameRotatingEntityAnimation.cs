using System;

namespace TarLib.Entities.Drawable {
    // TODO: Move to own file
    public class SingleFrameRotatingEntityAnimation<TEntity> : RotatingEntityAnimation<TEntity>
        where TEntity : IDrawableEntityWithRotatingAnimation {

        private readonly int SingleFrame;
        public override int FrameY => SingleFrame;

        public SingleFrameRotatingEntityAnimation(TEntity entity, int singleFrame) : base(entity) {
            SingleFrame = singleFrame;
        }
    }
}
