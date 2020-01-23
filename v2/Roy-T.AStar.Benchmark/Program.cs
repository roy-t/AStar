using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Roy_T.AStar.V2;

namespace Roy_T.AStar.Benchmark
{
    public class AStarBenchmark
    {
        private readonly Grid GradientGrid;
        private readonly Grid GridWithHole;
        private readonly Grid GridWithRandomLimits;

        public AStarBenchmark()
        {
            this.GradientGrid = new Grid(100, 100, 1, 1, Velocity.FromKilometersPerHour(100));

            var speedLimit = (this.GradientGrid.Rows * this.GradientGrid.Columns) + 1;
            for (var y = 0; y < this.GradientGrid.Rows; y++)
            {
                for (var x = 0; x < this.GradientGrid.Columns; x++)
                {
                    var node = this.GradientGrid.GetNode(x, y);
                    foreach (var edge in node.Incoming)
                    {
                        edge.TraversalVelocity = Velocity.FromKilometersPerHour(speedLimit);
                    }

                    speedLimit -= 1;
                }
            }

            this.GridWithHole = new Grid(100, 100, 1, 1, Velocity.FromKilometersPerHour(100));
            for (var i = 99; i >= 0; i--)
            {
                if (i != 50)
                {
                    this.GridWithHole.BlockNode(i, i);
                }
            }

            var numbers = new float[] {
                23, 24, 95, 72, 34, 63, 46, 43, 57, 61,
                12, 77, 30, 25, 49, 83, 54, 64, 42, 4,
                14, 43, 61, 81, 44, 51, 5, 62, 84, 60,
                42, 35, 90, 32, 7, 78, 58, 77, 67, 12,
                65, 47, 11, 66, 37, 12, 27, 61, 73, 42,
                51, 58, 27, 42, 42, 41, 43, 76, 72, 86,
                49, 74, 96, 20, 50, 13, 85, 71, 51, 48,
                13, 15, 35, 47, 87, 100, 53, 1, 9, 41,
                1, 28, 59, 15, 38, 70, 92, 41, 84, 87,
                6, 81, 80, 70, 1, 64, 94
            };

            this.GridWithRandomLimits = new Grid(100, 100, 1, 1, Velocity.FromKilometersPerHour(100));
            var z = 0;
            for (var y = 0; y < this.GradientGrid.Rows; y++)
            {
                for (var x = 0; x < this.GradientGrid.Columns; x++)
                {
                    var node = this.GradientGrid.GetNode(x, y);
                    foreach (var edge in node.Incoming)
                    {
                        edge.TraversalVelocity = Velocity.FromKilometersPerHour(numbers[z]);
                        z = (z + 1) % numbers.Length;
                    }
                }
            }
        }

        [Benchmark]
        public void GradientGridBench()
        {
            var maxSpeed = Velocity.FromKilometersPerHour((this.GradientGrid.Rows * this.GradientGrid.Columns) + 1);
            PathFinder.FindPath(
                this.GradientGrid.GetNode(0, 0),
                this.GradientGrid.GetNode(this.GradientGrid.Columns - 1, this.GradientGrid.Rows - 1),
                maxSpeed);
        }

        [Benchmark]
        public void GridWithHoleBench()
        {
            var maxSpeed = Velocity.FromKilometersPerHour(100);
            PathFinder.FindPath(
                this.GridWithHole.GetNode(0, 0),
                this.GridWithHole.GetNode(this.GradientGrid.Columns - 1, this.GradientGrid.Rows - 1),
                maxSpeed);
        }

        [Benchmark]
        public void GridWithRandomLimitsBench()
        {
            var maxSpeed = Velocity.FromKilometersPerHour(100);
            PathFinder.FindPath(
                this.GridWithRandomLimits.GetNode(0, 0),
                this.GridWithRandomLimits.GetNode(this.GradientGrid.Columns - 1, this.GradientGrid.Rows - 1),
                maxSpeed);
        }
    }

    public class Program
    {

        static void Main(string[] args)
        {
            BenchmarkRunner.Run<AStarBenchmark>();
            Console.ReadLine();
        }
    }
}
