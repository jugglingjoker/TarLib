using System;

namespace TarLib.Entities.Drawable {
    public interface IDrawableEntityWithAnimation : IDrawableEntity {
        int DrawAnimationFrameWidth { get; }
        int DrawAnimationFrameHeight { get; }
    }

    public interface IDrawableEntityWithAnimation<TStateTypesEnum> : IDrawableEntityWithAnimation, IGameEntity<TStateTypesEnum>
        where TStateTypesEnum : Enum {

    }
}
