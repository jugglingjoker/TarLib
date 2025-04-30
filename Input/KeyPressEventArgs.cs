using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace TarLib.Input {
    public class KeyPressEventArgs : EventArgs {
        private readonly HashSet<Keys> activeKeys;
        public IReadOnlySet<Keys> ActiveKeys => activeKeys;

        private readonly HashSet<Keys> usedKeys = new HashSet<Keys>();
        public IReadOnlySet<Keys> UsedKeys => usedKeys;

        public HashSet<Keys> AvailableKeys => ActiveKeys.Except(UsedKeys).ToHashSet();

        public Point MousePosition { get; }
        public GameTime GameTime { get; }

        public KeyPressEventArgs(HashSet<Keys> keys, GameTime gameTime) {
            activeKeys = keys.ToHashSet();
            GameTime = gameTime;
        }

        public void FlagKeysAsUsed(IEnumerable<Keys> buttons) {
            foreach (var button in buttons) {
                usedKeys.Add(button);
            }
        }

        public static implicit operator KeyPressEventArgs((HashSet<Keys> keys, GameTime gameTime) e) {
            return new KeyPressEventArgs(e.keys, e.gameTime);
        }
    }
}
