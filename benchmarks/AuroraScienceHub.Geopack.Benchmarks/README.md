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

#### Single Calculation Performance

| Method                      | Mean       | Error    | StdDev   | Ratio | Gen0   | Allocated | Alloc Ratio |
|---------------------------- |-----------:|---------:|---------:|------:|-------:|----------:|------------:|
| Calculate_IgrfMagneticField | 1,465.9 ns | 17.26 ns | 14.41 ns |  1.00 | 0.1869 |     392 B |        1.00 |
| Calculate_DipMagneticField  | 1,067.3 ns |  9.44 ns |  8.37 ns |  0.73 | 0.0496 |     104 B |        0.27 |
| Calculate_Sun               |   162.6 ns |  1.99 ns |  1.76 ns |  0.11 | 0.0267 |      56 B |        0.14 |

#### Spacecraft Tracing Performance

| Method                           | Mean     | Error     | StdDev    | Ratio | Gen0      | Allocated | Alloc Ratio |
|--------------------------------- |---------:|----------:|----------:|------:|----------:|----------:|------------:|
| Trace_SpacecraftFromNorthToSouth | 4.451 ms | 0.0274 ms | 0.0229 ms |  0.87 | 1648.4375 |    3.3 MB |        0.82 |
| Trace_SpacecraftFromSouthToNorth | 5.126 ms | 0.0325 ms | 0.0271 ms |  1.00 | 2023.4375 |   4.04 MB |        1.00 |
