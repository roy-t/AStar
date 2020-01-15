using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoyT.AStar;

namespace UnitTests
{
    public static class GridCatalog
    {
        /// <summary>
        /// Builds a 9x9 grid with all tiles around the center tile blocked
        /// </summary>        
        public static Grid GridWithEnclosedCenterTile()
        {
            var grid = new Grid(9, 9);
            for (var x = 3; x <= 5; x++)
            {
                for (var y = 3; y <= 5; y++)
                {
                    grid.BlockCell(new Position(x, y));
                }
            }

            return grid;
        }

        /// <summary>
        /// Builds a 9x9 grid with all tiles around the center tile blocked
        /// </summary>        
        public static Grid GridWithObstructedDiagonals()
        {
            var grid = new Grid(9, 9);
            for (var x = 0; x < 9; x++)
            {
                grid.BlockCell(new Position(x, x));
                grid.BlockCell(new Position(8 - x, x));
            }

            return grid;
        }

        /// <summary>
        /// Builds a 9x9 grid with the center tile blocked
        /// </summary>        
        public static Grid GridWithBlockedCenterTile()
        {
            var grid = new Grid(9, 9);
            grid.BlockCell(new Position(4, 4));            
            return grid;
        }

        /// <summary>
        /// Builds a 9x9 grid with the center costing 10x as much as the other tiles
        /// </summary>        
        public static Grid GridWithHighCostCenterTile()
        {
            var grid = new Grid(9, 9);
            grid.SetCellCost(new Position(4, 4), 10);            
            return grid;
        }

        /// <summary>
        /// Builds a 9x9 grid with no obstructions
        /// </summary>        
        public static Grid UnobstructedGrid()
        {
            return new Grid(9, 9);
        }
    }
}
