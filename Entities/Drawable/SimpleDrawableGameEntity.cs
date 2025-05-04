using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TarLib.Entities.Drawable;
using TarLib.Extensions;
using TarLib.Graphics;

namespace TarLib.Entities {

    public abstract class SimpleDrawableGameEntity<TCommandTypesEnum, TStateTypesEnum, TAnimationManager> : SimpleGameEntity<TCommandTypesEnum, TStateTypesEnum>, IDrawableEntityWithAnimation<TStateTypesEnum>
        where TCommandTypesEnum : Enum
        where TStateTypesEnum : Enum
        where TAnimationManager : IDrawableEntityAnimationManager {

        public abstract Texture2D DrawTexture { get; }
        public abstract Vector2 DrawOrigin { get; }
        public abstract float DrawRotation { get; }
        public abstract Vector2 DrawScale { get; }
        public abstract SpriteEffects DrawEffects { get; }
        public abstract Color DrawColor { get; }
        public abstract Vector2 DrawPosition { get; }
        public abstract float DrawDepth { get; }
        public abstract bool DrawVisible { get; }

        public abstract int DrawAnimationFrameWidth { get; }
        public abstract int DrawAnimationFrameHeight { get; }

        public virtual float DrawWidth => DrawAnimationFrameWidth;
        public virtual float DrawHeight => DrawAnimationFrameHeight;

        public TAnimationManager AnimationManager { get; protected set; }
        private Texture texture;

        public SimpleDrawableGameEntity() {
            texture = new(this);
        }

        public override void Update(float elapsedTime) {
            base.Update(elapsedTime);
            AnimationManager.Update(elapsedTime);
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 positionOffset = default, float startDepth = 0, float endDepth = 1) {
            spriteBatch.Draw(texture, positionOffset, startDepth, endDepth);
        }

        private class Texture : IDrawableTexture {
            public Texture2D DrawTexture => Entity.DrawTexture;
            public Rectangle? DrawTextureFrame => Entity.AnimationManager.Bounds;
            public Vector2 DrawOrigin => Entity.DrawOrigin;
            public float DrawRotation => Entity.DrawRotation;
            public Vector2 DrawScale => Entity.DrawScale;
            public SpriteEffects DrawEffects => Entity.DrawEffects;
            public Color DrawColor => Entity.DrawColor;
            public Vector2 DrawPosition => Entity.DrawPosition;
            public float DrawDepth => Entity.DrawDepth;
            public bool DrawVisible => Entity.DrawVisible;

            public SimpleDrawableGameEntity<TCommandTypesEnum, TStateTypesEnum, TAnimationManager> Entity { get; }

            public Texture(SimpleDrawableGameEntity<TCommandTypesEnum, TStateTypesEnum, TAnimationManager> entity) {
                Entity = entity;
            }
        }
    }
}
