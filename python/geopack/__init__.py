"""Geopack-2008 for Python.

A Pythonic wrapper around the NativeAOT-built ``geopack`` shared library
(magnetic field of the Earth, solar-terrestrial coordinate transforms). No .NET
runtime is required at runtime.

Typical usage::

    import geopack

    ctx = geopack.recalc(1997, 12, 16, 21, 0, 0, vx=-304, vy=13.78, vz=4)
    b = ctx.igrf_gsw(1, 1, 1)          # (bx, by, bz) nT, GSW
    r = ctx.gsw_to_geo(1, 1, 1)        # (x, y, z) Earth radii, GEO
    ctx.close()
"""

from __future__ import annotations

from datetime import datetime
from typing import NamedTuple, Optional, Sequence, Union

from . import _native
from ._enums import CoordinateSystem, MagnetopausePosition
from ._native import GeopackError

__all__ = [
    "GeopackError",
    "CoordinateSystem",
    "MagnetopausePosition",
    "Vector3",
    "Sun",
    "MagnetopauseResult",
    "Context",
    "DEFAULT_SOLAR_WIND",
    "recalc",
    "sun",
    "shu_mgnp",
    "t96_mgnp",
    "t89",
    "__version__",
]

__version__ = "2.2.1"

#: Default solar wind velocity (GSE, km/s) used when the caller does not provide one.
DEFAULT_SOLAR_WIND = (-400.0, 0.0, 0.0)


class Vector3(NamedTuple):
    """A Cartesian vector in Earth radii (positions) or nT (fields).
    
    Attributes:
        x: X component.
        y: Y component.
        z: Z component.
    """

    x: float
    y: float
    z: float


class Sun(NamedTuple):
    """Position of the Sun for a given UTC date/time.
    
    Attributes:
        gst: Greenwich mean sidereal time in radians.
        slong: Ecliptic longitude of the Sun in radians.
        srasn: Right ascension of the Sun in radians.
        sdec: Declination of the Sun in radians.
    """

    gst: float
    slong: float
    srasn: float
    sdec: float


class MagnetopauseResult(NamedTuple):
    """Result of a magnetopause model evaluation.
    
    Attributes:
        boundary: Nearest boundary point in GSW (Earth radii).
        dist: Distance from the input point to the boundary (Earth radii).
        position: Relative position of the point (Inside, Outside, NotDefined).
    """

    boundary: Vector3
    dist: float
    position: MagnetopausePosition


