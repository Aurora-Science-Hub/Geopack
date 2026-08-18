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
3. `deploy/python/` — a pure-Python package that loads the library through
   `ctypes` (stdlib only) and wraps it in an object API.
4. `deploy/build_wheel.sh` packages both into a **platform-specific wheel**
   (the `setup.py` shim forces a platform tag; wheels for other OS/arch must be
   built on that OS/arch).

## Commands

```bash
./deploy/build_native.sh          # -> deploy/out/<rid>/geopack.dylib
./deploy/build_wheel.sh           # -> deploy/dist/geopack-<ver>-...-macosx_*_arm64.whl

# smoke-test the source package directly (library must be built/copied first)
python3 deploy/python/tests/test_smoke.py
```

## Requirements

* .NET SDK 10 (pinned in `global.json`) + C toolchain for NativeAOT
* Python 3.8+ for the bindings

## Notes

* This tooling is **local-only by design**: the CI/CD pipeline
  (`.github/workflows/dotnet.yml`) is intentionally untouched. `src/Geopack.Native`
  is not part of `Geopack.slnx`.
* Only builds for the host platform. Cross-platform wheels require building on
  each target OS/arch (NativeAOT does not cross-compile).
* The wheel is self-contained: the .NET runtime is statically linked in, so end
  users need nothing but Python.
