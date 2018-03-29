using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RoyT.AStar
{
    /// <summary>
    /// Smooths a path using the string pulling algorithm
    /// </summary>
    public static partial class PathSmoother
    {
        /// <summary>
        /// Smooths a path, without affecting its cost, using the string pulling algorithm.
        /// </summary>
        /// <param name="grid">The grid</param>
        /// <param name="path">The path (updated in place)</param>
        /// <param name="movementPattern">Movement pattern for the agent</param>
        /// <param name="maxSmoothDistance">Max distance to consider when 'pulling the string'. A larger distance smooths the path over longer distances but costs more</param>
        public static void SmoothPath(Grid grid, Position[] path, Offset[] movementPattern, int maxSmoothDistance)
        {
            ClearSwapList();

            for (var stringLength = 1; stringLength < maxSmoothDistance; stringLength++)
            {
                for (var position = 0; (position + stringLength + 1) < path.Length; position++)
                {
                    var head = path[position];
                    var tail = path[position + stringLength + 1];

                    var line = Line(head, tail);
                    for (var i = 0; i < line.Count - 2; i++)
                    {
                        MessageIterate(path[position + i + 1], path[position + i + 1]);

                        var before = path[position + i];
                        var original = path[position + i + 1];
                        var after = path[position + i + 2];

                        var candidate = line[i + 1];

                        if(original == candidate)
                            continue;

                        var originalCost = grid.GetCellCostUnchecked(original);
                        var candidateCost = grid.GetCellCostUnchecked(candidate);                        
                        if (candidateCost > originalCost)
                            continue;

                        if (!Connected(before, candidate, movementPattern) ||
                            !Connected(candidate, after, movementPattern))
                            continue;

                        path[position + i + 1] = candidate;

                        MessageSwap(original, candidate);
                    }
                }
            }



            //// The tail of the string slowly moves forward
            //for (var tail = 0; tail < path.Length - 2; tail++)
            //{
            //    // While the head of the string moves from its max length (or the end of the path
            //    // towards the tail
            //    for (var head = Math.Min(path.Length - 1, tail + maxSmoothDistance); head > tail + 1; head--)
            //    {
            //        // Meanwhile a path segment between the head and tail is adjusted to
            //        // fit on a straight line between the head and the tail, if possible.
            //        var current = tail + 1;

            //        MessageIterate(path[current], path[current]);

            //        // Find if there is a cell on the straightest path between head and tail
            //        // using the movement pattern of the agent
            //        var direction = GetDirection(path[tail], path[head], movementPattern);
            //        if (!direction.HasValue)
            //            continue;

            //        var originalPosition = path[current];
            //        var candidatePosition = path[current - 1] + direction.Value;

            //        // If the best candidate is what we've already got, we can't improve
            //        if (originalPosition == candidatePosition)
            //            continue;

            //        // If the candidate would disconnect our path we can't improve
            //        if (!Connected(candidatePosition, path[current + 1], movementPattern))
            //            continue;

            //        var originalCost = grid.GetCellCostUnchecked(originalPosition);
            //        var candidateCost = grid.GetCellCostUnchecked(candidatePosition);

            //        // If the candidate is more costly than our original we can't improve
            //        if (candidateCost > originalCost)
            //            continue;

            //        path[current] = candidatePosition;

            //        MessageSwap(originalPosition, candidatePosition);
            //    }
            //}
        }


        public static List<Position> Line(Position start, Position end)
        {
            int x = start.X;
            int y = start.Y;
            int x2 = end.X;
            int y2 = end.Y;            
            var positions = new List<Position>();

            int w = x2 - x;
            int h = y2 - y;
            int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
            if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
            if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
            if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;
            int longest = Math.Abs(w);
            int shortest = Math.Abs(h);
            if (!(longest > shortest))
            {
                longest = Math.Abs(h);
                shortest = Math.Abs(w);
                if (h < 0) dy2 = -1; else if (h > 0) dy2 = 1;
                dx2 = 0;
            }
            int numerator = longest >> 1;
            for (int i = 0; i <= longest; i++)
            {
                positions.Add(new Position(x, y));                
                numerator += shortest;
                if (!(numerator < longest))
                {
                    numerator -= longest;
                    x += dx1;
                    y += dy1;
                }
                else
                {
                    x += dx2;
                    y += dy2;
                }
            }

            return positions;
        }

        /// <summary>
        /// Checks if two positions are connected according to the given movementPattern
        /// </summary>        
        private static bool Connected(Position a, Position b, Offset[] movementPattern)
        {
            return movementPattern.Any(offset => a + offset == b);
        }

        /// <summary>
        /// Computes a step in the direction from a to b, if possible with the given movementPattern
        /// </summary>        
        private static Offset? GetDirection(Position a, Position b, Offset[] movementPattern)
        {
            double dX = b.X - a.X;
            double dY = b.Y - a.Y;
            var length = Math.Sqrt(dX * dX + dY * dY);
            var x = Math.Round(dX / length);
            var y = Math.Round(dY / length);

            var direction = new Offset((int)x, (int)y);
            if (movementPattern.Contains(direction))
            {
                return direction;
            }

            return null;
        }
    }
}
