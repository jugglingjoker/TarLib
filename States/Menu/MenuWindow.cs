
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TarLib.Input;

namespace TarLib.States {

    public class MenuWindow : MenuWindow<MenuWindowContents, MenuWindowTitleBar> {
        public MenuWindow(
            string id,
            IGameMenu menu = null,
            List<MenuBlock> menuBlocks = null,
            AlignStyle? screenVerticalAlign = null,
            AlignStyle? screenHorizontalAlign = null,
            Vector2? exactPosition = null,
            string title = null,
            bool hasCloseButton = false,
            bool canMovePosition = false,
            bool rememberPosition = false,
            bool alwaysOnTop = false,
            bool captureInput = true) : base(id, menu, menuBlocks, screenVerticalAlign, screenHorizontalAlign, exactPosition, title, hasCloseButton, canMovePosition, rememberPosition, alwaysOnTop, captureInput) {
        }
    }

    public class MenuWindow<TWindowContents> : MenuWindow<TWindowContents, MenuWindowTitleBar>
        where TWindowContents : IMenuWindowContents, new() {
        public MenuWindow(
            string id,
            IGameMenu menu = null,
            List<MenuBlock> menuBlocks = null,
            AlignStyle? screenVerticalAlign = null,
            AlignStyle? screenHorizontalAlign = null,
            Vector2? exactPosition = null,
            string title = null,
            bool hasCloseButton = false,
            bool canMovePosition = false,
            bool rememberPosition = false,
            bool alwaysOnTop = false,
            bool captureInput = true) : base(id, menu, menuBlocks, screenVerticalAlign, screenHorizontalAlign, exactPosition, title, hasCloseButton, canMovePosition, rememberPosition, alwaysOnTop, captureInput) {
        }
    }

    public class MenuWindow<TWindowContents, TWindowTitleBar> : MenuContainer, IMenuWindow
        where TWindowContents : IMenuWindowContents, new()
        where TWindowTitleBar : IMenuWindowTitleBar, new() {
        public bool CanMovePosition { get; } // Can the container be moved?
        public bool RememberPosition { get; } // Is the position remembered for next time?
        public bool AlwaysOnTop { get; }
        public bool CaptureInput { get; }
        public bool IsExclusive { get; }

        public Vector2? ExactPosition { get; set; } // Current drawing position
        public AlignStyle? ScreenVerticalAlign { get; }
        public AlignStyle? ScreenHorizontalAlign { get; }

        protected override MenuBlockStyleTypeList StyleTypes => MenuBlockStyleType.Window;

        protected TWindowTitleBar TitleBar { get; }
        protected TWindowContents Contents { get; }
        public string Id { get; }

        public event EventHandler OnBeforeAddToView;
        public event EventHandler OnAfterAddToView;
        public event EventHandler OnBeforeRemoveFromView;
        public event EventHandler OnAfterRemoveFromView;

        public MenuWindow(string id,
            IGameMenu menu = null,
            List<MenuBlock> menuBlocks = null,
            AlignStyle? screenVerticalAlign = default,
            AlignStyle? screenHorizontalAlign = default,
            Vector2? exactPosition = default,
            string title = default,
            bool hasCloseButton = false,
            bool canMovePosition = false,
            bool rememberPosition = false,
            bool alwaysOnTop = false,
            bool captureInput = true) : base(menu: menu) {

            Id = id;

            if (ScreenVerticalAlign == null && ScreenHorizontalAlign == null && exactPosition == null) {
                // TODO: Throw position execption
            }
            ScreenVerticalAlign = screenVerticalAlign;
            ScreenHorizontalAlign = screenHorizontalAlign;
            ExactPosition = exactPosition;
            CanMovePosition = canMovePosition;
            RememberPosition = rememberPosition;
            AlwaysOnTop = alwaysOnTop;
            CaptureInput = captureInput;

            if(title != default) {
                TitleBar = new();
                TitleBar.Window = this;
                TitleBar.Title = title;
                TitleBar.HasCloseButton = hasCloseButton;
                TitleBar.Parent = this;
                blocks.Add(TitleBar);
            }

            Contents = new();
            Contents.Window = this;
            Contents.Parent = this;
            blocks.Add(Contents);

            if(menuBlocks != null) {
                foreach (var block in menuBlocks) {
                    Add(block);
                }
            }

            DefaultStyle = new MenuBlockStyleRule() {
                BackgroundColor = Color.White,
                BorderSize = (2, 2, 4, 2),
                BorderColor = Color.Black
            };
        }

        public void Close() {
            Menu.Remove(this);
        }

        public override void Add(IMenuBlock menuBlock) {
            Contents.Add(menuBlock);
        }

        public override void MouseClickStart(MouseClickEventArgs e) {
            base.MouseClickStart(e);
            if(CaptureInput) {
                e.FlagAsUsed();
            }
        }

        public override void MouseClickEnd(MouseClickEventArgs e) {
            base.MouseClickEnd(e);
            if (CaptureInput) {
                e.FlagAsUsed();
            }
        }

        public void BeforeAddToView() {
            OnBeforeAddToView?.Invoke(this, null);
        }

        public void AfterAddToView() {
            OnAfterAddToView?.Invoke(this, null);
        }

        public void BeforeRemoveFromView() {
            OnBeforeRemoveFromView?.Invoke(this, null);
        }

        public void AfterRemoveFromView() {
            OnAfterRemoveFromView?.Invoke(this, null);
        }
    }
}
