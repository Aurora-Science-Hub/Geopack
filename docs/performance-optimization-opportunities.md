# Performance Optimization Opportunities

## Scope

This document records optimization opportunities identified by reviewing the computational hot paths and benchmark configuration. The retained optimizations are internal, preserve public contracts, and pass the unchanged reference tests. Changes that did not demonstrate a benefit were reverted.

The most important initial finding is that the current benchmark suite does not provide a reliable baseline:

- Many benchmarks return `void`, allowing the JIT to eliminate unused computations.
- The benchmark project targets `net11.0`, while its custom BenchmarkDotNet job still targets `.NET 8.0`.
- A local attempt to run the `IgrfGsw` benchmark failed during restore with `NU1201`; no benchmarks were executed.

Fixing the benchmark infrastructure is therefore a prerequisite for evaluating the remaining proposals.

## Recommended Priority

1. Repair the benchmarks and establish a `net11.0` baseline.
2. Replace the fixed-exponent `Math.Pow` call in `Dip`.
3. Precompute invariant tables used by `Recalc`.
4. Remove redundant square roots and trigonometric operations from `ShuMgnp`.
5. Optimize the inner loops of `IgrfGeo` and `IgrfGsw`.
6. Re-measure `Trace`, which should benefit indirectly from model optimizations.

## Measured Results

The final .NET 11 ARM64 comparison is available in [v2.5.0 Performance Results](../benchmarks/AuroraScienceHub.Geopack.Benchmarks/Geopack/results/v2/v2.5.0_Performance_MacOS_NET11.md).

Key execution-time reductions relative to the repaired baseline:

- `Dip`: 69.88%.
- `Recalc`: 14.99%.
- `ShuMgnp`: 5.67%.
- `IgrfGeo` / `IgrfGsw`: 2.41% / 1.47%.
- `Trace_NS`: 1.71%.

## 1. Benchmark Reliability

**Location:** [`GeopackBenchmarks.cs`](../benchmarks/AuroraScienceHub.Geopack.Benchmarks/Geopack/GeopackBenchmarks.cs)

Most benchmark methods currently discard their results:

```csharp
[Benchmark]
public void GeoToGsw()
    => s_geopack.GeoToGsw(s_context, s_geoCartesian);
```

When such a method is inlined and has no observable side effects, the JIT can remove the computation. The near-zero results in the existing reports are consistent with this problem and must not be treated as actual execution times.

The custom job also uses `CoreRuntime.Core80` even though the benchmark project targets only `net11.0`. The attempted benchmark run produced:

```text
error NU1201: Project AuroraScienceHub.Geopack.Benchmarks is not compatible
with net10.0 (.NETCoreApp,Version=v10.0). Project
AuroraScienceHub.Geopack.Benchmarks supports: net11.0 (.NETCoreApp,Version=v11.0)
```

**Proposed changes:**

- Return the computed value from every benchmark method.
- Update the custom job to run on `net11.0`.
- Record the runtime, hardware, commit, and benchmark configuration with new results.
- Establish the baseline before changing computational code.

## 2. `Dip`: Replace Fixed-Exponent `Math.Pow`

