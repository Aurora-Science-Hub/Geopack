```

BenchmarkDotNet v0.15.4, Linux Ubuntu 24.04.2 LTS (Noble Numbat)
AMD Ryzen 5 5500U with Radeon Graphics 0.40GHz, 1 CPU, 12 logical and 6 physical cores
.NET SDK 9.0.302
  [Host]    : .NET 9.0.7 (9.0.7, 9.0.725.31616), X64 RyuJIT x86-64-v3
  .NET 9.0  : .NET 9.0.7 (9.0.7, 9.0.725.31616), X64 RyuJIT x86-64-v3
  NativeAOT : .NET 9.0.7, X64 NativeAOT x86-64-v3


```
| Method | Job       | Runtime       | Mean     | Error     | StdDev    | Allocated |
|------- |---------- |-------------- |---------:|----------:|----------:|----------:|
| Dip    | .NET 9.0  | .NET 9.0      | 6.907 ns | 0.0204 ns | 0.0181 ns |         - |
| Dip    | NativeAOT | NativeAOT 9.0 | 9.029 ns | 0.0203 ns | 0.0190 ns |         - |
