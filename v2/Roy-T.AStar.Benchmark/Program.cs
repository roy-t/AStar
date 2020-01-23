using System;
using BenchmarkDotNet.Running;

namespace Roy_T.AStar.Benchmark
{
    public class Program
    {

        static void Main(string[] _)
        {
            BenchmarkRunner.Run<AStarBenchmark>();
            Console.ReadLine();
        }
    }
}
