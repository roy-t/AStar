using System;
using System.Collections.Generic;
using System.Linq;
using RoyT.AStar;

namespace Viewer
{
    internal sealed class PathController
    {
        public int CurrentStep { get; set; }

        public void Start(IReadOnlyList<Cell> cells)
        {
            this.CurrentStep = 0;
            ReplaySteps(cells);
        }

        public void End(IReadOnlyList<Cell> cells)
        {
            this.CurrentStep = PathFinder.StepList.Count;
            ReplaySteps(cells);
        }

        public void Forward(IReadOnlyList<Cell> cells)
        {
            this.CurrentStep = Math.Min(this.CurrentStep + 1, PathFinder.StepList.Count - 1);
            ReplaySteps(cells);
        }

        public void Backward(IReadOnlyList<Cell> cells)
        {
            this.CurrentStep = Math.Max(this.CurrentStep - 1, 0);
            ReplaySteps(cells);
        }

        public void ReplaySteps(IReadOnlyList<Cell> cells)
        {
            ClearPathState(cells);

            for (var i = 0; i < this.CurrentStep && i < PathFinder.StepList.Count; i++)
            {
                var step = PathFinder.StepList[i];
                CellState cellState;
                switch (step.Type)
                {
                    case StepType.Current:
                        cellState = CellState.Current;
                        break;
                    case StepType.Open:
                        cellState = CellState.Open;
                        break;
                    case StepType.Close:
                        cellState = CellState.Closed;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                var cell = cells.First(c => c.X == step.Position.X && c.Y == step.Position.Y);
                if (cell.CellState != CellState.Start && cell.CellState != CellState.End)
                {
                    cell.CellState = cellState;
                }
            }
        }

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
                    cell.CellState == CellState.Open   ||
                    cell.CellState == CellState.Current)
                {
                    cell.CellState = CellState.Normal;
                }

                cell.CostSoFar = 0;
            }
        }
    }
}
