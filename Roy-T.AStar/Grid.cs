using System;
using System.Collections.Generic;

namespace RoyT.AStar
{
    /// <summary>
    /// Representation of your world for the pathfinding algorithm.
    /// Use SetCellCost to change the cost of traversing a cell.
    /// Use BlockCell to make a cell completely intraversable.
    /// </summary>
    public sealed class Grid
    {       
        private readonly float DefaultCost;
        private readonly float[] Weights;        

        /// <summary>
        /// Creates a grid
        /// </summary>
        /// <param name="dimX">The x-dimension of your world</param>
        /// <param name="dimY">The y-dimesion of your world</param>
        /// <param name="defaultCost">The default cost every cell is initialized with</param>
        public Grid(int dimX, int dimY, float defaultCost)
        {
            if (defaultCost <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    $"Argument {nameof(defaultCost)} with value {defaultCost} is invalid. The cost of traversing a cell cannot be less than, or equal to, zero");
            }

            this.DefaultCost = defaultCost;
            this.Weights = new float[dimX * dimY];
            this.DimX = dimX;
            this.DimY = dimY;

            for (var n = 0; n < this.Weights.Length; n++)
            {
                this.Weights[n] = defaultCost;
            }
        }

        /// <summary>
        /// X-dimension of the grid
        /// </summary>
        public int DimX { get; }
        
        /// <summary>
        /// Y-dimension of the grid
        /// </summary>
        public int DimY { get; }

        /// <summary>
        /// Sets the cost for traversing a cell
        /// </summary>
        /// <param name="position">A position inside the grid</param>
        /// <param name="cost">The cost of traversing the cell, always larger than zero</param>
        public void SetCellCost(Position position, float cost)
        {
            if (cost < 0)
            {
                throw new ArgumentOutOfRangeException(
                    $"Argument {nameof(cost)} with value {cost} is invalid. The cost of traversing a cell cannot be less than, or equal to, zero");
            }

            this.Weights[GetIndex(position.X, position.Y)] = cost;
        }

        /// <summary>
        /// Makes the cell intraversable
        /// </summary>
        /// <param name="position">A position inside the grid</param>
        public void BlockCell(Position position) => SetCellCost(position, float.PositiveInfinity);

        /// <summary>
        /// Makes the cell traversable, gives it the default traversal cost as provided in the constructor
        /// </summary>
        /// <param name="position">A position inside the grid</param>
        public void UnblockCell(Position position) => SetCellCost(position, this.DefaultCost);

        /// <summary>
        /// Looks-up the cost for traversing a given cell, if a cell is blocked (<see cref="BlockCell"/>) 
        /// +infinity is returned
        /// </summary>
        /// <param name="position">A position inside the grid</param>
        /// <returns>The cost</returns>
        public double GetCellCost(Position position)
        {
            return this.Weights[GetIndex(position.X, position.Y)];
        }

        /// <summary>
        /// Looks-up the cost for traversing a given cell, does not check
        /// if the position is inside the grid
        /// </summary>
        /// <param name="position">A position inside the grid</param>
        /// <returns>The cost</returns>
        internal float GetCellCostUnchecked(Position position)
        {
            return this.Weights[this.DimX * position.X + position.Y];
        }

        /// <summary>
        /// Computes the lowest-cost path from start to end inside the grid for an agent that can
        /// move both diagonal and lateral
        /// </summary>
        /// <param name="start">The start position</param>
        /// <param name="end">The end position</param>        
        /// <returns>positions of cells, from start to end, on the shortest path from start to end</returns>
        public IList<Position> GetPath(Position start, Position end)
            => GetPath(start, end, MovementPatterns.Full);

        /// <summary>
        /// Computes the lowest-cost path from start to end inside the grid for an agent with a custom
        /// movement pattern
        /// </summary>
        /// <param name="start">The start position</param>
        /// <param name="end">The end position</param>
        /// <param name="movementPattern">The movement pattern of the agent, <see cref="MovementPatterns"/> for several built-in options</param>
        /// <returns>positions of cells, from start to end, on the shortest path from start to end</returns>
        public IList<Position> GetPath(Position start, Position end, Offset[] movementPattern)
        {
            var steps = new List<Position>();

            var path = PathFinder.FindPath(this, start, end, movementPattern);            

            var current = path;
            while (current != null)
            {
                steps.Add(current.Position);
                current = current.Next;
            }

            return steps;
        }

        /// <summary>
        /// Converts a 2d index to a 1d index and performs bounds checking
        /// </summary>        
        private int GetIndex(int x, int y)
        {
            if (x < 0 || x >= this.DimX)
            {
                throw new ArgumentOutOfRangeException(
                    $"The x-coordinate {x} is outside of the expected range [0...{this.DimX})");
            }

            if (y < 0 || y >= this.DimY)
            {
                throw new ArgumentOutOfRangeException(
                    $"The y-coordinate {y} is outside of the expected range [0...{this.DimY})");
            }

            return this.DimX * y + x;
        }        
    }    
}

