namespace AuroraScienceHub.Geopack.Common;

/// <summary>
/// Spatial location abstraction
/// </summary>
/// <param name="CoordinateSystem"> Coordinate system </param>
public abstract record Location(CoordinateSystem? CoordinateSystem = null);
