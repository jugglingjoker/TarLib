using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TarLib.Entities.Drawable;

namespace TarLib.Graphics {
    public class SimpleStaticTextureMap {
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

                        var positionOffset = new Vector2(-1 * X * Map.QuadrantSize, -1 * Y * Map.QuadrantSize);
                        foreach(var drawable in drawables) {
                            drawable.Draw(new GameTime(), spriteBatch, positionOffset, 0, 1);
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
}
