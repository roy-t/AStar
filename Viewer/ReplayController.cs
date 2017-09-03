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

        public void Start(IReadOnlyList<Cell> cells, bool smoothPath)
        {
            this.CurrentStep = 0;
            ReplayPathFindingSteps(cells, smoothPath);
        }

        public void End(IReadOnlyList<Cell> cells, bool smoothPath)
        {
            this.CurrentStep = GetMaxStep(smoothPath);
            ReplayPathFindingSteps(cells, smoothPath);
        }

        public void Forward(IReadOnlyList<Cell> cells, bool smoothPath)
        {
            this.CurrentStep = Math.Min(this.CurrentStep + 1, GetMaxStep(smoothPath));
            ReplayPathFindingSteps(cells, smoothPath);
        }

        public void Backward(IReadOnlyList<Cell> cells, bool smoothPath)
        {
            this.CurrentStep = Math.Max(this.CurrentStep - 1, 0);
            ReplayPathFindingSteps(cells, smoothPath);
        }

        public static int GetMaxStep(bool smoothStep)
            => smoothStep
                ? PathFinder.StepList.Count + PathSmoother.SwapList.Count - 1
                : PathFinder.StepList.Count - 1;

        public void ReplayPathFindingSteps(IReadOnlyList<Cell> cells, bool smoothPath)
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

                if (smoothPath)
                {
                    ReplaySmoothSteps(cells);
                }
            }

            // Visualize the latest path, don't override the currently active cell
            foreach (var p in path)
            {
                var cell = cells.First(c => c.X == p.X && c.Y == p.Y);
                if (!Cell.UserCellStates.Contains(cell.CellState) &&
                    cell.CellState != CellState.Current &&
                    cell.CellState != CellState.Replaced)
                {
                    cell.CellState = CellState.OnPath;
                }
            }
        }        

        private void ReplaySmoothSteps(IReadOnlyList<Cell> cells)
        {
            var max = Math.Min(PathSmoother.SwapList.Count - 1, this.CurrentStep - PathFinder.StepList.Count);
            for (var i = 0; i <= max; i++)
            {
                var step = PathSmoother.SwapList[i];
                var original = cells.First(c => c.X == step.Original.X && c.Y == step.Original.Y);
                var replacement = cells.First(c => c.X == step.Replacement.X && c.Y == step.Replacement.Y);

                MarkNonUserCell(original, CellState.Replaced);
                MarkNonUserCell(replacement, CellState.OnPath);                
            }
        }

        private static void MarkNonUserCell(Cell cell, CellState cellState)
        {
            if (!Cell.UserCellStates.Contains(cell.CellState))
            {
                cell.CellState = cellState;
            }
        }
    }
}
