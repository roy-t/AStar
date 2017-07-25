``` ini

BenchmarkDotNet=v0.10.8, OS=Windows 10 Redstone 2 (10.0.15063)
Processor=Intel Core i5-4690 CPU 3.50GHz (Haswell), ProcessorCount=4
Frequency=3415990 Hz, Resolution=292.7409 ns, Timer=TSC
dotnet cli version=1.0.4
  [Host]     : .NET Core 4.6.25211.01, 64bit RyuJIT [AttachedDebugger]
  DefaultJob : .NET Core 4.6.25211.01, 64bit RyuJIT


```
 |             Method |       Mean |     Error |    StdDev |
 |------------------- |-----------:|----------:|----------:|
 | TestSmallEmptyGrid |   100.9 us |  1.138 us |  1.065 us |
 | TestLargeEmptyGrid | 1,260.7 us | 24.510 us | 35.152 us |
 |  TestGridWithSlope | 4,819.9 us | 75.271 us | 66.726 us |
