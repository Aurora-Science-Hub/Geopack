```

BenchmarkDotNet v0.15.4, Linux Ubuntu 24.04.2 LTS (Noble Numbat)
AMD Ryzen 5 5500U with Radeon Graphics 0.40GHz, 1 CPU, 12 logical and 6 physical cores
.NET SDK 10.0.100
  [Host]    : .NET 10.0.0 (10.0.0, 10.0.25.52411), X64 RyuJIT x86-64-v3
  .NET 10.0 : .NET 10.0.0 (10.0.0, 10.0.25.52411), X64 RyuJIT x86-64-v3
  .NET 8.0  : .NET 8.0.22 (8.0.22, 8.0.2225.52707), X64 RyuJIT x86-64-v3


```
| Method     | Job       | Runtime   | Mean     | Error   | StdDev  | Allocated |
|----------- |---------- |---------- |---------:|--------:|--------:|----------:|
| ToGeodetic | .NET 10.0 | .NET 10.0 | 208.9 ns | 0.32 ns | 0.28 ns |         - |
| ToGeodetic | .NET 8.0  | .NET 8.0  | 211.7 ns | 0.20 ns | 0.18 ns |         - |
