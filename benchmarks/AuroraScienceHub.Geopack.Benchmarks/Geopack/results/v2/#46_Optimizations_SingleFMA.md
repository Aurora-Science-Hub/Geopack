```

BenchmarkDotNet v0.15.4, Linux Ubuntu 24.04.3 LTS (Noble Numbat)
AMD Ryzen 5 5500U with Radeon Graphics 0.40GHz, 1 CPU, 12 logical and 6 physical cores
.NET SDK 10.0.100
  [Host]    : .NET 10.0.0 (10.0.0, 10.0.25.52411), X64 RyuJIT x86-64-v3
  .NET 10.0 : .NET 10.0.0 (10.0.0, 10.0.25.52411), X64 RyuJIT x86-64-v3
  .NET 8.0  : .NET 8.0.22 (8.0.22, 8.0.2225.52707), X64 RyuJIT x86-64-v3


```
| Method            | Job       | Runtime   | Mean      | Error     | StdDev    | Median    | Allocated |
|------------------ |---------- |---------- |----------:|----------:|----------:|----------:|----------:|
| ToSphericalVector | .NET 10.0 | .NET 10.0 | 0.0001 ns | 0.0003 ns | 0.0003 ns | 0.0000 ns |         - |
| ToSphericalVector | .NET 8.0  | .NET 8.0  | 9.4786 ns | 0.0167 ns | 0.0156 ns | 9.4750 ns |         - |
