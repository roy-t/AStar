using System;
using System.Collections.Generic;
using System.Linq;
using RoyT.AStar;

namespace Viewer
{
    internal sealed class PathController
    {
        private readonly List<string> Steps;
        private int step;
        

        public PathController()
        {
            this.Steps = new List<string>();
            this.step = 0;            
        }

        public void Start() => this.step = 0;
        public void End() => this.step = this.Steps.Count - 1;
        public void Forward() => this.step = Math.Min(this.step + 1, this.Steps.Count - 1);
        public void Backward() => this.step = Math.Max(this.step - 1, 0);                

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

                cell.CostSoFar = 0;
            }
        }
    }
}
