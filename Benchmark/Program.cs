using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using RoyT.AStar;

namespace Benchmark
{
    public class AStarBenchmark
    {
        private readonly Grid GridWithSlope;

        public AStarBenchmark()
        {
            // Create a grid with irratic costs, so the
            // heuristic is often wrong. This way we test 
            // the efficiency of our implementation
            // not the efficiency of the A* algorithm
            this.GridWithSlope  = new Grid(100, 100, 1.0f);

            var cost = 1.0f;
            for (var y = 0; y < this.GridWithSlope.DimY; y++)
            {
                for (var x = 0; x < this.GridWithSlope.DimX; x++)
                {
                    this.GridWithSlope.SetCellCost(new Position(x, y), cost);
                    cost += 0.5f;
                }
            }
        }        

        [Benchmark]
        public void Gradient100X100()
        {
            this.GridWithSlope.GetPath(
                new Position(0, 0),
                new Position(this.GridWithSlope.DimX - 1, this.GridWithSlope.DimY - 1),
                MovementPatterns.Full);
        }

        [Benchmark]
        public void Gradient100X100Limited()
        {
            this.GridWithSlope.GetPath(
                new Position(0, 0),
                new Position(this.GridWithSlope.DimX - 1, this.GridWithSlope.DimY - 1),
                MovementPatterns.Full,
                int.MaxValue);
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {           
            BenchmarkRunner.Run<AStarBenchmark>();            
            Console.ReadLine();
        }
    }
}