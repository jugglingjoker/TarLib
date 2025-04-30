using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace TarLib.Entities.Path {
    public interface IEntityWithPathProvider : IEntityWithPathProvider<object> {
        
    }

    public interface IEntityWithPathProvider<TEntity> {
        int Width { get; }
        int Height { get; }
        event EventHandler<PathObstacleChangeEventArgs> OnPathObstacleChange;

        bool IsValidPathMovementBetweenPoints(TEntity entity, Point? previous, Point start, Point end);
        int PathCostModifierBetweenPoints(TEntity entity, Point? previous, Point start, Point end);
        List<Point> CalculatePath(TEntity entity, Point start, Point end);
        List<Point> CalculateBestPath(TEntity entity, Point start, IReadOnlyList<Point> ends);
        int? CalculatePathCost(TEntity entity, IReadOnlyList<Point> path);
    }
}
