using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TarLib.Input;
using TarLib.Primitives;

namespace TarLib.States {
    public interface IMenuBlock {
        IGameMenu Menu { get; set; }

        void MouseMove(MouseMoveEventArgs e);
        void MouseHoverStart(MouseMoveEventArgs e);
        void MouseHoverEnd(MouseMoveEventArgs e);
        void MouseClickStart(MouseClickEventArgs e);
        void MouseClickEnd(MouseClickEventArgs e);
        void MouseClickCancel(MouseClickEventArgs e);
        void MouseScrollWheelChange(MouseScrollWheelChangeEventArgs e);
        void KeyPressStart(KeyPressEventArgs e);
        void KeyPressEnd(KeyPressEventArgs e);

        event EventHandler<MouseClickEventArgs> OnClickStart;
        event EventHandler<MouseClickEventArgs> OnClickEnd;
        event EventHandler<MouseClickEventArgs> OnClickCancel;
        event EventHandler<MouseMoveEventArgs> OnMouseMove;
        event EventHandler<MouseMoveEventArgs> OnMouseHoverStart;
        event EventHandler<MouseMoveEventArgs> OnMouseHoverEnd;
        event EventHandler<MouseScrollWheelChangeEventArgs> OnMouseScrollWheelChange;
        event EventHandler<KeyPressEventArgs> OnKeyPressStart;
        event EventHandler<KeyPressEventArgs> OnKeyPressEnd;

        void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position, float startDepth, float endDepth);
        void Update(GameTime gameTime);

        bool DoesIntersect(Point point);
        bool CanUseMouseButton(MouseButton mouseButton);

        int TotalWidth { get; }
        int TotalActualWidth { get; }
        int TotalActualHeight { get; }
        int TotalHeight { get; }
        int ContentWidth { get; }
        int ContentHeight { get; }
        int ContentActualWidth { get; }
        int ContentActualHeight { get; }
        bool IsFitContentWidth { get; }
        bool IsFitContentHeight { get; }

        SizeStyle WidthStyle { get; }
        SizeStyle HeightStyle { get; }
        ContentDirectionStyle ContentDirection { get; }

        RectanglePrimitive? HotZone { get; }
        void CalculateHotZone(Vector2 position);

        //string Id { get; set; }
        IMenuContainer Parent { get; set; }

        bool NeedsRefresh { get; set; }
        bool IsSelected { get; set; }
        bool IsVisible { get; set; }
        bool IsError { get; set; }
    }
}
