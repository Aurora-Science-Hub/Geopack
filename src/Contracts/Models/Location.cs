namespace AuroraScienceHub.Geopack.Contracts.Models;

/// <summary>
/// Spatial location abstraction
/// </summary>
/// <param name="CoordinateSystem"> Coordinate system </param>
public abstract record Location(CoordinateSystem? CoordinateSystem = null);
