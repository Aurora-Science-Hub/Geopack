# Geopack-2008 Unit Testing Framework

## Table of Contents
1. [Overview](#Overview)
2. [Prerequisites](#Prerequisites)
3. [Project Structure](#Project-Structure)
4. [Test Data Generators](#Test-Data-Generators)
   *  [BCARSPH_08](#BACARSPH_08)
   *  [Coordinate transformations](#Coordinate-transformations)
   *  [TRACE_08](#TRACE_08)
5. [Usage Examples](#usage-examples)
6. [Synchronization Guide](#synchronization-guide)
7. [Troubleshooting](#troubleshooting)

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
Generate your own test data using our set of example codes. Download original Geopack-2008dp code [here](https://geo.phys.spbu.ru/~tsyganenko/models/Geopack-2008_dp.for).

### BACARSPH_08
Use file `UnitTests/Geopack/FortranSource/BCARSPH_08.for`.
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

### Coordinate transformations
Use `UnitTests/Geopack/FortranSource/CoordinateTransformations.for` to test next procedures:
- `GeiGeo` / `GeoGei`
- `GeoGsw` / `GswGeo`
- `GeoMag` / `MagGeo`
- `GswGse`/ `GseGsw`
- `MagSm` / `SmMag`
- `SmGsw` / `GswSm`

As an example below we use `GSWGSW_08` procedure.

Setup a set of location coordinates:
```fortran
DATA X/6.5999999999999996D0,-6.5999999999999996D0,
     *1.D0,-1.D0,4.5678D0,-4.5678D0,0.D0/

DATA Y/6.5999999999999996D0,-6.5999999999999996D0,
*1.D0,-1.D0,4.5678D0,-4.5678D0,0.D0/

DATA Z/6.5999999999999996D0,-6.5999999999999996D0,
 *1.D0,-1.D0,4.5678D0,-4.5678D0,0.D0/
```

Setup date/time and solar wind direction:
```fortran
      IYEAR=1997
      IDAY=350
      IHOUR=21
      MIN=0
      ISEC=0

      VGSEX=-304.D0
      VGSEY= 13.D0
      VGSEZ= 4.D0
```

Setup transformation direction:
```fortran
J=-1
```

Specify output test data file name. Corresponding test data filenames you can find in `UnitTests/Geopack/TestData/`:
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
```bash
ifx Geopack_2008dp.for CoordinateTransformations.for -o gen_data && ./gen_data && rm gen_data && mv GswGeo.dat ../TestData/
```

Ensure the input parameters in these test generators remain synchronized with the actual unit tests.
Do not forget that test data file name should be synchronized with corresponding variable in test fixture, e.g.:
```text
private const string GswGseDatasetFileName =
        "AuroraScienceHub.Geopack.UnitTests.Geopack.TestData.GswGse.dat";
```

### TRACE_08
Copy and paste the whole `SUBROUTINE T89D_DP` from [here](https://geo.phys.spbu.ru/~tsyganenko/models/t89/T89d_dp.for) below the example code `TRACE_08.for`.
