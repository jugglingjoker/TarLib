using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;

namespace TarLib.Assets {
    public class TextureAssetManager : AssetManager<Texture2D> {
        protected const uint DEFAULT_TEXTURE_WIDTH = 1;
        protected const uint DEFAULT_TEXTURE_HEIGHT = 1;

        public TextureAssetManager(TarGame game) : base(game) {

        }

        public override Texture2D LoadDefault() {
            var defaultTexture = new Texture2D(Game.GraphicsDevice, (int)DEFAULT_TEXTURE_WIDTH, (int)DEFAULT_TEXTURE_HEIGHT);
            var colors = new Color[DEFAULT_TEXTURE_WIDTH * DEFAULT_TEXTURE_HEIGHT];
            for (int i = 0; i < DEFAULT_TEXTURE_HEIGHT; i++) {
                for (int j = 0; j < DEFAULT_TEXTURE_WIDTH; j++) {
                    colors[i * DEFAULT_TEXTURE_WIDTH + j] = Color.White;
                }
            }
            defaultTexture.SetData(colors);
            return defaultTexture;
        }

        protected override Texture2D GetAssetByFilename(string filename) {
            using FileStream fileStream = new FileStream(filename, FileMode.Open);
            Texture2D spriteAtlas = Texture2D.FromStream(Game.GraphicsDevice, fileStream);
            fileStream.Dispose();
            return spriteAtlas;
        }

        public override void LoadContent() {
            base.LoadContent();
            CreateDefaults();
        }

        private void CreateDefaults() {
            var circle = new Texture2D(Game.GraphicsDevice, 1024, 1024);
            var colors = new Color[circle.Width * circle.Height];
            var center = new Vector2(circle.Width / 2, circle.Height / 2);
            for (int i = 0; i < circle.Width; i++) {
                for (int j = 0; j < circle.Height; j++) {

                    var dist = Vector2.Distance(center, new Vector2(i, j));

                    if (dist < circle.Width / 2) {
                        colors[i * circle.Width + j] = Color.White;
                    } else {
                        colors[i * circle.Width + j] = Color.Transparent;
                    }

                }
            }
            circle.SetData(colors);
            Assets.Add("_default_circle", circle);
        }
    }
}
