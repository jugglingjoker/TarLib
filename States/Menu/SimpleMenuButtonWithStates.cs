using System;

namespace TarLib.States {
    public class SimpleMenuButtonWithStates<TState> : MenuButtonWithStates<TState, SimpleMenuButtonWithStates<TState>.ButtonLabel>
        where TState : Enum {

        public override TState State => throw new NotImplementedException();
        public override ButtonLabel DefaultLabel => throw new NotImplementedException();

        public class ButtonLabel : MenuText<ButtonLabel.TextSource> {
            private SimpleMenuButtonWithStates<TState> button;

            public ButtonLabel(SimpleMenuButtonWithStates<TState> button, IGameMenu menu = null) : base(source: new TextSource(), false, menu) {
                this.button = button;
                Source.ButtonLabel = this;
            }

            public class TextSource : IMenuTextSource {
                public ButtonLabel ButtonLabel { get; set; }
                public string Text => ButtonLabel?.button?.State.ToString() ?? "";
            }
        }
    }
}
