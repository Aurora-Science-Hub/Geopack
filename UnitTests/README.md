# Geopack-2008 Unit Testing Framework

## Table of Contents
1. [Overview](#Overview)
2. [Prerequisites](#Prerequisites)
3. [Project Structure](#Project-Structure)
4. [Test Data Generators](#Test-Data-Generators)
   * [BCARSPH_08](#BCARSPH_08)
   * [Coordinate transformations](#Coordinate-transformations)
   * [DIP_08](#DIP_08)
   * [GEODGEO_08](#GEODGEO_08)
   * [IGRF_08](#IGRF_08)
   * [SHUETAL_MGNP_08](#SHUETAL_MGNP_08)
   * [SPHCAR_08](#SPHCAR_08)
   * [SUN_08](#SUN_08)
   * [T96_MGNP_08](#T96_MGNP_08)
   * [TRACE_08](#TRACE_08)


## Overview

This repository provides a comprehensive validation framework for the C# .NET implementation of Geopack-2008 (double-precision).
The Fortran test data generators enable verification that our implementation maintains numerical accuracy matching
the original Fortran code by N. A. Tsyganenko to within 13 decimal places. The framework includes detailed test procedures and documentation
to facilitate generation of custom test data with user-specific configurations while ensuring precision consistency across implementations.

## Prerequisites

We recommend to use Intel Fortran Compiler to generate test reference data using original double-precision Geopack-2008 version by N. A. Tsyganenko.
See [Intel Website](http://intel.com) to get the compiler or follow steps below to install compiler on Linux (Ubuntu).

### Add Intel oneAPI repository

```bash
sudo apt install -y gpg-agent wget
wget -O- https://apt.repos.intel.com/intel-gpg-keys/GPG-PUB-KEY-INTEL-SW-PRODUCTS.PUB | gpg --dearmor | sudo tee /usr/share/keyrings/oneapi-archive-keyring.gpg > /dev/null
echo "deb [signed-by=/usr/share/keyrings/oneapi-archive-keyring.gpg] https://apt.repos.intel.com/oneapi all main" | sudo tee /etc/apt/sources.list.d/oneAPI.list
```

### Install compiler and configure environment
```bash
sudo apt update
sudo apt install intel-oneapi-compiler-fortran
echo 'source /opt/intel/oneapi/setvars.sh' >> ~/.bashrc
source ~/.bashrc
```

## Project structure
```
Geopack-2008/
└── UnitTests/
    ├── ExternalFieldModels/
    |   └── FortranSource/
    |       └── T89DP.for                           # T89 external magnetic field calculation
    └── Geopack/
        ├── FortranSource/
        │   ├── BCARSPH_08.for                      # Cartesian to spherical B-vector conversion
        │   ├── CoordinatesTransformations.for      # Bidirectional coordinate transformations
        │   ├── DIP_08.for                          # Dipole geomagnetic field
        │   ├── IGRF_08.for                         # IGRF geomagnetic field (GEO and GSW)
        │   ├── SHUETAL_MGNP_08.for                 # Shu et al. magnetopause model
        │   ├── SPHCAR_08.for                       # Spherical to Cartesian coordinates conversion
        │   ├── SUN_08.for                          # Sun position calculation
        │   ├── T96_MGNP_08.for                     # T96 magnetopause model
        │   └── TRACE_08.for                        # Magnetic field line tracing
        └── TestData/                               # Generated reference data
```
## Test data generators
Generate your own test data using our set of example codes. Download original Geopack-2008dp code [here](https://geo.phys.spbu.ru/~tsyganenko/models/Geopack-2008_dp.for)
and put it to the **UnitTests** folder: `UnitTests/Geopack/FortranSource/Geopack_2008dp.for`.

### BCARSPH_08
<details>
<summary>Use `UnitTests/Geopack/FortranSource/BCARSPH_08.for`</summary>

Specify vector coordinates and cartesian magnetic field components:
```fortran
x=0.D0
Y=0.D0
Z=0.D0

BX=1.D0
BY=1.D0
BZ=1.D0
```
Execute in terminal:
```bash
ifx Geopack_2008dp.for BCARSPH_08.for -o bcarsph && ./bcarsph && rm bcarsph
```

Copy/Paste input and output from terminal to the `GeopackTests.BCarSph_08` test as new `InlineData`, e.g.:
```
[InlineData(1, 1, 1, 1, 0, 0, 0.577350269189625842, 0.408248290463863017, -0.707106781186547462)]
```
</details>

### Coordinate transformations
<details>
<summary>Use `UnitTests/Geopack/FortranSource/CoordinateTransformations.for`:</summary>

Apply for:
- `GeiGeo_08` / `GeoGei_08`
- `GeoGsw_08` / `GswGeo_08`
- `GeoMag_08` / `MagGeo_08`
- `GswGse_08`/ `GseGsw_08`
- `MagSm_08` / `SmMag_08`
- `SmGsw_08` / `GswSm_08`

As an example below we test `GEOGSW_08` original procedure.

Set up a set of location coordinates:
```fortran
DATA X/6.5999999999999996D0,-6.5999999999999996D0,
     *1.D0,-1.D0,4.5678D0,-4.5678D0,0.D0/

DATA Y/6.5999999999999996D0,-6.5999999999999996D0,
*1.D0,-1.D0,4.5678D0,-4.5678D0,0.D0/

DATA Z/6.5999999999999996D0,-6.5999999999999996D0,
 *1.D0,-1.D0,4.5678D0,-4.5678D0,0.D0/
```

Setup transformation direction:
* GEO -> GSW:
```fortran
J=1
```
* GSW -> GEO:
```fortran
J=-1
```


Specify output test data file name. Corresponding test data filenames you can find in `UnitTests/Geopack/TestData/`:
* GEO -> GSW:
```fortran
OPEN(UNIT=1,FILE='GeoGsw.dat')
```
* GSW -> GEO:
```fortran
OPEN(UNIT=1,FILE='GswGeo.dat')
```

Specify testing procedure in the cycle. Ensure, that procedure name corresponds to the original Geopack-2008:
```fortran
CALL GEOGSW_08 (X(N),Y(M),Z(K),XR,YR,ZR,J)
...
CALL GEOGSW_08 (XR,YR,ZR,X(N),Y(M),Z(K),J)
```

Compile and execute:
* GEO -> GSW:
```bash
ifx Geopack_2008dp.for CoordinateTransformations.for -o gen_data && ./gen_data && rm gen_data && mv GeoGsw.dat ../TestData/
```
* GSW -> GEO:
```bash
ifx Geopack_2008dp.for CoordinateTransformations.for -o gen_data && ./gen_data && rm gen_data && mv GswGeo.dat ../TestData/
```

Ensure the input parameters in these test generators remain synchronized with the actual unit tests.
Do not forget that test data file name should be synchronized with corresponding variable in test fixture, e.g.:
* GEO -> GSW:
```text
private const string GeoGswDatasetFileName =
        "AuroraScienceHub.Geopack.UnitTests.Geopack.TestData.GeoGsw.dat";
```
* GSW -> GEO:
```text
private const string GswGeoDatasetFileName =
        "AuroraScienceHub.Geopack.UnitTests.Geopack.TestData.GswGeo.dat";
```

* GEO -> GSW:
Execute `GeopackTests.GeoGsw_08` unit tests.
* GSW -> GEO:
Execute `GeopackTests.GswGeo_08` unit tests.

</details>

### DIP_08
<details>
<summary>Use `UnitTests/Geopack/FortranSource/DIP_08.for`:</summary>

Specify location:
```fortran
XGSW=0.D0
YGSW=0.D0
ZGSW=0.D0
```

Execute in terminal:
```bash
ifx Geopack_2008dp.for DIP_08.for -o dip && ./dip && rm dip
```

Copy and paste input and output from terminal to the `GeopackTests.Dip_08` test as new `InlineData`, e.g.:
```
[InlineData(1.0D,1.0D, 1.0D, -5468.999024571849076892, -3525.612769882045540726, 1943.386254689803536166)]
```

</details>

### GEODGEO_08
<details>
<summary>Use `UnitTests/Geopack/FortranSource/GEODGEO_08.for`:</summary>

Setup test parameters:
* For `GEOD -> GEO` transformation:
```fortran
DATA H/0.D0,100.D0,400.D0,1000.D0,35786.D0/
DATA XMU/0.D0,0.5236D0,1.0472D0,1.5708D0,0.7854D0/
```
* Vice versa `GEO -> GEOD` transformation:
```fortran
DATA R/6378.137D0,6478.137D0,6767.810D0,7375.337557D0,42164.137D0/
DATA THETA/1.5708D0,1.3090D0,0.9273D0,0.D0,1.3090D0/
```

Specify transformation direction:
* GEOD -> GEO
```frotran
J=1
```
* GEO -> GEOD
```frotran
J=-1
```

Specify test data filename:
* GEOD -> GEO
```fortran
OPEN(UNIT=1,FILE='GeodGeo.dat')
```
* GEO -> GEOD
```fortran
OPEN(UNIT=1,FILE='GeoGeod.dat')
```

Execute in terminal:
* GEOD -> GEO
```bash
ifx Geopack_2008dp.for GEODGEO.for -o geodgeo && ./geodgeo && rm geodgeo && mv GeodGeo.dat ../TestData/
```
* GEO -> GEOD
```bash
ifx Geopack_2008dp.for GEODGEO.for -o geodgeo && ./geodgeo && rm geodgeo && mv GeoGeod.dat ../TestData/
```

Execute `GeopackTests.GeodGeo_08` unit tests.

</details>

### IGRF_08
<details>
<summary>Use `UnitTests/Geopack/FortranSource/IGRF_08.for`:</summary>

Setup location:
* `IGRF_GSW_08`:
```fortran
XGSW=0.D0
YGSW=0.0D0
ZGSW=-6.6D0
```
* `IGRF_GEO_08`:
```fortran
XLAT=89.9D0
XLON=0.D0
R=1.02D0
COLAT=(90.-XLAT) / RAD
PHI=XLON / RAD
```

Uncomment desired procedure:
* `IGRF_GSW_08`:
```frotran
CALL IGRF_GSW_08 (XGSW,YGSW,ZGSW,HXGSW,HYGSW,HZGSW)
```
* `IGRF_GSW_08`:
```frotran
CALL IGRF_GEO_08 (R,COLAT,PHI,BR,BTHETA,BPHI)
```

Uncomment corresponding output:
* `IGRF_GSW_08`:
```frotran
write(*, 10) HXGSW,HYGSW,HZGSW
```
* `IGRF_GSW_08`:
```frotran
write(*, 10) BR, BTHETA, BPHI
```

Execute in terminal:
```bash
ifx Geopack_2008dp.for IGRF_08.for -o igrf && ./igrf && rm igrf
```

Copy and paste output from terminal to the corresponding `InlineData` block and launch the test:
* IGRF_GSW_08 : `GeopackTests.IgrfGsw_08`
* IGRF_GEO_08 :` GeopackTests.IgrfGeo_08`

</details>

### SHUETAL_MGNP_08
<details>
<summary>Use `UnitTests/Geopack/FortranSource/SHUETAL_MGNP_08.for`:</summary>

Set up solar wind:
```fortran
XN_PD=99990.D0
VEL=999990.D0
BZIMF=999.D0
```

Set up location:
```fortran
XGSW=9.D0
YGSW=0.D0
ZGSW=0.D0
```

Execute in terminal:
```bash
ifx Geopack_2008dp.for SHUETAL_MGNP_08.for -o shu && ./shu && rm shu
```

Copy and paste input and output from terminal to the `GeopackTests.ShuMgnp_08` test as new `InlineData` e.g.:
```
[InlineData(5.0D, -350.0D, 5.0D, 9.0D, 0.0D, 0.0D, 9.003326462780140815, 0.000000000000000000, 0.000000000000000000, 0.003326462780140815, MagnetopausePosition.Inside)]
```
Note: `MagnetopausePosition` depends on fortran `ID` output.

</details>

### SPHCAR_08
<details>
<summary>Use `UnitTests/Geopack/FortranSource/SPHCAR_08.for`:</summary>

Uncomment and modify test location:
* SPH -> CAR:
```fortran
R=-1.D0
THETA=-1.D0
PHI=-1.D0
```
* CAR -> SPH:
```fortran
X=0.D0
Y=-1.D0
Z=0.D0
```

Set up conversion option:
* SPH -> CAR:
```fortran
DIR=1
```
* CAR -> SPH:
```fortran
DIR=-1
```

Uncomment corresponding output:
* SPH -> CAR:
```frotran
write(*, 10) X, Y, Z
C write(*, 10) R, THETA, PHI
```
* CAR -> SPH:
```frotran
C write(*, 10) X, Y, Z
write(*, 10) R, THETA, PHI
```

Execute in terminal:
```bash
ifx Geopack_2008dp.for SPHCAR_08.for -o sphcar && ./sphcar && rm sphcar
```

Copy and paste output from terminal to the corresponding `InlineData` block and launch the test:
* SPH -> CAR : `GeopackTests.SphCar_08`
* CAR -> SPH : `GeopackTests.CarSph_08`

</details>

### SUN_08
<details>
<summary>Use `UnitTests/Geopack/FortranSource/SUN_08.for`:</summary>

Specify date and time:
```fortran
IY=2004
IDAY=60
IHOUR=0
MIN=0
ISEC=0
```

Execute in terminal:
```bash
ifx Geopack_2008dp.for SUN_08.for -o sun && ./sun && rm sun
```

Copy and paste input and output from terminal to the `GeopackTests.Sun_08` test as new `InlineData`, e.g.:
```
[InlineData(2004, 2, 29, 0, 0, 0, 2.760256269651100602, 5.929696758033518478, 5.956663000518048534, -0.138172813779450315)]
```

</details>

### T96_MGNP_08
<details>
<summary>Use `UnitTests/Geopack/FortranSource/T96_MGNP_08.for`:</summary>

Set up solar wind:
```fortran
XN_PD=99990.D0
VEL=999990.D0
```

Set up location:
```fortran
XGSW=9.D0
YGSW=0.D0
ZGSW=0.D0
```

Execute in terminal:
```bash
ifx Geopack_2008dp.for T96_MGNP_08.for -o t96 && ./t96 && rm t96
```

Copy and paste input and output from terminal to the `GeopackTests.T96Mgnp_08` test as new `InlineData` e.g.:
```
[InlineData(5.0D, 350.0D, 9.0D, 0.0D, 0.0D, 11.917821173671217849D, 0.000000000000000000D, 0.000000000000000000D, 2.917821173671217849D, MagnetopausePosition.Inside)]
```
Note: `MagnetopausePosition` depends on fortran `ID` output.
</details>

### TRACE_08
<details>
<summary>Use `UnitTests/Geopack/FortranSource/TRACE_08.for`:</summary>

Copy and paste the whole `SUBROUTINE T89D_DP` from [here](https://geo.phys.spbu.ru/~tsyganenko/models/t89/T89d_dp.for) to the end of example code `TRACE_08.for`.

* Set up direction for North to South conjugate point:
```fortran
DIR=1.D0
```
* Set up direction for South to North conjugate point:
```fortran
DIR=-1.D0
```

Set up the rest parameters:
```fortran
DSMAX=0.1D0
ERR=0.0001D0
RLIM=60.D0
R0=1.D0
IOPT=1
XGSW=-1.02D0
YGSW=0.8D0
ZGSW=-0.9D0
```

Specify output filename:
* North to South conjugate point:
```fortran
OPEN(UNIT=1,FILE='TraceNSResult.dat')
```
* South to North conjugate point:
```fortran
OPEN(UNIT=1,FILE='TraceSNResult.dat')
```

Execute in terminal:
* North to South conjugate point:
```bash
ifx Geopack_2008dp.for TRACE_08.for -o trace && ./trace && rm trace && mv TraceNSResult.dat ../TestData/
```
* South to North conjugate point:
```bash
ifx Geopack_2008dp.for TRACE_08.for -o trace && ./trace && rm trace && mv TraceSNResult.dat ../TestData/
```

Execute `GeopackTests.Trace_08` unit test.

</details>
