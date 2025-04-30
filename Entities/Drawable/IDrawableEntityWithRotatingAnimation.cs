using System;

namespace TarLib.Entities.Drawable {
    public interface IDrawableEntityWithRotatingAnimation : IDrawableEntityWithAnimation {
        int DrawAnimationRotation { get; }
    }

    public interface IDrawableEntityWithRotatingAnimation<TStateTypesEnum> : IDrawableEntityWithRotatingAnimation, IGameEntity<TStateTypesEnum>, IDrawableEntityWithAnimation<TStateTypesEnum>
        where TStateTypesEnum : Enum {

    }
}
