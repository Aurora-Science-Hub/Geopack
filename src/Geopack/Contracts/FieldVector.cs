namespace AuroraScienceHub.Geopack.Contracts;

/// <summary>
/// Field vector abstraction
/// </summary>
/// <param name="CoordinateSystem"> Coordinate system </param>
public abstract record FieldVector(CoordinateSystem? CoordinateSystem = null);
