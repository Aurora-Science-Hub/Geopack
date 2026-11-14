"""Enumerations shared with the native Geopack ABI.

The numeric values mirror the C# enums behind the fixed integer codes of the
``gp_*`` entry points: ``CoordinateSystem``, ``MagnetopausePosition``,
``TraceDirection``, ``TraceTermination``, ``ExternalFieldModel`` and
``InternalFieldModel``.

Members keep the C# spelling (CamelCase) so the codes map 1:1 to the .NET enums.
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


class TraceDirection(IntEnum):
    """Direction of field-line tracing relative to the main magnetic field."""

    Parallel = -1
    AntiParallel = 1


class TraceTermination(IntEnum):
    """Reason why field-line tracing stopped."""

    BoundaryReached = 0
    MaximumPointsExceeded = 1


class ExternalFieldModel(IntEnum):
    """External field models selectable for field-line tracing."""

    T89 = 1


class InternalFieldModel(IntEnum):
    """Internal field models selectable for field-line tracing."""

    IgrfGsw = 1
    Dip = 2
