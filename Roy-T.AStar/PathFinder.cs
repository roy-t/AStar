using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace RoyT.AStar
{
    /// <summary>
    /// Computes a path in a grid according to the A* algorithm
    /// </summary>
    internal static partial class PathFinder
    {
        /// <summary>
        /// Find path
        /// </summary>
        /// <param name="grid">Grid</param>
        /// <param name="start">Start point</param>
        /// <param name="end">End point</param>
        /// <param name="movementPattern">Movement pattern</param>
        /// <param name="shape">Shape of an agent</param>
        /// <param name="iterationLimit">Maximum count of iterations</param>
        /// <returns>List of movement steps</returns>
        public static List<Position> FindPath(Grid grid, Position start, Position end, Offset[] movementPattern, AgentShape shape, int iterationLimit = int.MaxValue)
        {
            ClearStepList();

            // If is where should be, then path is simple
            if (start == end)
            {
                return new List<Position> { start };
            }

            // To avoid lot of grid boundaries checking calculations during path finding, do the math here.
            // So get the possible boundaries considering the shape of the agent.
            var boundary = new Boundary()
            {
                X1 = -shape.TopLeft.X,
                Y1 = -shape.TopLeft.Y,
                X2 = grid.DimX - shape.BottomRight.X - 1,
                Y2 = grid.DimY - shape.BottomRight.Y - 1
            };

            // Make sure start and end are within boundary
            if (!boundary.IsInside(start) || !boundary.IsInside(end))
            {
                // Can't find such path
                return null;
            }

            var head = new MinHeapNode(start, ManhattanDistance(start, end));
            var open = new MinHeap();
            open.Push(head);

            var costSoFar = new float[grid.DimX * grid.DimY];
            var cameFrom = new Position[grid.DimX * grid.DimY];
            
            while (open.HasNext() && iterationLimit > 0)
            {
                // Get the best candidate
                var current = open.Pop().Position;
                MessageCurrent(current, PartiallyReconstructPath(grid, start, current, cameFrom));

                if (current == end)
                {
                    return ReconstructPath(grid, start, end, cameFrom);
                }

                Step(grid, boundary, open, cameFrom, costSoFar, movementPattern, shape, current, end);

                MessageClose(current);

                --iterationLimit;
            }

            return null;
        }

        private static void Step(
            Grid grid,
            Boundary boundary,
            MinHeap open,
            Position[] cameFrom,
            float[] costSoFar,
            Offset[] movementPattern,
            AgentShape shape,
            Position current,
            Position end)
        {
            // Get the cost associated with getting to the current position
            var initialCost = costSoFar[grid.GetIndexUnchecked(current)];

            // Get all directions we can move to according to the movement pattern and the dimensions of the grid
            foreach (var option in GetMovementOptions(current, boundary, movementPattern))
            {
                var position = current + option;
                var cellCost = grid.GetCellCostUnchecked(position, shape);

                // Ignore this option if the cell is blocked
                if (float.IsInfinity(cellCost))
                    continue;

                var index = grid.GetIndexUnchecked(position);

                // Compute how much it would cost to get to the new position via this path
                var newCost = initialCost + cellCost * option.Cost;

                // Compare it with the best cost we have so far, 0 means we don't have any path that gets here yet
                var oldCost = costSoFar[index];
                if (!(oldCost <= 0) && !(newCost < oldCost))
                    continue;

                // Update the best path and the cost if this path is cheaper
                costSoFar[index] = newCost;
                cameFrom[index] = current;

                // Use the heuristic to compute how much it will probably cost 
                // to get from here to the end, and store the node in the open list
                var expectedCost = newCost + ManhattanDistance(position, end);
                open.Push(new MinHeapNode(position, expectedCost));

                MessageOpen(position);
            }
        }

        private static List<Position> ReconstructPath(Grid grid, Position start, Position end, Position[] cameFrom)
        {
            var path = new List<Position> { end };
            var current = end;
            do
            {
                var previous = cameFrom[grid.GetIndexUnchecked(current)];
                current = previous;
                path.Add(current);
            } while (current != start);

            return path;
        }

        /// <summary>
        /// Get movement options within boundary
        /// </summary>
        /// <param name="position">Current positin</param>
        /// <param name="boundary">Boundary</param>
        /// <param name="movementPattern">Movement pattern</param>
        /// <returns></returns>
        private static IEnumerable<Offset> GetMovementOptions(
            Position position,
            Boundary boundary,
            IEnumerable<Offset> movementPattern)
        {
            return movementPattern.Where(m => boundary.IsInside(position + m));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float ManhattanDistance(Position p0, Position p1)
        {
            var dx = Math.Abs(p0.X - p1.X);
            var dy = Math.Abs(p0.Y - p1.Y);
            return dx + dy;
        }
    }
}
