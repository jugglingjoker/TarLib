using Microsoft.Xna.Framework;
using System;

namespace TarLib.Entities.Path {
    public class ManhattanPathHeuristic : IPathHeuristic {
        public int GetCost(Point start, Point end) => Math.Abs(end.X - start.X) * 10 + Math.Abs(end.Y - start.Y) * 10;
    }
}
