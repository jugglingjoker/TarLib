using Microsoft.Xna.Framework;
using TarLib.Input;

namespace TarLib.States {

    public abstract class MenuButton : MenuContainer {
        public MenuButton(IGameMenu menu = default) : base(menu: menu) {
            OnClickStart += MenuButton_OnClickStart;
            OnClickEnd += MenuButton_OnClickEnd;

            DefaultStyle = new MenuBlockStyle() {
                Default = new MenuBlockStyleRule() {
                    Padding = (top: 8, left: 15, bottom: 2, right: 15),
                    Margin = 3,
                    BackgroundColor = new Color(218, 164, 2),
                    BorderSize = (2, 2, 4, 2),
                    BorderColor = new Color(185, 139, 1),
                },
                Hover = new MenuBlockStyleRule() {
                    BackgroundColor = new Color(250, 193, 48),
                    BorderColor = new Color(208, 161, 40)
                },
                Activate = new MenuBlockStyleRule() {
                    Padding = (top: 10, left: 15, bottom: 0, right: 15),
                    BorderSize = (4, 2, 2, 2),
                },
                Disabled = new MenuBlockStyleRule() {
                    Margin = 3,
                    Padding = (top: 8, left: 15, bottom: 2, right: 15),
                    BackgroundColor = Color.LightGray,
                    BorderColor = new Color(180, 180, 180)
                }
            };
        }

        protected override MenuBlockStyleTypeList StyleTypes => MenuBlockStyleType.Button;

        private void MenuButton_OnClickStart(object sender, MouseClickEventArgs e) {
            if(!IsDisabled) {
                e.FlagAsUsed();
            }
        }

        private void MenuButton_OnClickEnd(object sender, MouseClickEventArgs e) {
            if (!IsDisabled) {
                e.FlagAsUsed();
            }
        }
    }
}
