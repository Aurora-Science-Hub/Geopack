```

BenchmarkDotNet v0.15.4, Linux Ubuntu 24.04.2 LTS (Noble Numbat)
AMD Ryzen 5 5500U with Radeon Graphics 0.40GHz, 1 CPU, 12 logical and 6 physical cores
.NET SDK 9.0.302
  [Host]   : .NET 9.0.7 (9.0.7, 9.0.725.31616), X64 RyuJIT x86-64-v3
  .NET 9.0 : .NET 9.0.7 (9.0.7, 9.0.725.31616), X64 RyuJIT x86-64-v3

Job=.NET 9.0  Runtime=.NET 9.0  

```
| Method            | Mean     | Error    | StdDev   | Allocated |
|------------------ |---------:|---------:|---------:|----------:|
| ToCartesianVector | 27.21 ns | 0.036 ns | 0.032 ns |         - |
