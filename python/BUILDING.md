# Geopack Python bindings — local build tooling

This directory contains everything needed to expose the .NET Geopack library
(GEOPACK-2008) to Python as a native shared library with **no .NET runtime**
required at run time.

## How it works

1. `src/Geopack.Native/` — a .NET project that exports a flat C ABI over the
   Geopack core via `[UnmanagedCallersOnly]` (fields, coordinate transforms,
   magnetopause models; no `Trace` yet).
2. NativeAOT (`PublishAot=true`, `NativeLib=Shared`) compiles it into
   `geopack.dylib` / `geopack.so` / `geopack.dll`.
3. `python/` — a pure-Python package that loads the library through
   `ctypes` (stdlib only) and wraps it in an object API.
4. `python/build_wheel.sh` / `build_wheel.bat` packages both into a
   **platform-specific wheel** (the `setup.py` shim forces a platform tag; wheels
   for other OS/arch must be built on that OS/arch).

## Commands

**macOS / Linux:**

```bash
./python/build_native.sh          # -> python/out/<rid>/geopack.dylib
./python/build_wheel.sh           # -> python/dist/geopack_2008-<ver>-...-macosx_*_arm64.whl

# run the tests (library must be built/copied first)
python3 python/tests/test_api.py
python3 python/tests/test_parity.py
```

**Windows (cmd):**

```bat
.\python\build_native.bat         :: -> python\out\win-x64\geopack.dll
.\python\build_wheel.bat          :: -> python\dist\geopack_2008-<ver>-...-win_amd64.whl

:: run the tests (library must be built/copied first)
python python\tests\test_api.py
python python\tests\test_parity.py
```

## Tests

The Python suite (`python/tests/`) mirrors the C# unit tests in
`UnitTests/Geopack/GeopackTests.cs` **1:1 for every observable behaviour** —
the same test cases, the same reference data (`UnitTests/Geopack/TestData`),
and the same precision tolerance (`8E-12`). Numerical accuracy therefore
matches the original Fortran code to the same 12 decimal digits as the C#
library:

* `test_api.py` — the end-to-end API tests: reference IGRF value, round-trip
  transforms, `sun()`, field and magnetopause models, datetime input,
  error reporting via `gp_last_error`.
* `test_parity.py` — the full parity suite: `IgrfGsw` (7 cases), `IgrfGeo`
  (12 cases, incl. near-pole `Bphi ≈ 4.17e9 nT` matching bit-for-bit),
  `Dip` (7), `Sun` (4), `ShuMgnp` (8), `T96Mgnp` (8), and all **12 coordinate
  transforms** × 216 reference rows = **2592 assertions**.

Internal `ComputationContext` fields are intentionally not tested — the
opaque-handle design over the C ABI is preserved.

## Requirements

* .NET SDK 10 (pinned in `global.json`) + C toolchain for NativeAOT
* Python 3.8+ for the bindings

## Notes

* Only builds for the host platform. Cross-platform wheels require building on
  each target OS/arch (NativeAOT does not cross-compile).
* The wheel is self-contained: the .NET runtime is statically linked in, so end
  users need nothing but Python.
