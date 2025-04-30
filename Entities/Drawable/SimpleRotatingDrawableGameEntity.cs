using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TarLib.Entities.Drawable;
using TarLib.Extensions;

namespace TarLib.Entities {
    public abstract class SimpleRotatingDrawableGameEntity<TCommandTypesEnum, TStateTypesEnum> : SimpleGameEntity<TCommandTypesEnum, TStateTypesEnum>, IDrawableEntityWithRotatingAnimation<TStateTypesEnum>
        where TCommandTypesEnum : Enum
        where TStateTypesEnum : Enum {

        public abstract Texture2D DrawTexture { get; }
        public abstract Rectangle? DrawTextureFrame { get; }
        public abstract Vector2 DrawOrigin { get; }
        public abstract float DrawRotation { get; }
        public abstract Vector2 DrawScale { get; }
        public abstract SpriteEffects DrawEffects { get; }
        public abstract Color DrawColor { get; }
        public abstract Vector2 DrawPosition { get; }
        public abstract float DrawDepth { get; }
        public abstract bool DrawVisible { get; }
        public abstract int DrawAnimationRotation { get; }
        public abstract int DrawAnimationFrameWidth { get; }
        public abstract int DrawAnimationFrameHeight { get; }

        public virtual float DrawWidth => DrawAnimationFrameWidth;
        public virtual float DrawHeight => DrawAnimationFrameHeight;

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position = default, float startDepth = 0, float endDepth = 1) {
            /*
            spriteBatch.Draw(
                entity: this, 
                position: position,
                startDepth: startDepth, 
                endDepth: endDepth);
            */
        }
    }
}
