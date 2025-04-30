namespace TarLib.Entities {
    public interface IEntityWithCameraXClamp : IEntityWithCamera {
        float CameraXMin { get; }
        float CameraXMax { get; }
    }
}
