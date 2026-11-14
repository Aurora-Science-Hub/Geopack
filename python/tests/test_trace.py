"""Parity tests for field-line tracing, mirroring the C# suite.

Mirrors ``UnitTests/Geopack/GeopackTests.Trace.cs``:

* North -> South with ``VGSE = (-304, 13.78, 4)`` against ``TraceNSResult.dat``;
* South -> North with the default ``VGSE = (-400, 0, 0)`` against
  ``TraceSNResult.dat``;
* the ``lMax`` boundary cases (point limit, boundary at the last allowed
  point, boundary one point past the limit) and non-positive ``lMax``.

The same absolute tolerance as the C# tests is used::

    ShouldBe(expected, 0.000000000008)   // MinimalTestsPrecision = 8e-12

Run standalone:  python3 python/tests/test_trace.py
or with pytest:  pytest python/tests/test_trace.py
"""

from __future__ import annotations

import os
import sys
from pathlib import Path

sys.path.insert(0, os.path.join(os.path.dirname(__file__), ".."))

import geopack  # noqa: E402

# ---------------------------------------------------------------------------
# Shared constants
# ---------------------------------------------------------------------------

TOL = 0.000000000008  # MinimalTestsPrecision in TestExtensions.cs

# InputData.dat: IYEAR=1997 IDAY=350 IHOUR=21 MIN=0 -> 1997-12-16 21:00:00 UTC
YEAR, MONTH, DAY, HOUR, MINUTE, SECOND = 1997, 12, 16, 21, 0, 0

# GeopackTests.Trace.cs: NS uses VGSE = (-304, -16 + 29.78, 4); SN uses the
# Recalc default (-400, 0, 0).
NS_VX, NS_VY, NS_VZ = -304.0, -16.0 + 29.78, 4.0
SN_VX, SN_VY, SN_VZ = -400.0, 0.0, 0.0

_TESTDATA_DIR = Path(__file__).resolve().parents[2] / "UnitTests" / "Geopack" / "TestData"


def _assert_approx(actual, expected, label="", tol=TOL):
    assert abs(actual - expected) <= tol, (
        f"{label}: got {actual!r}, expected {expected!r}, "
        f"diff {abs(actual - expected)!r} > {tol!r}"
    )


def _read_points(name):
    path = _TESTDATA_DIR / name
    if not path.exists():
        raise AssertionError(
            f"reference data not found: {path}. Run this test from the repository root."
        )
    points = []
    for line in path.read_text().splitlines():
        if not line.strip():
            continue
        values = [float(token) for token in line.split()]
        assert len(values) == 3, f"{name}: expected 3 values per line, got {line!r}"
        points.append(tuple(values))
    return points


_NS_POINTS = _read_points("TraceNSResult.dat")
_SN_POINTS = _read_points("TraceSNResult.dat")


def _trace(
    start,
    direction,
    l_max,
    vx=NS_VX,
    vy=NS_VY,
    vz=NS_VZ,
):
    with geopack.recalc(YEAR, MONTH, DAY, HOUR, MINUTE, SECOND, vx=vx, vy=vy, vz=vz) as ctx:
        return ctx.trace(
            start[0],
            start[1],
            start[2],
            direction=direction,
            ds_max=0.1,
            err=0.0001,
            r_lim=60.0,
            r0=1.0,
            iopt=1,
            parmod=[0.0] * 10,
            ex_model=geopack.ExternalFieldModel.T89,
            in_model=geopack.InternalFieldModel.IgrfGsw,
            l_max=l_max,
        )


def _assert_points(actual, expected, start=0, stop=None):
    limit = len(expected) if stop is None else stop
    assert len(actual) >= limit
    for i in range(start, limit):
        for k, axis in enumerate("xyz"):
            _assert_approx(actual[i][k], expected[i][k], f"point[{i}].{axis}")


def _assert_end(result, expected):
    for k, axis in enumerate("xyz"):
        _assert_approx(result.end_point[k], expected[k], f"end.{axis}")


# ---------------------------------------------------------------------------
# Reference traces
# ---------------------------------------------------------------------------


def test_trace_north_to_south():
    result = _trace((-1.02, 0.8, 0.9), geopack.TraceDirection.AntiParallel, 500)

    assert result.actual_point_count == len(_NS_POINTS)
    assert len(result.points) == len(_NS_POINTS)
    assert result.termination_reason is geopack.TraceTermination.BoundaryReached
    _assert_points(result.points, _NS_POINTS)
    _assert_end(result, _NS_POINTS[-1])


