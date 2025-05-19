using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using TarLib.Input;

namespace TarLib.States {

    public class GameMenu : IGameMenu {
        public IGameState State { get; }

        private SpriteBatch SpriteBatch { get; }
        private List<IMenuWindow> Windows { get; } = new();
        private List<IMenuWindow> BottomWindows { get; } = new();
        private List<IMenuWindow> TopWindows { get; } = new();
        public IMenuWindow ExclusiveWindow { get; private set; }

        private readonly ObservableVariable<IMenuBlock> clickTarget = new();
        public IMenuBlock ClickTarget { get => clickTarget.Value; private set => clickTarget.Value = value; }

        private readonly ObservableVariable<IMenuBlock> hoverTarget = new();
        public IMenuBlock HoverTarget { get => hoverTarget.Value; private set => hoverTarget.Value = value; }

        private readonly ObservableVariable<IMenuBlock> selectTarget = new();
        public IMenuBlock SelectTarget { get => selectTarget.Value; private set => selectTarget.Value = value; }

        public GameMenu(IGameState state) {
            State = state;
            SpriteBatch = new SpriteBatch(State.BaseGame.GraphicsDevice);
        }

        public void Add(IMenuWindow window, string insertBefore = default, string insertAfter = default) {
            if (window != null && Windows.Where(x => x.Id == window.Id).ToList().Count == 0) {
                var containers = window.AlwaysOnTop ? TopWindows : BottomWindows;
                if (insertBefore != default || insertAfter != default) {
                    var _index = containers.FindIndex(matchContainer => matchContainer.Id == (insertBefore ?? insertAfter));
                    if (_index >= 0) {
                        containers.Insert(_index, window);
                    } else {
                        containers.Add(window);
                    }
                } else {
                    containers.Add(window);
                }

                window.Menu = this;

                window.BeforeAddToView();
                Windows.Add(window);
                window.AfterAddToView();
                SetExclusiveWindow();
            }
        }

        public void Remove(IMenuWindow window) {
            if(Windows.Contains(window)) {
                window.BeforeRemoveFromView();
                Windows.RemoveAll(x => x == window);
                BottomWindows.RemoveAll(x => x == window);
                TopWindows.RemoveAll(x => x == window);
                window.AfterRemoveFromView();
                SetExclusiveWindow();
            }
        }

        public void Clear() {
            foreach(var window in Windows.ToList()) {
                window.BeforeRemoveFromView();
                Windows.RemoveAll(x => x == window);
                BottomWindows.RemoveAll(x => x == window);
                TopWindows.RemoveAll(x => x == window);
                window.AfterRemoveFromView();
            }
            SetExclusiveWindow();
        }

        private void SetExclusiveWindow() {
            ExclusiveWindow = null;
            ClickTarget = null;
            foreach (var window in BottomWindows) {
                if (window.IsExclusive) {
                    ExclusiveWindow = window;
                }
            }
            foreach (var window in TopWindows) {
                if(window.IsExclusive) {
                    ExclusiveWindow = window;
                }
            }
        }

        public void Update(GameTime gameTime) {
            foreach(var window in Windows) {
                window.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime, float startDepth, float endDepth) {
            var range = (endDepth - startDepth);
            var allContainers = BottomWindows.ToList();
            allContainers.AddRange(TopWindows);

            SpriteBatch.Begin(
                sortMode: SpriteSortMode.FrontToBack,
                blendState: BlendState.AlphaBlend,
                samplerState: SamplerState.PointClamp,
                rasterizerState: SpriteBatch.GraphicsDevice.RasterizerState);

            for (int i = 0; i < allContainers.Count; i++) {
                var menuContainer = allContainers[i];
                var position = Vector2.Zero;
                if (menuContainer.ExactPosition != null) {
                    position = menuContainer.ExactPosition.Value;
                } else {
                    var size = new Vector2(menuContainer.TotalWidth, menuContainer.TotalHeight);
                    var safeDimensions = State.BaseGame.SafeDimensions;

                    float xMult = (menuContainer.ScreenHorizontalAlign == AlignStyle.End) ? 1 : ((menuContainer.ScreenHorizontalAlign == AlignStyle.Center) ? 0.5f : 0);
                    float yMult = (menuContainer.ScreenVerticalAlign == AlignStyle.End) ? 1 : ((menuContainer.ScreenVerticalAlign == AlignStyle.Center) ? 0.5f : 0);
                    position.X = ((safeDimensions.Width - size.X) * xMult) + safeDimensions.Left;
                    position.Y = ((safeDimensions.Height - size.Y) * yMult) + safeDimensions.Top;
                }

                menuContainer.Draw(
                    gameTime,
                    SpriteBatch,
                    position,
                    startDepth + i * range / allContainers.Count,
                    startDepth + (i + 1) * range / allContainers.Count);
            }

            SpriteBatch.End();
        }

        private IMenuBlock GetBlockAtPoint(Point point, MouseButton? mouseButton = default) {
            foreach (var window in TopWindows.Concat(BottomWindows)) {
                if (window.DoesIntersect(point)) {
                    foreach(var block in window.Blocks) {
                        if(block.DoesIntersect(point)) {
                            if(block is IMenuContainer containerBlock) {
                                return containerBlock.GetBlockAtPoint(point, mouseButton);
                            } else if (mouseButton == default || block.CanUseMouseButton(mouseButton.Value)) {
                                return block;
                            }
                        }
                    }
                    return window;
                }
            }
            return null;
        }

        public void MouseMove(MouseMoveEventArgs e) {
            var OldHoverTarget = HoverTarget;
            HoverTarget = ExclusiveWindow?.GetBlockAtPoint(e.MousePosition) ?? GetBlockAtPoint(e.MousePosition);
            if (OldHoverTarget != HoverTarget) {
                OldHoverTarget?.MouseHoverEnd(e);
                HoverTarget?.MouseHoverStart(e);
            }
            HoverTarget?.MouseMove(e);
        }

        public void MouseClickStart(MouseClickEventArgs e) {
            ClickTarget = ExclusiveWindow?.GetBlockAtPoint(e.MousePosition, e.MouseButton) ?? GetBlockAtPoint(e.MousePosition, e.MouseButton);
            ClickTarget?.MouseClickStart(e);
            SelectTarget = ClickTarget;
        }

        public void MouseClickEnd(MouseClickEventArgs e) {
            var newClickTarget = ExclusiveWindow?.GetBlockAtPoint(e.MousePosition, e.MouseButton) ?? GetBlockAtPoint(e.MousePosition, e.MouseButton);
            if (newClickTarget == ClickTarget) {
                ClickTarget?.MouseClickEnd(e);
            } else {
                ClickTarget?.MouseClickCancel(e);
            }
            ClickTarget = null;
        }

        public void MouseScrollWheelChange(MouseScrollWheelChangeEventArgs e) {
            HoverTarget?.MouseScrollWheelChange(e);
        }

        public void KeyPressStart(KeyPressEventArgs e) {
            SelectTarget?.KeyPressStart(e);
        }

        public void KeyPressEnd(KeyPressEventArgs e) {
            SelectTarget?.KeyPressEnd(e);
        }
    }
}
