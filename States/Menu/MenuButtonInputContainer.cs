namespace TarLib.States {
    public class MenuButtonInputContainer<TMenuButton> : MenuInputContainer<TMenuButton>
        where TMenuButton : MenuButton {

        public MenuButtonInputContainer(string label, TMenuButton inputBlock, IGameMenu menu = null) : base(label, inputBlock, menu) {

        }
    }
}
