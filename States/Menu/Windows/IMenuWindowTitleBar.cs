namespace TarLib.States {
    public interface IMenuWindowTitleBar : IMenuContainer {
        IMenuWindow Window { get; set; }
        bool HasCloseButton { get; set; }
        string Title { get; set; }
    }
}
