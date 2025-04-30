namespace TarLib.Entities {
    public interface IEntityWithCameraYWrap : IEntityWithCamera {
        float CameraYMin { get; }
        float CameraYMax { get; }
    }
}
