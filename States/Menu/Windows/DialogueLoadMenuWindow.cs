using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TarLib.Input;

namespace TarLib.States {
    public class AttemptLoadEventArgs : EventArgs {
        public AttemptLoadEventArgs(string label, string filename) {
            Label = label;
            Filename = filename;
        }

        public string Label { get; }
        public string Filename { get; }
    }

    public abstract class DialogueLoadMenuWindow : MenuWindow {
        public DialogueLoadMenuWindow(
            string id,
            IGameMenu menu = null,
            List<MenuBlock> menuBlocks = null,
            AlignStyle? screenVerticalAlign = null,
            AlignStyle? screenHorizontalAlign = null,
            Vector2? exactPosition = null,
            string title = null,
            bool hasCloseButton = false,
            bool canMovePosition = false,
            bool rememberPosition = false,
            bool alwaysOnTop = false) : base(id: id, menu: menu, menuBlocks: menuBlocks, screenVerticalAlign: screenVerticalAlign, screenHorizontalAlign: screenHorizontalAlign, exactPosition: exactPosition, title: title, hasCloseButton: hasCloseButton, canMovePosition: canMovePosition, rememberPosition: rememberPosition, alwaysOnTop: alwaysOnTop, captureInput: true) {

            Files = new FileList(this, menu);
            Add(Files);

            Buttons = new ButtonContainer(this, menu);
            LoadButton = new MenuTextButton("Load");
            LoadButton.OnClickEnd += LoadButton_OnClickEnd;
            CancelButton = new MenuTextButton("Cancel");
            CancelButton.OnClickEnd += CancelButton_OnClickEnd;
            Buttons.Add(LoadButton);
            Buttons.Add(CancelButton);
            Add(Buttons);

            if(Files.SelectedValue == null) {
                LoadButton.IsDisabled = true;
            }
            Files.OnChange += Files_OnChange;

            OnBeforeAddToView += DialogueLoadMenuWindow_OnBeforeAddToView;
        }

        private void DialogueLoadMenuWindow_OnBeforeAddToView(object sender, EventArgs e) {
            Files.SetValues(GetFileList(), Files.SelectedValue);
        }

        private void Files_OnChange(object sender, (string oldValue, string newValue) e) {
            LoadButton.IsDisabled = Files.SelectedValue == null;
        }

        public FileList Files { get; }
        public ButtonContainer Buttons { get; }
        public MenuButton LoadButton { get; }
        public MenuButton CancelButton { get; }

        public event EventHandler<AttemptLoadEventArgs> OnAttemptLoad;
        public event EventHandler<MouseClickEventArgs> OnCancel;

        private void LoadButton_OnClickEnd(object sender, MouseClickEventArgs e) {
            OnAttemptLoad?.Invoke(this, new AttemptLoadEventArgs(
                label: Files.SelectedButton.Label.Text,
                filename: Files.SelectedValue)
            );
        }

        private void CancelButton_OnClickEnd(object sender, MouseClickEventArgs e) {
            OnCancel?.Invoke(this, e);
        }

        public abstract Dictionary<string, string> GetFileList();

        public class FileList : MenuRadioButtonContainer<string> {
            public FileList(
                DialogueLoadMenuWindow window,
                IGameMenu menu = null) : base(menu) {
                Window = window;
                SetValues(window.GetFileList(), default);

                DefaultStyle = new MenuBlockStyleRule() {
                    WidthStyle = SizeStyle.Exact,
                    Width = 400,
                    HeightStyle = SizeStyle.Exact,
                    Height = 250,
                    BorderSize = 2,
                    BorderTop = 4,
                    BorderColor = Color.Black,
                    BackgroundColor = Color.White,
                    VerticalScroll = ScrollStyle.AlwaysShow
                } + DefaultStyle;
            }

            public DialogueLoadMenuWindow Window { get; }
            protected override MenuBlockStyleTypeList StyleTypes => MenuBlockStyleType.FileList;
        }

        public class ButtonContainer : MenuContainer<MenuButton> {
            
            public ButtonContainer(
                DialogueLoadMenuWindow window,
                IGameMenu menu = null) : base(menu) {
                Window = window;

                DefaultStyle = new MenuBlockStyleRule() {
                    ContentDirection = ContentDirectionStyle.Row,
                    Margin = (top: 5, left: 0, bottom: 0, right: 0),
                };
            }

            public DialogueLoadMenuWindow Window { get; }
            protected override MenuBlockStyleTypeList StyleTypes => new(MenuBlockStyleType.DialogueWindowButtons, MenuBlockStyleType.DialogueLoadWindowButtons);
        }
    }
}