class Context:
    """A frozen computation context for a single UTC date/time.

    Created by :func:`recalc`. All field and transform methods take coordinates
    in the documented system and return plain ``(x, y, z)`` tuples. The context
    is immutable on the native side and safe to share between threads.
    """

    __slots__ = ("_handle", "_closed")

    def __init__(self, handle: int):
        self._handle = handle
        self._closed = False

    # -- lifecycle ----------------------------------------------------------

    def close(self) -> None:
        """Release the native computation context. Idempotent."""
        if not self._closed:
            _native.context_release(self._handle)
            self._closed = True

    def __del__(self) -> None:  # pragma: no cover - interpreter shutdown
        try:
            self.close()
        except Exception:
            pass

    def __enter__(self) -> "Context":
        return self

    def __exit__(self, *exc) -> None:
        self.close()

    def _check_open(self) -> None:
        if self._closed:
            raise RuntimeError("Geopack context has been closed")

    # -- field models -------------------------------------------------------

    def igrf_gsw(self, x: float, y: float, z: float) -> Vector3:
        """IGRF magnetic field at ``(x, y, z)`` in GSW, returned in nT."""
        self._check_open()
        return Vector3(*_native.igrf_gsw(self._handle, x, y, z))

    def dip(self, x: float, y: float, z: float) -> Vector3:
        """Dipole magnetic field at ``(x, y, z)`` in GSW, returned in nT."""
        self._check_open()
        return Vector3(*_native.dip(self._handle, x, y, z))

    def igrf_geo(self, r: float, theta: float, phi: float) -> Vector3:
        """IGRF field in GEO spherical coordinates at radius ``r`` (Re),
        colatitude ``theta`` and longitude ``phi`` (radians).

        Returns the spherical components ``(Br, Btheta, Bphi)`` in nT.
        """
        self._check_open()
        return Vector3(*_native.igrf_geo(self._handle, r, theta, phi))

    # -- external field models ----------------------------------------------

    @property
    def psi(self) -> float:
        """Dipole tilt angle (radians) for this context's date/time, as computed by Recalc."""
        self._check_open()
        return _native.context_psi(self._handle)

    def t89(self, iopt: int, x: float, y: float, z: float, psi: Optional[float] = None) -> Vector3:
        """Tsyganenko (1989) external field at ``(x, y, z)`` in GSW.

        ``iopt`` is the disturbance-level index (1..7, maps to Kp intervals).
        ``psi`` is the dipole tilt in radians and defaults to this context's tilt.
        Returns the field vector in GSM (nT). Calls are serialized natively
        because the model keeps static state (not thread-safe).
        """
        self._check_open()
        if psi is None:
            psi = self.psi
        return Vector3(*_native.t89(iopt, psi, x, y, z))

    # -- coordinate transforms (in -> out, Earth radii) ---------------------

    def gsw_to_gse(self, x: float, y: float, z: float) -> Vector3:
        """Transform Cartesian (x, y, z) from GSW to GSE."""
        return self._transform(_native.gsw_to_gse, x, y, z)

    def gse_to_gsw(self, x: float, y: float, z: float) -> Vector3:
        """Transform Cartesian (x, y, z) from GSE to GSW."""
        return self._transform(_native.gse_to_gsw, x, y, z)

    def geo_to_mag(self, x: float, y: float, z: float) -> Vector3:
        """Transform Cartesian (x, y, z) from GEO to MAG."""
        return self._transform(_native.geo_to_mag, x, y, z)

    def mag_to_geo(self, x: float, y: float, z: float) -> Vector3:
        """Transform Cartesian (x, y, z) from MAG to GEO."""
        return self._transform(_native.mag_to_geo, x, y, z)

    def gei_to_geo(self, x: float, y: float, z: float) -> Vector3:
        """Transform Cartesian (x, y, z) from GEI to GEO."""
        return self._transform(_native.gei_to_geo, x, y, z)

    def geo_to_gei(self, x: float, y: float, z: float) -> Vector3:
        """Transform Cartesian (x, y, z) from GEO to GEI."""
        return self._transform(_native.geo_to_gei, x, y, z)

    def mag_to_sm(self, x: float, y: float, z: float) -> Vector3:
        """Transform Cartesian (x, y, z) from MAG to SM."""
        return self._transform(_native.mag_to_sm, x, y, z)

    def sm_to_mag(self, x: float, y: float, z: float) -> Vector3:
        """Transform Cartesian (x, y, z) from SM to MAG."""
        return self._transform(_native.sm_to_mag, x, y, z)

    def sm_to_gsw(self, x: float, y: float, z: float) -> Vector3:
        """Transform Cartesian (x, y, z) from SM to GSW."""
        return self._transform(_native.sm_to_gsw, x, y, z)

    def gsw_to_sm(self, x: float, y: float, z: float) -> Vector3:
        """Transform Cartesian (x, y, z) from GSW to SM."""
        return self._transform(_native.gsw_to_sm, x, y, z)

    def geo_to_gsw(self, x: float, y: float, z: float) -> Vector3:
        """Transform Cartesian (x, y, z) from GEO to GSW."""
        return self._transform(_native.geo_to_gsw, x, y, z)

    def gsw_to_geo(self, x: float, y: float, z: float) -> Vector3:
        """Transform Cartesian (x, y, z) from GSW to GEO."""
        return self._transform(_native.gsw_to_geo, x, y, z)

    def _transform(self, fn, x: float, y: float, z: float) -> Vector3:
        self._check_open()
        return Vector3(*fn(self._handle, x, y, z))


