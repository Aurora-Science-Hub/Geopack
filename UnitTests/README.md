# Geopack-2008 Unit Testing Framework

## Table of Contents
1. [Overview](#overview)
2. [Prerequisites](#prerequisites)
3. [Project Structure](#project-structure)
4. [Test Data Generators](#test-data-generators)
   *  [BCARSPH_08](#)
5. [Usage Examples](#usage-examples)
6. [Synchronization Guide](#synchronization-guide)
7. [Troubleshooting](#troubleshooting)

## Overview

This repository contains multiple Fortran test data generators for unit testing the Geopack-2008dp (double-precision) geomagnetic field library.
Each generator produces reference data for specific Geopack routines.

## Prerequisites

We recommend to use Intel Fortran Compiler to generate test reference data using [original double-precision Geopack-2008 version by N. A. Tsyganenko](https://geo.phys.spbu.ru/~tsyganenko/models/Geopack-2008_dp.for).
See [Intel Website](http://intel.com) to get the compiler or follow steps below to install compiler on Linux (Ubuntu).

#### Add Intel oneAPI repository

```bash
sudo apt install -y gpg-agent wget
wget -O- https://apt.repos.intel.com/intel-gpg-keys/GPG-PUB-KEY-INTEL-SW-PRODUCTS.PUB | gpg --dearmor | sudo tee /usr/share/keyrings/oneapi-archive-keyring.gpg > /dev/null
echo "deb [signed-by=/usr/share/keyrings/oneapi-archive-keyring.gpg] https://apt.repos.intel.com/oneapi all main" | sudo tee /etc/apt/sources.list.d/oneAPI.list
```

#### Install compiler and configure environment
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
