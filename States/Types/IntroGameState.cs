using System.Collections.Generic;

namespace TarLib.States {
    public abstract class IntroGameState<TTarGame> : SimpleGameState<TTarGame>
        where TTarGame : TarGame {

        protected IntroGameState(TTarGame game) : base(game) {

        }

        public override List<IGameStateViewLayer> InitLayers() {
            return new List<IGameStateViewLayer>();
        }
    }
}
