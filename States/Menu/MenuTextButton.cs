using Microsoft.Xna.Framework;

namespace TarLib.States {
    public class MenuTextButton : MenuButton {
        public MenuTextButton(
            string text,
            bool canWrapText = false,
            IGameMenu menu = null) : base(menu) {
            this.text = new ButtonText(text, canWrapText, menu);
            Add(this.text);
        }

        protected override MenuBlockStyleTypeList StyleTypes => base.StyleTypes + MenuBlockStyleType.TextButton;

        private ButtonText text;
        public string Text {
            get => text.Source.Text;
            set => text.Source.Text = value;
        }

        public class ButtonText : MenuText {
            public ButtonText(string text, bool canWrapText = false, IGameMenu menu = null) : base(text, canWrapText, menu) {
                DefaultStyle = new MenuBlockStyleRule() {
                    FontColor = Color.White,
                } + DefaultStyle;
            }

            protected override MenuBlockStyleTypeList StyleTypes => base.StyleTypes + MenuBlockStyleType.TextButtonText;
        }
    }
}
