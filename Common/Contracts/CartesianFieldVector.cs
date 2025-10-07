namespace AuroraScienceHub.Geopack.Common.Contracts;

/// <summary>
/// Cartesian field vector
/// </summary>
/// <param name="Bx"> Vector x-component </param>
/// <param name="By"> Vector y-component </param>
/// <param name="Bz"> Vector z-component </param>
/// <param name="CoordinateSystem"> Coordinate system </param>
public record CartesianFieldVector(double Bx, double By, double Bz, CoordinateSystem? CoordinateSystem)
    : FieldVector(CoordinateSystem);
