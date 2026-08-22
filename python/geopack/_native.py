"""Thin ctypes layer over the NativeAOT-built geopack shared library.

The library exposes a flat C ABI (see src/Geopack.Native/GeopackNative.cs):

  * every function returns an ``int`` error code (0 = success, non-zero = error);
  * results are written into caller-provided out-pointers;
  * a ``ComputationContext`` lives behind an opaque ``long`` handle created by
    ``gp_context_create`` and released by ``gp_context_release``;
  * the thread-local error message (UTF-8) is fetched with ``gp_last_error``.

This module only knows how to cross the FFI boundary. The object-oriented,
user-facing API lives in :mod:`geopack`.
"""

from __future__ import annotations

import ctypes
import ctypes.util
import os
import sys
from typing import NamedTuple, Optional, Tuple

#: Filenames to try, in order, per platform. The native build produces
#: ``geopack.dylib`` on macOS, ``geopack.so`` on Linux and ``geopack.dll`` on
#: Windows (no ``lib`` prefix). A GEOPACK_LIBRARY env var overrides all of them.
_LIBRARY_NAMES = {
    "darwin": ("geopack.dylib", "libgeopack.dylib"),
    "linux": ("geopack.so", "libgeopack.so"),
    "win32": ("geopack.dll",),
}
_FALLBACK_NAMES = ("geopack.dylib", "libgeopack.dylib", "geopack.so", "libgeopack.so", "geopack.dll")


class GeopackError(Exception):
    """Raised when the native library reports a non-zero error code."""


def _find_library_path() -> str:
    """Locate the native library, preferring the package directory."""
    package_dir = os.path.dirname(os.path.abspath(__file__))
    override = os.environ.get("GEOPACK_LIBRARY")
    candidates = [override] if override else []
    candidates += list(_LIBRARY_NAMES.get(sys.platform, ())) + list(_FALLBACK_NAMES)

    for name in candidates:
        if not name:
            continue
        path = os.path.join(package_dir, name)
        if os.path.exists(path):
            return path
        if os.path.isabs(name) and os.path.exists(name):
            return name

    raise ImportError(
        "Native geopack library not found. Build it with python/build_native.sh "
        f"and place it next to this module. Tried: {', '.join(candidates)}"
    )


_lib = ctypes.CDLL(_find_library_path())

# ---------------------------------------------------------------------------
# Prototypes
# ---------------------------------------------------------------------------

_c_handle = ctypes.c_int64
_c_double = ctypes.c_double
_c_int = ctypes.c_int
_p_handle = ctypes.POINTER(ctypes.c_int64)
_p_double = ctypes.POINTER(ctypes.c_double)
_p_byte = ctypes.POINTER(ctypes.c_byte)

_lib.gp_context_create.argtypes = [
    _c_int, _c_int, _c_int, _c_int, _c_int, _c_int,  # y mo d h mi s
    _c_double, _c_double, _c_double,                  # vx vy vz
    _p_handle,                                        # out handle
]
_lib.gp_context_create.restype = _c_int

_lib.gp_context_release.argtypes = [_c_handle]
_lib.gp_context_release.restype = None

_lib.gp_last_error.argtypes = [_p_byte, _c_int]
_lib.gp_last_error.restype = _c_int

# Field models + transforms: (i64 ctx, double x, y, z, double* o1, o2, o3)
_TRANSFORM_ARGTYPES = [
    _c_handle,
    _c_double, _c_double, _c_double,
    _p_double, _p_double, _p_double,
]

_lib.gp_igrf_gsw.argtypes = _TRANSFORM_ARGTYPES
_lib.gp_igrf_gsw.restype = _c_int
_lib.gp_dip.argtypes = _TRANSFORM_ARGTYPES
_lib.gp_dip.restype = _c_int

_lib.gp_igrf_geo.argtypes = [
    _c_handle,
    _c_double, _c_double, _c_double,  # r theta phi
    _p_double, _p_double, _p_double,  # out br bt bp
]
_lib.gp_igrf_geo.restype = _c_int

_lib.gp_sun.argtypes = [
    _c_int, _c_int, _c_int, _c_int, _c_int, _c_int,  # y mo d h mi s
    _p_double, _p_double, _p_double, _p_double,      # out gst slong srasn sdec
]
_lib.gp_sun.restype = _c_int

