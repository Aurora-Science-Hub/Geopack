# Changelog

All notable changes to the AuroraScienceHub.Geopack project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [2.2.2] - 2026-08-24

### Changed

- README install instructions now use `pip install geopack-2008` (pre-built
  wheels on PyPI); local build instructions moved to `python/BUILDING.md`

## [2.2.1] - 2026-08-24

### Added

- PyPI trusted publishing for the Python distribution `geopack-2008`
  (`python-package.yml` publishes wheels via OpenID Connect, no API token)

### Fixed

- Linux wheels are now tagged `manylinux_2_28_*` (built via cibuildwheel inside a
  manylinux container), so PyPI accepts them; the bare `linux_*` platform tag was
  previously rejected
- Wheels are now `py3-none-<platform>` so a single wheel installs on every
  Python 3.8+ (previously tied to the build CPython version)

## [2.2.0] - 2026-08-22

### Added

- T89 (Tsyganenko 1989) external field model in the Python bindings: new `gp_t89`
  C export (context-free, input GSW, output GSM, serialized natively because the
  model keeps static state) and `gp_context_psi` exposing the dipole tilt computed
  by `Recalc`
- Python API: module-level `geopack.t89(iopt, psi, x, y, z, parmod=None)` and
  `Context.t89(iopt, x, y, z, psi=None)` plus the `Context.psi` property
- Python parity tests mirroring `ExternalFieldModelsTests.T89.cs` 1:1 (same 8
  cases, same `new double[10]` parmod dummy, `1e-13` tolerance)
- GitHub Actions workflow `python-package.yml`: builds the Python package and
  runs its tests on 5 RIDs (linux-x64/arm64, win-x64/arm64, osx-arm64),
  uploading the wheels as artifacts (PyPI publishing deferred)

---

## [2.1.0] - 2026-08-18

### Added

- `src/Geopack.Native` — a C ABI layer over the Geopack core, compiled with NativeAOT
  into a self-contained shared library (`geopack.dylib` / `geopack.so` / `geopack.dll`)
  loadable from Python via `ctypes` without a .NET runtime
- Python package `python/geopack` exposing `recalc()`, `sun()`, coordinate transforms,
  field models and magnetopause models, plus local build tooling
  (`python/build_native.sh` / `build_wheel.sh` on macOS/Linux,
  `build_native.bat` / `build_wheel.bat` on Windows)
- Python test suite mirroring the C# unit tests 1:1 (same cases, reference data,
  `8E-12` tolerance): `IgrfGsw` / `IgrfGeo` / `Dip` / `Sun` / `ShuMgnp` / `T96Mgnp`
  plus 2592 coordinate-transform assertions

### Fixed

- `gp_last_error` no longer throws on an undersized buffer and keeps the message
  pending until the whole message has been read
- opaque handles typed as `c_int64` in the ctypes layer (`c_long` is 32-bit on Windows)
- `build_wheel.sh` also accepts the lib-prefixed NativeAOT library name on Unix

---

## [2.0.3] - 2026-08-13

### Added

- SourceLink support for GitHub (`Microsoft.SourceLink.GitHub`): NuGet packages now
  map to source in the repository
- ContinuousIntegrationBuild enabled on CI builds for deterministic PDBs

### Changed

- Package metadata consolidated into `src/Directory.Build.props`, removing duplicates
- Per-package tags no longer overwritten by the shared build

---

## [2.0.2] - 2026-07-16

### Fixed

- Out-of-bounds array access in `Extrapolate` method when using SIMD vectorisation for IGRF coefficients for years >= 2025. The `vectorizedLength` was not aligned to `Vector<double>.Count`, causing the last SIMD iteration to read past the coefficient array.

---

## [2.0.1] - 2026-04-26

### Changed

- Updated `AGENTS.md` with accurate, repo-specific guidance for AI coding agents (replaced stale template content from a different project)

---

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

