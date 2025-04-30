using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TarLib.Entities.Drawable;
using TarLib.Extensions;

namespace TarLib.Graphics {
    public class SimpleStaticStringEntity : IDrawableEntity {
        private SimpleStaticString texture;

        public SimpleStaticStringEntity() {
            texture = new(this);
        }

        public Vector2 DrawOrigin { get; set; } = Vector2.Zero;
        public float DrawRotation { get; set; } = 0;
        public Vector2 DrawScale { get; set; } = Vector2.One;
        public SpriteEffects DrawEffects { get; set; } = SpriteEffects.None;
        public Color DrawColor { get; set; } = Color.White;
        public Vector2 DrawPosition { get; set; } = Vector2.Zero;
        public float DrawDepth { get; set; } = 0;
        public bool DrawVisible { get; set; } = true;
        public SpriteFont DrawFont { get; set; } = default;
        public string DrawText { get; set; } = "";

        public float DrawWidth => texture.DrawFont.MeasureString(DrawText).X;
        public float DrawHeight => texture.DrawFont.MeasureString(DrawText).Y;

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position = default, float startDepth = 0, float endDepth = -1) {
            spriteBatch.Draw(texture, position, startDepth, endDepth);
        }

        private class SimpleStaticString : IDrawableString {
            public SimpleStaticString(SimpleStaticStringEntity entity) {
                Entity = entity;
            }

            public SimpleStaticStringEntity Entity { get; }

            public SpriteFont DrawFont => Entity.DrawFont;
            public string DrawText => Entity.DrawText;
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