def test_trace_south_to_north():
    result = _trace(
        (-1.02, 0.8, -0.9),
        geopack.TraceDirection.Parallel,
        500,
        vx=SN_VX,
        vy=SN_VY,
        vz=SN_VZ,
    )

    assert result.actual_point_count == len(_SN_POINTS)
    assert len(result.points) == len(_SN_POINTS)
    assert result.termination_reason is geopack.TraceTermination.BoundaryReached
    _assert_points(result.points, _SN_POINTS)
    _assert_end(result, _SN_POINTS[-1])


# ---------------------------------------------------------------------------
# lMax boundary behaviour
# ---------------------------------------------------------------------------


def test_trace_point_limit_ends_at_position_after_lmax_steps():
    l_max = 20
    result = _trace((-1.02, 0.8, 0.9), geopack.TraceDirection.AntiParallel, l_max)

    assert result.termination_reason is geopack.TraceTermination.MaximumPointsExceeded
    assert result.actual_point_count == l_max
    assert len(result.points) == l_max

    # The first lMax - 1 points are the ordinary trace; the last point and the
    # end point are the position after lMax steps (reference line lMax).
    _assert_points(result.points, _NS_POINTS, 0, l_max - 1)
    _assert_end(result, _NS_POINTS[l_max])
    for k, axis in enumerate("xyz"):
        _assert_approx(result.points[-1][k], _NS_POINTS[l_max][k], f"last.{axis}")


def test_trace_boundary_at_last_allowed_point():
    l_max = len(_NS_POINTS)
    result = _trace((-1.02, 0.8, 0.9), geopack.TraceDirection.AntiParallel, l_max)

    assert result.termination_reason is geopack.TraceTermination.BoundaryReached
    assert result.actual_point_count == len(_NS_POINTS)
    _assert_points(result.points, _NS_POINTS)


def test_trace_boundary_one_point_past_limit():
    l_max = len(_NS_POINTS) - 1
    result = _trace((-1.02, 0.8, 0.9), geopack.TraceDirection.AntiParallel, l_max)

    assert result.termination_reason is geopack.TraceTermination.MaximumPointsExceeded
    assert result.actual_point_count == l_max
    _assert_points(result.points, _NS_POINTS, 0, l_max - 1)


# ---------------------------------------------------------------------------
# Argument validation and model selection
# ---------------------------------------------------------------------------


def test_trace_non_positive_lmax_raises():
    for l_max in (0, -1, -2):
        try:
            _trace((-1.02, 0.8, 0.9), geopack.TraceDirection.AntiParallel, l_max)
        except ValueError:
            pass
        else:
            raise AssertionError(f"expected ValueError for l_max={l_max}")


def test_trace_parmod_length_raises():
    with geopack.recalc(YEAR, MONTH, DAY, HOUR, MINUTE, SECOND, vx=NS_VX, vy=NS_VY, vz=NS_VZ) as ctx:
        try:
            ctx.trace(-1.02, 0.8, 0.9, parmod=[0.0] * 9, l_max=10)
        except ValueError:
            pass
        else:
            raise AssertionError("expected ValueError for a short parmod")


def test_trace_unknown_models_raise():
    with geopack.recalc(YEAR, MONTH, DAY, HOUR, MINUTE, SECOND, vx=NS_VX, vy=NS_VY, vz=NS_VZ) as ctx:
        for kwargs in ({"ex_model": 99}, {"in_model": 99}):
            try:
                ctx.trace(-1.02, 0.8, 0.9, l_max=10, **kwargs)
            except geopack.GeopackError:
                pass
            else:
                raise AssertionError(f"expected GeopackError for {kwargs}")


def test_trace_dip_internal_model_is_finite():
    with geopack.recalc(YEAR, MONTH, DAY, HOUR, MINUTE, SECOND, vx=NS_VX, vy=NS_VY, vz=NS_VZ) as ctx:
        result = ctx.trace(
            -1.02,
            0.8,
            0.9,
            in_model=geopack.InternalFieldModel.Dip,
            l_max=50,
        )

    assert result.points
    for point in result.points:
        for value in point:
            assert value == value  # NaN-free


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
