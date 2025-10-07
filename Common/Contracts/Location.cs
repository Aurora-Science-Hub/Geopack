namespace AuroraScienceHub.Geopack.Common.Contracts;

/// <summary>
/// Spatial location abstraction
/// </summary>
/// <param name="CoordinateSystem"> Coordinate system </param>
public abstract record Location(CoordinateSystem? CoordinateSystem = null);
