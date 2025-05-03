using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TarLib.Entities.Drawable;
using TarLib.Extensions;

namespace TarLib.Graphics {

    public class SimpleStaticTextureEntity : IDrawableEntity {
        private SimpleStaticTexture texture;

        public SimpleStaticTextureEntity() {
            texture = new(this);
        }

        public Texture2D DrawTexture { get; set; } = default;
        public Rectangle? DrawTextureFrame { get; set; } = default;
        public Vector2 DrawOrigin { get; set; } = Vector2.Zero;
        public float DrawRotation { get; set; } = 0;
        public Vector2 DrawScale { get; set; } = Vector2.One;
        public SpriteEffects DrawEffects { get; set; } = SpriteEffects.None;
        public Color DrawColor { get; set; } = Color.White;
        public Vector2 DrawPosition { get; set; } = Vector2.Zero;
        public float DrawDepth { get; set; } = 0;
        public bool DrawVisible { get; set; } = true;

        public float DrawWidth => (texture.DrawTextureFrame?.Width ?? texture.DrawTexture.Width) * DrawScale.X;
        public float DrawHeight => (texture.DrawTextureFrame?.Height ?? texture.DrawTexture.Height) * DrawScale.Y;

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 positionOffset = default, float startDepth = 0, float endDepth = -1) {
            spriteBatch.Draw(texture, positionOffset, startDepth, endDepth);
        }

        private class SimpleStaticTexture : IDrawableTexture {
            public SimpleStaticTexture(SimpleStaticTextureEntity entity) {
                Entity = entity;
            }

            public SimpleStaticTextureEntity Entity { get; }
            public Texture2D DrawTexture => Entity.DrawTexture;
            public Rectangle? DrawTextureFrame => Entity.DrawTextureFrame;
            public Vector2 DrawOrigin => Entity.DrawOrigin;
            public float DrawRotation => Entity.DrawRotation;
            public Vector2 DrawScale => Entity.DrawScale;
            public SpriteEffects DrawEffects => Entity.DrawEffects;
            public Color DrawColor => Entity.DrawColor;
            public Vector2 DrawPosition => Entity.DrawPosition;
            public float DrawDepth => Entity.DrawDepth;
            public bool DrawVisible => Entity.DrawVisible;
        }
    }
}
