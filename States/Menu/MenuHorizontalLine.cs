using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TarLib.Entities.Drawable;
using TarLib.Extensions;

namespace TarLib.States {
    public class MenuHorizontalLine : MenuBlock {

        public MenuHorizontalLine(IGameMenu menu = default) : base(menu) {
            DefaultStyle = new MenuBlockStyleRule() {
                BackgroundColor = Color.Black,
                WidthStyle = SizeStyle.FitParent,
                HeightStyle = SizeStyle.Exact,
                Height = 3,
            };
        }

        public override int ContentActualWidth => 0;
        public override int ContentActualHeight => 0;
        protected override MenuBlockStyleTypeList StyleTypes => MenuBlockStyleType.HorizontalLine;

        protected override void DrawContent(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position, float startDepth, float endDepth) {
            // do nothing
        }
    }
}
