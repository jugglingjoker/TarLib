using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using TarLib.Entities.Drawable;
using TarLib.Graphics;
using TarLib.Input;
using TarLib.Primitives;

namespace TarLib.States {

    public abstract class MenuBlock : IMenuBlock {
        private bool isActivateHoverCache;
        private MenuBlockStyle? cachedStyle;
        private MenuBlockStyleRule? cachedActiveStyleRule = default;

        private readonly MenuBlockStyleTagList styleTags = new();
        private readonly ObservableVariable<IGameMenu> menu = new();
        private readonly ObservableVariable<MenuBlockStyle> style = new();

        private readonly ObservableVariable<bool> isError = new();
        private readonly ObservableVariable<bool> isActivate = new();
        private readonly ObservableVariable<bool> isHover = new();
        private readonly ObservableVariable<bool> isSelected = new();
        private readonly ObservableVariable<bool> isDisabled = new();
        private readonly ObservableVariable<bool> isVisible = new();

        public MenuBlock(IGameMenu menu) {
            Menu = menu;
            IsVisible = true;

            isDisabled.OnChange += IsDisabled_OnChange;
            isVisible.OnChange += IsVisible_OnChange;
            isHover.OnChange += IsHover_OnChange;
            isError.OnChange += IsError_OnChange;
            isSelected.OnChange += IsSelected_OnChange;

            style.OnChange += Style_OnChange;

            DefaultStyle = new();
        }

        public virtual IGameMenu Menu {
            get => Parent?.Menu ?? menu.Value;
            set => menu.Value = value;
        }

        public IMenuContainer Parent { get; set; }

        protected abstract MenuBlockStyleTypeList StyleTypes { get; }

        protected virtual MenuBlockStyle DefaultStyle { get; init; }

        public MenuBlockStyle Style {
            get => style.Value;
            set => style.Value = value; 
        }
        
        public MenuBlockStyle ActiveStyle { 
            get {
                if(cachedStyle == null) {
                    cachedStyle = Style + Menu.State.BaseGame.MenuStyles.Get(StyleTypes, styleTags) + DefaultStyle;
                }
                return cachedStyle.Value;
            }
        }

        public MenuBlockStyleRule ActiveStyleRule {
            get {
                if(cachedActiveStyleRule == null) {
                    MenuBlockStyleRule rule = default;

                    if (IsError) {
                        rule += ActiveStyle.Error;
                    }
                    if (IsActivate) {
                        rule += ActiveStyle.Activate;
                    }
                    if (IsHover) {
                        rule += ActiveStyle.Hover;
                    }
                    if (IsSelected) {
                        rule += ActiveStyle.Select;
                    }
                    if (IsDisabled) {
                        rule += ActiveStyle.Disabled;
                    }
                    rule += ActiveStyle.Default;

                    cachedActiveStyleRule = rule;
                }
                return cachedActiveStyleRule.Value;
            }
        }

        public bool IsError {
            get => isError.Value;
            set => isError.Value = value;
        }

        public bool IsActivate {
            get => isActivate.Value;
            private set => isActivate.Value = value;
        }

        public bool IsHover {
            get => isHover.Value;
            private set => isHover.Value = value;
        }

        public bool IsVisible {
            get => isVisible.Value;
            set => isVisible.Value = value;
        }

        public bool IsSelected {
            get => isSelected.Value;
            set => isSelected.Value = value;
        }

        public bool IsDisabled {
            get => isDisabled.Value;
            set => isDisabled.Value = value;
        }

        private RectanglePrimitive? hotZone;
        public RectanglePrimitive? HotZone {
            get => hotZone; // != null ? hotZone.Value + (Parent?.HotZone?.TopLeft ?? Vector2.Zero) : null;
            protected set => hotZone = value;
        }

        public int TotalWidth => IsVisible ? Margin.TotalHorizontal + BorderSize.TotalHorizontal + BlockWidth : 0;
        public int TotalHeight => IsVisible ? Margin.TotalVertical + BorderSize.TotalVertical + BlockHeight : 0;
        public Vector2 TotalSize => new(TotalWidth, TotalHeight);

        public int BlockWidth => Padding.TotalHorizontal + ContentWidth;
        public int BlockHeight => Padding.TotalVertical + ContentHeight;
        public Vector2 BlockSize => new(BlockWidth, BlockHeight);

