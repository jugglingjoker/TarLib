using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TarLib.Entities.Drawable;
using TarLib.Extensions;
using TarLib.Graphics;

namespace TarLib.States {
    public abstract class GameStateViewLayer<TGameModeView, TGameState> : IGameStateViewLayer
        where TGameModeView : IGameStateView
        where TGameState : IGameState {

        public List<EffectDefinition> Effects { get; protected set; }
        public TGameModeView View { get; }
        public TGameState State { get; }
        protected List<IDrawableEntity> Drawables { get; }

        public GameStateViewLayer(TGameModeView view, TGameState state) {
            View = view;
            State = state;
            Drawables = new List<IDrawableEntity>();
            Effects = new List<EffectDefinition>();
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, float startDepth = 0, float endDepth = 1) {
            Drawables.ForEach(drawable => drawable.Draw(gameTime, spriteBatch, Vector2.Zero, startDepth, endDepth));
        }

        public virtual void LoadContent() {

        }

        public virtual void UnloadContent() {

        }
    }
}
