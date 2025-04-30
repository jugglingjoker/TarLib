using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TarLib.Entities.Path {

    public class PathProvider<TEntityWithPathProvider> : PathProvider<TEntityWithPathProvider, object>
        where TEntityWithPathProvider : IEntityWithPathProvider {
        public PathProvider(TEntityWithPathProvider entity) : base(entity) {

        }
    }

    public class PathProvider<TEntityWithPathProvider, TEntity>
        where TEntityWithPathProvider : IEntityWithPathProvider<TEntity> {

        private const int POINT_UNEVALUATED = 0;

        private List<Point> openPoints = new();
        private HashSet<Point> closedPoints = new();
        private Dictionary<Point, Dictionary<Point, List<Point>>> pathsByStartAndEnd = new();
        private Dictionary<Point, List<List<Point>>> pathsByPoint = new();

        private int[,] pathCosts;
        private Point?[,] parents;

        public PathProvider(TEntityWithPathProvider entity) {
            EntityWithPathProvider = entity;
            EntityWithPathProvider.OnPathObstacleChange += Entity_OnPathObstacleChange;
            Init();
        }

        private void Entity_OnPathObstacleChange(object sender, PathObstacleChangeEventArgs e) {
            // cull everything that intersects newly added objects
            // cull everything adjacent to newly removed objects
        }

        private void Init() {
            pathCosts = new int[EntityWithPathProvider.Width, EntityWithPathProvider.Height];
            parents = new Point?[EntityWithPathProvider.Width, EntityWithPathProvider.Height];
            for (int i = 0; i < pathCosts.GetLength(0); i++) {
                for (int j = 0; j < pathCosts.GetLength(1); j++) {
                    pathCosts[i, j] = POINT_UNEVALUATED;
                    parents[i, j] = default;
                }
            }
        }

        public int? CalculatePathCost(TEntity entity, IReadOnlyList<Point> path) {
            var totalCost = 0;
            Point? previous = default;
            for(int i = 0; i < path.Count - 1; i++) {
                var start = path[i];
                var end = path[i + 1];
                var cost = CostBetweenPoints(entity, previous, start, end);
                if(cost != default) {
                    totalCost += cost.Value;
                } else {
                    return default;
                }
                previous = start;
            }
            return totalCost;
        }

        public List<Point> CalculateBestPath(TEntity entity, Point start, IReadOnlyList<Point> ends) {
            // TODO: add a more performant method using dijkstra
            List<Point> bestPath = default;
            int bestPathCost = 0;
            foreach(var end in ends) {
                var path = CalculatePath(entity, start, end);
                if(path != default) {
                    var pathCost = CalculatePathCost(entity, path);
                    if (pathCost != default) {
                        if (bestPath == default || pathCost < bestPathCost) {
                            bestPath = path;
                            bestPathCost = pathCost.Value;
                        }
                    }
                }
            }
            return bestPath;
        }

        public List<Point> CalculatePath(TEntity entity, Point start, Point end) {
            if (start == end) {
                var emptyPath = new List<Point> {
                    start
                };
                return emptyPath;
            }
            if (start.X < 0
                || end.X < 0
                || start.X >= EntityWithPathProvider.Width
                || end.X >= EntityWithPathProvider.Width
                || start.Y < 0
                || end.Y < 0
                || start.Y >= EntityWithPathProvider.Height
                || end.Y >= EntityWithPathProvider.Height) {
                return default; // no path possible
            }
            if (pathsByStartAndEnd.ContainsKey(start)) {
                if (pathsByStartAndEnd[start].ContainsKey(end)) {
                    return pathsByStartAndEnd[start][end].ToList();
                }
            }

            openPoints.Add(start);
            var endReached = false;
            do {
                var currentPoint = openPoints.First();
                var currentCost = pathCosts[currentPoint.X, currentPoint.Y];

                openPoints.Remove(currentPoint);
                closedPoints.Add(currentPoint);

                for (int i = Math.Max(0, currentPoint.X - 1); !endReached && i <= currentPoint.X + 1 && i < EntityWithPathProvider.Width; i++) {
                    for (int j = Math.Max(0, currentPoint.Y - 1); !endReached && j <= currentPoint.Y + 1 && j < EntityWithPathProvider.Height; j++) {
                        if (i != currentPoint.X || j != currentPoint.Y) {
                            var evaluatedPoint = new Point(i, j);
                            if (closedPoints.Contains(evaluatedPoint) || !EntityWithPathProvider.IsValidPathMovementBetweenPoints(entity, parents[currentPoint.X, currentPoint.Y], currentPoint, evaluatedPoint)) {
                                continue;
                            }

                            var evaluatedCost = CostBetweenPoints(entity, parents[currentPoint.X, currentPoint.Y], currentPoint, evaluatedPoint);
                            if (evaluatedCost == default) {
                                continue;
                            }

                            var totalCost = currentCost + evaluatedCost.Value;
                            if (openPoints.Contains(evaluatedPoint)) {
                                var comparedCost = pathCosts[i, j];
                                if (totalCost < comparedCost) {
                                    parents[i, j] = currentPoint;
                                    pathCosts[i, j] = totalCost;
                                }
                            } else {
                                openPoints.Add(evaluatedPoint);
                                parents[i, j] = currentPoint;
                                pathCosts[i, j] = totalCost;
                            }

                            if (evaluatedPoint == end) {
                                endReached = true;
                            }
                        }
                    }
                }

                openPoints.Sort((point1, point2) => {
                    var cost1 = pathCosts[point1.X, point1.Y] + Heuristic.GetCost(point1, end);
                    var cost2 = pathCosts[point2.X, point2.Y] + Heuristic.GetCost(point2, end);
                    return cost1.CompareTo(cost2);
                });

            } while (!endReached && openPoints.Count > 0);

            List<Point> path = default;
            if (endReached) {
                var nodes = new List<Point>();
                var currentNode = end;
                nodes.Add(currentNode);
                do {

                    currentNode = parents[currentNode.X, currentNode.Y].Value;
                    nodes.Add(currentNode);
                } while (currentNode != start);

                nodes.Reverse();
                path = new(nodes);
                if (CanCache) {
                    CachePath(path);
                }
            }

            openPoints.Clear();
            closedPoints.Clear();
            for (int i = 0; i < parents.GetLength(0); i++) {
                for (int j = 0; j < parents.GetLength(1); j++) {
                    parents[i, j] = default;
                    pathCosts[i, j] = POINT_UNEVALUATED;
                }
            }

            return path;
        }

        private int? CostBetweenPoints(TEntity entity, Point? previous, Point start, Point end) {
            var dist = end - start;
            var turningCost = (dist != start - previous) ? 2 : 1;
            // TODO: change isturning into a scalable factor based on turn radius

            // adjacent
            if (CanMoveStraight && (dist.X == 0 || dist.Y == 0)) {
                return EntityWithPathProvider.PathCostModifierBetweenPoints(entity, previous, start, end) * StraightMovementCost * turningCost;
            }
            // diagonal
            if (CanMoveDiagonal && Math.Abs(dist.X) <= 1 && Math.Abs(dist.X) <= 1) {
                return EntityWithPathProvider.PathCostModifierBetweenPoints(entity, previous, start, end) * DiagonalMovementCost * turningCost;
            }

            return null;
        }

        private void CachePath(List<Point> path) {
            for (int i = 0; i < path.Count(); i++) {
                var firstNode = path[i];
                for (int j = i + 1; j < path.Count(); j++) {
                    var secondNode = path[j];
                    if (firstNode != secondNode) {
                        var truncatedPath = path.GetRange(i, j - i + 1);

                        if (!pathsByStartAndEnd.ContainsKey(firstNode)) {
                            pathsByStartAndEnd[firstNode] = new();
                        }
                        pathsByStartAndEnd[firstNode][secondNode] = truncatedPath;

                        foreach (var node in truncatedPath) {
                            if (!pathsByPoint.ContainsKey(node)) {
                                pathsByPoint[node] = new();
                            }
                            pathsByPoint[node].Add(truncatedPath);
                        }
                    }
                }
            }
        }

        public TEntityWithPathProvider EntityWithPathProvider { get; }

        public int StraightMovementCost { get; set; } = 10;
        public bool CanMoveStraight { get; set; } = true;
        public int DiagonalMovementCost { get; set; } = 14;
        public bool CanMoveDiagonal { get; set; } = true;
        public IPathHeuristic Heuristic { get; set; } = ManhattanHeuristic;
        public bool CanCache { get; set; } = true;

        public static ManhattanPathHeuristic ManhattanHeuristic => new ManhattanPathHeuristic();
        public static DijkstraPathHeuristic DijkstraHeuristic => new DijkstraPathHeuristic();
    }
}
