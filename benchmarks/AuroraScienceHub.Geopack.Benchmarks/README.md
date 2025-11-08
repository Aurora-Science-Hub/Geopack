# Geopack-2008 Benchmarks

This project contains performance benchmarks for Geopack-2008 field line tracing procedures and magnetic field calculations, comparing implementations across different .NET runtimes.

## Prerequisites

- .NET SDK 9.0 or later
- BenchmarkDotNet 0.15.4 or compatible version

## Launch benchmarks

First, `cd` the benchmarks project folder:
```bash
cd benchmarks/AuroraScienceHub.Geopack.Benchmarks/
```
and execute:
```bash
dotnet run -c Release -- --filter *GeopackBenchmarks*
```

## Results

Detailed benchmark results with performance metrics, memory allocation analysis, and runtime comparisons are available by versions:

- [IntelFortran](Geopack/results/FortranBenchmarks.md)
- [v.1.0.2](Geopack/results/v1/v1_0_2.md)
