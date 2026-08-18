# geopack (Python)

Python bindings for the [Aurora-Science-Hub/Geopack](https://github.com/Aurora-Science-Hub/Geopack)
library — a C# port of the GEOPACK-2008 (double precision) magnetospheric field
model and solar-terrestrial coordinate transforms.

The bindings load a NativeAOT-built shared library (`geopack.dylib` /
`geopack.so` / `geopack.dll`) via `ctypes` from the Python standard library.
No .NET runtime is required at runtime.

## Install

Build the wheel for your platform (requires .NET SDK 10 + a C toolchain) and
`pip install` it:

```bash
./python/build_wheel.sh
pip install python/dist/geopack-*.whl
```

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
```

## API

All coordinates are in Earth radii; magnetic fields are in nT; angles are in
radians; solar wind velocity is in km/s.

* `recalc(date_or_year, month=None, day=None, hour=0, minute=0, second=0, vx=-400, vy=0, vz=0) -> Context`
* `Context` methods:
  * fields: `igrf_gsw(x,y,z)`, `dip(x,y,z)`, `igrf_geo(r,theta,phi)`
  * transforms: `gsw_to_gse`, `gse_to_gsw`, `geo_to_mag`, `mag_to_geo`,
    `gei_to_geo`, `geo_to_gei`, `mag_to_sm`, `sm_to_mag`, `sm_to_gsw`,
    `gsw_to_sm`, `geo_to_gsw`, `gsw_to_geo` — each `(x,y,z) -> (x,y,z)`
  * lifecycle: `close()`, context manager
* `sun(...) -> Sun(gst, slong, srasn, sdec)` — no context needed
* `shu_mgnp(xn_pd, vel, bz_imf, x, y, z)` and `t96_mgnp(xn_pd, vel, x, y, z)`
  return `MagnetopauseResult(boundary, dist, position)` — no context needed

`GEOPACK_LIBRARY` environment variable points the loader at a specific library
path if the packaged one is not present.
