# Changelog

All notable changes to the AuroraScienceHub.Geopack project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [2.0.0] - 2026-02-04

### Added

- Immutable `ComputationContext` record to hold pre-calculated coefficients for thread-safe operations
- Strongly-typed generic vector quantities: `CartesianVector<T>` and `SphericalVector<T>`
- Dependency injection support via `ServiceCollectionExtensions`
  - `AddGeopack()` extension method for registering `IGeopack` implementation
  - `AddExternalFieldModels()` extension method for registering `IT89` implementation
- `GeopackConstants` class containing well-documented physics and algorithm constants
- Null-checking extension methods in `ObjectExtensions` for parameter validation
- Explicit exception throwing for invalid inputs instead of returning NaN values
- SIMD vectorization using `Vector<double>` for IGRF coefficient interpolation and extrapolation
- Capacity hints for list initialization to reduce memory allocations

### Changed

- **BREAKING**: All coordinate transformation and field calculation methods now require `ComputationContext` parameter
- **BREAKING**: Method signatures refactored to accept strongly-typed location and vector objects instead of individual coordinate parameters
- **BREAKING**: Data model records converted from reference types to `readonly record struct` for improved performance
- **BREAKING**: Coordinate and vector transformations converted from standalone methods to instance methods on model types
- **BREAKING**: Exception handling updated to throw explicit exceptions if incorrect coordinate systems are provided or inputs are invalid
- `IGeopack` interface updated to use new strongly-typed contracts
- `Geopack` main class now requires `ILogger` via dependency injection
- Replaced `Math.Pow(x, 2)` with direct multiplication `x * x` throughout codebase
- Adopted `Math.SinCos()` for simultaneous sine and cosine calculations
- Optimized Newton's method iteration extracted to separate method
- Cached repeated trigonometric calculations to avoid recomputation
- Eliminated zero multiplications in coefficient processing
- Optimized dot product calculations in trace methods

### Removed

- **BREAKING**: Mutable shared state classes `Common1` and `Common2` removed
- **BREAKING**: Methods that relied on shared mutable state

### Fixed

- Thread-safety issues caused by mutable shared state
- Performance bottlenecks in mathematical operations

### Performance

- Significant performance improvements through SIMD vectorization
- Reduced memory allocations via `readonly record struct` conversion
- Optimized trigonometric calculations with `Math.SinCos()`
- Improved computational efficiency by eliminating redundant calculations
- Enhanced list operations with pre-allocated capacity

### Migration from v1.x to v2.0.0

#### Required Changes

1. **ComputationContext Parameter**

   Before:
   ```csharp
   geopack.GeoToGsw(xGeo, yGeo, zGeo, out xGsw, out yGsw, out zGsw);
   ```

   After:
   ```csharp
   var context = geopack.Recalc(dateTime, CartesianVector<Velocity>.New(vxGse, vyGse, vzGse, CoordinateSystem.GSE));
   var result = geopack.GeoToGsw(context, CartesianLocation.New(xGeo, yGeo, zGeo, CoordinateSystem.GEO));
   ```

2. **Strongly-Typed Objects**

   Before:
   ```csharp
   geopack.IgrfGeo(xGeo, yGeo, zGeo, out hxGeo, out hyGeo, out hzGeo);
   ```

   After:
   ```csharp
   var location = CartesianLocation.New(xGeo, yGeo, zGeo, CoordinateSystem.GEO);
   var fieldVector = geopack.IgrfGeo(context, location);
   ```

3. **Dependency Injection Setup**

   ```csharp
   services.AddGeopack();
   services.AddExternalFieldModels();
   ```

4. **Exception Handling**

   After: Methods throw exceptions for invalid inputs and incorrect coordinate systems. This increase code robustness but requires updating error handling in calling code.

#### Additional Considerations

- Review code that relied on mutable shared state (Common1/Common2)
- Update unit tests to use new API patterns
- Verify exception handling covers new explicit exceptions
- Consider leveraging dependency injection for better testability
- Check that readonly structs are passed by reference where appropriate for performance

