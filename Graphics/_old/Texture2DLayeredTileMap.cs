using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TarLib.Extensions;

namespace TarLib.Graphics {
    public class Texture2DLayeredTileMap {
        public const int DEFAULT_CHUNK_SIZE = 1200;

        private GraphicsDevice GraphicsDevice { get; }
        private Texture2D[,] Textures { get; }
        public Texture2D this[int x, int y] {
            get {
                if (Textures[x, y] == null) {
                    Textures[x, y] = GenerateTexture(x, y);
                }
                return Textures[x, y];
            }
        }

        public int Width { get; }
        public int Height { get; }
        public int TextureChunkWidth { get; }
        public int TextureChunkHeight { get; }
        public Texture2DLayeredTileMapParamType WidthType { get; }
        public Texture2DLayeredTileMapParamType HeightType { get; }

        public int MapWidth => Textures.GetLength(0);
        public int MapHeight => Textures.GetLength(1);

        private readonly List<(Texture2DLayeredTile texture, Point position)>[,] textureTilesToDraw;

        public Texture2DLayeredTileMap(
            GraphicsDevice graphicsDevice,
            int width,
            int height,
            int textureChunkWidth = DEFAULT_CHUNK_SIZE,
            int textureChunkHeight = DEFAULT_CHUNK_SIZE,
            Texture2DLayeredTileMapParamType widthType = Texture2DLayeredTileMapParamType.Clamp,
            Texture2DLayeredTileMapParamType heightType = Texture2DLayeredTileMapParamType.Clamp) {

            GraphicsDevice = graphicsDevice;
            Width = width;
            Height = height;
            TextureChunkWidth = textureChunkWidth;
            TextureChunkHeight = textureChunkHeight;
            WidthType = widthType;
            HeightType = heightType;

            var chunksWide = (int)Math.Ceiling((float)width / textureChunkWidth);
            var chunksHigh = (int)Math.Ceiling((float)height / textureChunkHeight);
            Textures = new Texture2D[chunksWide, chunksHigh];
            textureTilesToDraw = new List<(Texture2DLayeredTile, Point)>[chunksWide, chunksHigh];
        }

        public void AddTexture(Texture2DLayeredTile texture, Point position) {
            var startX = (int)Math.Floor((float)position.X / TextureChunkWidth);
            var startY = (int)Math.Floor((float)position.Y / TextureChunkHeight);
            var endX = (int)Math.Ceiling((float)(position.X + texture.Width) / TextureChunkWidth);
            var endY = (int)Math.Ceiling((float)(position.Y + texture.Height) / TextureChunkHeight);

            for (int i = startX; i < endX; i++) {
                for (int j = startY; j < endY; j++) {
                    var x = i; // TODO: Add wrap/clamp behavior
                    var y = j;

                    if (WidthType == Texture2DLayeredTileMapParamType.Wrap) {
                        while (x < 0) {
                            x += MapWidth;
                        }
                        x %= MapWidth;
                    }

                    if (HeightType == Texture2DLayeredTileMapParamType.Wrap) {
                        while (y < 0) {
                            y += MapHeight;
                        }
                        y %= MapHeight;
                    }

                    if (x >= 0 && x < MapWidth && y >= 0 && y < MapHeight) {
                        if (textureTilesToDraw[x, y] == null) {
                            textureTilesToDraw[x, y] = new List<(Texture2DLayeredTile texture, Point position)>();
                        }
                        textureTilesToDraw[x, y].Add((texture, position));
                    }
                }
            }
        }

        private Texture2D GenerateTexture(int x, int y) {
            if (x >= 0 && x < MapWidth && y >= 0 && y < MapHeight && textureTilesToDraw[x, y] != null) {
                var textureWidth = x == MapWidth - 1 && Width % TextureChunkWidth != 0 ? Width % TextureChunkWidth : TextureChunkWidth;
                var textureHeight = y == MapHeight - 1 && Height % TextureChunkHeight != 0 ? Height % TextureChunkHeight : TextureChunkHeight;
                RenderTarget2D texture = new RenderTarget2D(GraphicsDevice, textureWidth, textureHeight);
                SpriteBatch tmpBatch = new SpriteBatch(GraphicsDevice);
                var oldRenderTargets = GraphicsDevice.GetRenderTargets();

                GraphicsDevice.SetRenderTarget(texture);
                tmpBatch.Begin();
                foreach (var textureToDraw in textureTilesToDraw[x, y]) {
                    tmpBatch.Draw(
                        textureToDraw.texture,
                        textureToDraw.position.ToVector2() - new Vector2(x * TextureChunkWidth, y * TextureChunkHeight),
                        Color.White);
                }
                tmpBatch.End();
                GraphicsDevice.SetRenderTargets(oldRenderTargets);

                return texture;
            } else {
                return null;
            }
        }
    }

    public enum Texture2DLayeredTileMapParamType {
        Clamp = 0,
        Wrap
    }
}
