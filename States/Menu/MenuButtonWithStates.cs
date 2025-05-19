using System.Collections.Generic;

namespace TarLib.States {
    public abstract class MenuButtonWithStates<TState, TButtonLabel> : MenuButton
        where TButtonLabel : IMenuBlock {

        private Dictionary<TState, TButtonLabel> labels = new();

        public abstract TState State { get; }
        public abstract TButtonLabel DefaultLabel { get; }
        public TButtonLabel Label => labels.ContainsKey(State) ? labels[State] : DefaultLabel;

        public MenuButtonWithStates(IGameMenu menu = default) : base(menu) {

        }
    }
}
