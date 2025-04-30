namespace TarLib.Entities {
    public interface IEntityWithCameraXWrap : IEntityWithCamera {
        float CameraXMin { get; }
        float CameraXMax { get; }
    }
}