**Location:** [`Geopack.Dip.cs:32`](../src/Geopack/Geopack.Dip.cs#L32)

Current code:

```csharp
double q = dipmom / Math.Pow(p + t + u, 2.5);
```

The general-purpose `Math.Pow` call is unnecessary for the fixed exponent `2.5`. The expression can use multiplication and a square root instead:

```csharp
double r2 = p + t + u;
double q = dipmom / (r2 * r2 * Math.Sqrt(r2));
```

This is a small, localized change and is particularly relevant when `Dip` is used as the internal field model during field-line tracing.

**Validation:** Check the existing Fortran reference tests for numerical differences and benchmark both direct `Dip` calls and `Trace` with `Dip` as the internal model.

## 3. `Recalc`: Precompute Invariant Tables

**Locations:**

- [`Geopack.Recalc.cs:41`](../src/Geopack/Geopack.Recalc.cs#L41)
- [`Geopack.Recalc.cs:98`](../src/Geopack/Geopack.Recalc.cs#L98)
- [`ComputationContext.cs:18`](../src/Contracts/Engine/ComputationContext.cs#L18)

Each `Recalc` call currently recalculates:

- All 105 entries of `REC`, although they are invariant.
- The Schmidt normalization factors, including repeated `Math.Sqrt` calls, although these factors depend only on the harmonic indices.

**Proposed changes:**

- Precompute `REC` once and copy its values into the per-context array instead of repeating the arithmetic.
- Precompute the normalization factors and apply them to `G` and `H` by multiplication.
- Where practical, combine normalization with the existing coefficient-processing passes and retain SIMD where it remains beneficial.

Sharing one mutable array between all contexts is not recommended. `ComputationContext` publicly exposes `double[]` instances, so a consumer could mutate a shared table and corrupt unrelated calculations. Removing the remaining allocations safely would require an immutability-oriented design and should be considered separately from this localized optimization.

**Validation:** Compare complete `ComputationContext` values against the existing reference data, not only downstream magnetic-field results.

## 4. `ShuMgnp`: Simplify the Newton Iteration

**Location:** [`Geopack.ShuMgnp.cs:143`](../src/Geopack/Geopack.ShuMgnp.cs#L143)

The iteration computes a square root and then immediately squares the result:

```csharp
gradf = Math.Sqrt(gradf_r * gradf_r + gradf_t * gradf_t);
dr = -f / (gradf * gradf);
```

Since `gradf_r` is always `1.0`, this can be expressed directly as:

```csharp
double gradf2 = 1.0D + gradf_t * gradf_t;
dr = -f / gradf2;
```

Additional opportunities in the same method:

- Compare the squared step length against `1e-8` instead of calculating `ds` with `Math.Sqrt` solely for the `1e-4` convergence check.
- Reuse the `st` and `ct` values produced by the last iteration instead of calling `Math.SinCos(t)` again after the loop.
- Investigate sharing pressure and angular calculations with `T96Mgnp`, but only after measuring the simpler changes.

**Validation:** Test convergence, iteration counts, boundary positions, and exceptional cases in addition to output values within the reference tolerance.

## 5. IGRF: Reduce Work in the Inner Loops

**Locations:**

- [`Geopack.IgrfGsw.cs:84`](../src/Geopack/Geopack.IgrfGsw.cs#L84)
- [`Geopack.IgrfGeo.cs:72`](../src/Geopack/Geopack.IgrfGeo.cs#L72)

The inner loops repeatedly calculate the triangular coefficient index:

```csharp
int mn = n * (n - 1) / 2 + m;
```

The index can instead be updated incrementally. Moving from `n` to `n + 1` increases the triangular component by `n`, removing integer multiplication and division from the hot loop without changing the mathematical recurrence.

A second experiment is to use fixed maximum-length `stackalloc` buffers instead of variable-length buffers. This may simplify JIT code generation and bounds checks, but it should be retained only if benchmarks demonstrate an improvement.

The two IGRF implementations contain closely related computational kernels. Consolidating duplicated logic could make future optimizations easier to validate, but consolidation itself should not be presented as a performance improvement without measurements.

**Validation:** Run both GEO and GSW reference tests at multiple radii, including different expansion orders and near-polar inputs.

## 6. `Trace`: Optimize Through Its Field Models

**Locations:**

- [`Geopack.Trace.cs:182`](../src/Geopack/Geopack.Trace.cs#L182)
- [`Geopack.Trace.cs:143`](../src/Geopack/Geopack.Trace.cs#L143)

Each Runge–Kutta attempt evaluates the right-hand side five times, and every evaluation calls both the external and internal field models. Rejected steps require additional evaluations. Consequently, improvements to `Dip` and IGRF can affect `Trace` several times per integration step.

The current data representation is already reasonably allocation-conscious:

- Locations, vectors, step results, and right-hand-side values are structs.
- The point list reserves capacity in advance.
- No separate heap object is created for every point.

Pooling the returned `FieldLine.Points` list is not recommended: it is publicly mutable and owned by the returned object, so buffer reuse could corrupt previously returned field lines.

After the model-level optimizations, profile `Trace` again before considering delegate dispatch, validation removal, or specialized integration paths.

## 7. Smaller Trigonometric Improvements

Several coordinate conversions use separate `Math.Sin` and `Math.Cos` calls for the same angle. Replacing such pairs with `Math.SinCos` is a straightforward localized improvement.

Candidates include:

- [`SphericalLocation.ToCartesian`](../src/Contracts/Spherical/SphericalLocation.cs)
- [`GeodeticCoordinates.ToGeocentric`](../src/Contracts/Coordinates/GeodeticCoordinates.cs)
The double-angle substitution for `sin(2g)` in `Geopack.Sun` and the `cos(phi1 - xmus)` identity in `GeocentricCoordinates.ToGeodetic` were tested but reverted because they did not improve the ARM64 benchmark results.

These are lower-priority changes and should not be mixed into the same performance comparison as the major hot-path modifications.

## Validation Checklist

For each optimization:

1. Keep the change isolated enough to benchmark independently.
2. Run `dotnet format`, `dotnet build`, and `dotnet test` successfully.
3. Preserve the Geopack reference tolerance of `8E-12` and the ExternalFieldModels tolerance of `1E-13`.
4. Exercise boundary conditions and exceptional inputs, not only the standard benchmark inputs.
5. Compare Release-mode `net11.0` benchmarks using the same hardware and configuration.
6. Report both execution time and allocated memory.
7. Update Python parity tests only if an external contract, reference value, tolerance, or C ABI behavior changes; purely internal numerical optimizations should already be covered by existing parity tests.
