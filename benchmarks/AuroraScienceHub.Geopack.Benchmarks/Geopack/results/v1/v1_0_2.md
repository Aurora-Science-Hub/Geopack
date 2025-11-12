```

BenchmarkDotNet v0.15.4, Linux Ubuntu 24.04.2 LTS (Noble Numbat)
AMD Ryzen 5 5500U with Radeon Graphics 0.40GHz, 1 CPU, 12 logical and 6 physical cores
.NET SDK 9.0.302
  [Host]    : .NET 9.0.7 (9.0.7, 9.0.725.31616), X64 RyuJIT x86-64-v3
  .NET 9.0  : .NET 9.0.7 (9.0.7, 9.0.725.31616), X64 RyuJIT x86-64-v3
  NativeAOT : .NET 9.0.7, X64 NativeAOT x86-64-v3


```
| Method            | Job       | Runtime       | Mean            | Error         | StdDev        | Ratio   | RatioSD | Gen0     | Allocated | Alloc Ratio |
|------------------ |---------- |-------------- |----------------:|--------------:|--------------:|--------:|--------:|---------:|----------:|------------:|
| ToSphericalVector | .NET 9.0  | .NET 9.0      |      59.7672 ns |     0.2919 ns |     0.2438 ns |   0.064 |    0.00 |   0.0229 |      48 B |        0.86 |
| ToCartesianVector | .NET 9.0  | .NET 9.0      |       0.6896 ns |     0.0017 ns |     0.0013 ns |   0.001 |    0.00 |        - |         - |        0.00 |
| ToSpherical       | .NET 9.0  | .NET 9.0      |      39.4891 ns |     0.1667 ns |     0.1478 ns |   0.042 |    0.00 |   0.0229 |      48 B |        0.86 |
| ToCartesian       | .NET 9.0  | .NET 9.0      |       0.6928 ns |     0.0029 ns |     0.0027 ns |   0.001 |    0.00 |        - |         - |        0.00 |
| GeiToGeo          | .NET 9.0  | .NET 9.0      |       0.9600 ns |     0.0060 ns |     0.0053 ns |   0.001 |    0.00 |        - |         - |        0.00 |
| GeoToGei          | .NET 9.0  | .NET 9.0      |       0.9634 ns |     0.0079 ns |     0.0062 ns |   0.001 |    0.00 |        - |         - |        0.00 |
| ToGeocentric      | .NET 9.0  | .NET 9.0      |     162.8185 ns |     0.2715 ns |     0.2267 ns |   0.175 |    0.00 |   0.0229 |      48 B |        0.86 |
| ToGeodetic        | .NET 9.0  | .NET 9.0      |     432.9462 ns |     0.2198 ns |     0.1948 ns |   0.465 |    0.00 |   0.0229 |      48 B |        0.86 |
| GeoToGsw          | .NET 9.0  | .NET 9.0      |       9.1128 ns |     0.0337 ns |     0.0299 ns |   0.010 |    0.00 |   0.0229 |      48 B |        0.86 |
| GswToGeo          | .NET 9.0  | .NET 9.0      |       9.0282 ns |     0.0313 ns |     0.0277 ns |   0.010 |    0.00 |   0.0229 |      48 B |        0.86 |
| GeoToMag          | .NET 9.0  | .NET 9.0      |       1.8072 ns |     0.0098 ns |     0.0092 ns |   0.002 |    0.00 |        - |         - |        0.00 |
| MagToGeo          | .NET 9.0  | .NET 9.0      |       1.8136 ns |     0.0044 ns |     0.0042 ns |   0.002 |    0.00 |        - |         - |        0.00 |
| GswToGse          | .NET 9.0  | .NET 9.0      |       8.9024 ns |     0.0205 ns |     0.0192 ns |   0.010 |    0.00 |   0.0229 |      48 B |        0.86 |
| GseToGsw          | .NET 9.0  | .NET 9.0      |       8.9462 ns |     0.0207 ns |     0.0183 ns |   0.010 |    0.00 |   0.0229 |      48 B |        0.86 |
| MagToSm           | .NET 9.0  | .NET 9.0      |       0.9701 ns |     0.0072 ns |     0.0064 ns |   0.001 |    0.00 |        - |         - |        0.00 |
| SmToMag           | .NET 9.0  | .NET 9.0      |       0.9589 ns |     0.0035 ns |     0.0031 ns |   0.001 |    0.00 |        - |         - |        0.00 |
| SmToGsw           | .NET 9.0  | .NET 9.0      |       0.9650 ns |     0.0050 ns |     0.0047 ns |   0.001 |    0.00 |        - |         - |        0.00 |
| GswToSm           | .NET 9.0  | .NET 9.0      |       0.9673 ns |     0.0073 ns |     0.0065 ns |   0.001 |    0.00 |        - |         - |        0.00 |
| Recalc            | .NET 9.0  | .NET 9.0      |     930.8796 ns |     1.9132 ns |     1.6960 ns |   1.000 |    0.00 |   0.0267 |      56 B |        1.00 |
| ShuMgnp           | .NET 9.0  | .NET 9.0      |     518.3017 ns |     0.7657 ns |     0.6394 ns |   0.557 |    0.00 |   0.0267 |      56 B |        1.00 |
| T96Mgnp           | .NET 9.0  | .NET 9.0      |     113.4342 ns |     0.3340 ns |     0.2789 ns |   0.122 |    0.00 |   0.0267 |      56 B |        1.00 |
| IgrfGeo           | .NET 9.0  | .NET 9.0      |     517.5987 ns |     4.9924 ns |     4.6699 ns |   0.556 |    0.00 |   0.1526 |     320 B |        5.71 |
| IgrfGsw           | .NET 9.0  | .NET 9.0      |     476.9380 ns |     4.3309 ns |     4.0512 ns |   0.512 |    0.00 |   0.1602 |     336 B |        6.00 |
| Dip               | .NET 9.0  | .NET 9.0      |     129.6760 ns |     0.4417 ns |     0.4132 ns |   0.139 |    0.00 |   0.0229 |      48 B |        0.86 |
| Sun               | .NET 9.0  | .NET 9.0      |     156.1224 ns |     0.2478 ns |     0.2069 ns |   0.168 |    0.00 |   0.0267 |      56 B |        1.00 |
| T89               | .NET 9.0  | .NET 9.0      |     132.3734 ns |     0.5745 ns |     0.5374 ns |   0.142 |    0.00 |   0.0229 |      48 B |        0.86 |
| Trace_NS          | .NET 9.0  | .NET 9.0      | 391,165.6051 ns | 1,507.8918 ns | 1,259.1579 ns | 420.212 |    1.50 | 155.7617 |  326104 B |    5,823.29 |
| Trace_SN          | .NET 9.0  | .NET 9.0      | 363,925.3938 ns |   781.2145 ns |   652.3495 ns | 390.949 |    0.96 | 133.3008 |  279168 B |    4,985.14 |
|                   |           |               |                 |               |               |         |         |          |           |             |
| ToSphericalVector | NativeAOT | NativeAOT 9.0 |      59.1422 ns |     0.0643 ns |     0.0570 ns |   0.065 |    0.00 |   0.0229 |      48 B |        0.86 |
| ToCartesianVector | NativeAOT | NativeAOT 9.0 |      30.9974 ns |     0.0228 ns |     0.0213 ns |   0.034 |    0.00 |        - |         - |        0.00 |
| ToSpherical       | NativeAOT | NativeAOT 9.0 |      37.8884 ns |     0.0756 ns |     0.0707 ns |   0.041 |    0.00 |   0.0229 |      48 B |        0.86 |
| ToCartesian       | NativeAOT | NativeAOT 9.0 |      30.8959 ns |     0.0268 ns |     0.0224 ns |   0.034 |    0.00 |        - |         - |        0.00 |
| GeiToGeo          | NativeAOT | NativeAOT 9.0 |       1.7135 ns |     0.0052 ns |     0.0046 ns |   0.002 |    0.00 |        - |         - |        0.00 |
| GeoToGei          | NativeAOT | NativeAOT 9.0 |       1.7140 ns |     0.0052 ns |     0.0046 ns |   0.002 |    0.00 |        - |         - |        0.00 |
| ToGeocentric      | NativeAOT | NativeAOT 9.0 |     161.7982 ns |     0.2793 ns |     0.2476 ns |   0.177 |    0.00 |   0.0229 |      48 B |        0.86 |
| ToGeodetic        | NativeAOT | NativeAOT 9.0 |     428.5029 ns |     0.1314 ns |     0.1097 ns |   0.469 |    0.00 |   0.0229 |      48 B |        0.86 |
| GeoToGsw          | NativeAOT | NativeAOT 9.0 |       7.4614 ns |     0.0110 ns |     0.0092 ns |   0.008 |    0.00 |   0.0229 |      48 B |        0.86 |
| GswToGeo          | NativeAOT | NativeAOT 9.0 |       7.5153 ns |     0.0329 ns |     0.0292 ns |   0.008 |    0.00 |   0.0229 |      48 B |        0.86 |
| GeoToMag          | NativeAOT | NativeAOT 9.0 |       2.7315 ns |     0.0150 ns |     0.0140 ns |   0.003 |    0.00 |        - |         - |        0.00 |
| MagToGeo          | NativeAOT | NativeAOT 9.0 |       2.7008 ns |     0.0078 ns |     0.0065 ns |   0.003 |    0.00 |        - |         - |        0.00 |
| GswToGse          | NativeAOT | NativeAOT 9.0 |       7.4995 ns |     0.0243 ns |     0.0227 ns |   0.008 |    0.00 |   0.0229 |      48 B |        0.86 |
| GseToGsw          | NativeAOT | NativeAOT 9.0 |       7.8096 ns |     0.0077 ns |     0.0068 ns |   0.009 |    0.00 |   0.0229 |      48 B |        0.86 |
| MagToSm           | NativeAOT | NativeAOT 9.0 |       1.6939 ns |     0.0029 ns |     0.0027 ns |   0.002 |    0.00 |        - |         - |        0.00 |
| SmToMag           | NativeAOT | NativeAOT 9.0 |       1.7131 ns |     0.0027 ns |     0.0024 ns |   0.002 |    0.00 |        - |         - |        0.00 |
| SmToGsw           | NativeAOT | NativeAOT 9.0 |       1.6977 ns |     0.0053 ns |     0.0050 ns |   0.002 |    0.00 |        - |         - |        0.00 |
| GswToSm           | NativeAOT | NativeAOT 9.0 |       1.7152 ns |     0.0031 ns |     0.0028 ns |   0.002 |    0.00 |        - |         - |        0.00 |
| Recalc            | NativeAOT | NativeAOT 9.0 |     914.3908 ns |     1.1126 ns |     1.0407 ns |   1.000 |    0.00 |   0.0267 |      56 B |        1.00 |
| ShuMgnp           | NativeAOT | NativeAOT 9.0 |     517.3570 ns |     0.7402 ns |     0.6924 ns |   0.566 |    0.00 |   0.0534 |     112 B |        2.00 |
| T96Mgnp           | NativeAOT | NativeAOT 9.0 |     111.6492 ns |     0.1294 ns |     0.1147 ns |   0.122 |    0.00 |   0.0267 |      56 B |        1.00 |
| IgrfGeo           | NativeAOT | NativeAOT 9.0 |     511.5005 ns |     2.7796 ns |     2.6000 ns |   0.559 |    0.00 |   0.1526 |     320 B |        5.71 |
| IgrfGsw           | NativeAOT | NativeAOT 9.0 |     488.1209 ns |     0.7347 ns |     0.6872 ns |   0.534 |    0.00 |   0.2060 |     432 B |        7.71 |
| Dip               | NativeAOT | NativeAOT 9.0 |     127.0028 ns |     0.1310 ns |     0.1094 ns |   0.139 |    0.00 |   0.0229 |      48 B |        0.86 |
| Sun               | NativeAOT | NativeAOT 9.0 |     162.3006 ns |     0.1244 ns |     0.1103 ns |   0.177 |    0.00 |   0.0267 |      56 B |        1.00 |
| T89               | NativeAOT | NativeAOT 9.0 |     130.5251 ns |     0.2040 ns |     0.1908 ns |   0.143 |    0.00 |   0.0229 |      48 B |        0.86 |
| Trace_NS          | NativeAOT | NativeAOT 9.0 | 402,525.5413 ns |   175.5136 ns |   146.5618 ns | 440.212 |    0.51 | 194.8242 |  407824 B |    7,282.57 |
| Trace_SN          | NativeAOT | NativeAOT 9.0 | 381,386.3147 ns | 1,622.5544 ns | 1,517.7384 ns | 417.094 |    1.67 | 165.0391 |  345528 B |    6,170.14 |
