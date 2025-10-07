# Geopack-2008 test data
## Sources
Find the single precision version of GEOPACK-2008 [here](https://geo.phys.spbu.ru/~tsyganenko/Geopack-2008.html).
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
- cd /path/to/Geopack-2008 and T96 sources;
- Get the author's example code [here](https://geo.phys.spbu.ru/~tsyganenko/models/Examples_1_2.for);
- execute in shell:
```shell
ifX -O3 Geopack-2008.for T96.for <Example_code.for> -o gen_data && ./gen_data
```

# Fortran Test Examples for Geopack

This repository contains a set of Fortran programs designed to generate reference data for unit testing the Geopack library.
Each source file tests a specific Geopack routine by calculating its expected output for a given set of input parameters.

## Usage

Follow these steps to generate test data:

### 1. Set Input Parameters
Open the desired source file (e.g., `DIP_08.for`) and modify the input parameters directly in the code:

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

### 2. Compile and execute

Compile the double-precision Geopack library with the test source file. We use Intel Fortran compiler:

> ifx Geopack_2008dp.for <test_source_file>.for -o <executable_name> && ./<executable_name>

*Example:*

```bash
ifx Geopack_2008dp.for DIP_08.for -o dip08 && ./dip08
```

### 4. Test Integration

Copy the output values from the terminal into the corresponding unit test.

### 5. Synchronization

Ensure the input parameters in these test generators remain synchronized with the actual unit tests.

### File Structure
```text
├── Geopack_2008dp.for    # Double-precision Geopack library
├── DIP_08.for  # Example test generator for DIP routine
├── TRACE_08.for # Test generator for TRACE routine
└── ...
```
