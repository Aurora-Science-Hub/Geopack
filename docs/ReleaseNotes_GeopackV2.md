# Geopack v2.0.0 Release Notes

## Issue #49 #50 #52 #63 (PR #44):

This PR refactors the Geopack library to improve thread-safety and immutability by replacing shared mutable state (Common1/Common2 classes)
with an immutable **ComputationContext** pattern.

### Key changes:
- Introduces ComputationContext as an immutable record to hold pre-calculated coefficients instead of mutable Common1/Common2 instances;
- Updates all coordinate transformation and field calculation methods to accept ComputationContext as a parameter;
- Converts data model records (CartesianLocation, SphericalLocation, etc.) from reference types to readonly record structs for better performance.

## Issue #43 (PR #64):

This PR refactors the Geopack library to improve thread-safety, immutability, and API design by replacing mutable shared state with an immutable ComputationContext pattern. Key changes include:

### Key changes:
- Introducing strongly-typed vector quantities using generics (CartesianVector<T>, SphericalVector<T>)
- Converting coordinate/vector transformations from standalone methods to instance methods on model types
- Refactoring methods to accept structured objects instead of individual coordinate parameters
- Converting data models to readonly record structs for better performance
- Adding validation with explicit exception throwing instead of returning NaN values

## Issue #43 (PR #66):

This PR adds dependency injection support for the Geopack library by introducing ServiceCollectionExtensions classes that provide convenient registration methods for the library's services. The PR adds the necessary Microsoft.Extensions.DependencyInjection.Abstractions package reference to enable this functionality.

### Key changes
- Added AddGeopack() extension method to register IGeopack implementation
- Added AddExternalFieldModels() extension method to register IT89 implementation
- Added Microsoft.Extensions.DependencyInjection.Abstractions package dependency (version 9.0.10)

## Issue #46 (PR #70):

This PR implements comprehensive code optimizations focused on improving computational performance across the Geopack library.
The optimizations target mathematical operations, memory allocation, and leverage modern .NET features including SIMD vectorization.

Key changes:

- Replaced Math.Pow(x, 2) with direct multiplication x * x for better performance;
- Adopted Math.SinCos() to compute sine and cosine simultaneously, reducing redundant calculations;
- Extracted magic numbers into well-documented constants in GeopackConstants.cs;
- Implemented SIMD vectorization using Vector<double> for IGRF coefficient interpolation and extrapolation;
- Refactored Newton's method iteration into a separate method for better code organization;
- Optimized list initialization with capacity hints to reduce allocations.

| File                                                 | Description                                                                                                                   |
|------------------------------------------------------|-------------------------------------------------------------------------------------------------------------------------------|
| `src/Geopack/Geopack.Trace.cs`                       | Optimized list capacity, dot product calculation                                                                              |
| `src/Geopack/Geopack.T96Mgnp.cs`                     | Replaced Math.Pow with multiplication, used Math.SinCos, extracted pressure factor constant                                   |
| `src/Geopack/Geopack.Sun.cs`                         | Extracted constants, adopted Math.SinCos                                                                                      |
| `src/Geopack/Geopack.ShuMgnp.cs`                     | Extracted model coefficients as constants, refactored Newton's method to private method, optimized trigonometric calculations |
| `src/Geopack/Geopack.Recalc.cs`                      | Implemented SIMD vectorization for coefficient processing, eliminated zero multiplications, cached repeated calculations      |
| `src/Geopack/Geopack.IgrfGsw.cs`                     | Replaced Math.Pow with direct multiplication                                                                                  |
| `src/Geopack/Geopack.IgrfGeo.cs`                     | Adopted Math.SinCos for simultaneous trigonometric calculations                                                               |
| `src/Geopack/Geopack.Dip.cs`                         | Replaced Math.Pow with multiplication, cached repeated array accesses                                                         |
| `src/ExternalFieldModels/T89/T89.cs`                 | Adopted Math.SinCos                                                                                                           |
| `src/Contracts/Spherical/SphericalVector.cs`         | Minor formatting adjustment                                                                                                   |
| `src/Contracts/Spherical/SphericalLocation.cs`       | Cached trigonometric calculations to avoid recomputation                                                                      |
| `src/Contracts/Engine/GeopackConstants.cs`           | Added numerous well-documented constants for physics calculations and algorithm parameters                                    |
| `src/Contracts/Coordinates/GeodeticCoordinates.cs`   | Extracted WGS84 constants, replaced Math.Pow, adopted Math.SinCos                                                             |
| `src/Contracts/Coordinates/GeocentricCoordinates.cs` | Optimized iterative calculation with constant extraction and Math.SinCos                                                      |
| `src/Contracts/Cartesian/CartesianVector.cs`         | Replaced Math.Pow with direct multiplication                                                                                  |
