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
