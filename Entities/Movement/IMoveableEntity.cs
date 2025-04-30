using Microsoft.Xna.Framework;

namespace TarLib.Entities.Movement {
    // TODO: A lot more work to come
    public interface IMoveableEntity {
        public Vector2 Position { get; }
        public Vector2 Velocity { get; }
        public float MaximumSpeed { get; }
    }
}
