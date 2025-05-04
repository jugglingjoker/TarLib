using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TarLib.Entities.Drawable;
using TarLib.Extensions;
using TarLib.Graphics;

namespace TarLib.Entities {

    public abstract class SimpleDrawableGameEntityWithRotation<TCommandTypesEnum, TStateTypesEnum, TAnimationManager> : SimpleDrawableGameEntity<TCommandTypesEnum, TStateTypesEnum, TAnimationManager>, IDrawableEntityWithRotatingAnimation<TStateTypesEnum>
        where TCommandTypesEnum : Enum
        where TStateTypesEnum : Enum
        where TAnimationManager : IDrawableEntityAnimationManager {

        public abstract int DrawAnimationRotation { get; }
    }
}
