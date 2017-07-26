using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace RoyT.AStar
{
    /// <summary>
    /// Computes a path in a grid according to the A* algorithm
    /// </summary>
    internal static class PathFinder
    {
        public static SearchNode FindPath(Grid grid, Position start, Position end)
            // Flip start and end, since the algorithm will give the result from the end backwards
            => FindReversePath(grid, end, start);

        private static SearchNode FindReversePath(Grid grid, Position start, Position end)
        {
            var head = new SearchNode(start);
            var open = new MinHeap();
            open.Push(head);

            var marked = new BitArray(grid.DimX * grid.DimY);

            while (open.HasNext())
            {
                var current = open.Pop();

                if (current.Position.Equals(end))
                {
                    return current;
                }

                foreach (var p in GetNeighbours(current.Position, grid.DimX, grid.DimY))
                {
                    var index = grid.DimX * p.Y + p.X;
                    var cellCost = grid.GetCellCost(p);
                    if (!marked[index] && !double.IsInfinity(cellCost))
                    {
                        marked[index] = true;
                        
                        // Avoid zig-zag paths by correctly penalizing the cost of diagonal movement
                        var costSoFar    = current.CostSoFar + Distance(current.Position, p) * cellCost;                        
                        var expectedCost = costSoFar + Distance(p, end);

                        open.Push(new SearchNode(p, expectedCost, costSoFar) {Next = current});
                    }
                }               
            }

            return null;
        }

        private static IEnumerable<Position> GetNeighbours(Position position, int dimX, int dimY)
        {
            var x = position.X;
            var y = position.Y;

            var neighbours = new List<Position>
            {                
                new Position(x - 1, y - 1), new Position(x, y - 1), new Position(x + 1, y - 1),
                new Position(x - 1, y),                             new Position(x + 1, y),
                new Position(x - 1, y + 1), new Position(x, y + 1), new Position(x + 1, y + 1)                
            };

            return neighbours.Where(p => p.X >= 0 && p.X < dimX && p.Y >= 0 && p.Y < dimY);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static double Distance(Position p0, Position p1)
        {
            var x0 = p0.X;
            var y0 = p0.Y;

            var x1 = p1.X;
            var y1 = p1.Y;

            return Math.Sqrt((x1 - x0) * (x1 - x0) + (y1 - y0) * (y1 - y0));
        }      
    }
}
