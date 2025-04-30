using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TarLib.Graphics;

namespace TarLib.States {
    public interface IGameStateViewLayer {
        List<EffectDefinition> Effects { get; }

        void Draw(GameTime gameTime, SpriteBatch spriteBatch, float startDepth, float endDepth);
        void LoadContent();
        void UnloadContent();
    }
}
