using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TarLib.Entities.Drawable;
using TarLib.Extensions;

namespace TarLib.Entities {
    public abstract class SimpleDrawableGameEntity<TCommandTypesEnum, TStateTypesEnum> : SimpleGameEntity<TCommandTypesEnum, TStateTypesEnum>, IDrawableEntity
        where TCommandTypesEnum : Enum
        where TStateTypesEnum : Enum {

        public IDrawableEntity Drawable { get; }

        public float DrawWidth => Drawable.DrawWidth;
        public float DrawHeight => Drawable.DrawHeight;
        public Vector2 DrawPosition => Drawable.DrawPosition;

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position = default, float startDepth = 0, float endDepth = 1) {
            Drawable.Draw(gameTime, spriteBatch, position, startDepth, endDepth);
        }
    }
}
