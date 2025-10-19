# Geopack-2008 Benchmarks

This project contains performance benchmarks for Geopack-2008 field line tracing procedures and magnetic field calculations.

## 📊 Benchmark Results

### Magnetic Field Calculations Performance

**Environment:**
- BenchmarkDotNet v0.15.4
- Linux Ubuntu 24.04.2 LTS (Noble Numbat)
- AMD Ryzen 5 5500U with Radeon Graphics 0.40GHz
- 1 CPU, 12 logical and 6 physical cores
- .NET SDK 9.0.302
- .NET 9.0.7, X64 RyuJIT x86-64-v3

#### Simple Calculation Performance

| Method                    | Runtime | Mean     | Error   | StdDev  | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|---------------------------|---------|----------|---------|---------|-------|---------|--------|-----------|-------------|
| IGRF (GSW) magnetic field | .NET 9  | 486.2 ns | 8.70 ns | 7.26 ns | 1.00  | 0.02    | 0.1602 | 336 B     | 1.00        |
| IGRF (GSW) magnetic field | .NET 9  | 486.2 ns | 8.70 ns | 7.26 ns | 1.00  | 0.02    | 0.1602 | 336 B     | 1.00        |
| IGRF (GSW) magnetic field | .NET 9  | 486.2 ns | 8.70 ns | 7.26 ns | 1.00  | 0.02    | 0.1602 | 336 B     | 1.00        |
|                           |         |          |         |         |       |         |        |           |             |
| Dipole magnetic field     | .NET 9  | 104.4 ns | 0.27 ns | 0.23 ns | 0.21  | 0.00    | 0.0229 | 48 B      | 0.14        |
| Dipole magnetic field     | .NET 9  | 104.4 ns | 0.27 ns | 0.23 ns | 0.21  | 0.00    | 0.0229 | 48 B      | 0.14        |
| Dipole magnetic field     | .NET 9  | 104.4 ns | 0.27 ns | 0.23 ns | 0.21  | 0.00    | 0.0229 | 48 B      | 0.14        |
|                           |         |          |         |         |       |         |        |           |             |
| Sun position              | .NET 9  | 156.1 ns | 0.52 ns | 0.49 ns | 0.32  | 0.00    | 0.0267 | 56 B      | 0.17        |
| Sun position              | .NET 9  | 156.1 ns | 0.52 ns | 0.49 ns | 0.32  | 0.00    | 0.0267 | 56 B      | 0.17        |
| Sun position              | .NET 9  | 156.1 ns | 0.52 ns | 0.49 ns | 0.32  | 0.00    | 0.0267 | 56 B      | 0.17        |

#### Magnetic Field Line Tracing Performance

| Method                          | Runtime            |     Mean |   Error |  StdDev |    Ratio / RefRatio |  Gen0 | Allocated |    Alloc Ratio |
|---------------------------------|--------------------|---------:|--------:|--------:|--------------------:|------:|----------:|---------------:|
| Trace North -> South Hemisphere | Intel Fortran 2025 | 215.2 μs | 0.35 μs | 6.64 μs |         1.00 / 1.00 |   N/A |       N/A |    1.00 / 1.00 |
| Trace North -> South Hemisphere | .NET 9             | 307.1 μs | 1.09 μs | 0.96 μs |     1.00 / **1.43** | 108.4 | 222.21 KB |     1.00 / N/A |
| Trace North -> South Hemisphere | NativeAOT 9        |      N/A |     N/A |     N/A |          1.00 / N/A |   N/A |       N/A |     1.00 / N/A |
|                                 |                    |          |         |         |                     |       |           |                |
| Trace South -> North Hemisphere | Intel Fortran 2025 | 289.3 μs | 0.38 μs | 7.20 μs |     **1.34** / 1.00 |   N/A |       N/A |    1.00 / 1.00 |
| Trace South -> North Hemisphere | .NET 9             | 383.6 μs | 2.17 μs | 2.03 μs | **1.25** / **1.33** | 134.8 | 275.49 KB | **1.24** / N/A |
| Trace South -> North Hemisphere | NativeAOT 9        |      N/A |     N/A |     N/A |          1.00 / N/A |   N/A |       N/A |     1.00 / N/A |

