using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using TarLib.Assets;
using TarLib.Assets.Converters;
using TarLib.Entities;
using TarLib.Input;
using TarLib.States;

namespace TarLib {
    public abstract class TarGame : Game {
        protected GraphicsDeviceManager GraphicsDeviceManager { get; }

        private readonly object objectLock = new object();
        private readonly Random Random;

        private readonly ObservableVariable<IGameState> state = new ObservableVariable<IGameState>();
        public GameTime CurrentGameTime { get; private set; }

        public IGameState State {
            get => state.Value;
            set => state.Value = value;
        }
        public event EventHandler<(IGameState oldState, IGameState newState)> OnStateChange {
            add {
                lock(objectLock) {
                    state.OnChange += value;
                }
            }
            remove {
                lock (objectLock) {
                    state.OnChange -= value;
                }
            }
        }

        // TODO: Possibly move all assets into an asset manager manager 
        public TextureAssetManager Textures { get; }
        public FontAssetManager Fonts { get; }
        public EffectAssetManager Effects { get; }
        public InputManager Input { get; }
        public MenuBlockStylesManager MenuStyles { get; }

        public JsonSerializerOptions DefaultJsonOptions { get; }

        public Rectangle ScreenDimensions => GraphicsDevice.Viewport.Bounds;
        public Vector2 ScreenCenter => new Vector2(ScreenDimensions.Width / 2, ScreenDimensions.Height / 2);
        public Rectangle SafeDimensions => ScreenDimensions; // TODO: reduce by safe const
        public Rectangle OverscanDimensions => ScreenDimensions; // TODO: increase by overscan const

        protected virtual Color DefaultBackgroundColor => Color.Black;

        public TarGame() {
            GraphicsDeviceManager = new GraphicsDeviceManager(this);
            Textures = new TextureAssetManager(this);
            Fonts = new FontAssetManager(this);
            Effects = new EffectAssetManager(this);
            Input = new InputManager(this);
            
            MenuStyles = new MenuBlockStylesManager(this);

            DefaultJsonOptions = new JsonSerializerOptions();
            DefaultJsonOptions.Converters.Add(new Vector2JsonConverter());
            DefaultJsonOptions.Converters.Add(new RectanglePrimitiveJsonConverter());
            DefaultJsonOptions.Converters.Add(new RectangleGameZoneJsonConverter());
            DefaultJsonOptions.Converters.Add(new JsonStringEnumConverter());

            Random = new Random();

            Content.RootDirectory = "Content";

            state.OnChange += State_OnChange;
        }

        private void State_OnChange(object sender, (IGameState oldValue, IGameState newValue) e) {
            e.oldValue?.UnloadContent();
            e.newValue?.LoadContent();
        }

        public TarLibSeed GetRandomSeed() {
            return new TarLibSeed(Random.Next());
        }

        protected override void LoadContent() {
            base.LoadContent();

            GraphicsDevice.RasterizerState = new RasterizerState() { ScissorTestEnable = true };

            Fonts.LoadContent();
            Textures.LoadContent();
            Effects.LoadContent();

            Textures.Add("_default_up_arrow", "Content//Textures//up.png");
            Textures.Add("_default_down_arrow", "Content//Textures//down.png");
            Textures.Add("_default_play_button", "Content//Textures//play_button.png");
            Textures.Add("_default_pause_button", "Content//Textures//pause_button.png");
        }

        protected override void UnloadContent() {
            base.UnloadContent();
            Fonts.UnloadContent();
            Textures.UnloadContent();
            Effects.UnloadContent();
        }

        protected sealed override void Update(GameTime gameTime) {
            base.Update(gameTime);
            CurrentGameTime = gameTime;
            Input.Update();
            State?.Update(gameTime);
        }

        protected sealed override void Draw(GameTime gameTime) {
            base.Draw(gameTime);
            GraphicsDevice.Clear(DefaultBackgroundColor);
            State?.Draw(gameTime);
        }
    }
}
