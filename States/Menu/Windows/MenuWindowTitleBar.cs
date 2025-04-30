using Microsoft.Xna.Framework;
using TarLib.Input;

namespace TarLib.States {
    public class MenuWindowTitleBar : MenuContainer, IMenuWindowTitleBar {
        private bool hasCloseButton;

        public MenuWindowTitleBar() : base() {
            Label = new TitleBarLabel(this);
            Add(Label);

            CloseButton = new TitleBarCloseButton(this);
            CloseButton.OnClickEnd += CloseButton_OnClickEnd;
            Add(CloseButton);

            DefaultStyle = new MenuBlockStyleRule() {
                ContentDirection = ContentDirectionStyle.Row,
                BackgroundColor = Color.Black,
                ContentVerticalAlign = AlignStyle.Center,
                ContentHorizontalAlign = AlignStyle.SpaceBetween,
                WidthStyle = SizeStyle.FitParent,
                Padding = 5
            };
        }

        private void CloseButton_OnClickEnd(object sender, MouseClickEventArgs e) {
            Window.Close();
        }

        protected override MenuBlockStyleTypeList StyleTypes => MenuBlockStyleType.WindowTitleBar;
        public IMenuWindow Window { get; set; }

        public bool HasCloseButton {
            get => hasCloseButton;
            set {
                hasCloseButton = value;
                CloseButton.IsVisible = value;
            }
        }

        public string Title {
            get => Label.Source.Text;
            set => Label.Source.Text = value;
        }

        public TitleBarLabel Label { get; }
        public TitleBarCloseButton CloseButton { get; }

        public class TitleBarLabel : MenuText {
            public TitleBarLabel(MenuWindowTitleBar titleBar) : base(text: "") {
                TitleBar = titleBar;

                DefaultStyle = new MenuBlockStyleRule() {
                    Padding = (top: 8, left: 10, bottom: 2, right: 10),
                    FontColor = Color.White,
                    BackgroundColor = Color.Transparent
                } + DefaultStyle;
            }

            protected override MenuBlockStyleTypeList StyleTypes => base.StyleTypes + MenuBlockStyleType.WindowTitleBarLabel;
            public MenuWindowTitleBar TitleBar { get; }
        }

        public class TitleBarCloseButton : MenuTextButton {
            public TitleBarCloseButton(MenuWindowTitleBar titleBar) : base("x", false) {
                DefaultStyle = new MenuBlockStyle() {
                    Default = new MenuBlockStyleRule() {
                        Padding = (top: 0, left: 5, bottom: -2, right: 5),
                        BorderSize = (1, 1, 3, 1),
                        Margin = 5
                    },
                    Activate = new MenuBlockStyleRule() {
                        Padding = (top: 0, left: 5, bottom: -2, right: 5),
                        BorderSize = (3, 1, 1, 1),
                    }
                } + DefaultStyle;
            }

            protected override MenuBlockStyleTypeList StyleTypes => base.StyleTypes + MenuBlockStyleType.WindowTitleBarCloseButton;
            public MenuWindowTitleBar TitleBar { get; }
        }
    }
}
