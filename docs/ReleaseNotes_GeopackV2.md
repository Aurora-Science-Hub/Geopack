# Geopack v2.0.0 Release Notes

## Overview

Version 2.0.0 represents a major release of the Geopack library, implementing comprehensive architectural improvements focused on thread-safety, immutability, performance optimization, and API design enhancements.

## Summary of Changes

This release consolidates multiple pull requests addressing critical architectural and performance improvements:

- Replaces mutable shared state (Common1/Common2) with immutable ComputationContext pattern for thread-safety
- Introduces strongly-typed generic vector quantities (CartesianVector, SphericalVector) for improved type safety
- Converts data models from reference types to readonly record structs for enhanced performance
- Adds dependency injection support with ServiceCollectionExtensions
- Implements comprehensive performance optimizations including SIMD vectorization, Math.SinCos, and Math.FusedMultiplyAdd
- Extracts magic numbers to well-documented constants in GeopackConstants

## Modified Files Summary

| File                                         | Description                                                                       |
|----------------------------------------------|-----------------------------------------------------------------------------------|
| `src/Geopack/Utilities/ObjectExtensions.cs`  | Adds null-checking extension methods for required parameters                      |
| `src/Geopack/ServiceCollectionExtensions.cs` | Adds DI registration for Geopack services                                         |
| `src/Geopack/IGeopack.cs`                    | Updated interface using new strongly-typed contracts                              |
| `src/Geopack/Geopack.cs`                     | Main class now requires ILogger via DI                                            |
| `src/Geopack/Geopack.*.cs`                   | Multiple partial class files with refactored methods accepting ComputationContext |
| `src/Contracts/*`                            | New strongly-typed contract types replacing legacy models                         |
| `UnitTests/*`                                | Updated tests using new API patterns                                              |
| `benchmarks/*`                               | Updated benchmarks showing performance improvements                               |

## Detailed Changes by Issue

### Thread-Safety and Immutability (Issues #49, #50, #52, #63 - PR #44)

This change refactors the library to eliminate shared mutable state by introducing the ComputationContext pattern.

**Key Changes:**

- Introduced ComputationContext as an immutable record to hold pre-calculated coefficients, replacing mutable Common1/Common2 instances
- Updated all coordinate transformation and field calculation methods to accept ComputationContext as a parameter
- Converted data model records (CartesianLocation, SphericalLocation, etc.) from reference types to readonly record structs for improved performance

### Strongly-Typed API Design (Issue #43 - PR #64)

This change improves type safety and API design by introducing generic vector quantities and refactoring method signatures.

**Key Changes:**

- Introduced strongly-typed vector quantities using generics (CartesianVector<T>, SphericalVector<T>)
- Converted coordinate and vector transformations from standalone methods to instance methods on model types
- Refactored methods to accept structured objects instead of individual coordinate parameters
- Converted data models to readonly record structs for better performance
- Added validation with explicit exception throwing instead of returning NaN values

### Dependency Injection Support (Issue #43 - PR #66)

This change adds dependency injection support to enable integration with modern .NET applications.

**Key Changes:**

- Added `AddGeopack()` extension method to register IGeopack implementation
- Added `AddExternalFieldModels()` extension method to register IT89 implementation
- Added Microsoft.Extensions.DependencyInjection.Abstractions package dependency (version 9.0.10)

### Performance Optimizations (Issue #46 - PR #70)

This change implements comprehensive performance optimizations leveraging modern .NET features and mathematical computation best practices.

**Key Changes:**

- Replaced `Math.Pow(x, 2)` with direct multiplication `x * x` for improved performance
- Adopted `Math.SinCos()` to compute sine and cosine simultaneously, reducing redundant calculations
- Extracted magic numbers into well-documented constants in GeopackConstants.cs
- Implemented SIMD vectorization using `Vector<double>` for IGRF coefficient interpolation and extrapolation
- Refactored Newton's method iteration into a separate method for better code organization
- Optimized list initialization with capacity hints to reduce allocations

**Optimized Files:**

| File | Optimization Details |
|------|---------------------|
| `src/Geopack/Geopack.Trace.cs` | Optimized list capacity, dot product calculation |
| `src/Geopack/Geopack.T96Mgnp.cs` | Replaced Math.Pow with multiplication, used Math.SinCos, extracted pressure factor constant |
| `src/Geopack/Geopack.Sun.cs` | Extracted constants, adopted Math.SinCos |
| `src/Geopack/Geopack.ShuMgnp.cs` | Extracted model coefficients as constants, refactored Newton's method to private method, optimized trigonometric calculations |
| `src/Geopack/Geopack.Recalc.cs` | Implemented SIMD vectorization for coefficient processing, eliminated zero multiplications, cached repeated calculations |
| `src/Geopack/Geopack.IgrfGsw.cs` | Replaced Math.Pow with direct multiplication |
| `src/Geopack/Geopack.IgrfGeo.cs` | Adopted Math.SinCos for simultaneous trigonometric calculations |
| `src/Geopack/Geopack.Dip.cs` | Replaced Math.Pow with multiplication, cached repeated array accesses |
| `src/ExternalFieldModels/T89/T89.cs` | Adopted Math.SinCos |
| `src/Contracts/Spherical/SphericalVector.cs` | Minor formatting adjustment |
| `src/Contracts/Spherical/SphericalLocation.cs` | Cached trigonometric calculations to avoid recomputation |
| `src/Contracts/Engine/GeopackConstants.cs` | Added numerous well-documented constants for physics calculations and algorithm parameters |
| `src/Contracts/Coordinates/GeodeticCoordinates.cs` | Extracted WGS84 constants, replaced Math.Pow, adopted Math.SinCos |
| `src/Contracts/Coordinates/GeocentricCoordinates.cs` | Optimized iterative calculation with constant extraction and Math.SinCos |
| `src/Contracts/Cartesian/CartesianVector.cs` | Replaced Math.Pow with direct multiplication |

## Breaking Changes

Version 2.0.0 introduces breaking changes due to the architectural improvements:

- Method signatures have changed to accept ComputationContext and strongly-typed objects
- Data models are now readonly record structs
- Some methods have been converted from static to instance methods on model types
- Exception handling has been updated to throw explicit exceptions instead of returning NaN values

## Migration Guide

Users upgrading from v1.x to v2.0.0 should:

1. Update method calls to use ComputationContext obtained from `Recalc()` method
2. Replace individual coordinate parameters with strongly-typed location and vector objects
3. Update exception handling to catch explicit exceptions
4. Consider using dependency injection for service registration
5. Review and update any code that relied on mutable shared state

## Performance Improvements

Benchmark results demonstrate significant performance improvements across various operations due to the optimizations implemented in this release. See the benchmarks directory for detailed performance metrics.

