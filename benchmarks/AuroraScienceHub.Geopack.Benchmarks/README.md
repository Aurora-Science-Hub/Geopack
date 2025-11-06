# Geopack-2008 Benchmarks

This project contains performance benchmarks for Geopack-2008 field line tracing procedures and magnetic field calculations.

## Benchmark Results

**Environment:**
- BenchmarkDotNet v0.15.4
- Linux Ubuntu 24.04.2 LTS (Noble Numbat)
- AMD Ryzen 5 5500U with Radeon Graphics 0.40GHz
- 1 CPU, 12 logical and 6 physical cores
- .NET SDK 9.0.302
- .NET 9.0.7, X64 RyuJIT x86-64-v3

#### Simple Calculation Performance

| Method | Runtime       | Mean (ns) | Error (ns) | StdDev (ns) | Ratio / RefRatio | Gen0   | Allocated | Alloc Ratio |
|--------|---------------|-----------|------------|-------------|------------------|--------|-----------|-------------|
| IGRF   | IFortran`25   | 1802.00   | 52.43      | 1004.89     | 1.00 / 1.00      | N/A    | N/A       | N/A         |
| Dipole | IFortran`25   | 1359.00   | 36.99      | 708.95      | 0.75 / 1.00      | N/A    | N/A       | N/A         |
| Sun    | IFortran`25   | 1446.00   | 25.94      | 497.32      | 0.80 / 1.00      | N/A    | N/A       | N/A         |
|        |               |           |            |             |                  |        |           |             |
| IGRF   | .NET 9.0      | 476.7     | 3.81       | 3.57        | 1.00             | 0.1602 | 336 B     | 1.00        |
| Dipole | .NET 9.0      | 105.7     | 0.58       | 0.54        | 0.22             | 0.0229 | 48 B      | 0.14        |
| Sun    | .NET 9.0      | 156.7     | 0.31       | 0.28        | 0.33             | 0.0267 | 56 B      | 0.17        |
|        |               |           |            |             |                  |        |           |             |
| IGRF   | NativeAOT 9.0 | 490.0     | 2.03       | 1.90        | 1.00             | 0.2060 | 432 B     | 1.00        |
| Dipole | NativeAOT 9.0 | 101.4     | 1.43       | 1.19        | 0.21             | 0.0229 | 48 B      | 0.11        |
| Sun    | NativeAOT 9.0 | 163.7     | 0.60       | 0.56        | 0.33             | 0.0267 | 56 B      | 0.13        |

#### Magnetic Field Line Tracing Performance

| Method                          | Runtime       | Mean (ns) | Error (ns) | StdDev (ns) | Ratio / RefRatio |     Gen0 |      Allocated | Alloc Ratio |
|---------------------------------|---------------|----------:|-----------:|------------:|-----------------:|---------:|---------------:|------------:|
| Trace North -> South Hemisphere | IFortran`25   | 216379.00 |        554 |    10635.99 |      1.00 / 1.00 |      N/A |            N/A |         N/A |
| Trace South -> North Hemisphere | IFortran`25   | 288899.00 |        403 |     7735.02 |  **1.34** / 1.00 |      N/A |            N/A |         N/A |
|                                 |               |           |            |             |                  |          |                |             |
| Trace North -> South Hemisphere | .NET 9.0      | 305100.00 |       5290 |        4130 |             1.00 | 108.3984 |      222.21 KB |        1.00 |
| Trace South -> North Hemisphere | .NET 9.0      | 386800.00 |       1750 |        1560 |             1.27 | 134.7656 |      275.49 KB |        1.24 |
|                                 |               |           |            |             |                  |          |                |             |
| Trace North -> South Hemisphere | NativeAOT 9.0 | 313800.00 |       2080 |        1950 |             1.00 | 134.2773 |      274.36 KB |        1.00 |
| Trace South -> North Hemisphere | NativeAOT 9.0 | 389200.00 |       2760 |        2450 |             1.24 | 166.5039 |      340.77 KB |        1.24 |


