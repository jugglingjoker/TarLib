using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;

namespace TarLib.Assets {
    public class TextureAssetManager : AssetManager<Texture2D> {
        protected const uint DEFAULT_TEXTURE_WIDTH = 1;
        protected const uint DEFAULT_TEXTURE_HEIGHT = 1;

        private List<Texture2D> extraTextures = new();

        public TextureAssetManager(TarGame game) : base(game) {

        }

        public Texture2D Circle { get; private set; }

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

            Circle = new Texture2D(Game.GraphicsDevice, 1024, 1024);
            var colors = new Color[Circle.Width * Circle.Height];
            var center = new Vector2(Circle.Width / 2, Circle.Height / 2);
            for (int i = 0; i < Circle.Width; i++) {
                for (int j = 0; j < Circle.Height; j++) {

                    var dist = Vector2.Distance(center, new Vector2(i, j));

                    if(dist < Circle.Width / 2) {
                        colors[i * Circle.Width + j] = Color.White;
                    } else {
                        colors[i * Circle.Width + j] = Color.Transparent;
                    }
                    
                }
            }
            Circle.SetData(colors);
            extraTextures.Add(Circle);
        }

        public override void UnloadDefault() {
            foreach(var texture in extraTextures) {
                texture.Dispose();
            }
        }

        public override void UnloadContent() {
            base.UnloadContent();
            foreach (var asset in Assets) {
                asset.Value.Dispose();
            }
        }
    }
}
