```

BenchmarkDotNet v0.15.4, Linux Ubuntu 24.04.2 LTS (Noble Numbat)
AMD Ryzen 5 5500U with Radeon Graphics 0.40GHz, 1 CPU, 12 logical and 6 physical cores
.NET SDK 10.0.100
  [Host]    : .NET 10.0.0 (10.0.0, 10.0.25.52411), X64 RyuJIT x86-64-v3
  .NET 10.0 : .NET 10.0.0 (10.0.0, 10.0.25.52411), X64 RyuJIT x86-64-v3
  .NET 8.0  : .NET 8.0.22 (8.0.22, 8.0.2225.52707), X64 RyuJIT x86-64-v3


```
| Method | Job       | Runtime   | Mean     | Error     | StdDev    | Ratio | Gen0   | Allocated | Alloc Ratio |
|------- |---------- |---------- |---------:|----------:|----------:|------:|-------:|----------:|------------:|
| Recalc | .NET 10.0 | .NET 10.0 | 1.010 μs | 0.0085 μs | 0.0071 μs |  1.00 | 1.3828 |   2.83 KB |        1.00 |
|        |           |           |          |           |           |       |        |           |             |
| Recalc | .NET 8.0  | .NET 8.0  | 1.056 μs | 0.0035 μs | 0.0031 μs |  1.00 | 1.3828 |   2.83 KB |        1.00 |
