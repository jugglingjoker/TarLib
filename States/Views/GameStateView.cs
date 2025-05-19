using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TarLib.Input;
using System;

namespace TarLib.States {
    public abstract class GameStateView<TGameState> : IGameStateView
        where TGameState : IGameState {
        public TGameState State { get; }
        protected List<IGameStateViewLayer> Layers { get; }
        protected SpriteBatch SpriteBatch { get; set; }
        protected SpriteBatch LayerSpriteBatch { get; set; }

        protected RenderTarget2D LayerBaseTexture { get; set; }
        protected RenderTarget2D LayerEffectTexture { get; set; }

        public GameStateView(TGameState state) {
            State = state;
            Layers = InitLayers() ?? new List<IGameStateViewLayer>();
        }

        public event EventHandler<MouseMoveEventArgs> OnMouseMove;
        public event EventHandler<MouseClickEventArgs> OnMouseClickStart;
        public event EventHandler<MouseClickEventArgs> OnMouseClickEnd;
        public event EventHandler<MouseScrollWheelChangeEventArgs> OnMouseScrollWheelChange;
        public event EventHandler<KeyPressEventArgs> OnKeyPressStart;
        public event EventHandler<KeyPressEventArgs> OnKeyPressEnd;

        protected abstract List<IGameStateViewLayer> InitLayers();

        public virtual void LoadContent() {
            Layers.ForEach(x => x.LoadContent());
            SpriteBatch = new SpriteBatch(State.BaseGame.GraphicsDevice);

            LayerSpriteBatch = new SpriteBatch(State.BaseGame.GraphicsDevice);
            LayerBaseTexture = new RenderTarget2D(State.BaseGame.GraphicsDevice, State.BaseGame.ScreenDimensions.Width, State.BaseGame.ScreenDimensions.Height);
            LayerEffectTexture = new RenderTarget2D(State.BaseGame.GraphicsDevice, State.BaseGame.ScreenDimensions.Width, State.BaseGame.ScreenDimensions.Height);
        }

        public virtual void Start() {

        }

        public virtual void Update(GameTime gameTime) {

        }

        public virtual void End() {

        }

        public virtual void UnloadContent() {
            Layers.ForEach(x => x.UnloadContent());
        }

        public virtual void Draw(GameTime gameTime, float startDepth = 0, float endDepth = 1) {
            float Range = endDepth - startDepth;
            float RangeChunk = Range / Layers.Count;
#if DEBUG
            if (startDepth < 0 || endDepth > 1 || Range <= 0) {
                throw new Exception("Invalid start and end range. Valid range is between 0-1.");
            }
#endif

            SpriteBatch.Begin(
                sortMode: SpriteSortMode.FrontToBack,
                blendState: BlendState.AlphaBlend,
                samplerState: SamplerState.PointClamp,
                rasterizerState: SpriteBatch.GraphicsDevice.RasterizerState);

            for (var i = 0; i < Layers.Count; i++) {
                if(Layers[i].Effects.Count > 0) {
                    var graphicsDevice = State.BaseGame.GraphicsDevice;
                    var oldRenderTargets = graphicsDevice.GetRenderTargets();
                    
                    graphicsDevice.SetRenderTarget(LayerBaseTexture);
                    graphicsDevice.Clear(Color.Transparent);

                    LayerSpriteBatch.Begin(
                        sortMode: SpriteSortMode.FrontToBack,
                        blendState: BlendState.AlphaBlend,
                        samplerState: SamplerState.LinearWrap);
                    Layers[i].Draw(gameTime, LayerSpriteBatch, 0, 1);
                    LayerSpriteBatch.End();
                    
                    foreach(var effect in Layers[i].Effects) {
                        graphicsDevice.SetRenderTarget(LayerBaseTexture);
                        //graphicsDevice.Clear(Color.Transparent);

                        LayerSpriteBatch.Begin(
                            sortMode: SpriteSortMode.Immediate,
                            blendState: BlendState.AlphaBlend,
                            effect: effect.Effect
                        );
                        LayerSpriteBatch.Draw(
                            texture: LayerBaseTexture,
                            position: Vector2.Zero,
                            sourceRectangle: null,
                            color: Color.White,
                            rotation: 0,
                            origin: Vector2.Zero,
                            scale: Vector2.One,
                            effects: SpriteEffects.None,
                            layerDepth: 0);
                        LayerSpriteBatch.End();
                    }

                    graphicsDevice.SetRenderTargets(oldRenderTargets);
                    SpriteBatch.Draw(
                        texture: LayerBaseTexture,
                        position: Vector2.Zero,
                        sourceRectangle: null,
                        color: Color.White,
                        rotation: 0,
                        origin: Vector2.Zero,
                        scale: Vector2.One,
                        effects: SpriteEffects.None,
                        layerDepth: startDepth + (i * RangeChunk));
                } else {
                    Layers[i].Draw(gameTime, SpriteBatch, startDepth + (i * RangeChunk), startDepth + ((i + 1) * RangeChunk));
                }
            }
            SpriteBatch.End();
        }

        public void MouseMove(MouseMoveEventArgs e) {
            OnMouseMove?.Invoke(this, e);
        }

        public void MouseClickStart(MouseClickEventArgs e) {
            OnMouseClickStart?.Invoke(this, e);
        }

        public void MouseClickEnd(MouseClickEventArgs e) {
            OnMouseClickEnd?.Invoke(this, e);
        }

        public void MouseScrollWheelChange(MouseScrollWheelChangeEventArgs e) {
            OnMouseScrollWheelChange?.Invoke(this, e);
        }

        public void KeyPressStart(KeyPressEventArgs e) {
            OnKeyPressStart?.Invoke(this, e);
        }

        public void KeyPressEnd(KeyPressEventArgs e) {
            OnKeyPressEnd?.Invoke(this, e);
        }
    }
}
