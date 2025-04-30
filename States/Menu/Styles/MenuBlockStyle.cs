namespace TarLib.States {

    public struct MenuBlockStyle {
        public MenuBlockStyleRule Default { get; set; }
        public MenuBlockStyleRule Select { get; set; }
        public MenuBlockStyleRule Hover { get; set; }
        public MenuBlockStyleRule Activate { get; set; }
        public MenuBlockStyleRule Disabled { get; set; }
        public MenuBlockStyleRule Error { get; set; }

        public static implicit operator MenuBlockStyle(MenuBlockStyleRule rule) {
            return new MenuBlockStyle() {
                Default = rule
            };
        }

        public static MenuBlockStyle operator +(MenuBlockStyle left, MenuBlockStyle right) {
            return new MenuBlockStyle() {
                Default = left.Default + right.Default,
                Hover = left.Hover + right.Hover,
                Activate = left.Activate + right.Activate,
                Disabled = left.Disabled + right.Disabled,
                Select = left.Select + right.Select,
                Error = left.Error + right.Error
            };
        }
    }
}
