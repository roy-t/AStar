using System;
using System.Collections.Generic;
using System.Linq;
using RoyT.AStar;

namespace Viewer
{
    /// <summary>
    /// Translates the visual representation of the world to something the 
    /// path finding algorithm can work with
    /// </summary>
    internal sealed class PathController
    {       
        public void ComputePath(IReadOnlyList<Cell> cells, int iterationLimit)
        {
            // Remove previous path visualization
            foreach (var cell in cells.Where(c => Cell.ReplayCellStates.Contains(c.CellState)))
            {
                cell.CellState = CellState.Normal;
            }

            var lookup = new Dictionary<Position, Cell>();

            var grid = new Grid(10, 10);
            var start = new Position(0, 0);
            var end = new Position(9, 9);

            // Take the properties of the cells and translate them
            // to properties on the right positions in the grid
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
                    case CellState.Current:
                    case CellState.Open:
                    case CellState.Closed:
                    case CellState.OnPath:                    
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            
            var path = grid.GetPath(start, end, MovementPatterns.Full, iterationLimit);
            
            // Visualize the path in the cells, skip the start and end node, we already
            // have those in the visualization
            foreach (var p in path.Skip(1).Take(path.Length - 2))
            {
                var cell = lookup[p];
                cell.CellState = CellState.OnPath;                
            }
        }       
    }
}
