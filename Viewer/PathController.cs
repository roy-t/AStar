using System.Collections.Generic;
using System.Linq;
using RoyT.AStar;

namespace Viewer
{
    internal sealed class PathController
    {
        public void ComputePath(IReadOnlyList<Cell> cells)
        {
            ClearPathState(cells);

            var lookup = new Dictionary<Position, Cell>();

            var grid = new Grid(10, 10);
            var start = new Position(0, 0);
            var end = new Position(9, 9);

            foreach (var cell in cells)
            {
                var position = new Position(cell.X, cell.Y);
                lookup.Add(position, cell);

                switch (cell.CellState)
                {
                    case CellState.Normal:
                        grid.SetCellCost(position, cell.Cost);
                        break;
                    case CellState.Start:
                        start = position;
                        break;
                    case CellState.End:
                        end = position;
                        break;
                    case CellState.Blocked:
                        grid.BlockCell(position);
                        break;                  
                }
            }

            var path = grid.GetPath(start, end);

            foreach (var p in path.Skip(1).Take(path.Count - 2))
            {
                var cell = lookup[p];
                cell.CellState = CellState.OnPath;                
            }
        }

        private static void ClearPathState(IEnumerable<Cell> cells)
        {
            foreach (var cell in cells)
            {
                if (cell.CellState == CellState.OnPath ||
                    cell.CellState == CellState.Closed ||
                    cell.CellState == CellState.Open)
                {
                    cell.CellState = CellState.Normal;
                }                  
            }
        }
    }
}
