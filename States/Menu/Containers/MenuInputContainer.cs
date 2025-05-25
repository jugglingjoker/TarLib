namespace TarLib.States {
    public abstract class MenuInputContainer<TInputBlock> : MenuContainerPair<MenuInputLabel, TInputBlock>
        where TInputBlock : IMenuBlock {

        public MenuInputLabel Label => FirstBlock;
        public TInputBlock Input => SecondBlock;

        protected override MenuBlockStyleTypeList StyleTypes => MenuBlockStyleType.InputContainer;

        protected MenuInputContainer(string label, TInputBlock inputBlock, IGameMenu menu = null) : base(new MenuInputLabel(text: label, menu: menu), inputBlock, menu) {
            DefaultStyle = new MenuBlockStyleRule() {
                ContentDirection = ContentDirectionStyle.Row,
                ContentVerticalAlign = AlignStyle.Center,
                ContentHorizontalAlign = AlignStyle.Start
            } + DefaultStyle;
        }
    }
}
