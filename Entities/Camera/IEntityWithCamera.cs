using Microsoft.Xna.Framework;

namespace TarLib.Entities {
    public interface IEntityWithCamera {
        Vector2 CameraCenter { get; }
        Vector2 CameraTarget { get; }
        float CameraAcceleration { get; }
        float CameraMaximumSpeed { get; }
        float CameraMinimumZoom { get; }
        float CameraMaximumZoom { get; }
        float CameraTilt { get; }
        float CameraPixelsPerUnit { get; }
    }
}