for _name in (
    "gp_gsw_to_gse",
    "gp_gse_to_gsw",
    "gp_geo_to_mag",
    "gp_mag_to_geo",
    "gp_gei_to_geo",
    "gp_geo_to_gei",
    "gp_mag_to_sm",
    "gp_sm_to_mag",
    "gp_sm_to_gsw",
    "gp_gsw_to_sm",
    "gp_geo_to_gsw",
    "gp_gsw_to_geo",
):
    getattr(_lib, _name).argtypes = _TRANSFORM_ARGTYPES
    getattr(_lib, _name).restype = _c_int

_lib.gp_shu_mgnp.argtypes = [
    _c_double, _c_double, _c_double,  # xnPd vel bzImf
    _c_double, _c_double, _c_double,  # x y z
    _p_double, _p_double, _p_double,  # out mx my mz
    _p_double,                        # out dist
    _p_int := ctypes.POINTER(ctypes.c_int),
]
_lib.gp_shu_mgnp.restype = _c_int

_lib.gp_t96_mgnp.argtypes = [
    _c_double, _c_double,             # xnPd vel
    _c_double, _c_double, _c_double,  # x y z
    _p_double, _p_double, _p_double,  # out mx my mz
    _p_double,                        # out dist
    ctypes.POINTER(ctypes.c_int),
]
_lib.gp_t96_mgnp.restype = _c_int

# External field models (context-free, input GSW, output GSM)
_lib.gp_t89.argtypes = [
    _c_int, _c_double, _c_double, _c_double, _c_double,  # iopt psi x y z
    _p_double, _p_double, _p_double,                     # out bx by bz
]
_lib.gp_t89.restype = _c_int

# Context dipole tilt (radians), computed by Recalc
_lib.gp_context_psi.argtypes = [_c_handle, _p_double]
_lib.gp_context_psi.restype = _c_int

del _name  # loop variable cleanup

# ---------------------------------------------------------------------------
# Error plumbing
# ---------------------------------------------------------------------------


def _last_error() -> Optional[str]:
    """Read and clear the thread-local error message set by the native side."""
    # Probe: null buffer + zero capacity returns the UTF-8 byte length without
    # clearing the stored message.
    size = _lib.gp_last_error(None, 0)
    if size <= 0:
        return None
    buf = ctypes.create_string_buffer(size + 1)
    _lib.gp_last_error(ctypes.cast(buf, _p_byte), size + 1)
    return buf.value.decode("utf-8", errors="replace")


def _check(rc: int) -> None:
    if rc != 0:
        raise GeopackError(_last_error() or f"native geopack error (code {rc})")


# ---------------------------------------------------------------------------
# Raw wrappers
# ---------------------------------------------------------------------------


def context_create(
    year: int, month: int, day: int, hour: int, minute: int, second: int,
    vx: float, vy: float, vz: float,
) -> int:
    handle = ctypes.c_int64()
    rc = _lib.gp_context_create(
        year, month, day, hour, minute, second, vx, vy, vz, ctypes.byref(handle)
    )
    _check(rc)
    return handle.value


def context_release(handle: int) -> None:
    _lib.gp_context_release(handle)


def igrf_gsw(handle: int, x: float, y: float, z: float) -> Tuple[float, float, float]:
    return _call_vector(_lib.gp_igrf_gsw, handle, x, y, z)


def dip(handle: int, x: float, y: float, z: float) -> Tuple[float, float, float]:
    return _call_vector(_lib.gp_dip, handle, x, y, z)


def igrf_geo(handle: int, r: float, theta: float, phi: float) -> Tuple[float, float, float]:
    bx, bt, bp = ctypes.c_double(), ctypes.c_double(), ctypes.c_double()
    rc = _lib.gp_igrf_geo(
        handle, r, theta, phi, ctypes.byref(bx), ctypes.byref(bt), ctypes.byref(bp)
    )
    _check(rc)
    return bx.value, bt.value, bp.value


def sun(
    year: int, month: int, day: int, hour: int, minute: int, second: int,
) -> Tuple[float, float, float, float]:
    gst, slong, srasn, sdec = (ctypes.c_double() for _ in range(4))
    rc = _lib.gp_sun(
        year, month, day, hour, minute, second,
        ctypes.byref(gst), ctypes.byref(slong), ctypes.byref(srasn), ctypes.byref(sdec),
    )
    _check(rc)
    return gst.value, slong.value, srasn.value, sdec.value


