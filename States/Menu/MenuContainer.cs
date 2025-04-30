using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using TarLib.Input;

namespace TarLib.States {

    public abstract class ScrollBar : MenuContainer {
        public ScrollBar(IMenuContainer container, IGameMenu menu = null) : base(menu) {
            Container = container;

            DefaultStyle = new MenuBlockStyleRule() {
                BorderColor = Color.Black,
                BackgroundColor = Color.LightGray,
            };
        }

        public IMenuContainer Container { get; }
        protected override MenuBlockStyleTypeList StyleTypes => MenuBlockStyleType.ScrollBar;
    }

    public class VerticalScrollBar : ScrollBar {

        public VerticalScrollBar(IMenuContainer container, IGameMenu menu = null) : base(container, menu) {
            UpArrow = new UpArrowButton(this, menu);
            UpArrow.OnClickStart += UpArrow_OnClickStart;
            UpArrow.OnClickCancel += UpArrow_OnClickCancel;
            UpArrow.OnClickEnd += UpArrow_OnClickEnd;
            Add(UpArrow);

            Progress = new ProgressIndicator(this, menu);
            Progress.OnClickStart += Progress_OnClickStart;
            Progress.OnClickEnd += Progress_OnClickEnd;
            Progress.OnClickCancel += Progress_OnClickCancel;
            Add(Progress);

            DownArrow = new DownArrowButton(this, menu);
            DownArrow.OnClickStart += DownArrow_OnClickStart;
            DownArrow.OnClickCancel += DownArrow_OnClickCancel;
            DownArrow.OnClickEnd += DownArrow_OnClickEnd;
            Add(DownArrow);

            OnClickEnd += VerticalScrollBar_OnClickEnd;

            DefaultStyle = new MenuBlockStyleRule() {
                WidthStyle = SizeStyle.Exact,
                Width = 20,
                ContentHorizontalAlign = AlignStyle.Center,
                ContentVerticalAlign = AlignStyle.SpaceBetween,
                BorderLeft = 2
            } + DefaultStyle;
        }

        public override void Update(GameTime gameTime) {
            if (scrollAction == Action.ScrollUp) {
                Scroll(-33); // TODO: amount should accelerate the longer scrolling has continued?
            } else if (scrollAction == Action.ScrollDown) {
                Scroll(33);
            } else if (scrollAction == Action.Drag) {
                float diff = Menu.State.BaseGame.Input.MousePosition.Y - dragMouseStart;
                if(diff != 0) {
                    var totalBarHeight = Progress.BarHeight;
                    var changePercent = diff / totalBarHeight;
                    CurrentPosition = (int)(dragPositionStart + Container.ContentActualHeight * changePercent);
                }
            }

            base.Update(gameTime);
        }

        private void UpArrow_OnClickStart(object sender, MouseClickEventArgs e) {
            if(e.MouseButton == MouseButton.LeftButton && e.IsAvailable) {
                scrollAction = Action.ScrollUp;
                scrollActionStartTime = e.GameTime.TotalGameTime;
            }
        }

        private void UpArrow_OnClickCancel(object sender, MouseClickEventArgs e) {
            scrollAction = Action.None;
        }

        private void UpArrow_OnClickEnd(object sender, MouseClickEventArgs e) {
            scrollAction = Action.None;
        }

        private void DownArrow_OnClickStart(object sender, MouseClickEventArgs e) {
            if (e.MouseButton == MouseButton.LeftButton && e.IsAvailable) {
                scrollAction = Action.ScrollDown;
                scrollActionStartTime = e.GameTime.TotalGameTime;
            }
        }

        private void DownArrow_OnClickCancel(object sender, MouseClickEventArgs e) {
            scrollAction = Action.None;
        }

        private void DownArrow_OnClickEnd(object sender, MouseClickEventArgs e) {
            scrollAction = Action.None;
        }

        protected override void DrawContent(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position, float startDepth, float endDepth) {
            base.DrawContent(gameTime, spriteBatch, position, startDepth, endDepth);
        }

        private void Progress_OnClickStart(object sender, MouseClickEventArgs e) {
            dragMouseStart = e.MousePosition.Y;
            dragPositionStart = CurrentPosition;
            scrollAction = Action.Drag;
        }

        private void Progress_OnClickEnd(object sender, MouseClickEventArgs e) {
            scrollAction = Action.None;     
        }

        private void Progress_OnClickCancel(object sender, MouseClickEventArgs e) {
            scrollAction = Action.None;
        }

