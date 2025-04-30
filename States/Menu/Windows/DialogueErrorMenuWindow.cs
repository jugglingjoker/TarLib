using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TarLib.Input;

namespace TarLib.States {

    public class DialogueErrorMenuWindow : DialogueConfirmMenuWindow {
        public DialogueErrorMenuWindow(
            string id,
            string dialogueText,
            string confirmLabel = "Ok",
            IGameMenu menu = null,
            string title = "Error",
            bool hasCloseButton = false,
            bool canMovePosition = false,
            bool rememberPosition = false) : base(id, dialogueText, confirmLabel, false, default, menu, title, hasCloseButton, canMovePosition, rememberPosition) {

            OnConfirm += DialogueErrorMenuWindow_OnConfirm;
        }

        private void DialogueErrorMenuWindow_OnConfirm(object sender, DialogueConfirmEventArgs e) {
            Menu.State.Menu.Remove(this);
        }
    }
}
