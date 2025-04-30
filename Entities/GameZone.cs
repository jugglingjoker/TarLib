using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TarLib.Primitives;

namespace TarLib.Entities {


    // TODO: move to own file
    public class RectangleGameZone : GameZone<RectanglePrimitive> {

        public enum StickyLocation {
            TopLeft,
            TopMiddle,
            TopRight,
            CenterLeft,
            Center,
            CenterRight,
            BottomLeft,
            BottomMiddle,
            BottomRight
        }

        public override Vector2 Position {
            get {
                if(ManualPosition != null) {
                    return ManualPosition.Value;
                }

                switch (Location) {
                    case StickyLocation.TopLeft:
                        return Area.TopLeft;
                    case StickyLocation.TopMiddle:
                        return Area.TopMiddle;
                    case StickyLocation.TopRight:
                        return Area.TopRight;
                    case StickyLocation.CenterLeft:
                        return Area.CenterLeft;
                    case StickyLocation.Center:
                        return Area.Center;
                    case StickyLocation.CenterRight:
                        return Area.CenterRight;
                    case StickyLocation.BottomLeft:
                        return Area.BottomLeft;
                    case StickyLocation.BottomMiddle:
                        return Area.BottomMiddle;
                    case StickyLocation.BottomRight:
                        return Area.BottomRight;
                    default:
                        return base.Position;
                }
            }
        }

        public StickyLocation Location { get; set; } = StickyLocation.Center;
        public Vector2? ManualPosition { get; set; }

        public RectangleGameZone(RectanglePrimitive zone, Vector2? manualPosition, StickyLocation? location) : base(zone) {
            ManualPosition = manualPosition;
            Location = location ?? StickyLocation.Center;
        }

        public RectangleGameZone(Vector2 zonePosition, Vector2 zoneSize, Vector2? manualPosition = default, StickyLocation? location = default) : this(RectanglePrimitive.FromPosition(zonePosition, zoneSize), manualPosition, location) {

        }

        public RectangleGameZone(float x, float y, float width, float height, Vector2? manualPosition = default, StickyLocation? location = default) : this(new RectanglePrimitive(x, y, width, height), manualPosition, location) {

        }
    }

    public abstract class GameZone<TPrimitive>
        where TPrimitive : IPrimitiveWithCenter {

        public TPrimitive Area { get; set; }
        public virtual Vector2 Position => Area.Center;

        public GameZone(TPrimitive area) {
            Area = area;
        }
    }
}
