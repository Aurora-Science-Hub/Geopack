# Geopack-2008dp (double-precision) how to

## Install Intel Fortran compiler (Linux, Debian-based)
```shell
sudo apt install -y gpg-agent wget
wget -O- https://apt.repos.intel.com/intel-gpg-keys/GPG-PUB-KEY-INTEL-SW-PRODUCTS.PUB | gpg --dearmor | sudo tee /usr/share/keyrings/oneapi-archive-keyring.gpg > /dev/null
echo "deb [signed-by=/usr/share/keyrings/oneapi-archive-keyring.gpg] https://apt.repos.intel.com/oneapi all main" | sudo tee /etc/apt/sources.list.d/oneAPI.list
sudo apt update
sudo apt install intel-oneapi-compiler-fortran
```
In order to set up the environment variables, run the following command (current shell):
```shell
source /opt/intel/oneapi/setvars.sh
```
To set up the environment variables permanently, run the following command:
```shell
nano ~/.bashrc
```
Insert in the end of the file:
```shell
source /opt/intel/oneapi/setvars.sh
```
## Compile, build binary, generate data (Linux)
To compile and build binary (executable), follow these steps:
- cd /path/to/Geopack-2008
- execute in shell:
```shell
ifX Geopack-2008.for <test_data_generation_code.for> -o gen_data && ./gen_data
```

# Fortran Test Examples for Geopack

This repository contains a set of Fortran programs designed to generate reference data for unit testing the Geopack library.
Each source file tests a specific Geopack routine by calculating its expected output for a given set of input parameters.

## Usage

Follow steps below to generate test data for specified Geopack-2008dp procedure.

### 1. Coordinates transformation
- GeiGeo / GeoGei
- GeoGsw / GswGeo
- GeoMag / MagGeo
- GswGse / GseGsw
- MagSm / SmMag
- SmGsw / GswSm*

Open `UnitTests/Geopack/FortranSource/CoordinatesTestDataGenerator.for` sourceFile and modify the input parameters directly in the code:

#### Setup location test coordinates
```fortran
DATA X/6.5999999999999996D0,-6.5999999999999996D0,
     *1.D0,-1.D0,4.5678D0,-4.5678D0,0.D0/

DATA Y/6.5999999999999996D0,-6.5999999999999996D0,
*1.D0,-1.D0,4.5678D0,-4.5678D0,0.D0/

DATA Z/6.5999999999999996D0,-6.5999999999999996D0,
 *1.D0,-1.D0,4.5678D0,-4.5678D0,0.D0/
```
#### Setup date/time and solar wind direction
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
#### Setup transformation direction
```fortran
J=-1
```
#### Specify output test data file name
```fortran
OPEN(UNIT=1,FILE='GswGeo.dat')
```
#### Specify testing procedure in the cycle
```fortran
CALL GEOGSW_08 (X(N),Y(M),Z(K),XR,YR,ZR,J)
    ...
CALL GEOGSW_08 (XR,YR,ZR,X(N),Y(M),Z(K),J)
```

### 2. Compile and execute

Compile the double-precision Geopack library with the test source file.
`GEOGSW_08` as example:

```bash
ifx Geopack_2008dp.for CoordinatesTestDataGenerator.for -o gen_data && ./gen_data && rm gen_data && mv GswGeo.dat ../TestData/
```

### 3. Synchronization

Ensure the input parameters in these test generators remain synchronized with the actual unit tests.
Do not forget that test data file name should be synchronized with corresponding variable in test fixture, e.g.:
```
private const string GeoGswDatasetFileName =
        "AuroraScienceHub.Geopack.UnitTests.Geopack.TestData.GeoGsw.dat";
```
