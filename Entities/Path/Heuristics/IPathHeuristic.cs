using Microsoft.Xna.Framework;

namespace TarLib.Entities.Path {
    public interface IPathHeuristic {
        int GetCost(Point start, Point end);
    }
}
