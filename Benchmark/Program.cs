using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using RoyT.AStar;

namespace Benchmark
{
    public class AStarBenchmark
    {
        private readonly Grid SmallEmptyGrid;
        private readonly Grid LargeEmptyGrid;
        private readonly Grid GridWithSlope;

        public AStarBenchmark()
        {
            this.SmallEmptyGrid = new Grid(100, 100, 1.0);
            this.LargeEmptyGrid = new Grid(1000, 1000, 1.0);
            this.GridWithSlope  = new Grid(100, 100, 1.0);

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

        //[Benchmark]
        public void TestSmallEmptyGrid()
        {
            this.SmallEmptyGrid.GetPath(
                new Position(0, 0),
                new Position(this.SmallEmptyGrid.DimX - 1, this.SmallEmptyGrid.DimY - 1));
        }

        //[Benchmark]
        public void TestLargeEmptyGrid()
        {
            this.LargeEmptyGrid.GetPath(
                new Position(0, 0),
                new Position(this.LargeEmptyGrid.DimX - 1, this.LargeEmptyGrid.DimY - 1));
        }

        [Benchmark]
        public void TestGridWithSlope()
        {
            this.GridWithSlope.GetPath(
                new Position(0, 0),
                new Position(this.GridWithSlope.DimX - 1, this.GridWithSlope.DimY - 1));
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            //var bench = new AStarBenchmark();
            //for (var i = 0; i < 100; i++)
            //{
            //    bench.TestLargeEmptyGrid();
            //}
            //BenchmarkRunner.Run<AStarBenchmark>();

            int[] num = { 69, 56, 77, 44, 8, 0, 111, 566 };
            for (int i = 1; i < num.Length; i++)
            {
                int key = num[i];
                int j = i - 1;
                while (j >= 0 && num[j] > key)
                {
                    //loop
                    num[j + 1] = num[j];
                    j--;
                }
                num[j + 1] = key;
            }

            //display
            foreach (var item in num)
            {
                Console.WriteLine(item);
            }


            Console.ReadLine();
        }
    }
}