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
| Calculate_IgrfMagneticField | .NET 9.0      | .NET 9.0      | 524.93 ns | 4.647 ns | 4.119 ns |  1.00 | 0.1373 |     288 B |        1.00 |
| Calculate_DipMagneticField  | .NET 9.0      | .NET 9.0      |  96.50 ns | 0.214 ns | 0.200 ns |  0.18 |      - |         - |        0.00 |
| Calculate_Sun               | .NET 9.0      | .NET 9.0      | 150.40 ns | 0.235 ns | 0.220 ns |  0.29 |      - |         - |        0.00 |
|                             |               |               |           |          |          |       |        |           |             |
| Calculate_IgrfMagneticField | NativeAOT     | NativeAOT 9.0 | 520.23 ns | 2.409 ns | 2.254 ns |  1.00 | 0.1373 |     288 B |        1.00 |
| Calculate_DipMagneticField  | NativeAOT     | NativeAOT 9.0 |  95.79 ns | 0.083 ns | 0.078 ns |  0.18 |      - |         - |        0.00 |
| Calculate_Sun               | NativeAOT     | NativeAOT 9.0 | 158.93 ns | 0.105 ns | 0.098 ns |  0.31 |      - |         - |        0.00 |
|                             |               |               |           |          |          |       |        |           |             |
| Calculate_IgrfMagneticField | NativeAOT 9.0 | NativeAOT 9.0 | 521.09 ns | 1.662 ns | 1.473 ns |  1.00 | 0.1373 |     288 B |        1.00 |
| Calculate_DipMagneticField  | NativeAOT 9.0 | NativeAOT 9.0 |  96.15 ns | 0.098 ns | 0.092 ns |  0.18 |      - |         - |        0.00 |
| Calculate_Sun               | NativeAOT 9.0 | NativeAOT 9.0 | 158.74 ns | 0.137 ns | 0.129 ns |  0.30 |      - |         - |        0.00 |
