namespace TarLib.States {

    public class MenuNumberInputContainer : MenuInputContainer<MenuNumberInput> {

        protected override MenuBlockStyleTypeList StyleTypes => base.StyleTypes + MenuBlockStyleType.NumberInputContainer;

        public MenuNumberInputContainer(
            float initialValue,
            string label = default,
            IGameMenu menu = null) : base(label: label, inputBlock: new MenuNumberInput(
                initialValue: initialValue,
                menu: menu), menu: menu) {
        }
    }
}
