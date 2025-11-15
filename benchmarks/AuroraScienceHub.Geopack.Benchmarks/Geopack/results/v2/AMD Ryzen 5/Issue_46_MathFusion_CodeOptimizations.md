```

BenchmarkDotNet v0.15.4, Linux Ubuntu 24.04.2 LTS (Noble Numbat)
AMD Ryzen 5 5500U with Radeon Graphics 0.40GHz, 1 CPU, 12 logical and 6 physical cores
.NET SDK 9.0.302
  [Host]    : .NET 9.0.7 (9.0.7, 9.0.725.31616), X64 RyuJIT x86-64-v3
  .NET 9.0  : .NET 9.0.7 (9.0.7, 9.0.725.31616), X64 RyuJIT x86-64-v3
  NativeAOT : .NET 9.0.7, X64 NativeAOT x86-64-v3


```
| Method            | Job       | Runtime       | Mean           | Error       | StdDev      | Ratio   | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------ |---------- |-------------- |---------------:|------------:|------------:|--------:|--------:|-------:|----------:|------------:|
| ToSphericalVector | .NET 9.0  | .NET 9.0      |       6.015 ns |   0.0284 ns |   0.0237 ns |   0.005 |    0.00 |      - |         - |        0.00 |
| ToCartesianVector | .NET 9.0  | .NET 9.0      |      27.419 ns |   0.1126 ns |   0.0940 ns |   0.024 |    0.00 |      - |         - |        0.00 |
| ToSpherical       | .NET 9.0  | .NET 9.0      |      33.749 ns |   0.0963 ns |   0.0752 ns |   0.030 |    0.00 |      - |         - |        0.00 |
| ToCartesian       | .NET 9.0  | .NET 9.0      |      24.938 ns |   0.0367 ns |   0.0306 ns |   0.022 |    0.00 |      - |         - |        0.00 |
| GeiToGeo          | .NET 9.0  | .NET 9.0      |       3.769 ns |   0.0125 ns |   0.0117 ns |   0.003 |    0.00 |      - |         - |        0.00 |
| GeoToGei          | .NET 9.0  | .NET 9.0      |       4.484 ns |   0.0049 ns |   0.0041 ns |   0.004 |    0.00 |      - |         - |        0.00 |
| ToGeocentric      | .NET 9.0  | .NET 9.0      |      48.499 ns |   0.0799 ns |   0.0624 ns |   0.043 |    0.00 |      - |         - |        0.00 |
| ToGeodetic        | .NET 9.0  | .NET 9.0      |     180.313 ns |   0.1000 ns |   0.0835 ns |   0.159 |    0.00 |      - |         - |        0.00 |
| GeoToGsw          | .NET 9.0  | .NET 9.0      |       4.998 ns |   0.0133 ns |   0.0124 ns |   0.004 |    0.00 |      - |         - |        0.00 |
| GswToGeo          | .NET 9.0  | .NET 9.0      |       5.753 ns |   0.0164 ns |   0.0145 ns |   0.005 |    0.00 |      - |         - |        0.00 |
| GeoToMag          | .NET 9.0  | .NET 9.0      |       5.012 ns |   0.0286 ns |   0.0254 ns |   0.004 |    0.00 |      - |         - |        0.00 |
| MagToGeo          | .NET 9.0  | .NET 9.0      |       5.265 ns |   0.0223 ns |   0.0186 ns |   0.005 |    0.00 |      - |         - |        0.00 |
| GswToGse          | .NET 9.0  | .NET 9.0      |       5.275 ns |   0.0127 ns |   0.0106 ns |   0.005 |    0.00 |      - |         - |        0.00 |
| GseToGsw          | .NET 9.0  | .NET 9.0      |       5.244 ns |   0.0236 ns |   0.0197 ns |   0.005 |    0.00 |      - |         - |        0.00 |
| MagToSm           | .NET 9.0  | .NET 9.0      |       3.783 ns |   0.0110 ns |   0.0092 ns |   0.003 |    0.00 |      - |         - |        0.00 |
| SmToMag           | .NET 9.0  | .NET 9.0      |       4.489 ns |   0.0209 ns |   0.0185 ns |   0.004 |    0.00 |      - |         - |        0.00 |
| SmToGsw           | .NET 9.0  | .NET 9.0      |       3.773 ns |   0.0089 ns |   0.0084 ns |   0.003 |    0.00 |      - |         - |        0.00 |
| GswToSm           | .NET 9.0  | .NET 9.0      |       4.499 ns |   0.0214 ns |   0.0200 ns |   0.004 |    0.00 |      - |         - |        0.00 |
| Recalc            | .NET 9.0  | .NET 9.0      |   1,134.689 ns |   5.1221 ns |   4.2772 ns |   1.000 |    0.01 | 1.3828 |    2896 B |        1.00 |
| ShuMgnp           | .NET 9.0  | .NET 9.0      |     396.548 ns |   1.4619 ns |   1.2959 ns |   0.349 |    0.00 |      - |         - |        0.00 |
| T96Mgnp           | .NET 9.0  | .NET 9.0      |      63.408 ns |   0.3308 ns |   0.2763 ns |   0.056 |    0.00 |      - |         - |        0.00 |
| IgrfGeo           | .NET 9.0  | .NET 9.0      |     462.025 ns |   4.0048 ns |   3.7461 ns |   0.407 |    0.00 | 0.1297 |     272 B |        0.09 |
| IgrfGsw           | .NET 9.0  | .NET 9.0      |     364.864 ns |   0.8967 ns |   0.7488 ns |   0.322 |    0.00 |      - |         - |        0.00 |
| Dip               | .NET 9.0  | .NET 9.0      |       7.187 ns |   0.0530 ns |   0.0469 ns |   0.006 |    0.00 |      - |         - |        0.00 |
| Sun               | .NET 9.0  | .NET 9.0      |     136.126 ns |   0.3750 ns |   0.3324 ns |   0.120 |    0.00 |      - |         - |        0.00 |
| T89               | .NET 9.0  | .NET 9.0      |     124.663 ns |   0.1212 ns |   0.1075 ns |   0.110 |    0.00 |      - |         - |        0.00 |
| Trace_NS          | .NET 9.0  | .NET 9.0      | 327,918.775 ns | 980.8614 ns | 869.5081 ns | 288.998 |    1.28 | 7.8125 |   16592 B |        5.73 |
| Trace_SN          | .NET 9.0  | .NET 9.0      | 305,403.031 ns | 886.5344 ns | 829.2648 ns | 269.155 |    1.20 | 3.9063 |    8376 B |        2.89 |
|                   |           |               |                |             |             |         |         |        |           |             |
| ToSphericalVector | NativeAOT | NativeAOT 9.0 |       8.021 ns |   0.0352 ns |   0.0330 ns |   0.007 |    0.00 |      - |         - |        0.00 |
| ToCartesianVector | NativeAOT | NativeAOT 9.0 |      37.268 ns |   0.2042 ns |   0.1810 ns |   0.034 |    0.00 |      - |         - |        0.00 |
| ToSpherical       | NativeAOT | NativeAOT 9.0 |      35.106 ns |   0.0724 ns |   0.0605 ns |   0.032 |    0.00 |      - |         - |        0.00 |
| ToCartesian       | NativeAOT | NativeAOT 9.0 |      33.809 ns |   0.0637 ns |   0.0498 ns |   0.031 |    0.00 |      - |         - |        0.00 |
| GeiToGeo          | NativeAOT | NativeAOT 9.0 |       5.318 ns |   0.0287 ns |   0.0268 ns |   0.005 |    0.00 |      - |         - |        0.00 |
| GeoToGei          | NativeAOT | NativeAOT 9.0 |       5.006 ns |   0.0260 ns |   0.0217 ns |   0.005 |    0.00 |      - |         - |        0.00 |
| ToGeocentric      | NativeAOT | NativeAOT 9.0 |      57.021 ns |   0.2129 ns |   0.1992 ns |   0.052 |    0.00 |      - |         - |        0.00 |
| ToGeodetic        | NativeAOT | NativeAOT 9.0 |     184.438 ns |   0.2080 ns |   0.1946 ns |   0.167 |    0.00 |      - |         - |        0.00 |
| GeoToGsw          | NativeAOT | NativeAOT 9.0 |       6.009 ns |   0.0190 ns |   0.0159 ns |   0.005 |    0.00 |      - |         - |        0.00 |
| GswToGeo          | NativeAOT | NativeAOT 9.0 |       6.256 ns |   0.0176 ns |   0.0165 ns |   0.006 |    0.00 |      - |         - |        0.00 |
| GeoToMag          | NativeAOT | NativeAOT 9.0 |       6.860 ns |   0.0904 ns |   0.0846 ns |   0.006 |    0.00 |      - |         - |        0.00 |
| MagToGeo          | NativeAOT | NativeAOT 9.0 |       5.796 ns |   0.0164 ns |   0.0145 ns |   0.005 |    0.00 |      - |         - |        0.00 |
| GswToGse          | NativeAOT | NativeAOT 9.0 |       5.988 ns |   0.0156 ns |   0.0146 ns |   0.005 |    0.00 |      - |         - |        0.00 |
| GseToGsw          | NativeAOT | NativeAOT 9.0 |       6.010 ns |   0.0095 ns |   0.0079 ns |   0.005 |    0.00 |      - |         - |        0.00 |
| MagToSm           | NativeAOT | NativeAOT 9.0 |       5.762 ns |   0.0551 ns |   0.0460 ns |   0.005 |    0.00 |      - |         - |        0.00 |
| SmToMag           | NativeAOT | NativeAOT 9.0 |       5.260 ns |   0.0180 ns |   0.0159 ns |   0.005 |    0.00 |      - |         - |        0.00 |
| SmToGsw           | NativeAOT | NativeAOT 9.0 |       5.733 ns |   0.0102 ns |   0.0091 ns |   0.005 |    0.00 |      - |         - |        0.00 |
| GswToSm           | NativeAOT | NativeAOT 9.0 |       4.927 ns |   0.0758 ns |   0.0709 ns |   0.004 |    0.00 |      - |         - |        0.00 |
| Recalc            | NativeAOT | NativeAOT 9.0 |   1,102.394 ns |   3.8192 ns |   3.3856 ns |   1.000 |    0.00 | 1.3828 |    2896 B |        1.00 |
| ShuMgnp           | NativeAOT | NativeAOT 9.0 |     434.853 ns |   1.9510 ns |   1.7295 ns |   0.394 |    0.00 |      - |         - |        0.00 |
| T96Mgnp           | NativeAOT | NativeAOT 9.0 |      68.899 ns |   0.1755 ns |   0.1642 ns |   0.062 |    0.00 |      - |         - |        0.00 |
| IgrfGeo           | NativeAOT | NativeAOT 9.0 |     475.430 ns |   4.8892 ns |   4.5734 ns |   0.431 |    0.00 | 0.1297 |     272 B |        0.09 |
| IgrfGsw           | NativeAOT | NativeAOT 9.0 |     367.926 ns |   1.1653 ns |   1.0900 ns |   0.334 |    0.00 |      - |         - |        0.00 |
| Dip               | NativeAOT | NativeAOT 9.0 |       8.974 ns |   0.1253 ns |   0.1172 ns |   0.008 |    0.00 |      - |         - |        0.00 |
| Sun               | NativeAOT | NativeAOT 9.0 |     151.052 ns |   1.0273 ns |   0.9106 ns |   0.137 |    0.00 |      - |         - |        0.00 |
| T89               | NativeAOT | NativeAOT 9.0 |     132.235 ns |   0.2862 ns |   0.2390 ns |   0.120 |    0.00 |      - |         - |        0.00 |
| Trace_NS          | NativeAOT | NativeAOT 9.0 | 332,476.653 ns | 719.5065 ns | 637.8238 ns | 301.598 |    1.06 | 7.8125 |   16576 B |        5.72 |
| Trace_SN          | NativeAOT | NativeAOT 9.0 | 311,434.523 ns | 712.7694 ns | 631.8515 ns | 282.510 |    1.01 | 3.9063 |    8360 B |        2.89 |
