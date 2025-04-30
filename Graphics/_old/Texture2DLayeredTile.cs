using Microsoft.Xna.Framework.Graphics;

namespace TarLib.Graphics {
    public class Texture2DLayeredTile {
        // Whether the adjacent tile at the given location is the same kind
        // of tile
        public bool TopLeftMatch { get; set; }
        public bool TopMatch { get; set; }
        public bool TopRightMatch { get; set; }
        public bool LeftMatch { get; set; }
        public bool RightMatch { get; set; }
        public bool BottomLeftMatch { get; set; }
        public bool BottomMatch { get; set; }
        public bool BottomRightMatch { get; set; }

        public Texture2D BaseTexture { get; }
        public int LayersCount { get; }
        public Texture2DLayeredTileType Type { get; }

        public int Width => Type == Texture2DLayeredTileType.Texture ? BaseTexture.Width : BaseTexture.Width / 6;
        public int Height => Type == Texture2DLayeredTileType.Texture ? BaseTexture.Height : BaseTexture.Height / (4 * LayersCount);

        public Texture2DLayeredTile(Texture2D baseTexture, Texture2DLayeredTileType type = default, int layersCount = 1) {
            // TODO: Throw exception if layers count is less than 1,
            // or if the count of layers is not a perfect factor of the
            // texture height
            BaseTexture = baseTexture;
            LayersCount = layersCount;
            Type = type;
        }

        public static implicit operator Texture2DLayeredTile(Texture2D texture) {
            return new Texture2DLayeredTile(
                baseTexture: texture,
                type: Texture2DLayeredTileType.Texture,
                layersCount: 1);
        }
    }

    public enum Texture2DLayeredTileType {
        Texture = 0,
        Tile2x2
    }
}
