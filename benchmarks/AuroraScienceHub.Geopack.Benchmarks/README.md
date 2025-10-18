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

| Method                      | Mean       | Error    | StdDev   | Ratio | Gen0   | Allocated | Alloc Ratio |
|---------------------------- |-----------:|---------:|---------:|------:|-------:|----------:|------------:|
| Calculate_IgrfMagneticField | 1,471.6 ns | 17.51 ns | 14.62 ns |  1.00 | 0.1869 |     392 B |        1.00 |
| Calculate_DipMagneticField  | 1,062.1 ns |  8.45 ns |  7.90 ns |  0.72 | 0.0496 |     104 B |        0.27 |
| Calculate_Sun               |   158.1 ns |  1.03 ns |  0.92 ns |  0.11 | 0.0267 |      56 B |        0.14 |

#### Magnetic Field Line Tracing Performance

| Method                                    | Mean     | Error   | StdDev  | Ratio | Gen0    | Allocated | Alloc Ratio |
|------------------------------------------ |---------:|--------:|--------:|------:|--------:|----------:|------------:|
| Trace_FieldLineFromNorthToSouthHemisphere | 269.1 μs | 0.55 μs | 0.49 μs |  1.00 | 97.6563 | 199.73 KB |        1.00 |