        public bool IsFitContentWidth => (ActiveStyleRule.WidthStyle == SizeStyle.FitContentMaximum && ActiveStyleRule.Width != default && ContentActualWidth > ActiveStyleRule.Width.Value) || ActiveStyleRule.WidthStyle == SizeStyle.FitContent || ActiveStyleRule.WidthStyle == null || (ActiveStyleRule.WidthStyle == SizeStyle.FitParent && Parent != null && Parent.IsFitContentWidth);
        public virtual int ContentWidth => ActiveStyleRule.WidthStyle switch {
            SizeStyle.FitParent => (Parent != null ? (Parent.ContentDirection == ContentDirectionStyle.Row && Parent.Blocks.Count > 1 ? ContentActualWidth : Parent.ContentWidth) : Menu.State.BaseGame.ScreenDimensions.Width) - (Margin.TotalHorizontal + Padding.TotalHorizontal),
            SizeStyle.Exact => ActiveStyleRule.Width?.Value ?? 0,
            _ => ContentActualWidth,
        };

        public bool IsFitContentHeight => (ActiveStyleRule.HeightStyle == SizeStyle.FitContentMaximum && ActiveStyleRule.Height != default && ContentActualHeight > ActiveStyleRule.Height.Value) || ActiveStyleRule.HeightStyle == SizeStyle.FitContent || ActiveStyleRule.HeightStyle == null || (ActiveStyleRule.HeightStyle == SizeStyle.FitParent && Parent != null && Parent.IsFitContentHeight);
        public virtual int ContentHeight => ActiveStyleRule.HeightStyle switch {
            SizeStyle.FitParent => (Parent != null ? (Parent.ContentDirection == ContentDirectionStyle.Column && Parent.Blocks.Count > 1 ? ContentActualHeight : Parent.ContentHeight) : Menu.State.BaseGame.ScreenDimensions.Height) - (Margin.TotalVertical + Padding.TotalVertical),
            SizeStyle.Exact => ActiveStyleRule.Height?.Value ?? 0,
            _ => ContentActualHeight,
        };

        public int TotalActualWidth => IsVisible ? Margin.TotalHorizontal + Padding.TotalHorizontal + BorderSize.TotalHorizontal + ContentActualWidth : 0;
        public int TotalActualHeight => IsVisible ? Margin.TotalVertical + Padding.TotalVertical + BorderSize.TotalVertical + ContentActualHeight : 0;
        public abstract int ContentActualWidth { get; }
        public abstract int ContentActualHeight { get; }

        private bool needsRefresh;
        public virtual bool NeedsRefresh { get => needsRefresh; set => needsRefresh = value; }

        // Defaults
        public virtual Texture2D BackgroundTexture => Menu.State.BaseGame.Textures.Default;
        public virtual SpriteFont Font => Menu.State.BaseGame.Fonts[ActiveStyleRule.FontId ?? "_default"];
        public virtual Color FontColor => ActiveStyleRule.FontColor ?? Color.Black;
        public virtual Color BackgroundColor => ActiveStyleRule.BackgroundColor ?? Color.Transparent;
        public virtual MarginStyle Margin => ActiveStyleRule.Margin ?? default;
        public virtual PaddingStyle Padding => ActiveStyleRule.Padding ?? default;
        public virtual BorderColorStyle BorderColor => ActiveStyleRule.BorderColor ?? default;
        public virtual BorderSizeStyle BorderSize => ActiveStyleRule.BorderSize;
        public virtual SizeStyle WidthStyle => ActiveStyleRule.WidthStyle ?? default;
        public virtual SizeStyle HeightStyle => ActiveStyleRule.HeightStyle ?? default;
        public virtual ContentDirectionStyle ContentDirection => ActiveStyleRule.ContentDirection ?? default;

        public event EventHandler<MouseClickEventArgs> OnClickStart;
        public event EventHandler<MouseClickEventArgs> OnClickEnd;
        public event EventHandler<MouseClickEventArgs> OnClickCancel;
        public event EventHandler<MouseMoveEventArgs> OnMouseMove;
        public event EventHandler<MouseMoveEventArgs> OnMouseHoverStart;
        public event EventHandler<MouseMoveEventArgs> OnMouseHoverEnd;
        public event EventHandler<MouseScrollWheelChangeEventArgs> OnMouseScrollWheelChange;
        public event EventHandler<KeyPressEventArgs> OnKeyPressStart;
        public event EventHandler<KeyPressEventArgs> OnKeyPressEnd;


#if DEBUG
        //private readonly Texture2D HotZoneTexture;
        //~MenuBlock() {
        //    HotZoneTexture?.Dispose();
        //}
#endif

