"""Parity tests for the coordinate helper conversions.

Mirrors the C# suites:

* ``UnitTests/Geopack/GepackTests.ToCartesian.cs`` (``SPHCAR_08``)
* ``UnitTests/Geopack/GepackTests.ToSpherical.cs`` (``CARSPH_08``)
* ``UnitTests/Geopack/GeopackTests.ToSphericalVector.cs`` (``BCARSP_08``)
* ``UnitTests/Geopack/GeopackTests.ToCartesianVector.cs`` (``BSPCAR_08``)
* ``UnitTests/Geopack/GepackTests.GeodGeo.cs`` (``GEODGEO_08``)
* ``UnitTests/Geopack/TestData/BSpCarDataSet.dat``, ``GeodGeo.dat``,
  ``GeoGeod.dat``

The same absolute tolerance as the C# tests is used::

    ShouldBe(expected, 0.000000000008)   // MinimalTestsPrecision = 8e-12

Run standalone:  python3 python/tests/test_helpers.py
or with pytest:  pytest python/tests/test_helpers.py
"""

from __future__ import annotations

import math
import os
import sys
from pathlib import Path

sys.path.insert(0, os.path.join(os.path.dirname(__file__), ".."))

import geopack  # noqa: E402

# ---------------------------------------------------------------------------
# Shared constants
# ---------------------------------------------------------------------------

TOL = 0.000000000008  # MinimalTestsPrecision in TestExtensions.cs

_TESTDATA_DIR = Path(__file__).resolve().parents[2] / "UnitTests" / "Geopack" / "TestData"


def _assert_approx(actual, expected, label="", tol=TOL):
    assert abs(actual - expected) <= tol, (
        f"{label}: got {actual!r}, expected {expected!r}, "
        f"diff {abs(actual - expected)!r} > {tol!r}"
    )


def _assert_triple(actual, expected, label, tol=TOL):
    for k, axis in enumerate("xyz"):
        _assert_approx(actual[k], expected[k], f"{label}.{axis}", tol)


def _read_dat(name):
    path = _TESTDATA_DIR / name
    if not path.exists():
        raise AssertionError(
            f"reference data not found: {path}. Run this test from the repository root."
        )
    return path.read_text().splitlines()


# ---------------------------------------------------------------------------
# SphCar (SPHCAR_08): spherical location -> Cartesian location
# ---------------------------------------------------------------------------

# GepackTests.ToCartesian.cs: SphCar_Variances_ReturnCorrectValues
_SPH_CAR_CASES = [
    (0.0, 0.0, 0.0, 0.0, 0.0, 0.0),
    (1.0, 0.0, 0.0, 0.0, 0.0, 1.0),
    (0.0, 1.0, 0.0, 0.0, 0.0, 0.0),
    (0.0, 0.0, 1.0, 0.0, 0.0, 0.0),
    (1.0, 1.0, 1.0, 0.45464871341284091, 0.70807341827357118, 0.54030230586813977),
    (-1.0, 0.0, 0.0, 0.0, 0.0, -1.0),
    (0.0, -1.0, 0.0, 0.0, 0.0, 0.0),
    (0.0, 0.0, -1.0, 0.0, 0.0, 0.0),
    (-1.0, -1.0, -1.0, 0.45464871341284091, -0.70807341827357118, -0.54030230586813977),
]


def test_location_to_cartesian_cases():
    for i, (r, theta, phi, ex, ey, ez) in enumerate(_SPH_CAR_CASES):
        result = geopack.location_to_cartesian(r, theta, phi)
        _assert_triple(result, (ex, ey, ez), f"sph_car[{i}]")


def test_location_to_cartesian_reference():
    # GepackTests.ToCartesian.cs: SphCar_ShouldReturnCorrectValues
    result = geopack.location_to_cartesian(1.02, 1.0, 1.0)
    _assert_triple(result, (0.4637416876811, 0.7222348866390, 0.5511083519855), "sph_car_ref")


# ---------------------------------------------------------------------------
# CarSph (CARSPH_08): Cartesian location -> spherical location
# ---------------------------------------------------------------------------

