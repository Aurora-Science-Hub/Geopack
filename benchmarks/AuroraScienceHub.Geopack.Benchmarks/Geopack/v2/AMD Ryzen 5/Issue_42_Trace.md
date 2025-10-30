```

BenchmarkDotNet v0.15.4, Linux Ubuntu 24.04.2 LTS (Noble Numbat)
AMD Ryzen 5 5500U with Radeon Graphics 0.40GHz, 1 CPU, 12 logical and 6 physical cores
.NET SDK 9.0.302
  [Host]        : .NET 9.0.7 (9.0.7, 9.0.725.31616), X64 RyuJIT x86-64-v3
  .NET 9.0      : .NET 9.0.7 (9.0.7, 9.0.725.31616), X64 RyuJIT x86-64-v3
  NativeAOT     : .NET 9.0.7, X64 NativeAOT x86-64-v3
  NativeAOT 9.0 : .NET 9.0.7, X64 NativeAOT x86-64-v3


```
| Method                                    | Job           | Runtime       | Mean     | Error   | StdDev  | Ratio | RatioSD | Gen0     | Allocated | Alloc Ratio |
|------------------------------------------ |-------------- |-------------- |---------:|--------:|--------:|------:|--------:|---------:|----------:|------------:|
| Trace_FieldLineFromNorthToSouthHemisphere | .NET 9.0      | .NET 9.0      | 306.7 μs | 4.85 μs | 4.30 μs |  1.00 |    0.02 | 109.8633 | 224.86 KB |        1.00 |
| Trace_FieldLineFromSouthToNorthHemisphere | .NET 9.0      | .NET 9.0      | 377.3 μs | 6.43 μs | 6.02 μs |  1.23 |    0.03 | 135.7422 | 278.19 KB |        1.24 |
|                                           |               |               |          |         |         |       |         |          |           |             |
| Trace_FieldLineFromNorthToSouthHemisphere | NativeAOT     | NativeAOT 9.0 | 302.5 μs | 5.82 μs | 5.44 μs |  1.00 |    0.02 | 109.8633 | 224.88 KB |        1.00 |
| Trace_FieldLineFromSouthToNorthHemisphere | NativeAOT     | NativeAOT 9.0 | 373.7 μs | 3.42 μs | 3.20 μs |  1.24 |    0.02 | 136.2305 | 278.21 KB |        1.24 |
|                                           |               |               |          |         |         |       |         |          |           |             |
| Trace_FieldLineFromNorthToSouthHemisphere | NativeAOT 9.0 | NativeAOT 9.0 | 296.2 μs | 3.42 μs | 3.19 μs |  1.00 |    0.01 | 109.8633 | 224.88 KB |        1.00 |
| Trace_FieldLineFromSouthToNorthHemisphere | NativeAOT 9.0 | NativeAOT 9.0 | 377.0 μs | 6.87 μs | 6.43 μs |  1.27 |    0.02 | 136.2305 | 278.21 KB |        1.24 |
