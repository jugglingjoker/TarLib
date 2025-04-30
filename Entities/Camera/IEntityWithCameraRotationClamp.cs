namespace TarLib.Entities {
    public interface IEntityWithCameraRotationClamp : IEntityWithCamera {
        float CameraRotationMin { get; }
        float CameraRotationMax { get; }
    }
}
