namespace AuroraScienceHub.Geopack.Contracts.Models;

/// <summary>
/// Position in Cartesian coordinates
/// </summary>
/// <param name="X">X-coordinate</param>
/// <param name="Y">Y-coordinate</param>
/// <param name="Z">Z-coordinate</param>
/// <param name="CoordinateSystem">Coordinate system</param>
public readonly record struct CartesianLocation(double X, double Y, double Z, CoordinateSystem? CoordinateSystem = null);
