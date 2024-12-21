    ##########################################################################
    #                                                                        #
    #                             GEOPACK-2008                               #
    #                     (MAIN SET OF FORTRAN CODES)                        #
    #                 (IGRF coefficients updated 01/01/2020)                 #
    ##########################################################################

<details>
<summary>Details...</summary>

The source code is here: http://geo.phys.spbu.ru/~tsyganenko/Geopack-2008.html
This collection of subroutines is a result of several upgrades of the original package
written by N. A. Tsyganenko in 1978-1979.

PREFATORY NOTE TO THE VERSION OF FEBRUARY 8, 2008:

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

    ##########################################################################
    #                          T96 empiric model                             #
    #                     (MAIN SET OF FORTRAN CODES)                        #
    ##########################################################################

<details>
<summary>Details...</summary>

The FORTRAN source code T96_01.FOR is the last version (first release June 22,
1996, amended in July 2010 for compatibility with Intel Fortran compilers) of
a data-based model of the geomagnetospheric magnetic field with an explicitly
defined realistic magnetopause, large-scale Region 1 and 2 Birkeland current
systems, and the IMF penetration  across the boundary.

The file T96_01.FOR contains a set of 33 subroutines and functions. The first
subroutine, named T96_01, is the primary one, accepting the input values of the
solar wind pressure, Dst-index, By- and Bz-components of the interplanetary
magnetic field, the geodipole tilt angle, and GSM position of the observation
point (X,Y,Z). The subroutine returns GSM components of the external field
(i.e., the total vector of B minus the Earth's contribution).  The remaining
32 subroutines are invoked by T96_01.



The source code T96_01.FOR differs in several aspects from the previous version
T95_06.FOR (released in November 1995).

(1) The T96_01 does not include the AE-index as an input parameter.

(2) The number of terms in the ring current, tail modes, and in the shielding
fields was reduced to a necessary minimum, in order to increase the speed of the
code. The tail field is now a sum of two modes; the first one is responsible
for the near-Earth tail field and is similar to that in the previous version.
The second mode provides the asymptotic field in the far magnetotail.

(3) The way of representing the effects of the dipole tilt on the tail/ring
current field was revised:  instead of a "shear" transformation (introduced in
the T89 model), a radially dependent "space-warping" is used in this model,
which decreases tilt-induced spurious currents.

(4) The representation for the Region 2 Birkeland current field was completely
revised:  in the present version, a smooth approximation was developed for
the field inside the current layer. As a result, unphysical kinks in the Bz
profile on the nightside were eliminated.

    *******************************************
    | Users should be aware of the following. |
    *******************************************

(1) A simple linear dependence of the amplitudes of the field sources on the
SQRT(Pdyn), Dst, and the IMF-related parameter EPS=SQRT(N)*V*Bt*sin(theta/2)
was employed.  Hence, the best results should be expected near the most probable
values of the input parameters,  corresponding to the regions in the Pdyn-Dst-
ByIMF-BzIMF space with the highest density of the spacecraft measurements. For
the same reason, caution is needed in modeling situations with unusually low or
high values of these parameters: extrapolating the model too far beyond the
range of reliable approximation can lead to unrealistic results.  As a rough
estimate, the parameter values should remain within the intervals:
Pdyn:  between 0.5 and 10 nPa,
Dst:  between -100 and +20,
ByIMF and BzIMF: between -10 and +10 nT.

(2) The only parameter which controls the size of the model magnetopause is
the solar wind ram pressure Pdyn. No IMF dependence has been introduced so far
in the magnetopause shape/size.  This is planned to be done in future versions
of the model.
To facilitate using the model, we provide users with two supplementary
FORTRAN subroutines, named LOCATE and CROSSING.  The first one finds the point
on the model magnetopause which is closest to a given point of space, for any
values of the solar wind density and velocity (or, optionally, solar wind
pressure).  The second subroutine estimates the current value of the solar wind
ram pressure, based on the observed GSM position of the magnetopause at any
local time or latitude, sunward from XGSM=-60Re.

(3) In its present form, the subroutine T96_01 is compatible with new version
(April 16, 1996) of the software package GEOPACK for coordinate transformation
and line tracing, which replaces the old version and is available from the same
WWW site.

(4) This is not a "final version":  the model is supposed to be further
improved and upgraded in the future. In this regard, any kind of feedback from
the users is very important for us and will be greatly appreciated. In
particular, it is very important that any problems encountered in adapting and
using the code be reported to us as soon as possible.  Please send your
questions and comments to the address given in the end of this file.

(5) More details on the approach used in devising this model can be found in
the following publications:


      Tsyganenko, N.A. and M. Peredo, Analytical models of the magnetic field
        of disk-shaped current sheets,  J.Geophys.Res., v.99, pp.199-205, 1994.

      Tsyganenko, N.A., Modeling the Earth's magnetospheric magnetic field
        confined within a realistic magnetopause, J.Geophys.Res., v.100,
        pp.5599-5612, 1995.

      Fairfield, D.H., N.A. Tsyganenko, A.V. Usmanov, and M.V. Malkov, A large
        magnetosphere magnetic field database, J.Geophys.Res., v.99,
        pp.11319-11326, 1994.

      Tsyganenko, N.A. and D.P. Stern, Modeling the global magnetic field
        the large-scale Birkeland current systems, J.Geophys.Res., v.101,
        p.27187-27198, 1996.

      Tsyganenko, N.A., Effects of the solar wind conditions on the global
         magnetospheric configuration as deduced from data-based field
         models, in:  Proc.of 3rd International Conference on Substorms
         (ICS-3), Versailles, France, 12-17 May 1996, ESA SP-389, p.181-185,
         1996.

       (A PostScript file of the last paper, named versail.ps, can be ftp-ed
       from anonymous ftp-area at:   www-spof.gsfc.nasa.gov;   /pub/kolya)


Please send your questions, comments, and requests to:

Nikolai Tsyganenko

email:   nikolai.tsyganenko@gmail.com

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
