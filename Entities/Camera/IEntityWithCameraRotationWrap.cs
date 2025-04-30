namespace TarLib.Entities {
    public interface IEntityWithCameraRotationWrap : IEntityWithCamera {
        float CameraRotationMin { get; }
        float CameraRotationMax { get; }
    }
}
