namespace AuroraScienceHub.Geopack.Contracts.Models;

/// <summary>
/// Field vector with spherical components
/// </summary>
/// <param name="Br"> Vector radial component </param>
/// <param name="Btheta">Vector theta component </param>
/// <param name="Bphi"> Vector phi component </param>
/// <param name="CoordinateSystem"> Coordinate system </param>
public record SphericalFieldVector(double Br, double Btheta, double Bphi, CoordinateSystem? CoordinateSystem = null);
