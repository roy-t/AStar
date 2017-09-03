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
                var cell = GetCell(cells, step.Position.X, step.Position.Y);
                MarkNonUserCell(cell, cellState);

                // Update the latest path, if available
                if (step.Path.Count > 0)
                {
                    path = step.Path;
                }                
            }

            // Visualize the latest path, don't override the currently active cell
            foreach (var p in path)
            {
                var cell = GetCell(cells, p.X, p.Y);
                if (!Cell.UserCellStates.Contains(cell.CellState) &&
                    cell.CellState != CellState.Current &&
                    cell.CellState != CellState.Replaced)
                {
                    cell.CellState = CellState.OnPath;
                }
            }

            if (smoothPath)
            {
                ReplaySmoothSteps(cells);
            }
        }        

        private void ReplaySmoothSteps(IReadOnlyList<Cell> cells)
        {      
            // See how far we should compute the path smoothing steps
            var max = Math.Min(PathSmoother.SwapList.Count - 1, this.CurrentStep - PathFinder.StepList.Count);
            for (var i = 0; i <= max; i++)
            {
                var step = PathSmoother.SwapList[i];
                var original = GetCell(cells, step.Original.X, step.Original.Y);
                var replacement = GetCell(cells, step.Replacement.X, step.Replacement.Y);
                
                // Mark where the path was changed due to smoothing
                if (step.SwapType == SwapType.Swap)
                {
                    MarkNonUserCell(original, CellState.Replaced);
                    MarkNonUserCell(replacement, CellState.OnPath);
                }
            }           

            // Mark the last element in the path considered for smoothing, without overriding previous work
            var current = PathSmoother.SwapList.Take(max).LastOrDefault(x => x.SwapType == SwapType.Current);
            if (current != null)
            {
                var cell = GetCell(cells, current.Original.X, current.Original.Y);
                MarkNonUserCell(cell, CellState.Current);
            }            
        }

        private static Cell GetCell(IEnumerable<Cell> cells, int x, int y)
            => cells.First(c => c.X == x && c.Y == y);

        private static void MarkNonUserCell(Cell cell, CellState cellState)
        {
            if (!Cell.UserCellStates.Contains(cell.CellState))
            {
                cell.CellState = cellState;
            }
        }
    }
}
