"""Enumerations shared with the native Geopack ABI.

The numeric values mirror the C# ``CoordinateSystem`` and
``MagnetopausePosition`` enums so the integer codes crossing the C ABI stay
unambiguous.
"""

from __future__ import annotations

from enum import IntEnum


class CoordinateSystem(IntEnum):
    """Geographic coordinate system codes (GEO = 0, GSW = 1, ...)."""

    GEO = 0
    GSW = 1
    GSE = 2
    MAG = 3
    SM = 4
    GEI = 5
    GSM = 6


class MagnetopausePosition(IntEnum):
    """Position of a point relative to the magnetopause boundary."""

    NotDefined = 0
    Inside = 1
    Outside = 2
