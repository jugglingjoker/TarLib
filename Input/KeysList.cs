using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace TarLib.Input {
    public static class KeysList {

        private static HashSet<Keys> allVisible;
        public static IReadOnlySet<Keys> AllVisible {
            get {
                if(allVisible == null) {
                    allVisible = Alphanumeric.Concat(Symbols).ToHashSet();
                }
                return allVisible;
            }
        }

        private static HashSet<Keys> alphanumeric;
        public static IReadOnlySet<Keys> Alphanumeric {
            get {
                if (alphanumeric == null) {
                    alphanumeric = Alpha.Concat(Numeric).ToHashSet();
                }
                return alphanumeric;
            }
        }

        private static HashSet<Keys> alpha = new HashSet<Keys> { Keys.A, Keys.B, Keys.C, Keys.D, Keys.E, Keys.F, Keys.G, Keys.H, Keys.I, Keys.J, Keys.K, Keys.L, Keys.M, Keys.N, Keys.O, Keys.P, Keys.Q, Keys.R, Keys.S, Keys.T, Keys.U, Keys.V, Keys.W, Keys.X, Keys.Y, Keys.Z };
        public static IReadOnlySet<Keys> Alpha => alpha;

        private static HashSet<Keys> numeric = new HashSet<Keys> { Keys.D0, Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9, Keys.NumPad0, Keys.NumPad1, Keys.NumPad2, Keys.NumPad3, Keys.NumPad4, Keys.NumPad5, Keys.NumPad6, Keys.NumPad7, Keys.NumPad8, Keys.NumPad9 };
        public static IReadOnlySet<Keys> Numeric => numeric;

        private static HashSet<Keys> symbols = new HashSet<Keys> { Keys.Space, Keys.OemTilde, Keys.OemMinus, Keys.OemPlus, Keys.OemOpenBrackets, Keys.OemCloseBrackets, Keys.OemPipe, Keys.OemSemicolon, Keys.OemQuotes, Keys.OemComma, Keys.OemPeriod, Keys.OemBackslash, Keys.OemQuestion, Keys.Add, Keys.Subtract, Keys.Divide, Keys.Multiply, Keys.Decimal };
        public static IReadOnlySet<Keys> Symbols => symbols;

        private static HashSet<Keys> anyShift = new HashSet<Keys> { Keys.LeftShift, Keys.RightShift };
        public static IReadOnlySet<Keys> AnyShift => anyShift;

        private static HashSet<Keys> anyControl = new HashSet<Keys> { Keys.LeftControl, Keys.RightControl };
        public static IReadOnlySet<Keys> AnyControl => anyControl;

        private static HashSet<Keys> anyAlt = new HashSet<Keys> { Keys.LeftAlt, Keys.RightAlt };
        public static IReadOnlySet<Keys> AnyAlt => anyAlt;
    }
}
