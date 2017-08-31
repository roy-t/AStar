using System;

namespace RoyT.AStar
{
    /// <summary>
    /// Uses the string pulling algorithm to smooth
    /// </summary>
    internal static class PathSmoother
    {        
        public static SearchNode SmoothPath(Grid grid, SearchNode start, Offset[] movementPattern,int maxLength = 10)
        {            
            var maxDistance = Math.Min(PathLength(start) - 2, maxLength);
            for (var i = maxDistance; i > 1; i--)
            {
                start = SmoothPath(i, grid, start, movementPattern);
                return start;// TEMP
            }

            return start;
        }

        private static SearchNode SmoothPath(int distance, Grid grid, SearchNode start, Offset[] movementPattern)
        {
            var tail = start;
            var middle = start?.Next;            
            var head = middle?.Next;
            
            // tail->middle->head (distance = 1)
            for (var i = 1; i < distance; i++)
            {
                head = head?.Next;
            }
            // tail->middle->...distance nodes...-> head

            while (head != null)
            {
                var original = middle;
                // Find the cell on a straight path between tail and head                
                var direction = Direction(tail.Position, head.Position, movementPattern);
                var subPosition = new Position(tail.Position.X + direction.X, tail.Position.Y + direction.Y);
                if (original.Position != subPosition 
                    && ConnectsToNext(original, subPosition, movementPattern))
                {
                    var originalCost = grid.GetCellCostUnchecked(middle.Position);
                    var subCost = grid.GetCellCostUnchecked(subPosition);

                    // The replacement can, at best, cost exactly as much as the cell its
                    // replacing, if it costs more, or if its blocked we do not smooth the path
                    if (subCost <= originalCost)
                    {
                        middle = new SearchNode(subPosition, original.ExpectedCost, original.CostSoFar)
                        {
                            Next = original.Next,
                        };
                        tail.Next = middle;                        
                    }
                }

                // Move forward one position
                tail   = tail.Next;
                middle = middle.Next;
                head   = head.Next;
            }

            return start;
        }


        private static bool ConnectsToNext(SearchNode current, Position moveTo, Offset[] movementPattern)
        {
            // TODO: check that the position we move to still connects to the next node
            throw new Exception();
        }

        private static int PathLength(SearchNode start)
        {
            var current = start;
            var i = 0;
            while (current != null)
            {
                ++i;
                current = current.Next;
            }

            return i;
        }

        private static Offset Direction(Position a, Position b, Offset[] movementPattern)
        {
            // TODO: use movementPattern to figure out if the offset we want to use is legal
            double dX = b.X - a.X;
            double dY = b.Y - a.Y;            
            var length = Math.Sqrt(dX * dX + dY * dY);
            var x = Math.Round(dX / length);
            var y = Math.Round(dY / length);                       

            return new Offset((int)x, (int)y);
        }
    }
}