    /*
    public class Texture2DMap {
        private const uint DEFAULT_SEGMENT_SIZE = 1000;

        public uint SegmentSize { get; }
        private TwoDimDictionary<int, Texture2DMapQuadrant> TextureQuads { get; }
        private GraphicsDevice GraphicsDevice { get; }
        public Texture2D this[int x, int y] => TextureQuads[x, y]?.Texture;

        private int minX, maxX;
        public int Width => maxX - minX;

        private int minY, maxY;
        public int Height => maxY - minY;

        public Vector2 Center => new Vector2(Width / 2, Height / 2);

        public int MinQuadX => (int)Math.Floor((double)(minX / SegmentSize));
        public int MaxQuadX => (int)Math.Ceiling((double)(maxX / SegmentSize));
        public int MinQuadY => (int)Math.Floor((double)(minY / SegmentSize));
        public int MaxQuadY => (int)Math.Ceiling((double)(maxX / SegmentSize));

        public Texture2DMap(GraphicsDevice graphicsDevice, uint segmentSize = DEFAULT_SEGMENT_SIZE) {
            if (segmentSize == 0) {
                // TODO: throw segment size exception
            }
            GraphicsDevice = graphicsDevice;
            SegmentSize = segmentSize;
            TextureQuads = new TwoDimDictionary<int, Texture2DMapQuadrant>();
        }

        public void AddTexture(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth) {

            var textureWidth = texture.Width * scale.X;
            var textureHeight = texture.Height * scale.Y;
            var startX = position.X - origin.X * scale.Y;
            var endX = startX + textureWidth;
            var startY = position.Y - origin.Y * scale.Y;
            var endY = startY + textureHeight;
            
            var quadXStart = (int)(startX / SegmentSize);
            var quadXEnd = (int)(endX / SegmentSize);
            var quadYStart = (int)(startY / SegmentSize);
            var quadYEnd = (int)(endY / SegmentSize);

            for (var i = quadXStart; i <= quadXEnd; i++) {
                for (var j = quadYStart; j <= quadYEnd; j++) {
                    if (TextureQuads[i, j] == default) {
                        TextureQuads[i, j] = new Texture2DMapQuadrant(this);
                    }
                    TextureQuads[i, j].AddTexture(
                        texture: texture,
                        position: position + new Vector2(-1 * i * SegmentSize, -1 * j * SegmentSize),
                        sourceRectangle: sourceRectangle,
                        color: color,
                        rotation: rotation,
                        origin: origin,
                        scale: scale,
                        effects: effects,
                        layerDepth: layerDepth);
                }
            }

            minX = Math.Min(minX, (int)Math.Floor(startX));
            maxX = Math.Max(maxX, (int)Math.Ceiling(endX));
            minY = Math.Min(minY, (int)Math.Floor(startY));
            maxY = Math.Max(maxY, (int)Math.Ceiling(endY));
        }

        public void AddString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth) {
            var stringMeasurements = spriteFont.MeasureString(text);

            var textureWidth = stringMeasurements.X * scale.X;
            var textureHeight = stringMeasurements.Y * scale.Y;
            var startX = position.X - origin.X * scale.Y;
            var endX = startX + textureWidth;
            var startY = position.Y - origin.Y * scale.Y;
            var endY = startY + textureHeight;

            var quadXStart = (int)(startX / SegmentSize);
            var quadXEnd = (int)(endX / SegmentSize);
            var quadYStart = (int)(startY / SegmentSize);
            var quadYEnd = (int)(endY / SegmentSize);

            for (var i = quadXStart; i <= quadXEnd; i++) {
                for (var j = quadYStart; j <= quadYEnd; j++) {
                    if (TextureQuads[i, j] == default) {
                        TextureQuads[i, j] = new Texture2DMapQuadrant(this);
                    }
                    TextureQuads[i, j].AddString(
                        spriteFont: spriteFont,
                        text: text,
                        position: position + new Vector2(-1 * i * SegmentSize, -1 * j * SegmentSize),
                        color: color,
                        rotation: rotation,
                        origin: origin,
                        scale: scale,
                        effects: effects,
                        layerDepth: layerDepth);
                }
            }

            minX = Math.Min(minX, (int)Math.Floor(startX));
            maxX = Math.Max(maxX, (int)Math.Ceiling(endX));
            minY = Math.Min(minY, (int)Math.Floor(startY));
            maxY = Math.Max(maxY, (int)Math.Ceiling(endY));
        }

        public void Dispose() {
            throw new System.Exception();
            // TODO: Loop through dictionary and dispose all textures
        }

        private class Texture2DMapQuadrant {
            private Texture2DMap TextureMap { get; }
            private List<Texture2DDrawInstructions> TextureDrawInstructions { get; }
            private List<StringDrawInstructions> StringDrawInstructions { get; }

            private Texture2D _Texture;
            public Texture2D Texture {
                get {
                    if (_Texture == null && (TextureDrawInstructions.Count > 0 || StringDrawInstructions.Count > 0)) {
                        var renderTarget = new RenderTarget2D(TextureMap.GraphicsDevice, (int)TextureMap.SegmentSize, (int)TextureMap.SegmentSize);
                        var spriteBatch = new SpriteBatch(TextureMap.GraphicsDevice);
                        var oldRenderTargets = TextureMap.GraphicsDevice.GetRenderTargets();
                        TextureMap.GraphicsDevice.SetRenderTarget(renderTarget);
                        TextureMap.GraphicsDevice.Clear(Color.Transparent);
                        spriteBatch.Begin();
                        foreach (var textureDrawInstructions in TextureDrawInstructions) {
                            spriteBatch.Draw(textureDrawInstructions);
                        }
                        foreach (var stringDrawInstructions in StringDrawInstructions) {
                            spriteBatch.Draw(stringDrawInstructions);
                        }
                        spriteBatch.End();
                        TextureMap.GraphicsDevice.SetRenderTargets(oldRenderTargets);
                        _Texture = renderTarget;
                    }
                    return _Texture;
                }
            }

            public Texture2DMapQuadrant(Texture2DMap textureMap) {
                TextureMap = textureMap;
                TextureDrawInstructions = new List<Texture2DDrawInstructions>();
                StringDrawInstructions = new List<StringDrawInstructions>();
            }

            public void AddTexture(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth) {
                TextureDrawInstructions.Add(
                    new Texture2DDrawInstructions(
                        texture,
                        position,
                        sourceRectangle,
                        color,
                        rotation,
                        origin,
                        scale,
                        effects,
                        layerDepth)
                    );
            }

            public void AddString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth) {
                StringDrawInstructions.Add(
                    new StringDrawInstructions(
                        spriteFont,
                        text,
                        position,
                        color,
                        rotation,
                        origin,
                        scale,
                        effects,
                        layerDepth)
                    );
            }
        }
    }
    */
}
