"""Tests for the Tsyganenko (1989) external field model.

Mirrors UnitTests/ExternalFieldModels/ExternalFieldModelsTests.T89.cs 1:1:
the same 8 ``[InlineData]`` rows (iopt, ps, x, y, z -> Bx, By, Bz in GSM),
the same dummy ``parmod = new double[10]`` and the same absolute tolerance::

    ShouldBe(expected, 0.0000000000001)   // MinimalTestsPrecision = 1e-13

The model is context-free (unlike IGRF/dip), so the parity cases need no Recalc
fixture; the context only enters the consistency check at the end
(ctx.t89 defaults psi to the context's dipole tilt).

Run standalone:  python3 python/tests/test_t89.py
or with pytest:  pytest python/tests/test_t89.py
"""

from __future__ import annotations

import os
import sys

sys.path.insert(0, os.path.join(os.path.dirname(__file__), ".."))

import geopack  # noqa: E402

# ---------------------------------------------------------------------------
# Shared constants
# ---------------------------------------------------------------------------

TOL_T89 = 0.0000000000001  # ExternalFieldModelsTests.cs: MinimalTestsPrecision = 1e-13

# Recalc(1997-12-16 21:00 UTC, VGSE=(-304, 13, 4)) — the C# fixture _context.
YEAR, MONTH, DAY, HOUR, MINUTE, SECOND = 1997, 12, 16, 21, 0, 0
VX, VY, VZ = -304.0, 13.0, 4.0

# ExternalFieldModelsTests.T89.cs: [InlineData(iopt, ps, x, y, z, Bx, By, Bz)].
# The two on-axis cases carry By = 0.0 in the C# reference data.
_T89_CASES = [
    (1, 0.5, -6.6, 0.0, 0.0, -21.7428582008425, 0.0, -16.1753178949799),
    (1, 0.5, 6.6, 0.0, 0.0, 2.77460435954486, 0.0, 10.4563908886324),
    (7, 1.0, -1.02, -1.02, -1.02, -40.3174033286441, -4.28475694981233, -21.8189293247679),
    (6, 1.0, -1.02, -1.02, -1.02, -77.7524838720541, -1.48818282095091, -46.0916345034569),
    (5, 1.0, -1.02, -1.02, -1.02, -35.7991349979912, -1.12022295377554, -19.4019675803310),
    (4, 1.0, -1.02, -1.02, -1.02, -31.0211249343846, -0.446400971617370, -18.2770347314801),
    (3, 1.0, -1.02, -1.02, -1.02, -33.8409563532569, -0.947163635685137, -22.4120161221729),
    (2, 1.0, -1.02, -1.02, -1.02, -30.3828134463662, -1.13707597340196, -21.7515334042755),
]


def _assert_approx(actual, expected, label=""):
    assert abs(actual - expected) <= TOL_T89, (
        f"{label}: got {actual!r}, expected {expected!r}, "
        f"diff {abs(actual - expected)!r} > {TOL_T89!r}"
    )


# ---------------------------------------------------------------------------
# Parity (8 value cases)
# ---------------------------------------------------------------------------

def test_t89_cases():
    # new double[10] in the C# test -> a 10-element zeroed array here.
    for i, (iopt, psi, x, y, z, ebx, eby, ebz) in enumerate(_T89_CASES):
        b = geopack.t89(iopt, psi, x, y, z, parmod=[0.0] * 10)
        _assert_approx(b[0], ebx, f"t89[{i}].x")
        _assert_approx(b[1], eby, f"t89[{i}].y")
        _assert_approx(b[2], ebz, f"t89[{i}].z")


# ---------------------------------------------------------------------------
# Context consistency
# ---------------------------------------------------------------------------

def test_t89_context_psi_consistency():
    # Context.t89 defaults psi to the context's tilt; it must agree with the
    # module-level t89 called with the same tilt explicitly.
    with geopack.recalc(YEAR, MONTH, DAY, HOUR, MINUTE, SECOND, vx=VX, vy=VY, vz=VZ) as ctx:
        for i, (iopt, _, x, y, z, _, _, _) in enumerate(_T89_CASES):
            via_ctx = ctx.t89(iopt, x, y, z)
            via_module = geopack.t89(iopt, ctx.psi, x, y, z)
            for k in range(3):
                _assert_approx(via_ctx[k], via_module[k], f"ctx.t89[{i}].{k} vs module")


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