        public void AddStyleTag(string styleTag) {
            styleTags.Add(styleTag);
            cachedStyle = null;
            cachedActiveStyleRule = null;
        }

        public void RemoveStyleTag(string styleTag) {
            styleTags.Remove(styleTag);
            cachedStyle = null;
            cachedActiveStyleRule = null;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position = default, float startDepth = 0, float endDepth = 1) {
            if(IsVisible) {
                if(HotZone == null) {
                    CalculateHotZone(position);
                }

                var drawRange = endDepth - startDepth;

                // TODO: Change to IDrawableBoxTexture
                var drawPosition = position + Margin.Position;
                spriteBatch.Draw(
                    texture: BackgroundTexture,
                    position: drawPosition,
                    sourceRectangle: null,
                    color: BorderColor.Top,
                    rotation: 0,
                    origin: Vector2.Zero,
                    scale: BlockSize + BorderSize.Total,
                    effects: SpriteEffects.None,
                    layerDepth: startDepth);

                drawPosition += BorderSize.Position;
                spriteBatch.Draw(
                    texture: BackgroundTexture,
                    position: drawPosition,
                    sourceRectangle: null,
                    color: BackgroundColor,
                    rotation: 0,
                    origin: Vector2.Zero,
                    scale: BlockSize,
                    effects: SpriteEffects.None,
                    layerDepth: startDepth + drawRange / 4);

                drawPosition += Padding.Position;
                switch (ActiveStyleRule.ContentHorizontalAlign) {
                    case AlignStyle.Center:
                        drawPosition.X += (ContentWidth - ContentActualWidth) / 2;
                        break;
                    case AlignStyle.End:
                        drawPosition.X += (ContentWidth - ContentActualWidth);
                        break;
                }

                switch (ActiveStyleRule.ContentVerticalAlign) {
                    case AlignStyle.Center:
                        drawPosition.Y += (ContentHeight - ContentActualHeight) / 2;
                        break;
                    case AlignStyle.End:
                        drawPosition.Y += (ContentHeight - ContentActualHeight);
                        break;
                }

                DrawContent(gameTime, spriteBatch, drawPosition, startDepth + drawRange / 2, endDepth);

#if DEBUG
                //Debug hot zones, enable if you want to see a visual indicator of their zones
                //if ((OnClickEnd?.GetInvocationList().Length ?? 0) > 0
                //    || (OnClickStart?.GetInvocationList().Length ?? 0) > 0
                //    || (OnHoverStart?.GetInvocationList().Length ?? 0) > 0
                //    || (OnHoverEnd?.GetInvocationList().Length ?? 0) > 0) {
                //    if (HotZoneTexture == null) {
                //        HotZoneTexture = new Texture2D(Menu.State.BaseGame.GraphicsDevice, HotZone.Value.Width, HotZone.Value.Height);
                //        var tempColor = new Color[HotZone.Value.Width * HotZone.Value.Height];
                //        var thickness = 2;
                //        for (int i = 0; i < HotZone.Value.Height; i++) {
                //            for (int j = 0; j < HotZone.Value.Width; j++) {
                //                if (i < thickness || j < thickness || i >= HotZone.Value.Height - thickness || j >= HotZone.Value.Width - thickness) {
                //                    tempColor[j + i * HotZone.Value.Width] = Color.Yellow;
                //                } else {
                //                    tempColor[j + i * HotZone.Value.Width] = new Color(1, 0, 0, 0.5f);
                //                }
                //            }
                //        }
                //        HotZoneTexture.SetData(tempColor);
                //    }
                //    spriteBatch.Draw(
                //        texture: HotZoneTexture,
                //        position: new Vector2(HotZone.Value.X, HotZone.Value.Y),
                //        sourceRectangle: null,
                //        color: Color.White,
                //        rotation: 0,
                //        origin: Vector2.Zero,
                //        scale: Vector2.One,
                //        effects: SpriteEffects.None,
                //        layerDepth: endDepth);
                //}
#endif
            }
        }

        protected abstract void DrawContent(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position, float startDepth, float endDepth);

