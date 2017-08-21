using System;
using System.Collections.Generic;
using System.Linq;
using RoyT.AStar;

namespace Viewer
{
    /// <summary>
    /// Captures messages that the pathfinder outputs (only in debug builds) so we
    /// can visualize and replay the decision making process.
    /// </summary>
    internal sealed class ReplayController
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
            if (PathFinder.StepList == null || PathFinder.StepList.Count == 0)
                return; // Nothing to replay, did we forget to compute a path beforehand?

            // Remove previous replay visualizations
            foreach (var cell in cells.Where(c => Cell.ReplayCellStates.Contains(c.CellState)))
            {
                cell.CellState = CellState.Normal;
            }

            // Only some messages contain the entire path, store the 'newest' path
            IReadOnlyList<Position> path = new List<Position>(0);

            var max = Math.Max(0, Math.Min(PathFinder.StepList.Count - 1, this.CurrentStep));
            for (var i = 0; i <= max; i++)
            {
                var step = PathFinder.StepList[i];

                // Figure out how to visualize this step in the cell
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

                // Find the corresponding cell and apply the visualization
                var cell = cells.First(c => c.X == step.Position.X && c.Y == step.Position.Y);
                if (!Cell.UserCellStates.Contains(cell.CellState))
                {
                    cell.CellState = cellState;
                }

                // Update the latest path, if available
                if (step.Path.Count > 0)
                {
                    path = step.Path;
                }
            }

            // Visualize the latest path, don't override the currently active cell
            foreach (var p in path)
            {
                var cell = cells.First(c => c.X == p.X && c.Y == p.Y);
                if (!Cell.UserCellStates.Contains(cell.CellState) &&
                    cell.CellState != CellState.Current)
                {
                    cell.CellState = CellState.OnPath;
                }
            }
        }
    }
}
