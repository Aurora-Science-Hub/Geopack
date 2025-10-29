```

BenchmarkDotNet v0.15.4, Linux Ubuntu 24.04.2 LTS (Noble Numbat)
AMD Ryzen 5 5500U with Radeon Graphics 1.71GHz, 1 CPU, 12 logical and 6 physical cores
.NET SDK 9.0.302
  [Host]    : .NET 9.0.7 (9.0.7, 9.0.725.31616), X64 RyuJIT x86-64-v3
  .NET 9.0  : .NET 9.0.7 (9.0.7, 9.0.725.31616), X64 RyuJIT x86-64-v3
  NativeAOT : .NET 9.0.7, X64 NativeAOT x86-64-v3


```
| Method                      | Job       | Runtime       | Mean     | Error   | StdDev  | Ratio | Gen0   | Allocated | Alloc Ratio |
|---------------------------- |---------- |-------------- |---------:|--------:|--------:|------:|-------:|----------:|------------:|
| Calculate_IgrfMagneticField | .NET 9.0  | .NET 9.0      | 462.4 ns | 4.96 ns | 4.64 ns |  1.00 | 0.1602 |     336 B |        1.00 |
| Calculate_DipMagneticField  | .NET 9.0  | .NET 9.0      | 104.7 ns | 0.83 ns | 0.78 ns |  0.23 | 0.0229 |      48 B |        0.14 |
| Calculate_Sun               | .NET 9.0  | .NET 9.0      | 157.6 ns | 0.54 ns | 0.51 ns |  0.34 | 0.0267 |      56 B |        0.17 |
|                             |           |               |          |         |         |       |        |           |             |
| Calculate_IgrfMagneticField | NativeAOT | NativeAOT 9.0 | 464.3 ns | 3.00 ns | 2.81 ns |  1.00 | 0.1602 |     336 B |        1.00 |
| Calculate_DipMagneticField  | NativeAOT | NativeAOT 9.0 | 100.5 ns | 0.79 ns | 0.74 ns |  0.22 | 0.0229 |      48 B |        0.14 |
| Calculate_Sun               | NativeAOT | NativeAOT 9.0 | 164.3 ns | 0.60 ns | 0.50 ns |  0.35 | 0.0267 |      56 B |        0.17 |
