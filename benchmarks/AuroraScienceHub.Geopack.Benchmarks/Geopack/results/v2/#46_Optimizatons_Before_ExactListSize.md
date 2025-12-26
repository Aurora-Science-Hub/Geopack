```

BenchmarkDotNet v0.15.4, Linux Ubuntu 24.04.3 LTS (Noble Numbat)
AMD Ryzen 5 5500U with Radeon Graphics 0.40GHz, 1 CPU, 12 logical and 6 physical cores
.NET SDK 10.0.100
  [Host]    : .NET 10.0.0 (10.0.0, 10.0.25.52411), X64 RyuJIT x86-64-v3
  .NET 10.0 : .NET 10.0.0 (10.0.0, 10.0.25.52411), X64 RyuJIT x86-64-v3
  .NET 8.0  : .NET 8.0.22 (8.0.22, 8.0.2225.52707), X64 RyuJIT x86-64-v3


```
| Method   | Job       | Runtime   | Mean     | Error   | StdDev  | Gen0    | Allocated |
|--------- |---------- |---------- |---------:|--------:|--------:|--------:|----------:|
| Trace_NS | .NET 10.0 | .NET 10.0 | 351.4 μs | 2.56 μs | 2.27 μs | 97.6563 | 199.68 KB |
| Trace_SN | .NET 10.0 | .NET 10.0 | 342.3 μs | 1.36 μs | 1.28 μs | 87.4023 | 178.91 KB |
| Trace_NS | .NET 8.0  | .NET 8.0  | 384.6 μs | 2.06 μs | 1.92 μs | 97.6563 | 199.81 KB |
| Trace_SN | .NET 8.0  | .NET 8.0  | 373.1 μs | 1.82 μs | 1.71 μs | 87.4023 | 179.04 KB |
