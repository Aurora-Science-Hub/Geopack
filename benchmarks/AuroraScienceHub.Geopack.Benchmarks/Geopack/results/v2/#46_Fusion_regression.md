```

BenchmarkDotNet v0.15.4, Linux Ubuntu 24.04.2 LTS (Noble Numbat)
AMD Ryzen 5 5500U with Radeon Graphics 0.40GHz, 1 CPU, 12 logical and 6 physical cores
.NET SDK 10.0.100
  [Host]    : .NET 10.0.0 (10.0.0, 10.0.25.52411), X64 RyuJIT x86-64-v3
  .NET 10.0 : .NET 10.0.0 (10.0.0, 10.0.25.52411), X64 RyuJIT x86-64-v3
  .NET 8.0  : .NET 8.0.22 (8.0.22, 8.0.2225.52707), X64 RyuJIT x86-64-v3


```
| Method            | Job       | Runtime   | Mean     | Error    | StdDev   | Allocated |
|------------------ |---------- |---------- |---------:|---------:|---------:|----------:|
| ToCartesianVector | .NET 10.0 | .NET 10.0 | 21.19 ns | 0.046 ns | 0.043 ns |         - |
| ToCartesianVector | .NET 8.0  | .NET 8.0  | 31.21 ns | 0.045 ns | 0.037 ns |         - |
