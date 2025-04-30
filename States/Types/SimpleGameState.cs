using System.Collections.Generic;

namespace TarLib.States {
    public abstract class SimpleGameState<TTarGame> : GameState<TTarGame>
        where TTarGame : TarGame {
        private SimpleGameStateView SimpleView { get; set; }
        public override IGameStateView View => SimpleView;

        public SimpleGameState(TTarGame game) : base(game) {

        }

        public override void LoadContent() {
            base.LoadContent();
            SimpleView = new SimpleGameStateView(this);
            SimpleView.LoadContent();
        }

        public override void UnloadContent() {
            base.UnloadContent();
            SimpleView.UnloadContent();
        }

        public abstract List<IGameStateViewLayer> InitLayers();

        protected class SimpleGameStateView : GameStateView<SimpleGameState<TTarGame>> {
            public SimpleGameStateView(SimpleGameState<TTarGame> state) : base(state) {

            }

            protected override List<IGameStateViewLayer> InitLayers() {
                return State.InitLayers();
            }
        }

        protected abstract class SimpleGameStateViewLayer<TGameState> : GameStateViewLayer<SimpleGameStateView, SimpleGameState<TTarGame>>
            where TGameState : SimpleGameState<TTarGame> {
            public SimpleGameStateViewLayer(TGameState state) : base(state.SimpleView, state) {

            }
        }
    }
}
