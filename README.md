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

[![](https://img.shields.io/badge/.NET-9.0-512BD4)](https://dotnet.microsoft.com/)
[![](https://img.shields.io/badge/C%23-13.0-239120)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![License: GPL v3+](https://img.shields.io/badge/License-GPLv3+-blue.svg)](https://www.gnu.org/licenses/gpl-3.0)
[![DOI](https://zenodo.org/badge/782457774.svg)](https://doi.org/10.5281/zenodo.17437549)
[![Build & test](https://github.com/Aurora-Science-Hub/Geopack/actions/workflows/dotnet.yml/badge.svg)](https://github.com/Aurora-Science-Hub/Geopack/actions/workflows/dotnet.yml)

<a href="#validation">Validation</a> •
<a href="#benchmarks">Benchmarks</a> •
<a href="#tech-stack">Tech stack</a> •
<a href="#native-aot-compilation">Code Style</a> •
<a href="#licensing">Licensing</a>

</div>


This library provides numerical accuracy matching the original Fortran code by N. A. Tsyganenko to within 12 decimal digits (`8E-12D`).
For external magnetic field models, accuracy is raised to 13 digits (`1E-13D`).

## Validation
The implementation is rigorously validated against the original Fortran code using our comprehensive testing framework.
See [Unit Testing Framework](UnitTests/README.md) for details on test data generation and verification procedures.

## Benchmarks

Comprehensive performance benchmarks are available to measure the library's efficiency.
For detailed benchmark results, methodology, and running instructions,
see the [benchmarks documentation](benchmarks/AuroraScienceHub.Geopack.Benchmarks/README.md).

## Tech stack
- Supported .NET versions:
    - [.NET 9](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- [Native AOT compilation](https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot/)
- [Nullable reference types](https://learn.microsoft.com/en-us/dotnet/csharp/nullable-references)
- [Central package management](https://learn.microsoft.com/en-us/nuget/consume-packages/central-package-management)
- [SLNX - simpler solution file format in the .NET CLI](https://devblogs.microsoft.com/dotnet/introducing-slnx-support-dotnet-cli/)

## Native AOT compilation
To build the project with native AOT compilation, execute the following command (depending on the target platform):

```shell
dotnet publish --framework net9.0 -c Release -r linux-x64
```
```shell
dotnet publish --framework net9.0 -c Release -r win-x64
```
```shell
dotnet publish --framework net9.0 -c Release -r osx-x64
```

## Licensing
This C# implementation is a derivative work of the original FORTRAN code by Nikolai Tsyganenko, and is distributed under the same GNU GPL v3 license.

## How to cite

If you use this software in your research, please cite it as follows:

**Plain text citation:**

Nikolaev, A.V., Ermilov, A. O., Tsyganenko, N. A. (2024). Geopack-2008 C# .NET implementation (Version 1.0) [Computer software]. Zenodo. https://doi.org/10.5281/zenodo.17437549

## References

[1] Hapgood, M. A. (1992). Space physics coordinate transformations: A user guide. Planetary and Space Science, 40(5), 711–717. http://doi.org/10.1016/0032-0633(92)90012-D

[2] N. A. Tsyganenko, A new data-based model of the near magnetosphere magnetic field: 1. Mathematical structure. 2. Parameterization and fitting to observations (submitted to JGR, July 2001)

[3] N. A. Tsyganenko and M. I. Sitnov, Modeling the dynamics of the inner magnetosphere during strong geomagnetic storms, J. Geophys. Res., v. 110 (A3), A03208, doi: 10.1029/2004JA010798, 2005.