# GepackTests.ToSpherical.cs: CarSph_ZeroesAndOnes_ReturnsCorrectValues
_CAR_SPH_CASES = [
    (0.0, 0.0, 0.0, 0.0, 0.0, 0.0),
    (1.0, 0.0, 0.0, 1.0, 1.57079632679489656, 0.0),
    (0.0, 1.0, 0.0, 1.0, 1.57079632679489656, 1.57079632679489656),
    (0.0, 0.0, 1.0, 1.0, 0.0, 0.0),
    (1.0, 1.0, 1.0, 1.73205080756887719, 0.95531661812450930, 0.78539816339744828),
    (-1.0, 0.0, 0.0, 1.0, 1.57079632679489656, 3.14159265358979312),
    (0.0, -1.0, 0.0, 1.0, 1.57079632679489656, 4.71238898020510355),
    (0.0, 0.0, -1.0, 1.0, 3.14159265400000010, 0.0),
    (-1.0, -1.0, -1.0, 1.73205080756887719, 2.18627603546528393, 3.92699081680765527),
]


def test_location_to_spherical_cases():
    for i, (x, y, z, er, et, ep) in enumerate(_CAR_SPH_CASES):
        result = geopack.location_to_spherical(x, y, z)
        _assert_approx(result.r, er, f"car_sph[{i}].r")
        _assert_approx(result.theta, et, f"car_sph[{i}].theta")
        _assert_approx(result.phi, ep, f"car_sph[{i}].phi")


def test_location_to_spherical_reference():
    # GepackTests.ToSpherical.cs: CarSph_ShouldReturnCorrectValues
    result = geopack.location_to_spherical(1.0, 1.0, 1.0)
    _assert_approx(result.r, 1.7320508075689, "car_sph_ref.r")
    _assert_approx(result.theta, 0.9553166181245, "car_sph_ref.theta")
    _assert_approx(result.phi, 0.7853981633974, "car_sph_ref.phi")


# ---------------------------------------------------------------------------
# BCarSph (BCARSP_08): Cartesian field at a Cartesian location -> spherical
# ---------------------------------------------------------------------------

# GeopackTests.ToSphericalVector.cs: (x, y, z, bx, by, bz, br, btheta, bphi)
_TO_SPHERICAL_VECTOR_CASES = [
    (-1.2, 1, 1.5, 1, 11, -21, -10.020128994909539344, 19.492502934797215630, -9.090618475235615392),
    (1, 0, 0, 1, 1, 1, 1, -1, 1),
    (0, 1, 0, 1, 1, 1, 1, -1, -1),
    (0, 0, 1, 1, 1, 1, 1, 1, 1),
    (1, 1, 1, 1, 1, 1, 1.7320508075688776, 0, 0),
    (1, 1, 1, 1, 0, 0, 0.577350269189625842, 0.408248290463863017, -0.707106781186547462),
    (1, 1, 1, 0, 1, 0, 0.577350269189625842, 0.408248290463863017, 0.707106781186547462),
    (1, 1, 1, 0, 0, 1, 0.577350269189625842, -0.816496580927726145, 0),
    (1, 1, 1, 0, 0, 0, 0, 0, 0),
]


def test_vector_to_spherical_cases():
    for i, (x, y, z, bx, by, bz, ebr, ebt, ebp) in enumerate(_TO_SPHERICAL_VECTOR_CASES):
        result = geopack.vector_to_spherical(bx, by, bz, x, y, z)
        _assert_approx(result.r, ebr, f"bcarsph[{i}].r")
        _assert_approx(result.theta, ebt, f"bcarsph[{i}].theta")
        _assert_approx(result.phi, ebp, f"bcarsph[{i}].phi")


def test_vector_to_spherical_zero_radius_raises():
    # GeopackTests.ToSphericalVector.cs: ToSphericalVector_ShouldReturnNaND_IfDivideByZero
    try:
        geopack.vector_to_spherical(1.0, 1.0, 1.0, 0.0, 0.0, 0.0)
    except geopack.GeopackError:
        pass
    else:
        raise AssertionError("expected GeopackError for a zero-radius location")


# ---------------------------------------------------------------------------
# BSphCar (BSPCAR_08): spherical field at a spherical location -> Cartesian
# ---------------------------------------------------------------------------

# GeopackTests.ToCartesianVector.cs: ((r, theta, phi), (br, btheta, bphi), (x, y, z))
_TO_CARTESIAN_VECTOR_CASES = [
    ((0.0, 0.0, 0.0), (1.0, 1.0, 1.0), (1.0, 1.0, 1.0)),
    (
        (0.0, -math.pi / 4, -math.pi / 4),
        (1.0, 1.0, 1.0),
        (0.70710678118654757, 0.70710678118654746, 1.4142135623730949),
    ),
    ((0.0, 2 * math.pi, 2 * math.pi), (1.0, 1.0, 1.0), (1.0, 1.0, 1.0)),
    ((0.0, 1e-10, 1e-10), (1.0, 1.0, 1.0), (1.0, 1.0000000001, 0.99999999989999999)),
]