        private void VerticalScrollBar_OnClickEnd(object sender, MouseClickEventArgs e) {
            if (e.MouseButton == MouseButton.LeftButton && e.IsAvailable) {
                if (Progress.HotZone != null && e.MousePosition.Y < Progress.HotZone?.Top) {
                    PageUp();
                } else if (Progress.HotZone != null && e.MousePosition.Y > Progress.HotZone?.Bottom) {
                    PageDown();
                }
                e.FlagAsUsed();
            }
        }

        public void PageUp() {
            CurrentPosition -= Container.ContentHeight;
        }

        public void PageDown() {
            CurrentPosition += Container.ContentHeight;
        }

        public void Scroll(int amount) {
            CurrentPosition += amount;
        }

        
        public UpArrowButton UpArrow { get; }
        public DownArrowButton DownArrow { get; }
        public ProgressIndicator Progress { get; }

        private readonly ObservableVariable<int> currentPosition = new();
        private int dragMouseStart;
        private int dragPositionStart;
        private Action scrollAction = Action.None;
        private TimeSpan scrollActionStartTime;

        public int CurrentPosition {
            get => currentPosition.Value;
            protected set => currentPosition.Value = MathHelper.Clamp(value, 0, MaxScrollValue);
        }

        public int MaxScrollValue => Container.ContentActualHeight - Container.ContentHeight;

        protected override MenuBlockStyleTypeList StyleTypes => MenuBlockStyleType.VerticalScrollBar + base.StyleTypes;
        public override int ContentHeight => Container.ContentHeight - (BorderSize.TotalVertical + Padding.TotalVertical + Margin.TotalVertical);

        public override IGameMenu Menu {
            get => Container?.Menu ?? base.Menu;
            set => base.Menu = value;
        }

        public enum Action {
            None = 0,
            ScrollUp = 1,
            ScrollDown = 2,
            Drag = 3
        }

        public class UpArrowButton : ArrowButton {
            public UpArrowButton(VerticalScrollBar scrollBar, IGameMenu menu = null) : base("up_arrow", scrollBar, menu) {
                DefaultStyle = new MenuBlockStyleRule() {
                    BorderBottom = 2,
                } + DefaultStyle;
            }

            protected override MenuBlockStyleTypeList StyleTypes => base.StyleTypes + MenuBlockStyleType.ScrollBarUpArrowButton;
        }

        public class DownArrowButton : ArrowButton {
            public DownArrowButton(VerticalScrollBar scrollBar, IGameMenu menu = null) : base("down_arrow", scrollBar, menu) {
                DefaultStyle = new MenuBlockStyleRule() {
                    BorderTop = 2,
                } + DefaultStyle;
            }

            protected override MenuBlockStyleTypeList StyleTypes => base.StyleTypes + MenuBlockStyleType.ScrollBarDownArrowButton;
        }

        public abstract class ArrowButton : MenuImageButton {
            public ArrowButton(string textureId, VerticalScrollBar scrollBar, IGameMenu menu = default) : base(textureId, menu) {
                ScrollBar = scrollBar;
                Image.Scale = new Vector2(0.33f, 0.33f);
                Image.Color = new Color(64, 64, 64);

                DefaultStyle = new MenuBlockStyleRule() {
                    HeightStyle = SizeStyle.Exact,
                    Height = 20,
                    WidthStyle = SizeStyle.FitParent,
                    BorderColor = Color.Black,
                    BackgroundColor = Color.Gray,
                    ContentVerticalAlign = AlignStyle.Center,
                    ContentHorizontalAlign = AlignStyle.Center,
                };
            }

            public VerticalScrollBar ScrollBar { get; }
            protected override MenuBlockStyleTypeList StyleTypes => MenuBlockStyleType.ScrollBarArrowButton;
        }

        public class ProgressIndicator : MenuBlock {
            public ProgressIndicator(VerticalScrollBar scrollBar, IGameMenu menu = default) : base(menu) {
                ScrollBar = scrollBar;

                DefaultStyle = new MenuBlockStyleRule() {
                    WidthStyle = SizeStyle.FitParent,
                    BackgroundColor = Color.Gray,
                    BorderTop = 2,
                    BorderBottom = 2,
                    BorderColor = Color.Black,
                    Margin = (-2, 0)
                };
            }

            public VerticalScrollBar ScrollBar { get; }
            public override int ContentActualWidth => 0;
            public override int ContentActualHeight => (int)(BarHeight * Math.Min(1f, (float)ScrollBar.Container.ContentHeight / ScrollBar.Container.ContentActualHeight));