def _call_vector(fn, handle: int, x: float, y: float, z: float) -> Tuple[float, float, float]:
    ox, oy, oz = ctypes.c_double(), ctypes.c_double(), ctypes.c_double()
    rc = fn(handle, x, y, z, ctypes.byref(ox), ctypes.byref(oy), ctypes.byref(oz))
    _check(rc)
    return ox.value, oy.value, oz.value


# Coordinate transforms: (long ctx, double x, y, z) -> (ox, oy, oz)
def gsw_to_gse(handle: int, x: float, y: float, z: float) -> Tuple[float, float, float]:
    return _call_vector(_lib.gp_gsw_to_gse, handle, x, y, z)


def gse_to_gsw(handle: int, x: float, y: float, z: float) -> Tuple[float, float, float]:
    return _call_vector(_lib.gp_gse_to_gsw, handle, x, y, z)


def geo_to_mag(handle: int, x: float, y: float, z: float) -> Tuple[float, float, float]:
    return _call_vector(_lib.gp_geo_to_mag, handle, x, y, z)


def mag_to_geo(handle: int, x: float, y: float, z: float) -> Tuple[float, float, float]:
    return _call_vector(_lib.gp_mag_to_geo, handle, x, y, z)


def gei_to_geo(handle: int, x: float, y: float, z: float) -> Tuple[float, float, float]:
    return _call_vector(_lib.gp_gei_to_geo, handle, x, y, z)


def geo_to_gei(handle: int, x: float, y: float, z: float) -> Tuple[float, float, float]:
    return _call_vector(_lib.gp_geo_to_gei, handle, x, y, z)


def mag_to_sm(handle: int, x: float, y: float, z: float) -> Tuple[float, float, float]:
    return _call_vector(_lib.gp_mag_to_sm, handle, x, y, z)


def sm_to_mag(handle: int, x: float, y: float, z: float) -> Tuple[float, float, float]:
    return _call_vector(_lib.gp_sm_to_mag, handle, x, y, z)


def sm_to_gsw(handle: int, x: float, y: float, z: float) -> Tuple[float, float, float]:
    return _call_vector(_lib.gp_sm_to_gsw, handle, x, y, z)


def gsw_to_sm(handle: int, x: float, y: float, z: float) -> Tuple[float, float, float]:
    return _call_vector(_lib.gp_gsw_to_sm, handle, x, y, z)


def geo_to_gsw(handle: int, x: float, y: float, z: float) -> Tuple[float, float, float]:
    return _call_vector(_lib.gp_geo_to_gsw, handle, x, y, z)


def gsw_to_geo(handle: int, x: float, y: float, z: float) -> Tuple[float, float, float]:
    return _call_vector(_lib.gp_gsw_to_geo, handle, x, y, z)


def shu_mgnp(
    xn_pd: float, vel: float, bz_imf: float, x: float, y: float, z: float,
) -> Tuple[float, float, float, float, int]:
    return _call_mgnp(_lib.gp_shu_mgnp, xn_pd, vel, bz_imf, x, y, z)


def t96_mgnp(xn_pd: float, vel: float, x: float, y: float, z: float) -> Tuple[float, float, float, float, int]:
    return _call_mgnp(_lib.gp_t96_mgnp, xn_pd, vel, x, y, z)


def _call_mgnp(fn, *args: float) -> Tuple[float, float, float, float, int]:
    mx, my, mz, dist = (ctypes.c_double() for _ in range(4))
    position = ctypes.c_int()
    rc = fn(
        *args,
        ctypes.byref(mx), ctypes.byref(my), ctypes.byref(mz),
        ctypes.byref(dist), ctypes.byref(position),
    )
    _check(rc)
    return mx.value, my.value, mz.value, dist.value, position.value


def t89(iopt: int, psi: float, x: float, y: float, z: float) -> Tuple[float, float, float]:
    """Tsyganenko (1989) external field at (x, y, z) in GSW, GSM components in nT."""
    bx, by, bz = ctypes.c_double(), ctypes.c_double(), ctypes.c_double()
    rc = _lib.gp_t89(iopt, psi, x, y, z, ctypes.byref(bx), ctypes.byref(by), ctypes.byref(bz))
    _check(rc)
    return bx.value, by.value, bz.value


def context_psi(handle: int) -> float:
    """Dipole tilt (radians) for the given context, as computed by Recalc."""
    psi = ctypes.c_double()
    rc = _lib.gp_context_psi(handle, ctypes.byref(psi))
    _check(rc)
    return psi.value
