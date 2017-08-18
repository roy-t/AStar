using System;
using System.Collections.Generic;
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
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            var path = grid.GetPath(start, end);

            foreach (var p in path)
            {
                var cell = lookup[p];
                cell.PathState = PathState.OnPath;                
            }
        }

        private static void ClearPathState(IEnumerable<Cell> cells)
        {
            foreach (var cell in cells)
            {
                cell.PathState = PathState.Undetermined;
            }
        }
    }
}