        public void CalculateHotZone(Vector2 position) {
            HotZone = RectanglePrimitive.FromPosition(position + Margin.Position + BorderSize.Position, BlockSize);
        }

        public virtual void Update(GameTime gameTime) {

        }

        public bool CanUseMouseButton(MouseButton mouseButton) {
            return mouseButton switch {
                MouseButton.LeftButton => CanUseLeftMouseButton,
                MouseButton.RightButton => CanUseRightMouseButton,
                _ => false,
            };
        }

        public virtual bool CanUseRightMouseButton => false;
        public virtual bool CanUseLeftMouseButton => false;

        public virtual void MouseClickStart(MouseClickEventArgs e) {
            if (!IsDisabled) {
                IsActivate = true;
                OnClickStart?.Invoke(this, e);
            }
            if(e.IsAvailable) {
                Parent?.MouseClickStart(e);
            }
        }

        public virtual void MouseClickEnd(MouseClickEventArgs e) {
            if (!IsDisabled) {
                IsActivate = false;
                isActivateHoverCache = false;
                OnClickEnd?.Invoke(this, e);
            }
            if(e.IsAvailable) {
                Parent?.MouseClickEnd(e);
            }
        }

        public virtual void MouseClickCancel(MouseClickEventArgs e) {
            if (!IsDisabled) {
                IsActivate = false;
                isActivateHoverCache = false;
                OnClickCancel?.Invoke(this, e);
            }
            Parent?.MouseClickCancel(e);
        }

        public virtual void MouseMove(MouseMoveEventArgs e) {
            if (!IsDisabled) {
                OnMouseMove?.Invoke(this, e);
            }
            Parent?.MouseMove(e);
        }

        public virtual void MouseHoverStart(MouseMoveEventArgs e) {
            if (!IsDisabled) {
                IsHover = true;
                IsActivate = isActivateHoverCache;
                OnMouseHoverStart?.Invoke(this, e);
            }
            Parent?.MouseHoverStart(e);
        }

        public virtual void MouseHoverEnd(MouseMoveEventArgs e) {
            if (!IsDisabled) {
                IsHover = false;
                isActivateHoverCache = IsActivate;
                IsActivate = false;
                OnMouseHoverEnd?.Invoke(this, e);
            }
            Parent?.MouseHoverEnd(e);
        }

        public virtual void MouseScrollWheelChange(MouseScrollWheelChangeEventArgs e) {
            if (!IsDisabled) {
                OnMouseScrollWheelChange?.Invoke(this, e);
            }
            Parent?.MouseScrollWheelChange(e);
        }

        public virtual void KeyPressStart(KeyPressEventArgs e) {
            if (!IsDisabled) {
                OnKeyPressStart?.Invoke(this, e);
            }
            Parent?.KeyPressStart(e);
        }

        public virtual void KeyPressEnd(KeyPressEventArgs e) {
            if (!IsDisabled) {
                OnKeyPressEnd?.Invoke(this, e);
            }
            Parent?.KeyPressEnd(e);
        }

        public virtual bool DoesIntersect(Point point) {
            return HotZone?.DoesIntersect(point.ToVector2()) ?? false;
        }

        private void Style_OnChange(object sender, (MenuBlockStyle oldValue, MenuBlockStyle newValue) e) {
            cachedStyle = null;
            cachedActiveStyleRule = null;
            NeedsRefresh = true;
        }

        private void IsDisabled_OnChange(object sender, (bool oldValue, bool newValue) e) {
            cachedActiveStyleRule = null;
            IsActivate = false;
            IsHover = false;
            isActivateHoverCache = false;
            NeedsRefresh = true;
        }

        private void IsVisible_OnChange(object sender, (bool oldValue, bool newValue) e) {
            cachedActiveStyleRule = null;
            NeedsRefresh = true;
        }

        private void IsSelected_OnChange(object sender, (bool oldValue, bool newValue) e) {
            cachedActiveStyleRule = null;
            NeedsRefresh = true;
        }

        private void IsError_OnChange(object sender, (bool oldValue, bool newValue) e) {
            cachedActiveStyleRule = null;
            NeedsRefresh = true;
        }

        private void IsHover_OnChange(object sender, (bool oldValue, bool newValue) e) {
            cachedActiveStyleRule = null;
            NeedsRefresh = true;
        }
    }
}
