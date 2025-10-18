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

| Method                      | Mean     | Error   | StdDev  | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|---------------------------- |---------:|--------:|--------:|------:|--------:|-------:|----------:|------------:|
| Calculate_IgrfMagneticField | 486.2 ns | 8.70 ns | 7.26 ns |  1.00 |    0.02 | 0.1602 |     336 B |        1.00 |
| Calculate_DipMagneticField  | 104.4 ns | 0.27 ns | 0.23 ns |  0.21 |    0.00 | 0.0229 |      48 B |        0.14 |
| Calculate_Sun               | 156.1 ns | 0.52 ns | 0.49 ns |  0.32 |    0.00 | 0.0267 |      56 B |        0.17 |

#### Magnetic Field Line Tracing Performance

| Method                                    | Mean     | Error   | StdDev  | Ratio | Gen0    | Allocated | Alloc Ratio |
|------------------------------------------ |---------:|--------:|--------:|------:|--------:|----------:|------------:|
| Trace_FieldLineFromNorthToSouthHemisphere | 267.9 μs | 1.76 μs | 1.64 μs |  1.00 | 97.6563 | 199.67 KB |        1.00 |
| Trace_FieldLineFromSouthToNorthHemisphere | 202.2 μs | 1.36 μs | 1.21 μs |  0.75 | 66.6504 | 136.59 KB |        0.68 |

