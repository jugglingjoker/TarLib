using Microsoft.Xna.Framework.Input;

namespace TarLib.Input {
    public static class KeysExtension {

        public static string ToActualString(this Keys key, bool shift = false, bool capsLock = false) {
            switch (key) {
                case Keys.A:
                    return shift ^ capsLock ? "A" : "a";
                case Keys.B:
                    return shift ^ capsLock ? "B" : "b";
                case Keys.C:
                    return shift ^ capsLock ? "C" : "c";
                case Keys.D:
                    return shift ^ capsLock ? "D" : "d";
                case Keys.E:
                    return shift ^ capsLock ? "E" : "e";
                case Keys.F:
                    return shift ^ capsLock ? "F" : "f";
                case Keys.G:
                    return shift ^ capsLock ? "G" : "g";
                case Keys.H:
                    return shift ^ capsLock ? "H" : "h";
                case Keys.I:
                    return shift ^ capsLock ? "I" : "i";
                case Keys.J:
                    return shift ^ capsLock ? "J" : "j";
                case Keys.K:
                    return shift ^ capsLock ? "K" : "k";
                case Keys.L:
                    return shift ^ capsLock ? "L" : "l";
                case Keys.M:
                    return shift ^ capsLock ? "M" : "m";
                case Keys.N:
                    return shift ^ capsLock ? "N" : "n";
                case Keys.O:
                    return shift ^ capsLock ? "O" : "o";
                case Keys.P:
                    return shift ^ capsLock ? "P" : "p";
                case Keys.Q:
                    return shift ^ capsLock ? "Q" : "q";
                case Keys.R:
                    return shift ^ capsLock ? "R" : "r";
                case Keys.S:
                    return shift ^ capsLock ? "S" : "s";
                case Keys.T:
                    return shift ^ capsLock ? "T" : "t";
                case Keys.U:
                    return shift ^ capsLock ? "U" : "u";
                case Keys.V:
                    return shift ^ capsLock ? "V" : "v";
                case Keys.W:
                    return shift ^ capsLock ? "W" : "w";
                case Keys.X:
                    return shift ^ capsLock ? "X" : "x";
                case Keys.Y:
                    return shift ^ capsLock ? "Y" : "y";
                case Keys.Z:
                    return shift ^ capsLock ? "Z" : "z";
                case Keys.D1:
                    return shift ? "!" : "1";
                case Keys.D2:
                    return shift ? "@" : "2";
                case Keys.D3:
                    return shift ? "#" : "3";
                case Keys.D4:
                    return shift ? "$" : "4";
                case Keys.D5:
                    return shift ? "%" : "5";
                case Keys.D6:
                    return shift ? "^" : "6";
                case Keys.D7:
                    return shift ? "&" : "7";
                case Keys.D8:
                    return shift ? "*" : "8";
                case Keys.D9:
                    return shift ? "(" : "9";
                case Keys.D0:
                    return shift ? ")" : "0";
                case Keys.NumPad1:
                    return "1";
                case Keys.NumPad2:
                    return "2";
                case Keys.NumPad3:
                    return "3";
                case Keys.NumPad4:
                    return "4";
                case Keys.NumPad5:
                    return "5";
                case Keys.NumPad6:
                    return "6";
                case Keys.NumPad7:
                    return "7";
                case Keys.NumPad8:
                    return "8";
                case Keys.NumPad9:
                    return "9";
                case Keys.NumPad0:
                    return "0";
                case Keys.Multiply:
                    return "*";
                case Keys.Space:
                    return " ";
                case Keys.Add:
                    return "+";
                case Keys.Subtract:
                    return "-";
                case Keys.Decimal:
                    return ".";
                case Keys.Divide:
                    return "/";
                case Keys.OemSemicolon:
                    return !shift ? ";" : ":";
                case Keys.OemMinus:
                    return !shift ? "-" : "_";
                case Keys.OemPlus:
                    return !shift ? "=" : "+";
                case Keys.OemComma:
                    return !shift ? "," : "<";
                case Keys.OemPeriod:
                    return !shift ? "." : ">";
                case Keys.OemQuestion:
                    return !shift ? "/" : "?";
                case Keys.OemTilde:
                    return !shift ? "`" : "~";
                case Keys.OemOpenBrackets:
                    return !shift ? "[" : "{";
                case Keys.OemPipe:
                    return !shift ? "\\" : "|";
                case Keys.OemCloseBrackets:
                    return !shift ? "]" : "}";
                case Keys.OemQuotes:
                    return !shift ? "'" : "\"";
                case Keys.OemBackslash:
                    return !shift ? "\\" : "|";
                default:
                    return " ";
            }
        }

    }
}
