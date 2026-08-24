# Geopack-2008 — GEOPACK-2008 geomagnetic field model for Python

[![Python](https://img.shields.io/badge/Python-3.8+-3776AB?logo=python&label=Python)](https://www.python.org/)
[![Python Package](https://github.com/Aurora-Science-Hub/Geopack/actions/workflows/python-package.yml/badge.svg)](https://github.com/Aurora-Science-Hub/Geopack/actions/workflows/python-package.yml)
[![License: GPL v3+](https://img.shields.io/badge/License-GPLv3+-blue.svg)](https://www.gnu.org/licenses/gpl-3.0)
[![GitHub Stars](https://img.shields.io/github/stars/Aurora-Science-Hub/Geopack?style=social)](https://github.com/Aurora-Science-Hub/Geopack)
[![PyPI](https://img.shields.io/pypi/v/geopack-2008)](https://pypi.org/project/geopack-2008/)

**Geopack-2008** (imported as `geopack`) is a high-performance, self-contained, zero-dependency Python package for the
**GEOPACK-2008** geomagnetic field model: the **International Geomagnetic
Reference Field (IGRF)**, the **dipole field**, the **Tsyganenko (1989)
external field model (T89)**, the **Shue et al. (1998) and Tsyganenko (1996)
magnetopause models**, the **Sun position** computation, and the standard
**solar-terrestrial coordinate transforms** (GEO, GSW, GSE, SM, MAG, GEI, GSM).

It is a Python binding over a [C# port of GEOPACK-2008](https://github.com/Aurora-Science-Hub/Geopack)
compiled ahead-of-time with NativeAOT into a shared library (`geopack.dylib` /
`geopack.so` / `geopack.dll`), loaded through `ctypes` from the Python standard
library. No .NET runtime is required at runtime, and there are no Python
dependencies (no NumPy/SciPy).

- **High performance**: SIMD vectorization, `Math.SinCos`, and other optimized
  mathematical operations relative to the original Fortran code
- **Double precision**: accuracy matches the original Fortran code to 12–13 decimal digits
- **Standard library only** — no third-party runtime dependencies
- **Thread-safe** immutable computation contexts
- **Platform wheels** for Linux (x64/arm64), Windows (x64/arm64) and macOS (arm64)

## Install

```bash
pip install geopack-2008
```

Pre-built wheels are published to PyPI for Linux (x64/arm64, `manylinux`),
Windows (x64/arm64) and macOS (arm64) — no .NET SDK or C toolchain required.

To build the wheel from source (e.g. for another platform or during
development), see [`BUILDING.md`](BUILDING.md).

## Usage

```python
import geopack

ctx = geopack.recalc(1997, 12, 16, 21, 0, 0, vx=-304, vy=13.78, vz=4)
b = ctx.igrf_gsw(1, 1, 1)     # (bx, by, bz) nT in GSW
r = ctx.gsw_to_geo(1, 1, 1)   # (x, y, z) Earth radii in GEO
ctx.close()

# A datetime works too, and contexts are context managers:
with geopack.recalc(datetime(1997, 12, 16, 21, 0), vx=-304, vy=13.78, vz=4) as ctx:
    b = ctx.igrf_gsw(1, 1, 1)

s = geopack.sun(1997, 12, 16, 21, 0, 0)   # (gst, slong, srasn, sdec), radians

mp = geopack.shu_mgnp(xn_pd=2.0, vel=400.0, bz_imf=0.0, x=10, y=0, z=0)
print(mp.boundary, mp.dist, mp.position)  # Vector3, float, MagnetopausePosition

# Tsyganenko (1989) external field — input GSW, output GSM (nT):
b = geopack.t89(iopt=3, psi=ctx.psi, x=-6.6, y=0, z=0)
b = ctx.t89(iopt=3, x=-6.6, y=0, z=0)     # psi defaults to ctx.psi
```

A runnable example with all of the above (sun position, IGRF/dipole fields,
coordinate transforms, magnetopause models) is in
[`python/example.py`](example.py):

```bash
python3 python/example.py
```

## Implemented models and coordinate systems

| Category | Model | Python API |
| --- | --- | --- |
| Internal field | IGRF (geomagnetic main field) | `Context.igrf_gsw`, `Context.igrf_geo` |
| Internal field | Dipole | `Context.dip` |
| External field | Tsyganenko (1989) — T89 | `Context.t89`, `geopack.t89` |
| Magnetopause | Shue et al. (1998) | `geopack.shu_mgnp` |
| Magnetopause | Tsyganenko (1996) | `geopack.t96_mgnp` |
| Sun position | Greenwich sidereal time, solar ephemeris | `geopack.sun` |

Supported coordinate systems (see `CoordinateSystem`): **GEO** (geographic),
**GSW** (geocentric solar wind), **GSE** (geocentric solar ecliptic),
**SM** (solar magnetic), **MAG** (geomagnetic), **GEI** (geocentric solar
equatorial inertial) and **GSM** (geocentric solar magnetospheric).

## API

All coordinates are in Earth radii; magnetic fields are in nT; angles are in
radians; solar wind velocity is in km/s.

* `recalc(date_or_year, month=None, day=None, hour=0, minute=0, second=0, vx=-400, vy=0, vz=0) -> Context`
* `Context` methods:
  * fields: `igrf_gsw(x,y,z)`, `dip(x,y,z)`, `igrf_geo(r,theta,phi)`,
    `t89(iopt,x,y,z, psi=None)`, property `psi` (dipole tilt, radians)
  * transforms: `gsw_to_gse`, `gse_to_gsw`, `geo_to_mag`, `mag_to_geo`,
    `gei_to_geo`, `geo_to_gei`, `mag_to_sm`, `sm_to_mag`, `sm_to_gsw`,
    `gsw_to_sm`, `geo_to_gsw`, `gsw_to_geo` — each `(x,y,z) -> (x,y,z)`
  * lifecycle: `close()`, context manager
* `sun(...) -> Sun(gst, slong, srasn, sdec)` — no context needed
* `shu_mgnp(xn_pd, vel, bz_imf, x, y, z)` and `t96_mgnp(xn_pd, vel, x, y, z)`
  return `MagnetopauseResult(boundary, dist, position)` — no context needed
* `t89(iopt, psi, x, y, z, parmod=None) -> Vector3` — Tsyganenko (1989) external
  field, input GSW, output GSM (nT); `parmod` is a dummy accepted for parity with
  the .NET `IT89.Calculate` contract (unused) — no context needed

`GEOPACK_LIBRARY` environment variable points the loader at a specific library
path if the packaged one is not present.

## How to Cite

If you use this software in your research, please cite it — see the
[`How to Cite`](https://github.com/Aurora-Science-Hub/Geopack#how-to-cite)
section of the main repository README (APA and BibTeX entries with a Zenodo DOI).

## References

The implementation is based on the GEOPACK-2008 library and the following
scientific works:

1. **Tsyganenko, N. A.** (1989). *A magnetospheric magnetic field model with a warped tail current sheet*. Planetary and Space Science, 37(1), 5–20. https://doi.org/10.1016/0032-0633(89)90066-4

2. **Tsyganenko, N. A.** (1996). *Effects of the solar wind conditions on the global magnetospheric configuration as deduced from data-based field models*. ESA SP-389, 181–185.

3. **Shue, J.-H., et al.** (1998). *Magnetopause location under extreme solar wind conditions*. Journal of Geophysical Research, 103(A8), 17691–17700. https://doi.org/10.1029/98JA01103

4. **Hapgood, M. A.** (1992). *Space physics coordinate transformations: A user guide*. Planetary and Space Science, 40(5), 711–717. https://doi.org/10.1016/0032-0633(92)90012-D

5. **Tsyganenko, N. A.** (2002). *A model of the near magnetosphere with a dawn-dusk asymmetry 1. Mathematical structure*. Journal of Geophysical Research, 107(A8). https://doi.org/10.1029/2001JA000219

6. **Tsyganenko, N. A., & Sitnov, M. I.** (2005). *Modeling the dynamics of the inner magnetosphere during strong geomagnetic storms*. Journal of Geophysical Research: Space Physics, 110(A3), A03208. https://doi.org/10.1029/2004JA010798
