namespace AuroraScienceHub.Geopack.Contracts;

/// <summary>
/// Location with spherical coordinates
/// </summary>
/// <param name="R"> Radial component </param>
/// <param name="Theta"> Theta angle </param>
/// <param name="Phi"> Phi angle </param>
/// <param name="CoordinateSystem"> Coordinate system </param>
public record SphericalLocation(double R, double Theta, double Phi, CoordinateSystem? CoordinateSystem = null)
    : Location(CoordinateSystem);
