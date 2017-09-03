using System;
using System.Linq;

namespace RoyT.AStar
{
    public static partial class PathSmoother
    {
        public static void SmoothPath(Grid grid, Position[] path, Offset[] movementPattern, int maxLength)
        {
            ClearSwapList();

            var maxDistance = Math.Min(path.Length - 2, maxLength);
            for (var i = maxDistance; i > 1; i--)
            {
                SmoothStep(i, grid, path, movementPattern);
            }
        }

        private static void SmoothStep(int distance, Grid grid, Position[] path, Offset[] movementPattern)
        {
            var tail = -1;
            var current = 0;            

            for(var head = distance; head < path.Length; head++)
            {
                tail++;
                current++;

                // Find the cell on a straight path between tail and head, only consider
                // this if its a legal move according to the movement pattern
                var direction = GetDirection(path[tail], path[head], movementPattern);
                if (!direction.HasValue)
                    continue;
                
                var originalPosition = path[current];
                var candidatePosition = path[tail] + direction.Value;

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

        private static bool Connected(Position a, Position b, Offset[] movementPattern)
        {
            return movementPattern.Any(offset => a + offset == b);
        }

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
