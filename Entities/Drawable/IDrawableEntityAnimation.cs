namespace TarLib.Entities.Drawable {
    public interface IDrawableEntityAnimation {
        int FrameX { get; }
        int FrameY { get; }

        void Update(float elapsedTime);
        void Reset();
    }
}
