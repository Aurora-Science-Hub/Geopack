# Geopack-2008 (double-precision) C# .NET implementation
## Tech stack
- Supported .NET versions:
  - [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
  - [.NET 9](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- [Native AOT compilation](https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot/)
- [Nullable reference types](https://learn.microsoft.com/en-us/dotnet/csharp/nullable-references)
- [Central package management](https://learn.microsoft.com/en-us/nuget/consume-packages/central-package-management)

## Startup project

## Geopack.Cli
A command-line interface for running Geopack calculations.

### Command line arguments
- `--help` - Displays help information.
- `calculate` - Performs Geopack calculations.

# Building and publishing
_In order to build the project, you need to have the .NET SDK installed._

## Native AOT compilation
To build the project with native AOT compilation, execute the following command (depending on the target platform):

```shell
dotnet publish -c Release -r linux-x64
```
```shell
dotnet publish -c Release -r win-x64
```
```shell
dotnet publish -c Release -r osx-x64
```

# Benchmarks
The solution contains benchmarks for the Geopack calculations located in the [Geopack.Benchmarks](benchmarks/AuroraScienceHub.Geopack.Benchmarks/) project.

Just run the benchmarks project in Release configuration to see the results for different .NET versions.
