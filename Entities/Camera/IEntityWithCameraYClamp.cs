namespace TarLib.Entities {
    public interface IEntityWithCameraYClamp : IEntityWithCamera {
        float CameraYMin { get; }
        float CameraYMax { get; }
    }
}
