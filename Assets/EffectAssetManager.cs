using Microsoft.Xna.Framework.Graphics;

namespace TarLib.Assets {
    public class EffectAssetManager : AssetManager<Effect> {
        public EffectAssetManager(TarGame game) : base(game) {

        }

        public override Effect LoadDefault() {
            return new BasicEffect(Game.GraphicsDevice);
        }

        protected override Effect GetAssetByFilename(string filename) {
            return Game.Content.Load<Effect>(filename);
        }

        public override void UnloadDefault() {
            // Default asset disposed by content manager
        }
    }
}
