
          ##########################################################################
          #                                                                        #
          #                          GEOPACK-2008_dp                               #
          #                     (MAIN SET OF FORTRAN CODES)                        #
          #               (double-precision version of 03/21/08)                   #
          #                (IGRF coefficients updated 01/01/20)                    #
          ##########################################################################
<details>
<summary>Original FORTRAN Program Text</summary>

This collection of subroutines is a result of several upgrades of the original package
written by N. A. Tsyganenko in 1978-1979.

PREFATORY NOTE TO THE VERSION OF FEBRUARY 4, 2008:

To avoid inappropriate use of obsolete subroutines from earlier versions, a suffix 08 was
added to the name of each subroutine in this release.

A possibility has been added in this version to calculate vector components in the
"Geocentric Solar Wind" (GSW) coordinate system, which, to our knowledge, was first
introduced by Hones et al., Planet. Space Sci., v.34, p.889, 1986 (aka GSWM, see Appendix,
Tsyganenko et al., JGRA, v.103(A4), p.6827, 1998). The GSW system is analogous to the
standard GSM, except that its X-axis is antiparallel to the currently observed solar wind
flow vector, rather than aligned with the Earth-Sun line. The orientation of axes in the
GSW system can be uniquely defined by specifying three components (VGSEX,VGSEY,VGSEZ) of
the solar wind velocity, and in the case of a strictly radial anti-sunward flow (VGSEY=
VGSEZ=0) the GSW system becomes identical to the standard GSM, which fact was used here
to minimize the number of subroutines in the package. To that end, instead of the special
case of the GSM coordinates, this version uses a more general GSW system, and three more
input parameters are added in the subroutine RECALC_08, the observed values (VGSEX,VGSEY,
VGSEZ) of the solar wind velocity. Invoking RECALC_08 with VGSEY=VGSEZ=0 restores the
standard (sunward) orientation of the X axis, which allows one to easily convert vectors
between GSW and GSM, as well as to/from other standard and commonly used systems. For more
details, see the documentation file GEOPACK-2008.DOC.

Another modification allows users to have more control over the procedure of field line
mapping using the subroutine TRACE_08. To that end, three new input parameters were added
in that subroutine, allowing one to set (i) an upper limit, DSMAX, on the automatically
adjusted step size, (ii) a permissible step error, ERR, and (iii) maximal length, LMAX,
of arrays where field line point coordinates are stored. Minor changes were also made in
the tracing subroutine, to make it more compact and easier for understanding, and to
prevent the algorithm from making uncontrollable large number of multiple loops in some
cases with plasmoid-like field structures.

One more subroutine, named GEODGEO_08, was added to the package, allowing one to convert
geodetic coordinates of a point in space (altitude above the Earth's WGS84 ellipsoid and
geodetic latitude) to geocentric radial distance and colatitude, and vice versa.

For a complete list of modifications made earlier in previous versions, see the
documentation file GEOPACK-2008.DOC.

</details>

## Tech stack
- Supported .NET versions:
  - [.NET 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
  - [.NET 7](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
  - [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
  - [.NET 9](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- [Native AOT compilation](https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot/)
- [Nullable reference types](https://learn.microsoft.com/en-us/dotnet/csharp/nullable-references)
- [Central package management](https://learn.microsoft.com/en-us/nuget/consume-packages/central-package-management)

# Startup project

## [Geopack.Cli](src/Geopack.Cli/)
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
