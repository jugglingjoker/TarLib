using System;
using System.Linq;

namespace TarLib.States {
    public class SimpleMenuButtonWithStates<TState> : MenuButtonWithStates<TState, SimpleMenuButtonWithStates<TState>.ButtonLabel>
        where TState : Enum {

        private ObservableVariable<TState> state;
        private ButtonLabel buttonLabel;

        public SimpleMenuButtonWithStates(TState defaultState = default, IGameMenu menu = default) : base(menu) {
            state = new(defaultState);
            
            foreach(var name in Enum.GetNames(typeof(TState))) {
                object parsed;
                if(Enum.TryParse(typeof(TState), name, out parsed) && parsed is TState enumValue) {
                    labels.Add(enumValue, new ButtonLabel(enumValue, menu));
                    labels[enumValue].IsVisible = enumValue.Equals(defaultState);
                }
            }
        }

        public override TState State => state;
        public override ButtonLabel DefaultLabel => buttonLabel;

        public void SetState(TState state) {
            this.state.Value = state;
            labels.Where(kvp => !kvp.Key.Equals(state)).Select(kvp => kvp.Value).ToList().ForEach(label => label.IsVisible = false);
            labels[state].IsVisible = true;
        }

        public class ButtonLabel : MenuText {
            public ButtonLabel(TState state, IGameMenu menu = null) : base(text: state.ToString(), false, menu) {
                
            }
        }
    }
}
