using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TarLib.Input;

namespace TarLib.States {

    public abstract class GameState<TTarGame> : GameState<TTarGame, IGameStateView>
        where TTarGame : TarGame {

        public GameState(TTarGame game) : base(game) {

        }
    }

    public abstract class GameState<TTarGame, TTarGameView> : IGameState
        where TTarGame : TarGame
        where TTarGameView : IGameStateView {

        protected readonly object objectLock = new();

        public TarGame BaseGame => Game;
        public TTarGame Game { get; }

        public IGameMenu Menu { get; }
        protected SpriteBatch SpriteBatch { get; private set; }

        private readonly ObservableVariable<TTarGameView> view = new();
        public virtual TTarGameView View {
            get => view.Value;
            set => view.Value = value;
        }
        public event EventHandler<(TTarGameView oldValue, TTarGameView newValue)> OnViewChange {
            add {
                lock (objectLock) {
                    view.OnChange += value;
                }
            }
            remove {
                lock (objectLock) {
                    view.OnChange -= value;
                }
            }
        }

        public event EventHandler<MouseMoveEventArgs> OnMouseMove;
        public event EventHandler<MouseClickEventArgs> OnMouseClickStart;
        public event EventHandler<MouseClickEventArgs> OnMouseClickEnd;
        public event EventHandler<MouseScrollWheelChangeEventArgs> OnMouseScrollWheelChange;
        public event EventHandler<KeyPressEventArgs> OnKeyPressStart;
        public event EventHandler<KeyPressEventArgs> OnKeyPressEnd;

        public GameState(TTarGame game) {
            Game = game;
            Menu = new GameMenu(this);

            OnViewChange += GameState_OnViewChange;
        }

        private void GameState_OnViewChange(object sender, (TTarGameView oldValue, TTarGameView newValue) e) {
            e.oldValue?.End();
            e.newValue?.Start();
        }

        public virtual void LoadContent() {
            SpriteBatch = new SpriteBatch(Game.GraphicsDevice);
        }

        public virtual void UnloadContent() {

        }

        public virtual void Update(GameTime gameTime) {
            if (Game.Input.IsMouseMoved) {
                MouseMove((Game.Input.MousePosition, gameTime));
            }
            if (Game.Input.IsMouseScrollWheelMoved) {
                MouseScrollWheelChange((Game.Input.MousePosition, Game.Input.MouseScrollWheelChange, gameTime));
            }
            if(Game.Input.StartingMouseInputsDown.Count > 0) {
                foreach(var mouseButton in Game.Input.StartingMouseInputsDown) {
                    MouseClickStart((Game.Input.MousePosition, mouseButton, gameTime));
                }
            }
            if (Game.Input.EndingMouseInputsDown.Count > 0) {
                foreach (var mouseButton in Game.Input.EndingMouseInputsDown) {
                    MouseClickEnd((Game.Input.MousePosition, mouseButton, gameTime));
                }
            }
            if (Game.Input.StartingKeysDown.Count > 0) {
                KeyPressStart((Game.Input.StartingKeysDown, gameTime));
            }
            if (Game.Input.EndingKeysDown.Count > 0) { 
                KeyPressEnd((Game.Input.EndingKeysDown, gameTime));
            }

            View?.Update(gameTime);
            Menu?.Update(gameTime);
        }

        public virtual void Draw(GameTime gameTime) {
            SpriteBatch.Begin(
                sortMode: SpriteSortMode.FrontToBack,
                blendState: BlendState.AlphaBlend,
                samplerState: SamplerState.LinearWrap,
                rasterizerState: SpriteBatch.GraphicsDevice.RasterizerState);
            View?.Draw(gameTime, 0, 0.49f);
            Menu?.Draw(gameTime, 0.5f, 1f);
            SpriteBatch.End();
        }

        public void MouseMove(MouseMoveEventArgs e) {
            Menu.MouseMove(e);
            if(Menu.ExclusiveWindow == null) {
                OnMouseMove?.Invoke(this, e);
                View?.MouseMove(e);
            }
        }

        public void MouseClickStart(MouseClickEventArgs e) {
            Menu.MouseClickStart(e);
            if (Menu.ExclusiveWindow == null && e.IsAvailable) {
                OnMouseClickStart?.Invoke(this, e);
                if(e.IsAvailable) { 
                    View?.MouseClickStart(e);
                }
            }
        }

        public void MouseClickEnd(MouseClickEventArgs e) {
            Menu.MouseClickEnd(e);
            if (Menu.ExclusiveWindow == null && e.IsAvailable) {
                OnMouseClickEnd?.Invoke(this, e);
                if (e.IsAvailable) {
                    View?.MouseClickEnd(e);
                }
            }
        }

        public void MouseScrollWheelChange(MouseScrollWheelChangeEventArgs e) {
            Menu.MouseScrollWheelChange(e);
            if(Menu.ExclusiveWindow == null) {
                OnMouseScrollWheelChange?.Invoke(this, e);
                View?.MouseScrollWheelChange(e);
            }
        }

        public void KeyPressStart(KeyPressEventArgs e) {
            Menu.KeyPressStart(e);
            if (Menu.ExclusiveWindow == null && e.AvailableKeys.Count > 0) {
                OnKeyPressStart?.Invoke(this, e);
                if (e.AvailableKeys.Count > 0) {
                    View?.KeyPressStart(e);
                }
            }
        }

        public void KeyPressEnd(KeyPressEventArgs e) {
            Menu.KeyPressEnd(e);
            if (Menu.ExclusiveWindow == null && e.AvailableKeys.Count > 0) {
                OnKeyPressEnd?.Invoke(this, e);
                if (e.AvailableKeys.Count > 0) {
                    View?.KeyPressEnd(e);
                }
            }
        }
    }
}
