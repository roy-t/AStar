using System;
using System.Collections.Generic;

namespace RoyT.AStar
{
    public sealed class Grid
    {
        private readonly double DefaultCost;
        private readonly double[] Weights;

        public Grid(int dimX, int dimY, double defaultCost)
        {
            if (defaultCost <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    $"Argument {nameof(defaultCost)} with value {defaultCost} is invalid. The cost of traversing a cell cannot be less than, or equal to, zero");
            }

            this.DefaultCost = defaultCost;
            this.Weights = new double[dimX * dimY];
            this.DimX = dimX;
            this.DimY = dimY;

            for (var n = 0; n < this.Weights.Length; n++)
            {
                this.Weights[n] = defaultCost;
            }
        }

        public int DimX { get; }
        public int DimY { get; }

        public void SetCellCost(Position position, double cost)
        {
            if (cost < 0)
            {
                throw new ArgumentOutOfRangeException(
                    $"Argument {nameof(cost)} with value {cost} is invalid. The cost of traversing a cell cannot be less than, or equal to, zero");
            }

            this.Weights[GetIndex(position.X, position.Y)] = cost;
        }

        public void BlockCell(Position position) => SetCellCost(position, double.PositiveInfinity);
        public void UnblockCell(Position position) => SetCellCost(position, this.DefaultCost);

        public double GetCellCost(Position position)
        {
            return this.Weights[GetIndex(position.X, position.Y)];
        }

        public IList<Position> GetPath(Position start, Position end)
        {
            var steps = new List<Position>();

            var path = PathFinder.FindPath(this, start, end);            

            var current = path;
            while (current != null)
            {
                steps.Add(current.Position);
                current = current.Next;
            }

            return steps;
        }

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
