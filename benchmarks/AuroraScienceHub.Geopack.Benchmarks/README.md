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

| Method | Runtime     | Mean (μs) | Error   | StdDev  | Ratio / RefRatio | RatioSD | Gen0   | Allocated (KB) | Alloc Ratio |
|--------|-------------|-----------|---------|---------|------------------|---------|--------|----------------|-------------|
| IGRF   | IFortran`25 | 1.74      | 0.03    | 0.65    | 1.00 / 1.00      | N/A     | N/A    | N/A            | N/A         |
| IGRF   | .NET 9      | 0.486     | 0.0087  | 0.00726 | 1.00 / **0.28**  | 0.02    | 0.1602 | 0.328          | 1.00        |
| IGRF   | NativeAOT 9 | N/A       | N/A     | N/A     | N/A              | N/A     | N/A    | N/A            | N/A         |
|        |             |           |         |         |                  |         |        |                |             |
| Dipole | IFortran`25 | 1.34      | 0.02    | 0.51    | 0.77 / 1.00      | N/A     | N/A    | N/A            | N/A         |
| Dipole | .NET 9      | 0.104     | 0.00027 | 0.00023 | 0.21 / **0.06**  | 0.00    | 0.0229 | 0.046          | 0.14        |
| Dipole | NativeAOT 9 | N/A       | N/A     | N/A     | N/A              | N/A     | N/A    | N/A            | N/A         |
|        |             |           |         |         |                  |         |        |                |             |
| Sun    | IFortran`25 | 1.49      | 0.04    | 0.78    | 0.86 / 1.00      | N/A     | N/A    | N/A            | N/A         |
| Sun    | .NET 9      | 0.156     | 0.00052 | 0.00049 | 0.32 / **0.09**  | 0.00    | 0.0267 | 0.054          | 0.17        |
| Sun    | NativeAOT 9 | N/A       | N/A     | N/A     | N/A              | N/A     | N/A    | N/A            | N/A         |

#### Magnetic Field Line Tracing Performance

| Method                          | Runtime     | Mean (μs) | Error (μs) | StdDev (μs) |    Ratio / RefRatio |  Gen0 | Allocated (KB) |    Alloc Ratio |
|---------------------------------|-------------|----------:|-----------:|------------:|--------------------:|------:|---------------:|---------------:|
| Trace North -> South Hemisphere | IFortran`25 |     215.2 |       0.35 |        6.64 |         1.00 / 1.00 |   N/A |            N/A |    1.00 / 1.00 |
| Trace North -> South Hemisphere | .NET 9      |     307.1 |       1.09 |        0.96 |     1.00 / **1.43** | 108.4 |         222.21 |     1.00 / N/A |
| Trace North -> South Hemisphere | NativeAOT 9 |       N/A |        N/A |         N/A |          1.00 / N/A |   N/A |            N/A |     1.00 / N/A |
|                                 |             |           |            |             |                     |       |                |                |
| Trace South -> North Hemisphere | IFortran`25 |     289.3 |       0.38 |        7.20 |     **1.34** / 1.00 |   N/A |            N/A |    1.00 / 1.00 |
| Trace South -> North Hemisphere | .NET 9      |     383.6 |       2.17 |        2.03 | **1.25** / **1.33** | 134.8 |         275.49 | **1.24** / N/A |
| Trace South -> North Hemisphere | NativeAOT 9 |       N/A |        N/A |         N/A |          1.00 / N/A |   N/A |            N/A |     1.00 / N/A |

