using System;
using System.Linq;

namespace TarLib.States {
    public class SimpleMenuButtonWithStates<TState> : MenuButtonWithStates<TState, MenuText>
        where TState : Enum {

        private ObservableVariable<TState> state;
        private MenuText buttonLabel;

        public SimpleMenuButtonWithStates(TState defaultState = default, IGameMenu menu = default) : base(menu) {
            state = new(defaultState);
            foreach(var name in Enum.GetNames(typeof(TState))) {
                object parsed;
                if(Enum.TryParse(typeof(TState), name, out parsed) && parsed is TState enumValue) {
                    labels.Add(enumValue, new MenuText(enumValue.ToString(), false, menu));
                    labels[enumValue].IsVisible = enumValue.Equals(defaultState);
                }
            }
            if(labels.Count > 0) {
                buttonLabel = labels.First().Value;
            }
        }

        public override TState State => state;
        public override MenuText DefaultLabel => buttonLabel;

        public void SetState(TState state) {
            this.state.Value = state;
            labels.Where(kvp => !kvp.Key.Equals(state)).Select(kvp => kvp.Value).ToList().ForEach(label => label.IsVisible = false);
            labels[state].IsVisible = true;
        }
    }
}
