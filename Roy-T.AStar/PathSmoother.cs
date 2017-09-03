using System;
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

            // The tail of the string slowly moves forward
            for (var tail = 0; tail < path.Length - 2; tail++)
            {
                // While the head of the string moves from its max length (or the end of the path
                // towards the tail
                for (var head = Math.Min(path.Length - 1, tail + maxSmoothDistance); head > tail + 1; head--)
                {
                    // Meanwhile a path segment between the head and tail is adjusted to
                    // fit on a straight line between the head and the tail, if possible.
                    var current = tail + 1;

                    MessageIterate(path[current], path[current]);

                    // Find if there is a cell on the straightest path between head and tail
                    // using the movement pattern of the agent
                    var direction = GetDirection(path[tail], path[head], movementPattern);
                    if (!direction.HasValue)
                        continue;

                    var originalPosition = path[current];
                    var candidatePosition = path[current - 1] + direction.Value;

                    // If the best candidate is what we've already got, we can't improve
                    if (originalPosition == candidatePosition)
                        continue;

                    // If the candidate would disconnect our path we can't improve
                    if (!Connected(candidatePosition, path[current + 1], movementPattern))
                        continue;

                    var originalCost = grid.GetCellCostUnchecked(originalPosition);
                    var candidateCost = grid.GetCellCostUnchecked(candidatePosition);

                    // If the candidate is more costly than our original we can't improve
                    if (candidateCost > originalCost)
                        continue;

                    path[current] = candidatePosition;

                    MessageSwap(originalPosition, candidatePosition);
                }
            }
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
