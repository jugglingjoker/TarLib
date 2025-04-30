using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using TarLib.Input;
using TarLib.Primitives;

namespace TarLib.States {

    public class InterfaceMenuWindow : InterfaceMenuWindow<IMenuContainer, IMenuContainer, IMenuContainer, IMenuContainer> {
        public InterfaceMenuWindow(
            string id, 
            IMenuContainer topControls = default,
            IMenuContainer bottomControls = default,
            IMenuContainer leftControls = default,
            IMenuContainer rightControls = default,
            IGameMenu menu = null) : base(id, topControls ?? new MenuContainer(menu), bottomControls ?? new MenuContainer(menu), leftControls ?? new MenuContainer(menu), rightControls ?? new MenuContainer(menu), menu) {

        }
    }

    public class InterfaceMenuWindow<TTopControls, TBottomControls, TLeftControls, TRightControls> : MenuWindow<InterfaceMenuWindow<TTopControls, TBottomControls, TLeftControls, TRightControls>.WindowContents>
        where TTopControls : IMenuContainer
        where TBottomControls : IMenuContainer
        where TLeftControls : IMenuContainer
        where TRightControls : IMenuContainer {

        public InterfaceMenuWindow(string id, TTopControls topControls, TBottomControls bottomControls, TLeftControls leftControls, TRightControls rightControls, IGameMenu menu = null) : base(
                id: id,
                menu: menu,
                menuBlocks: default,
                screenVerticalAlign: AlignStyle.Start,
                screenHorizontalAlign: AlignStyle.Start,
                exactPosition: default,
                title: default,
                hasCloseButton: false,
                canMovePosition: false,
                rememberPosition: true,
                alwaysOnTop: false,
                captureInput: false) {

            Contents.SetControls(topControls, bottomControls, leftControls, rightControls);

            DefaultStyle = new MenuBlockStyleRule() {
                WidthStyle = SizeStyle.FitParent,
                HeightStyle = SizeStyle.FitParent,
                ContentDirection = ContentDirectionStyle.Row,
            };
        }

        public TTopControls TopControls => Contents.TopControls;
        public TBottomControls BottomControls => Contents.BottomControls;
        public TLeftControls LeftControls => Contents.LeftControls;
        public TRightControls RightControls => Contents.RightControls;

        public RectanglePrimitive MiddleDimensions => Menu != null ? RectanglePrimitive.FromPoints(new Vector2(LeftControls?.TotalWidth ?? 0, TopControls?.TotalHeight ?? 0), new Vector2(Menu.State.BaseGame.ScreenDimensions.Width - RightControls?.TotalWidth ?? 0, Menu.State.BaseGame.ScreenDimensions.Height - BottomControls?.TotalHeight ?? 0)) : default;

        protected override MenuBlockStyleTypeList StyleTypes => MenuBlockStyleType.InterfaceMenu + base.StyleTypes;

        public class WindowContents : MenuWindowContents {
            private MiddleControls middleControls;

            public WindowContents() {
                DefaultStyle = new MenuBlockStyleRule() {
                    ContentDirection = ContentDirectionStyle.Column,
                    ContentHorizontalAlign = AlignStyle.Center,
                    ContentVerticalAlign = AlignStyle.SpaceBetween,
                    WidthStyle = SizeStyle.FitParent,
                    HeightStyle = SizeStyle.FitParent,
                };
            }

            internal void SetControls(TTopControls topControls, TBottomControls bottomControls, TLeftControls leftControls, TRightControls rightControls) {
                Clear();

                TopControls = topControls;
                middleControls = new MiddleControls(Window as InterfaceMenuWindow<TTopControls, TBottomControls, TLeftControls, TRightControls>, leftControls, rightControls);
                BottomControls = bottomControls;

                Add(TopControls);
                Add(middleControls);
                Add(BottomControls);
            }

            public TTopControls TopControls { get; private set; }
            public TBottomControls BottomControls { get; private set; }
            public TLeftControls LeftControls => middleControls.LeftControls;
            public TRightControls RightControls => middleControls.RightControls;
        }

        public class MiddleControls : MenuContainerPair<TLeftControls, TRightControls> {
            public MiddleControls(InterfaceMenuWindow<TTopControls, TBottomControls, TLeftControls, TRightControls> interfaceMenu, TLeftControls firstBlock, TRightControls secondBlock, IGameMenu menu = null) : base(firstBlock, secondBlock, menu) {
                InterfaceMenu = interfaceMenu;
                
                DefaultStyle = new MenuBlockStyleRule() {
                    ContentDirection = ContentDirectionStyle.Row,
                    ContentVerticalAlign = AlignStyle.Center,
                    ContentHorizontalAlign = AlignStyle.SpaceBetween,
                    WidthStyle = SizeStyle.FitParent,
                    HeightStyle = SizeStyle.Exact,
                    Height = new MiddleControlsHeightStyle(this),
                };
            }

            public TLeftControls LeftControls => FirstBlock;
            public TRightControls RightControls => SecondBlock;
            public InterfaceMenuWindow<TTopControls, TBottomControls, TLeftControls, TRightControls> InterfaceMenu { get; }

            public class MiddleControlsHeightStyle : IntStyle {
                public MiddleControlsHeightStyle(MiddleControls controls) {
                    Controls = controls;
                }

                public MiddleControls Controls { get; }
                public override int Value => Controls.Menu.State.BaseGame.ScreenDimensions.Height - (Controls.InterfaceMenu.Contents.TopControls.TotalHeight + Controls.InterfaceMenu.Contents.BottomControls.TotalHeight);
            }
        }
    }
}
