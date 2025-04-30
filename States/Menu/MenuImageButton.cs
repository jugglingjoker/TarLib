namespace TarLib.States {
    public class MenuImageButton : MenuButton { 
        public MenuImageButton(
            string textureId,
            IGameMenu menu = null) : base(menu) {
            Image = new ButtonImage(textureId, menu);
            Add(Image);
        }


        protected override MenuBlockStyleTypeList StyleTypes => base.StyleTypes + MenuBlockStyleType.ImageButton;

        public ButtonImage Image { get; }

        public class ButtonImage : MenuImage {
            public ButtonImage(string textureId, IGameMenu menu = null) : base(textureId, menu) {

            }

            protected override MenuBlockStyleTypeList StyleTypes => base.StyleTypes + MenuBlockStyleType.ImageButtonImage;
        }
    }
}
