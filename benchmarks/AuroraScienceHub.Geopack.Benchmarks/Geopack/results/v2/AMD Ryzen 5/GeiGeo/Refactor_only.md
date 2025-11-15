```

BenchmarkDotNet v0.15.4, Linux Ubuntu 24.04.2 LTS (Noble Numbat)
AMD Ryzen 5 5500U with Radeon Graphics 0.40GHz, 1 CPU, 12 logical and 6 physical cores
.NET SDK 9.0.302
  [Host]    : .NET 9.0.7 (9.0.7, 9.0.725.31616), X64 RyuJIT x86-64-v3
  .NET 9.0  : .NET 9.0.7 (9.0.7, 9.0.725.31616), X64 RyuJIT x86-64-v3
  NativeAOT : .NET 9.0.7, X64 NativeAOT x86-64-v3


```
| Method   | Job       | Runtime       | Mean     | Error     | StdDev    | Allocated |
|--------- |---------- |-------------- |---------:|----------:|----------:|----------:|
| GeiToGeo | .NET 9.0  | .NET 9.0      | 3.778 ns | 0.0098 ns | 0.0087 ns |         - |
| GeiToGeo | NativeAOT | NativeAOT 9.0 | 5.351 ns | 0.0376 ns | 0.0352 ns |         - |
