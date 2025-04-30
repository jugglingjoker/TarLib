
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace TarLib.States {
    public interface IMenuWindow : IMenuContainer {
        void Close();

        string Id { get; }
        bool AlwaysOnTop { get; }
        bool CaptureInput { get; }
        bool IsExclusive { get; }
        Vector2? ExactPosition { get; } 
        AlignStyle? ScreenVerticalAlign { get; }
        AlignStyle? ScreenHorizontalAlign { get; }

        void BeforeAddToView();
        void AfterAddToView();
        void BeforeRemoveFromView();
        void AfterRemoveFromView();
    }
}