            public int BarHeight => ScrollBar.ContentHeight - (ScrollBar.UpArrow.TotalHeight + ScrollBar.DownArrow.TotalHeight + base.Margin.TotalVertical + BorderSize.TotalVertical);
            protected int RemainingHeight => BarHeight - ContentActualHeight;
            protected int TopMargin => RemainingHeight * ScrollBar.CurrentPosition / ScrollBar.MaxScrollValue;

            public override MarginStyle Margin => (top: TopMargin + base.Margin.Top, left: 0, bottom: RemainingHeight - TopMargin + base.Margin.Bottom, right: 0);

            protected override MenuBlockStyleTypeList StyleTypes => MenuBlockStyleType.ScrollBarProgressIndicator;

            protected override void DrawContent(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position, float startDepth, float endDepth) {
                // do nothing
            }
        }
    }

    public abstract class MenuContainerPair<TFirstBlock, TSecondBlock> : MenuContainer 
        where TFirstBlock : IMenuBlock
        where TSecondBlock : IMenuBlock {

        private ObservableVariable<TFirstBlock> firstBlock = new();
        public TFirstBlock FirstBlock {
            get => firstBlock.Value;
            set {
                firstBlock.Value = value;
                blocks[0] = firstBlock.Value;
            }
        }

        private ObservableVariable<TSecondBlock> secondBlock = new();
        public TSecondBlock SecondBlock {
            get => secondBlock.Value;
            set {
                secondBlock.Value = value;
                blocks[1] = secondBlock.Value;
            }
        }

        public MenuContainerPair(TFirstBlock firstBlock, TSecondBlock secondBlock, IGameMenu menu = null) : base(menu, default) {
            base.Add(firstBlock);
            base.Add(secondBlock);
            FirstBlock = firstBlock;
            SecondBlock = secondBlock;
            this.secondBlock.OnChange += SecondBlock_OnChange;
            this.firstBlock.OnChange += FirstBlock_OnChange;
        }

        private void SecondBlock_OnChange(object sender, (TSecondBlock oldValue, TSecondBlock newValue) e) {
            NeedsRefresh = true;
        }

        private void FirstBlock_OnChange(object sender, (TFirstBlock oldValue, TFirstBlock newValue) e) {
            NeedsRefresh = true;
        }

        public sealed override void Add(IMenuBlock menuBlock) {
            // Do nothing
        }

        public sealed override void Remove(IMenuBlock menuBlock) {
            // Do nothing
        }

        public sealed override void Clear() {
            // Do nothing
        }
    }

    public class MenuContainer : MenuContainer<IMenuBlock> {
        protected override MenuBlockStyleTypeList StyleTypes => MenuBlockStyleType.Container;

        public MenuContainer(
            IGameMenu menu = null,
            List<IMenuBlock> menuBlocks = null) : base(menu, menuBlocks) {
            
        }
    }

