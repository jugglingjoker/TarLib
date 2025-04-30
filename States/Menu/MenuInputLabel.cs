    namespace TarLib.States {
    public class MenuInputLabel : MenuText {
        public MenuInputLabel(string text, IGameMenu menu = null) : base(text: text, menu: menu) {
            DefaultStyle = new MenuBlockStyleRule() {
                TextAfter = ":",
            } + DefaultStyle;
        }

        protected override MenuBlockStyleTypeList StyleTypes => MenuBlockStyleType.InputLabel + base.StyleTypes;
    }
}
