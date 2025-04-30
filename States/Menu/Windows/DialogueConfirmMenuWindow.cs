using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TarLib.Input;

namespace TarLib.States {

    public class DialogueDenyEventArgs : MouseClickEventArgs {
        public DialogueDenyEventArgs(Point mousePosition, MouseButton mouseButton, GameTime gameTime) : base(mousePosition, mouseButton, gameTime) {

        }

        public DialogueDenyEventArgs(MouseClickEventArgs mouseClick) : base(mouseClick.MousePosition, mouseClick.MouseButton, mouseClick.GameTime) {

        }
    }

    public class DialogueConfirmEventArgs : MouseClickEventArgs {
        public DialogueConfirmEventArgs(Point mousePosition, MouseButton mouseButton, GameTime gameTime) : base(mousePosition, mouseButton, gameTime) {

        }

        public DialogueConfirmEventArgs(MouseClickEventArgs mouseClick) : base(mouseClick.MousePosition, mouseClick.MouseButton, mouseClick.GameTime) {

        }
    }

    public class DialogueConfirmMenuWindow : MenuWindow {
        public DialogueConfirmMenuWindow(
            string id,
            string dialogueText,
            string confirmLabel = "Yes",
            bool useDenyButton = true,
            string denyLabel = "No",
            IGameMenu menu = null,
            string title = null,
            bool hasCloseButton = false,
            bool canMovePosition = false,
            bool rememberPosition = false) : base(id: id, menu: menu, title: title, hasCloseButton: hasCloseButton, canMovePosition: canMovePosition, rememberPosition: rememberPosition, alwaysOnTop: true, captureInput: true, screenHorizontalAlign: AlignStyle.Center, screenVerticalAlign: AlignStyle.Center) {

            DialogueText = new Text(
                window: this,
                text: dialogueText,
                canWrapText: true,
                menu: menu);
            Add(DialogueText);

            Buttons = new ButtonContainer(window: this, menu: menu);

            ConfirmButton = new MenuTextButton(confirmLabel);
            ConfirmButton.OnClickEnd += ConfirmButton_OnClickEnd;
            Buttons.Add(ConfirmButton);

            if (useDenyButton) {
                DenyButton = new MenuTextButton(denyLabel);
                DenyButton.OnClickEnd += DenyButton_OnClickEnd;
                Buttons.Add(DenyButton);
            }

            Add(Buttons);
        }

        public Text DialogueText { get; }
        public ButtonContainer Buttons { get; }
        public MenuButton ConfirmButton { get; }
        public MenuButton DenyButton { get; }

        public event EventHandler<DialogueConfirmEventArgs> OnConfirm;
        public event EventHandler<DialogueDenyEventArgs> OnDeny;

        protected override MenuBlockStyleTypeList StyleTypes => base.StyleTypes + MenuBlockStyleType.DialogueWindow;

        private void DenyButton_OnClickEnd(object sender, MouseClickEventArgs e) {
            OnDeny?.Invoke(this, new DialogueDenyEventArgs(e));
        }

        private void ConfirmButton_OnClickEnd(object sender, MouseClickEventArgs e) {
            OnConfirm?.Invoke(this, new DialogueConfirmEventArgs(e));
        }

        public class Text : MenuText {
            public Text(DialogueConfirmMenuWindow window, string text, bool canWrapText = true, IGameMenu menu = null) : base(text, canWrapText, menu) {
                Window = window;

                DefaultStyle = new MenuBlockStyleRule() {
                    WidthStyle = SizeStyle.Exact,
                    Width = 350,
                    Padding = 10,
                } + DefaultStyle;
            }

            public DialogueConfirmMenuWindow Window { get; }
            protected override MenuBlockStyleTypeList StyleTypes => MenuBlockStyleType.DialogueWindowText + base.StyleTypes;
        }

        public class ButtonContainer : MenuContainer<MenuButton> {
            
            public ButtonContainer(
                DialogueConfirmMenuWindow window,
                IGameMenu menu = null) : base(menu) {
                Window = window;

                DefaultStyle = new MenuBlockStyleRule() {
                    ContentDirection = ContentDirectionStyle.Row,
                    Margin = (top: 5, left: 0, bottom: 0, right: 0),
                };
            }

            public DialogueConfirmMenuWindow Window { get; }
            protected override MenuBlockStyleTypeList StyleTypes => new(MenuBlockStyleType.DialogueWindowButtons, MenuBlockStyleType.DialogueConfirmWindowButtons);

        }
    }

    
}
