# Benchmarks overview
For all benchmarks the graph is layed out in a grid like pattern and contains 10,000 nodes and 39,204 outgoing edges. All benchmarks try to find a path from the top-left node to the bottom-right node.

## GridBench
All edges have the same traversal velocity. The A* algorithm will guess 100% right all the time. This benchmark is useful because it shows the absolute best case scenario. Note that this benchmark is so fast that the measuring error is usually several times greater than the mean, so it is hard to say how fast it really ran.  

## GridWithHoleBench 
All edges have the same traversal velocity. All nodes, on a diagonal from the top right to the bottom left, have had their incoming edges removed. Except for the node next to the center node of which all edges remain intact.

This benchmark is designed to see how fast the algorithm can find the node to pass through. It is useful because it shows that the heuristic searches through the best candidate nodes first. This should be a very fast benchmark.

## GridWithRandomHoles
All edges have the same traversal velocity, pseudo-randomly 50% of the nodes have been disconnected.

This benchmark is useful because it shows you how the A* algorithm will behave in a realistic setting, in a sparsely connected graph. The heuristic guides the search, but is not always right.

## GridWithRandomLimitsBench
All edges have pseudo random traversal velocities between 80 and 100km/h. This benchmark was designed so that the A* heuristic has more trouble figuring out what the best path is. The general direction will be correct, but there will be a lot of small detours in the best path.

This benchmark is useful because it shows how you how the A* algorithm will behave in a realistic setting, in a well connected graph.

## GridWithUnreachableTargetBench
Disconnects the left part of the graph from the right part of the graph. Forcing the A* algorithm to inspect a lot of edges, before it can conclude that the target is unreachable. 

This benchmark is useful because it shows the worst-case performance of the A* algorithm. Because it has to search through all reachable nodes before it can definitely conclude that the target is unreachable.

## GridWithGradientBench 
Edges in the top left of the grid have the highest traversal velocity, while edges in the bottom right have the lowest traversal velocity. This means that the A* algorithm continously guesses wrong and has to search through almost the entire grid before finding the answer. This should be considered an adversial/torture benchmarks and an absolute worst case scenario. It is useful because it benchmarks the speed of our algorithm, without the heuristic getting in the way. 

This benchmark is compararable to the `Gradient100X100` benchmark from older versions.

# Benchmarks
_From newest to oldest_

## 2020-02-05 Remove superseded nodes during search
_git hash `302e2685743700d48d63f08c13df6794aed0e936`_
BenchmarkDotNet=v0.12.0, OS=Windows 10.0.17763.973 (1809/October2018Update/Redstone5)
Intel Core i9-9900K CPU 3.60GHz (Coffee Lake), 1 CPU, 16 logical and 8 physical cores
.NET Core SDK=3.0.100
-  [Host]     : .NET Core 3.0.0 (CoreCLR 4.700.19.46205, CoreFX 4.700.19.46214), X64 RyuJIT
-  DefaultJob : .NET Core 3.0.0 (CoreCLR 4.700.19.46205, CoreFX 4.700.19.46214), X64 RyuJIT

|                         Method |            Mean |        Error |       StdDev |
|------------------------------- |----------------:|-------------:|-------------:|
|                      GridBench |     94,338.7 ns |    529.27 ns |    469.18 ns |
|              GridWithHoleBench |        120.2 ns |      0.88 ns |      0.82 ns |
|       GridWithRandomHolesBench |    119,319.2 ns |    474.85 ns |    420.94 ns |
|      GridWithRandomLimitsBench |  6,799,357.4 ns | 30,951.38 ns | 28,951.94 ns |
| GridWithUnreachableTargetBench |  4,658,572.3 ns | 29,277.32 ns | 27,386.02 ns |
|          GridWithGradientBench | 10,435,749.8 ns | 91,339.85 ns | 85,439.36 ns |


## 2020-02-04 Precalculate travel duration over edges
_git hash `973d6b86f5adaecf22e0db4401ee878817ab1b6c`_
BenchmarkDotNet=v0.12.0, OS=Windows 10.0.17763.973 (1809/October2018Update/Redstone5)
Intel Core i9-9900K CPU 3.60GHz (Coffee Lake), 1 CPU, 16 logical and 8 physical cores
.NET Core SDK=3.0.100
- [Host]     : .NET Core 3.0.0 (CoreCLR 4.700.19.46205, CoreFX 4.700.19.46214), X64 RyuJIT
- DefaultJob : .NET Core 3.0.0 (CoreCLR 4.700.19.46205, CoreFX 4.700.19.46214), X64 RyuJIT

