namespace TarLib.States {
    public class MenuTextInput : MenuInput<string> {
        protected override MenuBlockStyleTypeList StyleTypes => base.StyleTypes + MenuBlockStyleType.TextInput;

        public MenuTextInput(string initialValue, IGameMenu menu = null) : base(initialValue, menu) {

        }

        protected override string TranslateInput(string testInput) {
            return testInput;
        }
    }
}
