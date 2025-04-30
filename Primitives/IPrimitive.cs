using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace TarLib.Primitives {

    public interface IPrimitiveWithCenter : IPrimitive {
        Vector2 Center { get; }
    }

    public interface IPrimitive {
        bool DoesIntersect(Vector2 point);
        bool DoesIntersect(IPrimitive primitive);
        float DistanceTo(Vector2 point);
        float DistanceTo(IPrimitive primitive);
        List<Vector2> GetIntersectPoints(IPrimitive primitive);
    }
}
