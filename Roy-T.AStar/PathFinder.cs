using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace RoyT.AStar
{
    /// <summary>
    /// Computes a path in a grid according to the A* algorithm
    /// </summary>
    internal static  partial class PathFinder
    {
        public static SearchNode FindPath(Grid grid, Position start, Position end, Offset[] movementPattern)
            // Flip start and end, since the algorithm will give the result from the end backwards
            => FindReversePath(grid, end, start, movementPattern);

        private static SearchNode FindReversePath(Grid grid, Position start, Position end, Offset[] movementPattern)
        {
            ClearStepList();

            var head = new SearchNode(start);
            var open = new MinHeap();
            open.Push(head);

            var marked = new bool[grid.DimX * grid.DimY];

            while (open.HasNext())
            {
                var current = open.Pop();

                if (current.Position.Equals(end))
                {
                    return current;
                }

                MessageCurrent(current.Position);
                
                foreach (var p in GetNeighbours(current.Position, grid.DimX, grid.DimY, movementPattern))
                {
                    var index = grid.GetIndexUnchecked(p.X, p.Y);

                    // Use the unchecked variant here since GetNeighbours already filters out positions that are out of bounds
                    var cellCost = grid.GetCellCostUnchecked(p);
                    if (!marked[index] && !float.IsInfinity(cellCost))
                    {
                        marked[index] = true;
                        MessageOpen(p);
                                                
                        var costSoFar    = current.CostSoFar + cellCost;                        
                        var expectedCost = costSoFar + ChebyshevDistance(p, end);

                        open.Push(new SearchNode(p, expectedCost, costSoFar) {Next = current});
                    }
                }

                MessageClose(current.Position);
            }

            return null;
        }

        private static IEnumerable<Position> GetNeighbours(
            Position position,
            int dimX,
            int dimY,
            IEnumerable<Offset> movementPattern)
        {
            return movementPattern.Select(n => new Position(position.X + n.X, position.Y + n.Y))
                                .Where(p => p.X >= 0 && p.X < dimX && p.Y >= 0 && p.Y < dimY);
        }

        /// <summary>
        /// Chebyshev distance heuristic, more admisible for traversing a grid than 
        /// the euclidian distance.
        /// </summary>        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float ChebyshevDistance(Position p0, Position p1)
        {
            const float D = 1;
            const float D2 = 1;

            var dx = Math.Abs(p0.X - p1.X);
            var dy = Math.Abs(p0.Y - p1.Y);            

            return D * (dx + dy) + (D2 - 2 * D) * Math.Min(dx, dy);
        }
    }
}
