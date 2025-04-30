namespace TarLib.States {

    public class MenuWindowContents : MenuWindowContents<IMenuContainer> {

    }

    public class MenuWindowContents<TMenuBlock> : MenuContainer<TMenuBlock>, IMenuWindowContents
        where TMenuBlock : IMenuBlock {
        public MenuWindowContents() : base() {
            DefaultStyle = new MenuBlockStyleRule() {
                WidthStyle = SizeStyle.FitParent,
                ContentDirection = ContentDirectionStyle.Column,
                ContentVerticalAlign = AlignStyle.Center,
                ContentHorizontalAlign = AlignStyle.Center,
                Padding = 5
            } + DefaultStyle;
        }

        protected override MenuBlockStyleTypeList StyleTypes => MenuBlockStyleType.WindowContents;
        public IMenuWindow Window { get; set; }
    }
}
