"""API tests for the geopack Python package.

Run from the repository root after a native build::

    python/geopack/geopack.dylib  must exist
    python -m pytest python/tests/test_api.py

or standalone::

    python python/tests/test_api.py

The suite mirrors the C# unit tests (see test_parity.py for the full parity set).
"""

from __future__ import annotations

import math
import sys
import os

sys.path.insert(0, os.path.join(os.path.dirname(__file__), ".."))

import geopack  # noqa: E402

# Reference values from UnitTests/Geopack/GepackTests.IgrfGsw.cs + InputData.dat:
#   Recalc(1997-12-16 21:00 UTC, VGSE=(-304, 13, 4))   <-- test hardcodes vy=13.0
#   IgrfGsw(1, 1, 1) ~= (-5474.5721411268, -3598.5022435295, 1833.2152736286)
YEAR, MONTH, DAY = 1997, 12, 16
HOUR, MINUTE, SECOND = 21, 0, 0
VX, VY, VZ = -304.0, 13.0, 4.0
EXPECTED_IGRF_GSW = (-5474.5721411268, -3598.5022435295, 1833.2152736286)


def test_igrf_gsw_reference() -> None:
    ctx = geopack.recalc(YEAR, MONTH, DAY, HOUR, MINUTE, SECOND, vx=VX, vy=VY, vz=VZ)
    try:
        b = ctx.igrf_gsw(1, 1, 1)
    finally:
        ctx.close()
    assert isinstance(b, geopack.Vector3)
    for got, want in zip(b, EXPECTED_IGRF_GSW):
        assert abs(got - want) < 1e-6, f"{b} != {EXPECTED_IGRF_GSW}"


def test_transform_round_trip() -> None:
    with geopack.recalc(YEAR, MONTH, DAY, HOUR, MINUTE, SECOND, vx=VX, vy=VY, vz=VZ) as ctx:
        p = (0.7, 1.2, -0.4)
        gsw = ctx.geo_to_gsw(*p)
        back = ctx.gsw_to_geo(*gsw)
    for got, want in zip(back, p):
        assert abs(got - want) < 1e-9, f"round trip failed: {back} != {p}"


def test_igrf_geo_finite() -> None:
    with geopack.recalc(YEAR, MONTH, DAY, HOUR, MINUTE, SECOND, vx=VX, vy=VY, vz=VZ) as ctx:
        b = ctx.igrf_geo(1.0, 1.0, 2.0)
    assert all(math.isfinite(v) for v in b)


def test_sun_finite() -> None:
    s = geopack.sun(YEAR, MONTH, DAY, HOUR, MINUTE, SECOND)
    assert all(math.isfinite(v) for v in s)
    assert 0.0 <= s.gst <= 2 * math.pi
    assert -math.pi <= s.sdec <= math.pi


def test_dip_finite() -> None:
    with geopack.recalc(YEAR, MONTH, DAY, HOUR, MINUTE, SECOND, vx=VX, vy=VY, vz=VZ) as ctx:
        b = ctx.dip(1, 1, 1)
    assert all(math.isfinite(v) for v in b)


def test_shu_mgnp() -> None:
    r = geopack.shu_mgnp(xn_pd=2.0, vel=400.0, bz_imf=0.0, x=10.0, y=0.0, z=0.0)
    assert all(math.isfinite(v) for v in r.boundary)
    assert math.isfinite(r.dist)
    assert r.position in (geopack.MagnetopausePosition.Inside,
                          geopack.MagnetopausePosition.Outside,
                          geopack.MagnetopausePosition.NotDefined)


def test_t96_mgnp() -> None:
    r = geopack.t96_mgnp(xn_pd=2.0, vel=400.0, x=10.0, y=0.0, z=0.0)
    assert all(math.isfinite(v) for v in r.boundary)
    assert math.isfinite(r.dist)
    assert isinstance(r.position, geopack.MagnetopausePosition)


def test_datetime_input() -> None:
    from datetime import datetime, timezone

    dt = datetime(1997, 12, 16, 21, 0, 0, tzinfo=timezone.utc)
    with geopack.recalc(dt, vx=VX, vy=VY, vz=VZ) as ctx:
        b = ctx.igrf_gsw(1, 1, 1)
    assert abs(b[0] - EXPECTED_IGRF_GSW[0]) < 1e-6


def test_bad_datetime_raises() -> None:
    try:
        geopack.recalc(1997, 2, 30, 0, 0, 0)  # invalid day
    except geopack.GeopackError as exc:
        assert str(exc)
    else:
        raise AssertionError("expected GeopackError for invalid date")


def test_use_after_close_raises() -> None:
    ctx = geopack.recalc(YEAR, MONTH, DAY, HOUR, MINUTE, SECOND)
    ctx.close()
    ctx.close()  # idempotent
    try:
        ctx.igrf_gsw(1, 1, 1)
    except RuntimeError:
        pass
    else:
        raise AssertionError("expected RuntimeError after close")


def test_last_error_undersized_buffer_keeps_message() -> None:
    # C ABI contract: gp_last_error must never throw on an undersized buffer and
    # must keep the message pending until the whole message has been read.
    import ctypes
    from geopack import _native

    # Set a fresh error without reading it (recalc() would auto-clear it via _check).
    handle = ctypes.c_int64()
    rc = _native._lib.gp_context_create(
        1997, 2, 30, 0, 0, 0, -400.0, 0.0, 0.0, ctypes.byref(handle)
    )
    assert rc != 0  # invalid day -> error set, still pending

    needed = _native._lib.gp_last_error(None, 0)
    assert needed > 0

    # An undersized buffer reports the required size and does NOT clear the message.
    small = ctypes.create_string_buffer(4)
    got = _native._lib.gp_last_error(ctypes.cast(small, _native._p_byte), 4)
    assert got == needed

    # The message is still pending, so a full-size read retrieves it.
    full = ctypes.create_string_buffer(needed + 1)
    _native._lib.gp_last_error(ctypes.cast(full, _native._p_byte), needed + 1)
    assert full.value


def _main() -> int:
    failures = 0
    for name, fn in sorted(globals().items()):
        if name.startswith("test_") and callable(fn):
            try:
                fn()
                print(f"PASS {name}")
            except Exception as exc:  # noqa: BLE001
                failures += 1
                print(f"FAIL {name}: {exc!r}")
    print(f"\n{sum(1 for k in globals() if k.startswith('test_') and callable(globals()[k])) - failures}/{sum(1 for k in globals() if k.startswith('test_') and callable(globals()[k]))} passed")
    return 1 if failures else 0


if __name__ == "__main__":
    sys.exit(_main())
