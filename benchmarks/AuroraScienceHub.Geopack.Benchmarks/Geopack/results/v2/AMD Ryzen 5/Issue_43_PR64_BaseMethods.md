```

BenchmarkDotNet v0.15.4, Linux Ubuntu 24.04.2 LTS (Noble Numbat)
AMD Ryzen 5 5500U with Radeon Graphics 0.40GHz, 1 CPU, 12 logical and 6 physical cores
.NET SDK 9.0.302
  [Host]        : .NET 9.0.7 (9.0.7, 9.0.725.31616), X64 RyuJIT x86-64-v3
  .NET 9.0      : .NET 9.0.7 (9.0.7, 9.0.725.31616), X64 RyuJIT x86-64-v3
  NativeAOT     : .NET 9.0.7, X64 NativeAOT x86-64-v3
  NativeAOT 9.0 : .NET 9.0.7, X64 NativeAOT x86-64-v3


```
| Method                      | Job           | Runtime       | Mean      | Error    | StdDev   | Ratio | Gen0   | Allocated | Alloc Ratio |
|---------------------------- |-------------- |-------------- |----------:|---------:|---------:|------:|-------:|----------:|------------:|
| Calculate_IgrfMagneticField | .NET 9.0      | .NET 9.0      | 520.82 ns | 2.844 ns | 2.660 ns |  1.00 | 0.1373 |     288 B |        1.00 |
| Calculate_DipMagneticField  | .NET 9.0      | .NET 9.0      |  95.29 ns | 0.068 ns | 0.057 ns |  0.18 |      - |         - |        0.00 |
| Calculate_Sun               | .NET 9.0      | .NET 9.0      | 158.82 ns | 0.511 ns | 0.478 ns |  0.30 | 0.0267 |      56 B |        0.19 |
|                             |               |               |           |          |          |       |        |           |             |
| Calculate_IgrfMagneticField | NativeAOT     | NativeAOT 9.0 | 514.47 ns | 2.429 ns | 2.272 ns |  1.00 | 0.1373 |     288 B |        1.00 |
| Calculate_DipMagneticField  | NativeAOT     | NativeAOT 9.0 |  95.76 ns | 0.112 ns | 0.105 ns |  0.19 |      - |         - |        0.00 |
| Calculate_Sun               | NativeAOT     | NativeAOT 9.0 | 164.14 ns | 0.231 ns | 0.216 ns |  0.32 | 0.0267 |      56 B |        0.19 |
|                             |               |               |           |          |          |       |        |           |             |
| Calculate_IgrfMagneticField | NativeAOT 9.0 | NativeAOT 9.0 | 515.92 ns | 1.603 ns | 1.499 ns |  1.00 | 0.1373 |     288 B |        1.00 |
| Calculate_DipMagneticField  | NativeAOT 9.0 | NativeAOT 9.0 |  95.96 ns | 0.079 ns | 0.074 ns |  0.19 |      - |         - |        0.00 |
| Calculate_Sun               | NativeAOT 9.0 | NativeAOT 9.0 | 163.28 ns | 0.492 ns | 0.460 ns |  0.32 | 0.0267 |      56 B |        0.19 |
