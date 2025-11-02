```

BenchmarkDotNet v0.15.4, Linux Ubuntu 24.04.2 LTS (Noble Numbat)
AMD Ryzen 5 5500U with Radeon Graphics 0.40GHz, 1 CPU, 12 logical and 6 physical cores
.NET SDK 9.0.302
  [Host]        : .NET 9.0.7 (9.0.7, 9.0.725.31616), X64 RyuJIT x86-64-v3
  .NET 9.0      : .NET 9.0.7 (9.0.7, 9.0.725.31616), X64 RyuJIT x86-64-v3
  NativeAOT     : .NET 9.0.7, X64 NativeAOT x86-64-v3
  NativeAOT 9.0 : .NET 9.0.7, X64 NativeAOT x86-64-v3


```
| Method                                    | Job           | Runtime       | Mean     | Error   | StdDev  | Ratio | Gen0    | Allocated | Alloc Ratio |
|------------------------------------------ |-------------- |-------------- |---------:|--------:|--------:|------:|--------:|----------:|------------:|
| Trace_FieldLineFromNorthToSouthHemisphere | .NET 9.0      | .NET 9.0      | 329.7 μs | 1.96 μs | 1.84 μs |  1.00 | 72.2656 | 147.91 KB |        1.00 |
| Trace_FieldLineFromSouthToNorthHemisphere | .NET 9.0      | .NET 9.0      | 418.0 μs | 0.75 μs | 0.71 μs |  1.27 | 89.3555 |  182.7 KB |        1.24 |
|                                           |               |               |          |         |         |       |         |           |             |
| Trace_FieldLineFromNorthToSouthHemisphere | NativeAOT     | NativeAOT 9.0 | 330.2 μs | 1.11 μs | 1.04 μs |  1.00 | 72.2656 |  147.9 KB |        1.00 |
| Trace_FieldLineFromSouthToNorthHemisphere | NativeAOT     | NativeAOT 9.0 | 410.6 μs | 1.21 μs | 1.13 μs |  1.24 | 89.3555 | 182.68 KB |        1.24 |
|                                           |               |               |          |         |         |       |         |           |             |
| Trace_FieldLineFromNorthToSouthHemisphere | NativeAOT 9.0 | NativeAOT 9.0 | 331.6 μs | 1.30 μs | 1.22 μs |  1.00 | 72.2656 |  147.9 KB |        1.00 |
| Trace_FieldLineFromSouthToNorthHemisphere | NativeAOT 9.0 | NativeAOT 9.0 | 413.3 μs | 1.89 μs | 1.77 μs |  1.25 | 89.3555 | 182.68 KB |        1.24 |
