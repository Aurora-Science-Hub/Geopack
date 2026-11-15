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
from typing import NamedTuple, Optional, Sequence, Tuple, Union

from . import _native
from ._enums import (
    CoordinateSystem,
    ExternalFieldModel,
    InternalFieldModel,
    MagnetopausePosition,
    TraceDirection,
    TraceTermination,
)
from ._native import GeopackError

__all__ = [
    "GeopackError",
    "CoordinateSystem",
    "MagnetopausePosition",
    "TraceDirection",
    "TraceTermination",
    "ExternalFieldModel",
    "InternalFieldModel",
    "Vector3",
    "SphericalVector3",
    "GeocentricCoordinates",
    "GeodeticCoordinates",
    "Sun",
    "MagnetopauseResult",
    "TraceResult",
    "Context",
    "DEFAULT_SOLAR_WIND",
    "recalc",
    "sun",
    "shu_mgnp",
    "t96_mgnp",
    "t89",
    "location_to_spherical",
    "location_to_cartesian",
    "vector_to_spherical",
    "vector_to_cartesian",
    "geodetic_to_geocentric",
    "geocentric_to_geodetic",
    "__version__",
]

__version__ = "2.7.0"

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


class SphericalVector3(NamedTuple):
    """A vector in spherical components.

    For a location the components are ``(r, theta, phi)`` (radius, colatitude,
    longitude); for a field they are ``(Br, Btheta, Bphi)``.

    Attributes:
        r: Radial component (Earth radii for a location, nT for a field).
        theta: Colatitude (radians).
        phi: Longitude (radians).
    """

    r: float
    theta: float
    phi: float


class GeocentricCoordinates(NamedTuple):
    """Geocentric radius and colatitude.

    Attributes:
        r: Geocentric radius (km).
        theta: Colatitude (radians).
    """

    r: float
    theta: float


class GeodeticCoordinates(NamedTuple):
    """Geodetic latitude and altitude above the WGS84 ellipsoid.

    Attributes:
        latitude: Geodetic latitude (radians).
        altitude: Altitude above the ellipsoid (km).
    """

    latitude: float
    altitude: float


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


class TraceResult(NamedTuple):
    """Result of :meth:`Context.trace`.

    Attributes:
        points: Traced field-line points in GSW (Earth radii).
        end_point: Final point (boundary crossing or point-limit position).
        termination_reason: Why the tracing stopped.
    """

    points: Tuple[Vector3, ...]
    end_point: Vector3
    termination_reason: TraceTermination

    @property
    def actual_point_count(self) -> int:
        """Number of traced points (always ``len(points)``)."""
        return len(self.points)


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
        """Release the native computation context. Idempotent.

        Call it only when no other thread is using this context.
        """
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

    def igrf_geo(self, r: float, theta: float, phi: float) -> SphericalVector3:
        """IGRF field in GEO spherical coordinates at radius ``r`` (Re),
        colatitude ``theta`` and longitude ``phi`` (radians).

        Returns the spherical components ``(Br, Btheta, Bphi)`` in nT as a
        :class:`SphericalVector3`.
        """
        self._check_open()
        return SphericalVector3(*_native.igrf_geo(self._handle, r, theta, phi))

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
        Returns the field vector in GSM (nT). The model is thread-safe.
        """
        self._check_open()
        if psi is None:
            psi = self.psi
        return Vector3(*_native.t89(iopt, psi, x, y, z))

    # -- field-line tracing -------------------------------------------------

    def trace(
        self,
        x: float,
        y: float,
        z: float,
        direction: Union[TraceDirection, int] = TraceDirection.AntiParallel,
        ds_max: float = 0.1,
        err: float = 0.0001,
        r_lim: float = 60.0,
        r0: float = 1.0,
        iopt: int = 1,
        parmod: Optional[Sequence[float]] = None,
        ex_model: Union[ExternalFieldModel, int] = ExternalFieldModel.T89,
        in_model: Union[InternalFieldModel, int] = InternalFieldModel.IgrfGsw,
        l_max: int = 500,
    ) -> TraceResult:
        """Trace a field line starting at ``(x, y, z)`` in GSW (Earth radii).

        ``direction`` selects tracing antiparallel (north to south) or parallel
        to the total field. ``ds_max``, ``err``, ``r_lim``, ``r0``, ``iopt`` and
        ``parmod`` have the same meaning as in the C# ``Trace`` method;
        ``parmod`` is a 10-element sequence (``None`` means all zeros).
        ``ex_model`` selects the external field model (only T89 is available)
        and ``in_model`` the internal one (IGRF in GSW, or DIP). ``l_max`` is
        the maximum number of traced points (at most 1,000,000; the output
        buffer is ``l_max * 3`` doubles, 8 bytes each).
        """
        self._check_open()
        points, end_point, termination = _native.trace(
            self._handle,
            x, y, z, int(direction),
            ds_max, err, r_lim, r0, iopt,
            parmod,
            int(ex_model), int(in_model), l_max,
        )
        return TraceResult(
            tuple(Vector3(*point) for point in points),
            Vector3(*end_point),
            TraceTermination(termination),
        )

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


# ---------------------------------------------------------------------------
# Coordinate conversion helpers (context-free, coordinate-system agnostic)
# ---------------------------------------------------------------------------


def location_to_spherical(x: float, y: float, z: float) -> SphericalVector3:
    """Convert Cartesian location components to ``(r, theta, phi)`` (CARSPH_08)."""
    return SphericalVector3(*_native.location_to_spherical(x, y, z))


def location_to_cartesian(r: float, theta: float, phi: float) -> Vector3:
    """Convert spherical location components to Cartesian ``(x, y, z)`` (SPHCAR_08)."""
    return Vector3(*_native.location_to_cartesian(r, theta, phi))


def vector_to_spherical(
    bx: float,
    by: float,
    bz: float,
    x: float,
    y: float,
    z: float,
) -> SphericalVector3:
    """Convert a Cartesian field at ``(x, y, z)`` to ``(Br, Btheta, Bphi)`` (BCARSP_08)."""
    return SphericalVector3(*_native.vector_to_spherical(bx, by, bz, x, y, z))


def vector_to_cartesian(
    br: float,
    btheta: float,
    bphi: float,
    r: float,
    theta: float,
    phi: float,
) -> Vector3:
    """Convert ``(Br, Btheta, Bphi)`` at ``(r, theta, phi)`` to Cartesian (BSPCAR_08)."""
    return Vector3(*_native.vector_to_cartesian(br, btheta, bphi, r, theta, phi))


def geodetic_to_geocentric(
    latitude: float, altitude: float
) -> GeocentricCoordinates:
    """Convert geodetic latitude (radians) and altitude (km) to geocentric (GEODGEO_08)."""
    return GeocentricCoordinates(*_native.geodetic_to_geocentric(latitude, altitude))


def geocentric_to_geodetic(r: float, theta: float) -> GeodeticCoordinates:
    """Convert geocentric radius (km) and colatitude (radians) to geodetic (GEODGEO_08)."""
    return GeodeticCoordinates(*_native.geocentric_to_geodetic(r, theta))
