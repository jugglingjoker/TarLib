using System.Collections.Generic;

namespace TarLib.Assets {
    public abstract class AssetManager<TAsset> {
        protected Dictionary<string, TAsset> Assets { get; } = new Dictionary<string, TAsset>();
        protected TarGame Game { get; }

        // TODO: Throw exception if asset not found and in STRICT mode?
        public TAsset this[string key] => ContainsKey(key) ? Assets[key] : Default;
        public bool ContainsKey(string key) => Assets.ContainsKey(key);
        public TAsset Default { get; protected set; }

        public AssetManager(TarGame game) {
            Game = game;
        }

        public void Add(string id, string filename) {
            TAsset asset = GetAssetByFilename(filename);
            if (asset != null) {
                if(!Assets.ContainsKey(id)) {
                    Assets.Add(id, asset);
                } else {
                    Assets[id] = asset;
                }
            } else {
                // TODO: Throw exception if asset not found?
            }
        }

        protected abstract TAsset GetAssetByFilename(string filename);

        public virtual void LoadContent() {
            Default = LoadDefault();
        }

        public abstract TAsset LoadDefault();

        public virtual void UnloadContent() {
            UnloadDefault();
        }

        public abstract void UnloadDefault();
    }
}
