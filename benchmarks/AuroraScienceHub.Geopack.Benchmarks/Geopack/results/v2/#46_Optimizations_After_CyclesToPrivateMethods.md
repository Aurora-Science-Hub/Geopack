xxxxxxxxxxxxx```

BenchmarkDotNet v0.15.4, Linux Ubuntu 24.04.2 LTS (Noble Numbat)
AMD Ryzen 5 5500U with Radeon Graphics 0.40GHz, 1 CPU, 12 logical and 6 physical cores
.NET SDK 10.0.100
  [Host]    : .NET 10.0.0 (10.0.0, 10.0.25.52411), X64 RyuJIT x86-64-v3
  .NET 10.0 : .NET 10.0.0 (10.0.0, 10.0.25.52411), X64 RyuJIT x86-64-v3
  .NET 8.0  : .NET 8.0.22 (8.0.22, 8.0.2225.52707), X64 RyuJIT x86-64-v3


```
| Method   | Job       | Runtime   |         Mean |       Error |      StdDev |  Ratio | RatioSD |    Gen0 | Allocated | Alloc Ratio |
|----------|-----------|-----------|-------------:|------------:|------------:|-------:|--------:|--------:|----------:|------------:|
| Recalc   | .NET 10.0 | .NET 10.0 |   1,023.7 ns |     3.27 ns |     2.90 ns |   1.00 |    0.00 |  1.3828 |    2896 B |        1.00 |
| IgrfGeo  | .NET 10.0 | .NET 10.0 |     443.9 ns |     2.08 ns |     1.95 ns |   0.43 |    0.00 |  0.1297 |     272 B |        0.09 |
| IgrfGsw  | .NET 10.0 | .NET 10.0 |     463.8 ns |     1.35 ns |     1.05 ns |   0.45 |    0.00 |  0.1373 |     288 B |        0.10 |
| Trace_NS | .NET 10.0 | .NET 10.0 | 355,022.5 ns |   836.67 ns |   741.68 ns | 346.79 |    1.18 | 97.6563 |  204472 B |       70.60 |
| Trace_SN | .NET 10.0 | .NET 10.0 | 353,929.2 ns | 1,293.25 ns | 1,209.71 ns | 345.72 |    1.48 | 87.4023 |  183200 B |       63.26 |
|          |           |           |              |             |             |        |         |         |           |             |
| Recalc   | .NET 8.0  | .NET 8.0  |   1,063.9 ns |     5.56 ns |     5.20 ns |   1.00 |    0.01 |  1.3828 |    2896 B |        1.00 |
| IgrfGeo  | .NET 8.0  | .NET 8.0  |     464.2 ns |     4.92 ns |     4.60 ns |   0.44 |    0.00 |  0.1297 |     272 B |        0.09 |
| IgrfGsw  | .NET 8.0  | .NET 8.0  |     485.0 ns |     3.75 ns |     3.51 ns |   0.46 |    0.00 |  0.1373 |     288 B |        0.10 |
| Trace_NS | .NET 8.0  | .NET 8.0  | 382,441.2 ns | 1,668.04 ns | 1,560.29 ns | 359.49 |    2.23 | 97.6563 |  204608 B |       70.65 |
| Trace_SN | .NET 8.0  | .NET 8.0  | 373,890.2 ns | 4,023.29 ns | 3,763.39 ns | 351.45 |    3.81 | 87.4023 |  183336 B |       63.31 |
