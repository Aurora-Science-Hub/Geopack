"""Parity suite: mirrors the observable C# unit tests 1:1.

Every case the C ABI can reach is reproduced with the SAME reference data the
C# suite uses (the inline ``[InlineData]`` rows and the ``TestData/*.dat``
files) and the SAME absolute tolerance::

    ShouldBe(expected, 0.000000000008)   // MinimalTestsPrecision = 8e-12

The context used by every case matches the C# fixture::

    Recalc(1997-12-16 21:00:00 UTC, VGSE = (-304, 13, 4))

Intentionally NOT mirrored — they exercise API surface the C ABI does not expose:
  * RecalcCommonBlocks_ShouldBeCorrect      — internal ComputationContext fields
  * Recalc_Throw (velocity in wrong CS)     — the ABI hardcodes GSE velocity
  * BSphCar/CarSph/SphCar/ToCartesian/...   — cartesian<->spherical helpers
  * GeoGeod/GeodGeo (geodetic)              — not part of the 12 transforms
  * Trace (field-line tracing)              — out of scope for the native layer
  * Rec* internals (G/H/REC finiteness)     — replaced by the observable
    RecalcExtrapolate_2026 test below

Run standalone:  python3 python/tests/test_parity.py
or with pytest:  pytest python/tests/test_parity.py
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

TOL = 0.000000000008  # ShouldBe(expected, 8e-12) in TestExtensions.cs

# GeopackTests.cs: private const double Rad = 57.295779513D;  (NOT math.radians)
_RAD = 57.295779513

# Recalc(1997-12-16 21:00 UTC, VGSE=(-304, 13, 4)) — the C# fixture _context.
YEAR, MONTH, DAY, HOUR, MINUTE, SECOND = 1997, 12, 16, 21, 0, 0
VX, VY, VZ = -304.0, 13.0, 4.0

_TESTDATA_DIR = Path(__file__).resolve().parents[2] / "UnitTests" / "Geopack" / "TestData"


def _assert_approx(actual, expected, label="", tol=TOL):
    assert abs(actual - expected) <= tol, (
        f"{label}: got {actual!r}, expected {expected!r}, "
        f"diff {abs(actual - expected)!r} > {tol!r}"
    )


def _read_dat(name):
    path = _TESTDATA_DIR / name
    if not path.exists():
        raise AssertionError(
            f"reference data not found: {path}. Run this test from the repository root."
        )
    return path.read_text().splitlines()


# ---------------------------------------------------------------------------
# Fixture
# ---------------------------------------------------------------------------

_ctx = geopack.recalc(YEAR, MONTH, DAY, HOUR, MINUTE, SECOND, vx=VX, vy=VY, vz=VZ)


# ---------------------------------------------------------------------------
# IgrfGsw  (7 value cases + 1 throw)
# ---------------------------------------------------------------------------

_IGRF_GSW_CASES = [
    (1.0, 1.0, 1.0, -5474.572141126816859469, -3598.502243529560473689, 1833.215273628635031855),
    (-6.6, 0.0, 0.0, 69.106253595272519874, -3.195943937178284955, 99.368806132912794737),
    (6.6, 0.0, 0.0, 71.769903387016739771, 2.701657129752604192, 98.759878146673571564),
    (0.0, 6.6, 0.0, -32.015610653569630983, 0.868906002339342010, 95.911657058327008940),
    (0.0, -6.6, 0.0, -37.971249476612108253, -0.209072020223322497, 102.021488966220218231),
    (0.0, 0.0, 6.6, -35.363226591977408475, -2.562520059996487021, -199.159374390148741440),
    (0.0, 0.0, -6.6, -34.869248473030353352, 3.554189099127370355, -195.320739944871291982),
]


def test_igrf_gsw_cases():
    for i, (x, y, z, ebx, eby, ebz) in enumerate(_IGRF_GSW_CASES):
        b = _ctx.igrf_gsw(x, y, z)
        _assert_approx(b[0], ebx, f"igrf_gsw[{i}].x")
        _assert_approx(b[1], eby, f"igrf_gsw[{i}].y")
        _assert_approx(b[2], ebz, f"igrf_gsw[{i}].z")


def test_igrf_gsw_zero_throws():
    try:
        _ctx.igrf_gsw(0.0, 0.0, 0.0)
    except geopack.GeopackError:
        pass
    else:
        raise AssertionError("expected GeopackError for zero coordinates")


# ---------------------------------------------------------------------------
# IgrfGeo  (12 value cases + 1 throw; lat/lon -> theta/phi via hardcoded Rad)
# ---------------------------------------------------------------------------

# (xLat, xLon, r, Br, Btheta, Bphi) — theta = (90 - lat)/Rad, phi = lon/Rad.
_IGRF_GEO_CASES = [
    (73.0, 175.0, 1.02, -52864.305642992534558289, -8687.997645029174236697, 651.714296303753599204),
    (90.0, 0.0, 1.02, -53078.710753819832461886, -1566.578021744054694864, -997.428703425218827761),
    (90.0, 45.0, 1.02, -53078.710753819832461886, -1813.026542374652080980, 402.449342492993991982),
    (90.0, 180.0, 1.02, -53078.710753819832461886, 1566.578021748556693638, 997.428703418148188575),
    (90.0, 359.0, 1.02, -53078.710753819832461886, -1548.931893208286282970, -1024.617346539782374748),
    (-90.0, 0.0, 1.02, 50101.895603876902896445, -13178.029910924784417148, -7289.356921431653972832),
    (-90.0, 120.0, 1.02, 50101.895603514029062353, 12901.783226819290575804, -7767.830214052999508567),
    (0.0, 0.0, 6.6, -7.466237418809549276, -100.329164113115780310, -17.725359682701444797),
    (-26.0, 135.0, 1.02, 44172.875491145212436095, -26655.578798915645165835, 2603.779522109869958513),
    (50.0, -90.0, 1.02, -53752.810795780664193444, -12788.775549677107846946, -495.322683045213238984),
    (89.9, 0.0, 1.02, -53063.222394293967226986, -1617.228827444984290196, -998.914807598020502155),
    (-89.9999, 0.0, 1.02, 50101.815124357046443038, -13178.076581014527619118, 4176496199.650153636932373047),
]


def test_igrf_geo_cases():
    # Note: the last case (near the pole) has Bphi ~ 4.2e9 nT; its value is a single
    # IEEE division, so the strict 8e-12 absolute tolerance holds bit-identically.
    for i, (lat, lon, r, ebr, ebt, ebp) in enumerate(_IGRF_GEO_CASES):
        theta = (90.0 - lat) / _RAD
        phi = lon / _RAD
        b = _ctx.igrf_geo(r, theta, phi)
        _assert_approx(b[0], ebr, f"igrf_geo[{i}].r")
        _assert_approx(b[1], ebt, f"igrf_geo[{i}].theta")
        _assert_approx(b[2], ebp, f"igrf_geo[{i}].phi")


def test_igrf_geo_zero_throws():
    try:
        _ctx.igrf_geo(0.0, 1.0, 1.0)
    except geopack.GeopackError:
        pass
    else:
        raise AssertionError("expected GeopackError for r=0")


# ---------------------------------------------------------------------------
# Dip  (7 value cases + 1 throw)
# ---------------------------------------------------------------------------

_DIP_CASES = [
    (6.5999999999999996, 0.0, 0.0, 70.248846561769155983, 0.0, 98.845731875605991945),
    (0.0, 6.5999999999999996, 0.0, -35.124423280884577991, 0.0, 98.845731875605991945),
    (0.0, 0.0, 6.5999999999999996, -35.124423280884577991, 0.0, -197.691463751211983890),
    (1.0, 1.0, 1.0, -5468.999024571849076892, -3525.612769882045540726, 1943.386254689803536166),
    (-6.5999999999999996, 0.0, 0.0, 70.248846561769155983, 0.0, 98.845731875605991945),
    (0.0, -6.5999999999999996, 0.0, -35.124423280884577991, 0.0, 98.845731875605991945),
    (0.0, 0.0, -6.5999999999999996, -35.124423280884577991, 0.0, -197.691463751211983890),
]


def test_dip_cases():
    for i, (x, y, z, ebx, eby, ebz) in enumerate(_DIP_CASES):
        b = _ctx.dip(x, y, z)
        _assert_approx(b[0], ebx, f"dip[{i}].x")
        _assert_approx(b[1], eby, f"dip[{i}].y")
        _assert_approx(b[2], ebz, f"dip[{i}].z")


def test_dip_zero_throws():
    try:
        _ctx.dip(0.0, 0.0, 0.0)
    except geopack.GeopackError:
        pass
    else:
        raise AssertionError("expected GeopackError for zero coordinates")


# ---------------------------------------------------------------------------
# Sun  (4 cases; year 1800 -> all zeros)
# ---------------------------------------------------------------------------

_SUN_CASES = [
    (1800, 1, 1, 0, 0, 0, 0.0, 0.0, 0.0, 0.0),
    (2000, 1, 1, 12, 0, 0, 4.894948822912354558, 4.893575238075353440, 4.909361453634409678, -0.402014132081864151),
    (2004, 2, 29, 0, 0, 0, 2.760256269651100602, 5.929696758033518478, 5.956663000518048534, -0.138172813779450315),
    (1999, 12, 31, 23, 59, 59, 1.744681852526303700, 4.884680534002757035, 4.899722720606541237, -0.402689816598403527),
]


def test_sun_cases():
    for i, (y, mo, d, h, mi, s, egst, eslong, esrasn, esdec) in enumerate(_SUN_CASES):
        sun = geopack.sun(y, mo, d, h, mi, s)
        _assert_approx(sun.gst, egst, f"sun[{i}].gst")
        _assert_approx(sun.slong, eslong, f"sun[{i}].slong")
        _assert_approx(sun.srasn, esrasn, f"sun[{i}].srasn")
        _assert_approx(sun.sdec, esdec, f"sun[{i}].sdec")


# ---------------------------------------------------------------------------
# ShuMgnp  (4 value cases + 4 throw)
# ---------------------------------------------------------------------------

_SHU_CASES = [
    (5.0, -350.0, 5.0, 9.0, 0.0, 0.0, 9.003326462780140815, 0.0, 0.0, 0.003326462780140815, 1),
    (5.0, -350.0, 5.0, 15.0, 0.0, 0.0, 9.003326462780140815, 0.0, 0.0, 5.996673537219859185, 2),
    (5.0, 350.0, 5.0, 9.0, 0.0, 0.0, 11.193293023217856685, 0.0, 0.0, 2.193293023217856685, 1),
    (99990.0, 999990.0, 999.0, 9.0, 0.0, 0.0, 0.224290174048649371, 0.0, 0.0, 8.775709825951350851, 2),
]

# (xnPd, vel, bzImf, x, y, z) — these inputs make the model non-converge -> throw.
_SHU_NAN_CASES = [
    (0.0, 0.0, 0.0, 9.0, 0.0, 0.0),
    (0.0, 0.0, 999.0, 9.0, 0.0, 0.0),
    (0.0, 9990.0, 999.0, 9.0, 0.0, 0.0),
    (99990.0, 0.0, 999.0, 9.0, 0.0, 0.0),
]


def test_shu_mgnp_cases():
    for i, (pd, vel, bz, x, y, z, ex, ey, ez, edist, epos) in enumerate(_SHU_CASES):
        r = geopack.shu_mgnp(pd, vel, bz, x, y, z)
        _assert_approx(r.boundary[0], ex, f"shu[{i}].x")
        _assert_approx(r.boundary[1], ey, f"shu[{i}].y")
        _assert_approx(r.boundary[2], ez, f"shu[{i}].z")
        _assert_approx(r.dist, edist, f"shu[{i}].dist")
        assert int(r.position) == epos, f"shu[{i}].position: got {r.position}, expected {epos}"


def test_shu_mgnp_nan_throws():
    for i, case in enumerate(_SHU_NAN_CASES):
        try:
            geopack.shu_mgnp(*case)
        except geopack.GeopackError:
            pass
        else:
            raise AssertionError(f"expected GeopackError for shu_mgnp case {i}: {case}")


# ---------------------------------------------------------------------------
# T96Mgnp  (5 value cases + 3 throw)
# ---------------------------------------------------------------------------

_T96_CASES = [
    (5.0, 350.0, 9.0, 0.0, 0.0, 11.917821173671217849, 0.0, 0.0, 2.917821173671217849, 1),
    (5.0, 350.0, 12.0, 1.0, 0.0, 11.875615424737137715, 0.989433928116373318, 0.000000000000000061, 0.124832545589572186, 2),
    (1.0, -1350.0, 9.0, 0.0, 0.0, 12.209108683912852200, 0.0, 0.0, 3.209108683912852200, 1),
    (1.0, -1350.0, 15.0, 0.0, 0.0, 12.209108683912852200, 0.0, 0.0, 2.790891316087147800, 2),
    (5.0, 350.0, 0.0, 0.0, 0.0, 5.551897632673411742, 0.0, 11.912899382845374419, 13.143087119451134726, 1),
]

_T96_NAN_CASES = [
    (0.0, 0.0, 9.0, 0.0, 0.0),
    (0.0, 99999999.0, 9.0, 0.0, 0.0),
    (99999999.0, 0.0, 9.0, 0.0, 0.0),
]


def test_t96_mgnp_cases():
    for i, (pd, vel, x, y, z, ex, ey, ez, edist, epos) in enumerate(_T96_CASES):
        r = geopack.t96_mgnp(pd, vel, x, y, z)
        _assert_approx(r.boundary[0], ex, f"t96[{i}].x")
        _assert_approx(r.boundary[1], ey, f"t96[{i}].y")
        _assert_approx(r.boundary[2], ez, f"t96[{i}].z")
        _assert_approx(r.dist, edist, f"t96[{i}].dist")
        assert int(r.position) == epos, f"t96[{i}].position: got {r.position}, expected {epos}"


def test_t96_mgnp_nan_throws():
    for i, case in enumerate(_T96_NAN_CASES):
        try:
            geopack.t96_mgnp(*case)
        except geopack.GeopackError:
            pass
        else:
            raise AssertionError(f"expected GeopackError for t96_mgnp case {i}: {case}")


# ---------------------------------------------------------------------------
# Coordinate transforms  (12 .dat files x 216 rows each)
# ---------------------------------------------------------------------------

# Each line: X=.. Y=.. Z=.. XR=.. YR=.. ZR=..  (input XYZ -> expected XR YR ZR).
_TRANSFORMS = [
    ("GswGse.dat", "gsw_to_gse"),
    ("GseGsw.dat", "gse_to_gsw"),
    ("GeoMag.dat", "geo_to_mag"),
    ("MagGeo.dat", "mag_to_geo"),
    ("GeiGeo.dat", "gei_to_geo"),
    ("GeoGei.dat", "geo_to_gei"),
    ("MagSm.dat", "mag_to_sm"),
    ("SmMag.dat", "sm_to_mag"),
    ("SmGsw.dat", "sm_to_gsw"),
    ("GswSm.dat", "gsw_to_sm"),
    ("GeoGsw.dat", "geo_to_gsw"),
    ("GswGeo.dat", "gsw_to_geo"),
]


def _make_transform_test(dat_name, method_name):
    def test():
        fn = getattr(_ctx, method_name)
        lines = _read_dat(dat_name)
        assert len(lines) == 216, f"{dat_name}: expected 216 rows, got {len(lines)}"
        for i, line in enumerate(lines):
            tok = line.split()
            # ["X=", x, "Y=", y, "Z=", z, "XR=", xr, "YR=", yr, "ZR=", zr]
            x, y, z = float(tok[1]), float(tok[3]), float(tok[5])
            ex, ey, ez = float(tok[7]), float(tok[9]), float(tok[11])
            r = fn(x, y, z)
            label = f"{method_name}[{i}]"
            _assert_approx(r[0], ex, label + ".x")
            _assert_approx(r[1], ey, label + ".y")
            _assert_approx(r[2], ez, label + ".z")

    test.__name__ = f"test_transform_{method_name}"
    test.__doc__ = f"{method_name}: all 216 rows from {dat_name} match C# reference."
    return test


for _dat_name, _method_name in _TRANSFORMS:
    globals()[f"test_transform_{_method_name}"] = _make_transform_test(_dat_name, _method_name)


# ---------------------------------------------------------------------------
# Recalc extrapolation (observable mirror of RecalcExtrapolate_ShouldNotThrow)
# ---------------------------------------------------------------------------

def test_recalc_extrapolate_2026_finite():
    with geopack.recalc(2026, 6, 15, 12, 0, 0) as ctx:
        b = ctx.igrf_gsw(1, 1, 1)
        assert all(math.isfinite(v) for v in b), b
        for pt in ((0.7, 1.2, -0.4), (6.6, -6.6, 0.0)):
            r = ctx.geo_to_gsw(*pt)
            assert all(math.isfinite(v) for v in r), r


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
    _ctx.close()
    return 1 if failures else 0


if __name__ == "__main__":
    sys.exit(_main())
