namespace AuroraScienceHub.Geopack.Contracts.Models;

/// <summary>
/// Location with spherical coordinates
/// </summary>
/// <param name="R"> Radial component in Earth radii (Re) </param>
/// <param name="Theta"> Theta angle in radians (rad)</param>
/// <param name="Phi"> Phi angle in radians (rad)</param>
/// <param name="CoordinateSystem"> Coordinate system </param>
public readonly record struct SphericalLocation(double R, double Theta, double Phi, CoordinateSystem? CoordinateSystem = null);