def recalc(
    date_or_year: Union[datetime, int],
    month: Optional[int] = None,
    day: Optional[int] = None,
    hour: int = 0,
    minute: int = 0,
    second: int = 0,
    vx: float = DEFAULT_SOLAR_WIND[0],
    vy: float = DEFAULT_SOLAR_WIND[1],
    vz: float = DEFAULT_SOLAR_WIND[2],
) -> Context:
    """Create a :class:`Context` for a UTC date/time and solar wind velocity.

    ``date_or_year`` is either a ``datetime.datetime`` (then ``month``/``day``
    must be omitted) or a bare year. The solar wind velocity defaults to
    ``(-400, 0, 0)`` km/s (GSE) as in the original GEOPACK-2008.
    """
    if isinstance(date_or_year, datetime):
        if month is not None or day is not None:
            raise ValueError("pass either a datetime or (year, month, day), not both")
        dt = date_or_year
        year, month, day = dt.year, dt.month, dt.day
        hour, minute, second = dt.hour, dt.minute, dt.second
    else:
        if month is None or day is None:
            raise ValueError("month and day are required when a year is given")
        year = date_or_year

    handle = _native.context_create(
        year, month, day, hour, minute, second, vx, vy, vz
    )
    return Context(handle)


def sun(date_or_year: Union[datetime, int], month: Optional[int] = None, day: Optional[int] = None,
        hour: int = 0, minute: int = 0, second: int = 0) -> Sun:
    """Position of the Sun for a UTC date/time (see :func:`recalc`)."""
    if isinstance(date_or_year, datetime):
        if month is not None or day is not None:
            raise ValueError("pass either a datetime or (year, month, day), not both")
        dt = date_or_year
        year, month, day = dt.year, dt.month, dt.day
        hour, minute, second = dt.hour, dt.minute, dt.second
    else:
        if month is None or day is None:
            raise ValueError("month and day are required when a year is given")
        year = date_or_year

    return Sun(*_native.sun(year, month, day, hour, minute, second))


def shu_mgnp(
    xn_pd: float, vel: float, bz_imf: float, x: float, y: float, z: float,
) -> MagnetopauseResult:
    """Shue et al. (1998) magnetopause model. Input in GSW (Re).

    ``xn_pd`` is either the solar wind proton number density (per cm3) if
    ``vel > 0``, or the solar wind ram pressure in nPa if ``vel < 0``.
    ``vel`` is the velocity in km/s (a negative value signals that ``xn_pd``
    is a pressure, not a density). ``bz_imf`` is the IMF Bz in nT.
    ``(x, y, z)`` is the probe point in GSW.
    """
    mx, my, mz, dist, position = _native.shu_mgnp(xn_pd, vel, bz_imf, x, y, z)
    return MagnetopauseResult(Vector3(mx, my, mz), dist, MagnetopausePosition(position))


def t96_mgnp(xn_pd: float, vel: float, x: float, y: float, z: float) -> MagnetopauseResult:
    """Tsyganenko 1996 magnetopause model. Input in GSW (Re).

    ``xn_pd`` is either the solar wind proton number density (per cm3) if
    ``vel > 0``, or the solar wind ram pressure in nPa if ``vel < 0``.
    ``vel`` is the velocity in km/s (a negative value signals that ``xn_pd``
    is a pressure, not a density). ``(x, y, z)`` is the probe point in GSW.
    """
    mx, my, mz, dist, position = _native.t96_mgnp(xn_pd, vel, x, y, z)
    return MagnetopauseResult(Vector3(mx, my, mz), dist, MagnetopausePosition(position))


def t89(
    iopt: int,
    psi: float,
    x: float,
    y: float,
    z: float,
    parmod: Optional[Sequence[float]] = None,
) -> Vector3:
    """Tsyganenko (1989) external field model. Input in GSW (Re).

    ``iopt`` is the disturbance-level index (1..7, maps to Kp intervals).
    ``psi`` is the dipole tilt angle in radians.
    ``parmod`` is a dummy parameter array accepted for parity with the .NET
    ``IT89.Calculate`` contract; it is not used by T89 and defaults to ``None``.
    ``(x, y, z)`` is the probe point in GSW. Returns the field vector in GSM (nT).
    """
    return Vector3(*_native.t89(iopt, psi, x, y, z))
