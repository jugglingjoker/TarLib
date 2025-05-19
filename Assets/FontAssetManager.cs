using Microsoft.Xna.Framework.Graphics;

namespace TarLib.Assets {
    public class FontAssetManager : AssetManager<SpriteFont> {
        public FontAssetManager(TarGame game) : base(game) {

        }

        public override SpriteFont LoadDefault() {
            return GetAssetByFilename("Fonts//Default");
        }

        protected override SpriteFont GetAssetByFilename(string filename) {
            return Game.Content.Load<SpriteFont>(filename);
        }
    }
}