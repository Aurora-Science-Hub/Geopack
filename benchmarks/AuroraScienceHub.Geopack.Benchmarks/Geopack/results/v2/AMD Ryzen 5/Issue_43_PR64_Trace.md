```

BenchmarkDotNet v0.15.4, Linux Ubuntu 24.04.2 LTS (Noble Numbat)
AMD Ryzen 5 5500U with Radeon Graphics 0.40GHz, 1 CPU, 12 logical and 6 physical cores
.NET SDK 9.0.302
  [Host]        : .NET 9.0.7 (9.0.7, 9.0.725.31616), X64 RyuJIT x86-64-v3
  .NET 9.0      : .NET 9.0.7 (9.0.7, 9.0.725.31616), X64 RyuJIT x86-64-v3
  NativeAOT     : .NET 9.0.7, X64 NativeAOT x86-64-v3
  NativeAOT 9.0 : .NET 9.0.7, X64 NativeAOT x86-64-v3


```
| Method                                    | Job           | Runtime       | Mean     | Error   | StdDev  | Ratio | Gen0     | Allocated | Alloc Ratio |
|------------------------------------------ |-------------- |-------------- |---------:|--------:|--------:|------:|---------:|----------:|------------:|
| Trace_FieldLineFromNorthToSouthHemisphere | .NET 9.0      | .NET 9.0      | 333.0 μs | 3.04 μs | 2.85 μs |  1.00 |  84.4727 | 172.73 KB |        1.00 |
| Trace_FieldLineFromSouthToNorthHemisphere | .NET 9.0      | .NET 9.0      | 414.7 μs | 1.85 μs | 1.64 μs |  1.25 | 104.0039 | 212.94 KB |        1.23 |
|                                           |               |               |          |         |         |       |          |           |             |
| Trace_FieldLineFromNorthToSouthHemisphere | NativeAOT     | NativeAOT 9.0 | 328.4 μs | 1.07 μs | 1.00 μs |  1.00 |  84.4727 | 172.76 KB |        1.00 |
| Trace_FieldLineFromSouthToNorthHemisphere | NativeAOT     | NativeAOT 9.0 | 407.3 μs | 1.51 μs | 1.41 μs |  1.24 | 104.0039 | 212.96 KB |        1.23 |
|                                           |               |               |          |         |         |       |          |           |             |
| Trace_FieldLineFromNorthToSouthHemisphere | NativeAOT 9.0 | NativeAOT 9.0 | 328.3 μs | 2.01 μs | 1.88 μs |  1.00 |  84.4727 | 172.76 KB |        1.00 |
| Trace_FieldLineFromSouthToNorthHemisphere | NativeAOT 9.0 | NativeAOT 9.0 | 409.0 μs | 1.04 μs | 0.97 μs |  1.25 | 104.0039 | 212.96 KB |        1.23 |
