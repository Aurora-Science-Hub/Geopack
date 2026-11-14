# Geopack-2008 Benchmarks

This project contains performance benchmarks for Geopack-2008 field line tracing procedures and magnetic field calculations, comparing implementations across different .NET runtimes.

## Prerequisites

- .NET SDK 11.0 or later
- BenchmarkDotNet 0.16.0-preview.2 or compatible version

## Launch benchmarks

First, `cd` the benchmarks project folder:
```bash
cd benchmarks/AuroraScienceHub.Geopack.Benchmarks/
```
and execute:
```bash
dotnet run -c Release --filter *GeopackBenchmarks*
```

## Results

Detailed benchmark results with performance metrics, memory allocation analysis, and runtime comparisons are available by versions:

- [v1.0.0 IntelFortran](Geopack/results/v1/v1.0.0_IntelFortran_Linux.md)
- [v1.0.2](Geopack/results/v1/v1.0.2_Linux.md)
- [v1.0.3 (.NET 8)](Geopack/results/v1/v1.0.3_Linux_NET8.md)
- [v1.0.3 (.NET 10)](Geopack/results/v1/v1.0.3_Linux_NET10.md)
- [v1.0.3 (macOS)](Geopack/results/v1/v1.0.3_MacOS.md)
- [v2.0.0 (.NET 8)](Geopack/results/v2/v2.0.0_Linux_NET8.md)
- [v2.0.0 (.NET 10)](Geopack/results/v2/v2.0.0_Linux_NET10.md)
- [v2.0.0 (macOS)](Geopack/results/v2/v2.0.0_MacOS.md)