|                         Method |            Mean |         Error |        StdDev |
|------------------------------- |----------------:|--------------:|--------------:|
|                      GridBench |     83,950.3 ns |     283.08 ns |     264.79 ns |
|              GridWithHoleBench |        138.4 ns |       1.21 ns |       1.07 ns |
|       GridWithRandomHolesBench |    104,597.0 ns |     342.93 ns |     320.77 ns |
|      GridWithRandomLimitsBench |  6,582,456.5 ns |  46,309.74 ns |  43,318.16 ns |
| GridWithUnreachableTargetBench |  7,065,479.0 ns |  60,852.18 ns |  50,814.33 ns |
|          GridWithGradientBench | 13,863,809.2 ns | 142,322.59 ns | 133,128.63 ns |


## 2020-01-26 Moving from a linked list to a binary min heap
_git hash `a58f52404bb77a5a836768488809eb9c8b6f4ad0`_

BenchmarkDotNet=v0.12.0, OS=Windows 10.0.17763.973 (1809/October2018Update/Redstone5)
Intel Core i9-9900K CPU 3.60GHz (Coffee Lake), 1 CPU, 16 logical and 8 physical cores
.NET Core SDK=3.0.100
- [Host]     : .NET Core 3.0.0 (CoreCLR 4.700.19.46205, CoreFX 4.700.19.46214), X64 RyuJIT
- DefaultJob : .NET Core 3.0.0 (CoreCLR 4.700.19.46205, CoreFX 4.700.19.46214), X64 RyuJIT

|                         Method |            Mean |         Error |        StdDev |
|------------------------------- |----------------:|--------------:|--------------:|
|                      GridBench |     95,021.7 ns |     350.61 ns |     327.96 ns |
|              GridWithHoleBench |        129.7 ns |       0.73 ns |       0.68 ns |
|       GridWithRandomHolesBench |    118,499.2 ns |     576.81 ns |     450.33 ns |
|      GridWithRandomLimitsBench |  7,289,230.7 ns |  25,773.80 ns |  24,108.83 ns |
| GridWithUnreachableTargetBench |  8,004,423.0 ns |  55,038.19 ns |  51,482.75 ns |
|          GridWithGradientBench | 15,575,041.1 ns | 169,958.60 ns | 158,979.38 ns |


## 2020-01-23 Re-implementation using a graph
_git hash `bbadc1325c942b9f2175b4d045cc5254c2cb04e6`_

BenchmarkDotNet=v0.12.0, OS=Windows 10.0.17763.973 (1809/October2018Update/Redstone5)
Intel Core i9-9900K CPU 3.60GHz (Coffee Lake), 1 CPU, 16 logical and 8 physical cores
.NET Core SDK=3.0.100
-  [Host]     : .NET Core 3.0.0 (CoreCLR 4.700.19.46205, CoreFX 4.700.19.46214), X64 RyuJIT
-  DefaultJob : .NET Core 3.0.0 (CoreCLR 4.700.19.46205, CoreFX 4.700.19.46214), X64 RyuJIT

|                         Method |            Mean |         Error |        StdDev |
|------------------------------- |----------------:|--------------:|--------------:|
|                      GridBench |    326,209.3 ns |   1,109.91 ns |   1,038.21 ns |
|              GridWithHoleBench |        100.7 ns |       0.15 ns |       0.12 ns |
|       GridWithRandomHolesBench |    278,367.3 ns |     899.29 ns |     797.20 ns |
|      GridWithRandomLimitsBench | 22,880,403.8 ns |  97,026.26 ns |  86,011.25 ns |
| GridWithUnreachableTargetBench | 24,213,165.6 ns | 242,178.42 ns | 226,533.85 ns |
|          GridWithGradientBench | 31,464,366.2 ns | 115,321.25 ns | 107,871.57 ns |

# Benchmarks scores for older versions
_Note: Gradient100X100 is approximately similar to the GridWithGradientBench benchmark in newer versions_

## 2020-01-23
_git hash `eaedfb12d9918977a8a3cde49a461e932f1a4e2b`_

BenchmarkDotNet=v0.12.0, OS=Windows 10.0.17763.973 (1809/October2018Update/Redstone5)
Intel Core i9-9900K CPU 3.60GHz (Coffee Lake), 1 CPU, 16 logical and 8 physical cores
.NET Core SDK=3.0.100
-  [Host]     : .NET Core 3.0.0 (CoreCLR 4.700.19.46205, CoreFX 4.700.19.46214), X64 RyuJIT
-  DefaultJob : .NET Core 3.0.0 (CoreCLR 4.700.19.46205, CoreFX 4.700.19.46214), X64 RyuJIT

|          Method |     Mean |    Error |   StdDev |
|---------------- |---------:|---------:|---------:|
| Gradient100X100 | 21.51 ms | 0.128 ms | 0.119 ms |