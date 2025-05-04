using System;
using TarLib.Entities.Drawable;

namespace TarLib.Entities {
    public abstract class SimpleDrawableGameEntityNoAnimation<TCommandTypesEnum, TStateTypesEnum> : SimpleDrawableGameEntity<TCommandTypesEnum, TStateTypesEnum, SimpleDrawableGameEntityNoAnimation<TCommandTypesEnum, TStateTypesEnum>.NoAnimationAnimationManager>
        where TCommandTypesEnum : Enum
        where TStateTypesEnum : Enum {

        public override int DrawAnimationFrameWidth => DrawTexture.Width;
        public override int DrawAnimationFrameHeight => DrawTexture.Height;

        public SimpleDrawableGameEntityNoAnimation() {
            AnimationManager = new NoAnimationAnimationManager(this);
        }

        public class NoAnimationAnimationManager : DrawableEntityAnimationManager<SimpleDrawableGameEntity<TCommandTypesEnum, TStateTypesEnum, NoAnimationAnimationManager>, TStateTypesEnum> {
            private readonly NoAnimation noAnimation;

            public NoAnimationAnimationManager(SimpleDrawableGameEntityNoAnimation<TCommandTypesEnum, TStateTypesEnum> entity) : base(entity) {
                noAnimation = new NoAnimation();
            }

            protected override IDrawableEntityAnimation this[TStateTypesEnum state] => noAnimation;
            protected override IDrawableEntityAnimation DefaultAnimation => noAnimation;

            private class NoAnimation : IDrawableEntityAnimation {
                public NoAnimation() {
                    
                }

                public int FrameX => 0;
                public int FrameY => 0;
                public void Reset() { }
                public void Update(float elapsedTime) { }
            }
        }
    }
}
