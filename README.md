<h1 align="center">
    <br>
    <picture>
      <source media="(prefers-color-scheme: dark)" srcset="docs/logo/Gp_white.png">
      <source media="(prefers-color-scheme: light)" srcset="docs/logo/Gp_black.png">
      <img src="docs/logo/logo-black.png" style="width:400px;">
    </picture>
    <br>
    Geopack-2008 C# .NET  implementation
    <br>
</h1>

<div align="center">
    High-performance C# implementation of the Geopack-2008 geomagnetic field model with double-precision accuracy.
    <br><br>

[![NuGet Version](https://img.shields.io/nuget/v/AuroraScienceHub.Geopack?logo=nuget&label=NuGet)](https://www.nuget.org/packages/AuroraScienceHub.Geopack/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/AuroraScienceHub.Geopack?logo=nuget&label=Downloads)](https://www.nuget.org/packages/AuroraScienceHub.Geopack/)
[![](https://img.shields.io/badge/.NET-8.0%20%7C%2010.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![](https://img.shields.io/badge/C%23-13.0-239120?logo=csharp)](https://learn.microsoft.com/en-us/dotnet/csharp/)
<br>
[![Build & Test](https://github.com/Aurora-Science-Hub/Geopack/actions/workflows/dotnet.yml/badge.svg)](https://github.com/Aurora-Science-Hub/Geopack/actions/workflows/dotnet.yml)
[![License: GPL v3+](https://img.shields.io/badge/License-GPLv3+-blue.svg)](https://www.gnu.org/licenses/gpl-3.0)
[![DOI](https://zenodo.org/badge/782457774.svg)](https://doi.org/10.5281/zenodo.17437549)
[![GitHub Stars](https://img.shields.io/github/stars/Aurora-Science-Hub/Geopack?style=social)](https://github.com/Aurora-Science-Hub/Geopack)

<a href="#features">Features</a> •
<a href="#installation">Installation</a> •
<a href="#quick-start">Quick Start</a> •
<a href="#validation">Validation</a> •
<a href="#benchmarks">Benchmarks</a> •
<a href="#changelog">Changelog</a> •
<a href="#tech-stack">Tech Stack</a> •
<a href="#native-aot-compilation">Native AOT</a> •
<a href="#licensing">Licensing</a> •
<a href="#how-to-cite">How to Cite</a> •
<a href="#references">References</a>

</div>


This library provides numerical accuracy matching the original Fortran code by N. A. Tsyganenko to within 12 decimal digits (`8E-12D`).
For external magnetic field models, accuracy is raised to 13 digits (`1E-13D`).

## Features

- **High Precision**: Numerical accuracy matching original Fortran code to 12-13 decimal digits
- **Thread-Safe**: Immutable ComputationContext pattern eliminates shared mutable state
- **Type-Safe**: Strongly-typed generic vector quantities for Cartesian and spherical coordinates
- **Performance Optimized**: SIMD vectorization, Math.SinCos, and optimized mathematical operations
- **Modern .NET**: Native AOT compilation support, nullable reference types, C# 13 features
- **Dependency Injection**: Built-in DI support with ServiceCollectionExtensions
- **Comprehensive Testing**: 100+ unit tests validated against original Fortran implementation
- **Well Documented**: Clear API documentation and extensive benchmarks

## Installation

Install the package via NuGet Package Manager:

```shell
dotnet add package AuroraScienceHub.Geopack
```

Or via Package Manager Console:

```powershell
Install-Package AuroraScienceHub.Geopack
```

For external field models (T89, T96, etc.):

```shell
dotnet add package AuroraScienceHub.Geopack.ExternalFieldModels
```

## Quick Start

### Basic Usage

```csharp
using AuroraScienceHub.Geopack;
using AuroraScienceHub.Geopack.Contracts.Cartesian;
using AuroraScienceHub.Geopack.Contracts.Coordinates;
using AuroraScienceHub.Geopack.Contracts.PhysicalQuantities;

// Create Geopack instance
var geopack = new Geopack();

// Define date/time and solar wind velocity
var dateTime = new DateTime(1997, 12, 21, 21, 0, 0, DateTimeKind.Utc);
var swVelocity = CartesianVector<Velocity>.New(-304.0, 13.0, 4.0, CoordinateSystem.GSE);

// Calculate computation context
var context = geopack.Recalc(dateTime, swVelocity);

// Transform coordinates GEO -> GSW
var geoLocation = CartesianLocation.New(1.0, 2.0, 3.0, CoordinateSystem.GEO);
var gswLocation = geopack.GeoToGsw(context, geoLocation);

// Calculate IGRF magnetic field
var fieldVector = geopack.IgrfGeo(context, geoLocation);
Console.WriteLine($"Magnetic field: Bx={fieldVector.X}, By={fieldVector.Y}, Bz={fieldVector.Z}");
```

## Validation
The implementation is rigorously validated against the original Fortran code using our comprehensive testing framework with 100+ unit tests.
See [Unit Testing Framework](UnitTests/README.md) for details on test data generation and verification procedures.

## Benchmarks

Comprehensive performance benchmarks are available to measure the library's efficiency.
For detailed benchmark results, methodology, and running instructions,
see the [benchmarks documentation](benchmarks/AuroraScienceHub.Geopack.Benchmarks/README.md).

## Changelog

### Version 2.0.1 (Latest)

- Updated `AGENTS.md` with accurate, repo-specific guidance for AI coding agents

### Version 2.0.0

This is a major release with significant architectural improvements and breaking changes. Key updates include:

- **Thread-Safety**: Replaced mutable shared state with immutable ComputationContext pattern
- **Strongly-Typed API**: Generic vector quantities (CartesianVector<T>, SphericalVector<T>)
- **Performance**: SIMD vectorization, Math.SinCos, optimized mathematical operations
- **Data Models**: Converted to readonly record structs for better performance
- **Dependency Injection**: Full DI support with ServiceCollectionExtensions

⚠️ **Breaking Changes**: Method signatures have changed. See [CHANGELOG.md](CHANGELOG.md) for detailed migration guide.

## Tech Stack
- Supported .NET versions:
    - [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
    - [.NET 10](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)
- [Native AOT compilation](https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot/)
- [Nullable reference types](https://learn.microsoft.com/en-us/dotnet/csharp/nullable-references)
- [Central package management](https://learn.microsoft.com/en-us/nuget/consume-packages/central-package-management)
- [SLNX - simpler solution file format in the .NET CLI](https://devblogs.microsoft.com/dotnet/introducing-slnx-support-dotnet-cli/)

## Native AOT compilation
To build the project with native AOT compilation, execute the following command (depending on the target platform):

```shell
dotnet publish --framework net10.0 -c Release -r linux-x64
```
```shell
dotnet publish --framework net10.0 -c Release -r win-x64
```
```shell
dotnet publish --framework net10.0 -c Release -r osx-x64
```

## Licensing
This C# implementation is a derivative work of the original FORTRAN code by Nikolai Tsyganenko, and is distributed under the same GNU GPL v3 license.

## How to Cite

If you use this software in your research, please cite it using the following metadata:

**APA Style:**
Nikolaev, A., Ermilov, A., & Tsyganenko, N. (2026). Geopack-2008 C# .NET implementation (v1.0.3). Zenodo. https://doi.org/10.5281/zenodo.17441603

**BibTeX:**
```bibtex
@software{nikolaev_2026_17441603,
  author       = {Nikolaev, Alexander and
                  Ermilov, Aleksei and
                  Tsyganenko, Nikolai},
  title        = {Geopack-2008 C\# .NET implementation},
  month        = jan,
  year         = 2026,
  publisher    = {Zenodo},
  version      = {v1.0.3},
  doi          = {10.5281/zenodo.17441603},
  url          = {https://doi.org/10.5281/zenodo.17441603},
  swhid        = {swh:1:dir:6803d108518ce91041b35fc1ad3ac77fb24ae680
                   ;origin=https://doi.org/10.5281/zenodo.17437549;vi
                   sit=swh:1:snp:7e657a3370ceade1723538a54de709ceae47
                   cf88;anchor=swh:1:rel:60165de1aba64c409a029ed9c082
                   6d500f400274;path=Aurora-Science-Hub-
                   Geopack-96b844e
                  },
}
```

## References

The homepages of the original Geopack library and the associated scientific literature are available at the following links: [old page](https://geo.phys.spbu.ru/~tsyganenko/modeling.html) and [new page](https://geo.phys.spbu.ru/~tsyganenko/empirical-models/).

This implementation is based on and extends the following scientific works:

1. **Tsyganenko, N. A.** (2002). *A model of the near magnetosphere with a dawn-dusk asymmetry 1. Mathematical structure*. J. Geophys. Res., 107 (A8), https://doi.org/10.1029/2001JA000219

2. **Tsyganenko, N. A., & Sitnov, M. I.** (2005). *Modeling the dynamics of the inner magnetosphere during strong geomagnetic storms*. Journal of Geophysical Research: Space Physics, 110(A3), A03208. https://doi.org/10.1029/2004JA010798

3. **Hapgood, M. A.** (1992). *Space physics coordinate transformations: A user guide*. Planetary and Space Science, 40(5), 711–717. https://doi.org/10.1016/0032-0633(92)90012-D

4. **Tsyganenko, N. A., Andreeva, V. A.** (2016), *An empirical RBF model of the magnetosphere parameterized by interplanetary and ground-based drivers*, J. Geophys. Res. Space Physics, v.121(11), 10,786-10,802, https://doi.org/10.1002/2016JA023217