    public abstract class MenuContainer<TMenuBlockType> : MenuBlock, IMenuContainer
        where TMenuBlockType : IMenuBlock {

        private SpriteBatch contentsSpriteBatch;

        public MenuContainer(
            IGameMenu menu = null,
            List<TMenuBlockType> blocks = null) : base(menu) {
            if (blocks != null) {
                foreach (var block in blocks) {
                    Add(block);
                }
            }

            if (!(this is VerticalScrollBar) && !(this is MenuButton)) {
                VerticalScrollBar = new VerticalScrollBar(this, menu);
                OnMouseScrollWheelChange += MenuContainer_OnMouseScrollWheelChange;
            }
        }

        protected List<TMenuBlockType> menuBlocks = new();
        protected List<IMenuBlock> blocks = new();
        public IReadOnlyList<TMenuBlockType> MenuBlocks => menuBlocks;
        public IReadOnlyList<IMenuBlock> Blocks => blocks;

        public VerticalScrollBar VerticalScrollBar { get; }
        public bool HasVerticalScrollBar => VerticalScrollBar != null && ActiveStyleRule.VerticalScroll == ScrollStyle.AlwaysShow || (ActiveStyleRule.VerticalScroll == ScrollStyle.Overflow && (ActiveStyleRule.HeightStyle == SizeStyle.Exact && ActiveStyleRule.HeightStyle == SizeStyle.FitContentMaximum) && ActiveStyleRule.Height != null && ActiveStyleRule.Height.Value > ContentActualHeight);

        private int? contentActualWidth;
        public override int ContentActualWidth {
            get {
                if(contentActualWidth == null || NeedsRefresh) {
                    CalculateDimensions();
                    NeedsRefresh = false;
                }
                return contentActualWidth ?? 0;
            }
        }

        public override int ContentWidth => base.ContentWidth + (HasVerticalScrollBar ? VerticalScrollBar.TotalWidth : 0);

        private int? contentActualHeight;
        public override int ContentActualHeight {
            get {
                if(contentActualHeight == null || NeedsRefresh) {
                    CalculateDimensions();
                    NeedsRefresh = false;
                }
                return contentActualHeight ?? 0;
            }
        }

        public override bool NeedsRefresh {
            get => base.NeedsRefresh || Blocks.Any(menuBlock => menuBlock.NeedsRefresh);
            set {
                base.NeedsRefresh = value;
                blocks.ForEach(menuBlock => {
                    if(value || menuBlock is not IMenuContainer) {
                        menuBlock.NeedsRefresh = value;
                    }
                });
            }
        }

        public virtual void Add(IMenuBlock menuBlock) {
            if (menuBlock is TMenuBlockType menuBlockType && menuBlockType != null) {
                menuBlock.Parent = this;
                blocks.Add(menuBlock);
                menuBlocks.Add(menuBlockType);
                NeedsRefresh = true;
            }
        }

        public bool Contains(IMenuBlock menuBlock) {
            foreach(var block in Blocks) {
                if(menuBlock.Equals(block) || (block is IMenuContainer containerBlock && containerBlock.Contains(menuBlock))) {
                    return true;
                }
            }
            return false;
        }

        public virtual void Remove(IMenuBlock menuBlock) {
            if(menuBlock is TMenuBlockType menuBlockType && Contains(menuBlockType)) {
                blocks.Remove(menuBlockType);
                menuBlocks.Remove(menuBlockType);
                NeedsRefresh = true;
            }
        }

        public virtual void Clear() {
            blocks.Clear();
            menuBlocks.Clear();
            NeedsRefresh = true;
        }

        protected override void DrawContent(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position, float startDepth, float endDepth) {
            var oldScissor = spriteBatch.GraphicsDevice.ScissorRectangle;
            var endBatch = false;
            if((ContentActualHeight > ContentHeight || ContentActualWidth > ContentWidth)) {
                spriteBatch.End();
                if(contentsSpriteBatch == null) {
                    contentsSpriteBatch = new SpriteBatch(spriteBatch.GraphicsDevice);
                }
                contentsSpriteBatch.Begin(
                    sortMode: SpriteSortMode.FrontToBack,
                    blendState: BlendState.AlphaBlend,
                    samplerState: SamplerState.PointClamp,
                    rasterizerState: contentsSpriteBatch.GraphicsDevice.RasterizerState);
                contentsSpriteBatch.GraphicsDevice.ScissorRectangle = (Rectangle)HotZone;
                endBatch = true;
            } else {
                contentsSpriteBatch = spriteBatch;
            }

            var currentPosition = Vector2.Zero;
            if (HasVerticalScrollBar) {
                currentPosition.Y -= VerticalScrollBar.CurrentPosition;
            }
            var fullWidth = ContentActualWidth;
            var fullHeight = ContentActualHeight;

            foreach (var menuBlock in Blocks) {
                var alignmentPosition = Vector2.Zero;

                if (ActiveStyleRule.ContentDirection == ContentDirectionStyle.Row) {
                    if (ActiveStyleRule.ContentVerticalAlign == AlignStyle.Center) {
                        alignmentPosition.Y += (fullHeight - menuBlock.TotalHeight) / 2;
                    } else if (ActiveStyleRule.ContentVerticalAlign == AlignStyle.End) {
                        alignmentPosition.Y += (fullHeight - menuBlock.TotalHeight);
                    }
                } else {
                    if (ActiveStyleRule.ContentHorizontalAlign == AlignStyle.Center) {
                        alignmentPosition.X += (fullWidth - menuBlock.TotalWidth) / 2;
                    } else if (ActiveStyleRule.ContentHorizontalAlign == AlignStyle.End) {
                        alignmentPosition.X += (fullWidth - menuBlock.TotalWidth);
                    }
                }

                menuBlock.CalculateHotZone(position + alignmentPosition + currentPosition);
                if (HotZone.Value.DoesIntersect(menuBlock.HotZone)) {
                    menuBlock.Draw(gameTime, contentsSpriteBatch, position + alignmentPosition + currentPosition, startDepth, endDepth);
                }

                if (ActiveStyleRule.ContentDirection == ContentDirectionStyle.Row) {
                    currentPosition.X += menuBlock.TotalWidth;
                    if (ActiveStyleRule.ContentHorizontalAlign == AlignStyle.SpaceBetween && Blocks.Count > 1) {
                        currentPosition.X += (ContentWidth - fullWidth) / (Blocks.Count - 1);
                    }
                } else {
                    currentPosition.Y += menuBlock.TotalHeight;
                    if (ActiveStyleRule.ContentVerticalAlign == AlignStyle.SpaceBetween && Blocks.Count > 1) {
                        currentPosition.Y += (ContentHeight - fullHeight) / (Blocks.Count - 1);
                    }
                }
            }

            if(endBatch) {
                contentsSpriteBatch.End();
                spriteBatch.GraphicsDevice.ScissorRectangle = oldScissor;
                spriteBatch.Begin(
                    sortMode: SpriteSortMode.FrontToBack,
                    blendState: BlendState.AlphaBlend,
                    samplerState: SamplerState.PointClamp,
                    rasterizerState: contentsSpriteBatch.GraphicsDevice.RasterizerState);
            }

            if (HasVerticalScrollBar) {
                var scrollBarPosition = position + new Vector2(ContentWidth - VerticalScrollBar.TotalWidth, 0);
                VerticalScrollBar.Draw(gameTime, spriteBatch, scrollBarPosition, startDepth + (endDepth - startDepth) / 2, endDepth);
            }
        }

        private void MenuContainer_OnMouseScrollWheelChange(object sender, MouseScrollWheelChangeEventArgs e) {
            if(e.ScrollWheelChange != 0) {
                VerticalScrollBar.Scroll((int)(-1 * e.ScrollWheelChange));
            }
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
            foreach(var menuBlock in Blocks) {
                menuBlock.Update(gameTime);
            }

            if(HasVerticalScrollBar) {
                VerticalScrollBar.Update(gameTime);
            }
        }

        public IMenuBlock GetBlockAtPoint(Point point, MouseButton? mouseButton = default) {

            if (VerticalScrollBar?.DoesIntersect(point) ?? false) {
                return VerticalScrollBar.GetBlockAtPoint(point, mouseButton);
            }

            foreach (var block in Blocks) {
                if(block.DoesIntersect(point)) {
                    if(block is IMenuContainer containerBlock) {
                        return containerBlock.GetBlockAtPoint(point, mouseButton);
                    } else if(mouseButton == default || block.CanUseMouseButton(mouseButton.Value)) {
                        return block;
                    }
                }
            }
            return this;
        }

        private void CalculateDimensions() {
            contentActualHeight = 0;
            if (Blocks.Count > 0) {
                if (ActiveStyleRule.ContentDirection != ContentDirectionStyle.Row) {
                    contentActualHeight = Blocks.Select(x => x.TotalHeight).Sum();
                } else {
                    foreach (var block in Blocks) {
                        if (IsFitContentHeight && block.HeightStyle == SizeStyle.FitParent) {
                            contentActualHeight = Math.Max(block.TotalActualHeight, contentActualHeight ?? 0);
                        } else {
                            contentActualHeight = Math.Max(block.TotalHeight, contentActualHeight ?? 0);
                        }
                    }
                }

                if (ActiveStyleRule.ContentDirection == ContentDirectionStyle.Row) {
                    contentActualWidth = Blocks.Select(x => x.TotalWidth).Sum();
                } else {
                    foreach (var block in Blocks) {
                        if (IsFitContentWidth && block.WidthStyle == SizeStyle.FitParent) {
                            contentActualWidth = Math.Max(block.TotalActualWidth, contentActualWidth ?? 0);
                        } else {
                            contentActualWidth = Math.Max(block.TotalWidth, contentActualWidth ?? 0);
                        }
                    }
                }
            } else {
                contentActualHeight = 0;
                contentActualWidth = 0;
            }
        }
    }

    public interface IMenuContainer : IMenuBlock {
        IReadOnlyList<IMenuBlock> Blocks { get; }
        IMenuBlock GetBlockAtPoint(Point point, MouseButton? mouseButton = default);

        void Add(IMenuBlock block);
        void Remove(IMenuBlock block);
        bool Contains(IMenuBlock block);
    }
}
