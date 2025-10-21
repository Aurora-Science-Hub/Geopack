# Geopack-2008 (C# .NET implementation)

High-performance C# implementation of the Geopack-2008 geomagnetic field model with double-precision accuracy.
This library provides numerical accuracy matching the original Fortran code by N. A. Tsyganenko to within 12 decimal digits (`8E-12D`).
For external magnetic field models accuracy raised up to 13 digits (`1E-13D`)

## Validation
The implementation is rigorously validated against the original Fortran code using our comprehensive testing framework.
See [Unit Testing Framework](UnitTests/README.md) for details on test data generation and verification procedures.

## Benchmarks

Comprehensive performance benchmarks are available to measure the library's efficiency.
For detailed benchmark results, methodology, and running instructions,
see the [benchmarks documentation](benchmarks/AuroraScienceHub.Geopack.Benchmarks/README.md).

## Tech stack
- Supported .NET versions:
    - [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
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