def test_vector_to_cartesian_cases():
    for i, (location, field, expected) in enumerate(_TO_CARTESIAN_VECTOR_CASES):
        r, theta, phi = location
        br, btheta, bphi = field
        result = geopack.vector_to_cartesian(br, btheta, bphi, r, theta, phi)
        _assert_triple(result, expected, f"bspcar[{i}]")


def test_vector_to_cartesian_dataset():
    # GeopackTests.ToCartesianVector.cs: ToCartesian_ReturnsCorrectValues
    for i, line in enumerate(_read_dat("BSpCarDataSet.dat")):
        if not line.strip():
            continue
        tokens = line.split()
        theta = float(tokens[1])
        phi = float(tokens[3])
        br = float(tokens[5])
        btheta = float(tokens[7])
        bphi = float(tokens[9])
        expected = (float(tokens[11]), float(tokens[13]), float(tokens[15]))
        result = geopack.vector_to_cartesian(br, btheta, bphi, 1.0, theta, phi)
        _assert_triple(result, expected, f"bspcar_dataset[{i}]")


# ---------------------------------------------------------------------------
# GEODGEO_08: geodetic <-> geocentric
# ---------------------------------------------------------------------------


def test_geodetic_to_geocentric_dataset():
    # GepackTests.GeodGeo.cs: ToPolar_ReturnsCorrectValues
    # Line: H=<altitude> XMU=<latitude> RR=<r> THETAR=<theta>
    for i, line in enumerate(_read_dat("GeodGeo.dat")):
        if not line.strip():
            continue
        tokens = line.split()
        altitude = float(tokens[1])
        latitude = float(tokens[3])
        expected_r = float(tokens[5])
        expected_theta = float(tokens[7])

        result = geopack.geodetic_to_geocentric(latitude, altitude)
        _assert_approx(result.r, expected_r, f"geod_geo[{i}].r")
        _assert_approx(result.theta, expected_theta, f"geod_geo[{i}].theta")


def test_geocentric_to_geodetic_dataset():
    # GepackTests.GeodGeo.cs: ToGeodetic_ReturnsCorrectValues
    # Line: H=<r> XMU=<theta> RR=<altitude> THETAR=<latitude>
    for i, line in enumerate(_read_dat("GeoGeod.dat")):
        if not line.strip():
            continue
        tokens = line.split()
        r = float(tokens[1])
        theta = float(tokens[3])
        expected_altitude = float(tokens[5])
        expected_latitude = float(tokens[7])

        result = geopack.geocentric_to_geodetic(r, theta)
        _assert_approx(result.latitude, expected_latitude, f"geo_geod[{i}].latitude")
        _assert_approx(result.altitude, expected_altitude, f"geo_geod[{i}].altitude")


# ---------------------------------------------------------------------------
# Public result types and round trips
# ---------------------------------------------------------------------------


def test_igrf_geo_returns_spherical_vector():
    with geopack.recalc(1997, 12, 16, 21, 0, 0, vx=-304.0, vy=13.0, vz=4.0) as ctx:
        field = ctx.igrf_geo(1.02, 1.0, 2.0)

    assert isinstance(field, geopack.SphericalVector3)
    assert field.r == field[0]
    assert field.theta == field[1]
    assert field.phi == field[2]
    assert all(math.isfinite(value) for value in field)


def test_location_round_trip():
    for point in ((0.7, 1.2, -0.4), (1.0, 1.0, 1.0), (-3.0, 2.0, 0.5)):
        spherical = geopack.location_to_spherical(*point)
        back = geopack.location_to_cartesian(*spherical)
        _assert_triple(back, point, f"round_trip[{point}]")


# ---------------------------------------------------------------------------
# Standalone runner
# ---------------------------------------------------------------------------


def _main() -> int:
    failures = 0
    total = 0
    for name, fn in sorted(globals().items()):
        if name.startswith("test_") and callable(fn) and name != "test_":
            total += 1
            try:
                fn()
                print(f"PASS {name}")
            except Exception as exc:  # noqa: BLE001
                failures += 1
                print(f"FAIL {name}: {exc!r}")
    print(f"\n{total - failures}/{total} passed")
    return 1 if failures else 0


if __name__ == "__main__":
    sys.exit(_main())
