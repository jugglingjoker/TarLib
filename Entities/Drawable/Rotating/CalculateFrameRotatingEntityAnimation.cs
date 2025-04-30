using System;

namespace TarLib.Entities.Drawable {
    public class CalculateFrameRotatingEntityAnimation<TEntity> : RotatingEntityAnimation<TEntity>
        where TEntity : IDrawableEntityWithRotatingAnimation {

        private Func<int> CalculationAction { get; }
        public override int FrameY => CalculationAction();

        public CalculateFrameRotatingEntityAnimation(TEntity entity, Func<int> calculationAction) : base(entity) {
            CalculationAction = calculationAction;
        }
    }
}
