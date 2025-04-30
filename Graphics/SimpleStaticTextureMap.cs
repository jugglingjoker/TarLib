using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TarLib.Entities.Drawable;

namespace TarLib.Graphics {
    public class SimpleStaticTextureMap : IDrawableEntity {
        private const uint DEFAULT_SIZE = 1000;

        private TwoDimGrid<StaticTextureMapQuadrant> quads = new();
        public int QuadrantSize { get; }
        public GraphicsDevice GraphicsDevice { get; }
        public Texture2D this[int x, int y] => quads[x, y]?.Texture;

        public Color BackgroundColor { get; set; } = Color.Transparent;

        public int MinQuadX => quads.MinX;
        public int MaxQuadX => quads.MaxX;
        public int MinQuadY => quads.MinY;
        public int MaxQuadY => quads.MaxY;
        public int Width => MaxQuadX - MinQuadX + 1;
        public int Height => MaxQuadY - MinQuadY + 1;

        public float DrawWidth => Width * QuadrantSize;
        public float DrawHeight => Height * QuadrantSize;
        public Vector2 DrawPosition => Vector2.Zero;

        public Vector2 DrawOrigin { get; set; } = Vector2.Zero;
        public Vector2 DrawScale { get; set; } = Vector2.One;
        public float DrawRotation { get; set; } = 0;
        public Color DrawColor { get; set; } = Color.White;

        public SimpleStaticTextureMap(GraphicsDevice graphicsDevice, uint quadrantSize = DEFAULT_SIZE) {
            GraphicsDevice = graphicsDevice;
            QuadrantSize = (int)quadrantSize;
        }

        private ((int x, int y) quadStart, (int x, int y) quadEnd) CalculateQuadStats(float startX, float startY, float width, float height) {
            var endX = startX + width - 1;
            var endY = startY + height - 1;
            var quadXStart = (int)(startX / QuadrantSize);
            var quadXEnd = (int)(endX / QuadrantSize);
            var quadYStart = (int)(startY / QuadrantSize);
            var quadYEnd = (int)(endY / QuadrantSize);
            return ((quadXStart, quadYStart), (quadXEnd, quadYEnd));
        }

        public void Clear() {
            quads.Clear();
        }

        public void Add(IDrawableEntity drawableEntity) {
            var quadStats = CalculateQuadStats(
                startX: drawableEntity.DrawPosition.X, //drawableTexture.DrawPosition.X - drawableTexture.DrawOrigin.X * drawableTexture.DrawScale.X,
                startY: drawableEntity.DrawPosition.Y, // drawableTexture.DrawPosition.Y - drawableTexture.DrawOrigin.Y * drawableTexture.DrawScale.Y,
                width: drawableEntity.DrawWidth, // (drawableTexture.DrawTextureFrame?.Width ?? drawableTexture.DrawTexture.Width) * drawableTexture.DrawScale.X,
                height: drawableEntity.DrawHeight); //(drawableTexture.DrawTextureFrame?.Height ?? drawableTexture.DrawTexture.Height) * drawableTexture.DrawScale.Y);

            for(int i = quadStats.quadStart.x; i <= quadStats.quadEnd.x; i++) {
                for (int j = quadStats.quadStart.y; j <= quadStats.quadEnd.y; j++) {
                    if(quads[i, j] == default) {
                        quads[i, j] = new StaticTextureMapQuadrant(this, i, j);
                    }
                    quads[i, j].Add(drawableEntity);
                }
            }
        }

        public void Remove(IDrawableEntity drawableEntity) {
            var quadStats = CalculateQuadStats(
                startX: drawableEntity.DrawPosition.X, //drawableTexture.DrawPosition.X - drawableTexture.DrawOrigin.X * drawableTexture.DrawScale.X,
                startY: drawableEntity.DrawPosition.Y, // drawableTexture.DrawPosition.Y - drawableTexture.DrawOrigin.Y * drawableTexture.DrawScale.Y,
                width: drawableEntity.DrawWidth, // (drawableTexture.DrawTextureFrame?.Width ?? drawableTexture.DrawTexture.Width) * drawableTexture.DrawScale.X,
                height: drawableEntity.DrawHeight); //(drawableTexture.DrawTextureFrame?.Height ?? drawableTexture.DrawTexture.Height) * drawableTexture.DrawScale.Y);

            for (int i = quadStats.quadStart.x; i <= quadStats.quadEnd.x; i++) {
                for (int j = quadStats.quadStart.y; j <= quadStats.quadEnd.y; j++) {
                    if (quads[i, j] == default) {
                        quads[i, j] = new StaticTextureMapQuadrant(this, i, j);
                    }
                    quads[i, j].Remove(drawableEntity);
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position = default, float startDepth = 0, float endDepth = 1) {
            for (int i = MinQuadX; i <= MaxQuadX; i++) {
                for (int j = MinQuadY; j <= MaxQuadY; j++) {
                    if (quads[i, j] != default) {
                        var cornerPosition = Vector2.Transform((new Vector2(i * QuadrantSize, j * QuadrantSize) - DrawOrigin) * DrawScale, Matrix.CreateRotationZ(DrawRotation));
                        spriteBatch.Draw(
                            texture: quads[i, j].Texture,
                            position: cornerPosition + position,
                            sourceRectangle: null,
                            color: DrawColor,
                            rotation: DrawRotation,
                            origin: DrawOrigin,
                            scale: DrawScale,
                            effects: SpriteEffects.None,
                            layerDepth: startDepth);
                    }
                }
            }
        }

        private class StaticTextureMapQuadrant {
            public SimpleStaticTextureMap Map { get; }
            public int X { get; }
            public int Y { get; }

            private List<IDrawableEntity> drawables = new();

            private RenderTarget2D texture;
            public Texture2D Texture {
                get {
                    if(texture == default) {
                        texture = new RenderTarget2D(Map.GraphicsDevice, Map.QuadrantSize, Map.QuadrantSize);

                        var spriteBatch = new SpriteBatch(Map.GraphicsDevice);
                        var oldRenderTargets = Map.GraphicsDevice.GetRenderTargets();

                        Map.GraphicsDevice.SetRenderTarget(texture);
                        Map.GraphicsDevice.Clear(Map.BackgroundColor);
                        spriteBatch.Begin(
                            sortMode: SpriteSortMode.FrontToBack,
                            blendState: BlendState.AlphaBlend,
                            samplerState: SamplerState.PointClamp,
                            rasterizerState: Map.GraphicsDevice.RasterizerState);

                        var position = new Vector2(-1 * X * Map.QuadrantSize, -1 * Y * Map.QuadrantSize);
                        foreach(var drawable in drawables) {
                            drawable.Draw(new GameTime(), spriteBatch, position, 0, 1);
                        }

                        spriteBatch.End();
                        Map.GraphicsDevice.SetRenderTargets(oldRenderTargets);
                    }
                    return texture;
                }
            }

            public StaticTextureMapQuadrant(SimpleStaticTextureMap map, int x, int y) {
                Map = map;
                X = x;
                Y = y;
            }

            public void Add(IDrawableEntity drawable) {
                drawables.Add(drawable);
                texture = null;
            }

            public void Remove(IDrawableEntity drawable) {
                drawables.Remove(drawable);
                texture = null;
            }
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
