using AuroraScienceHub.Geopack.Contracts.Interfaces;

namespace AuroraScienceHub.Geopack.Contracts.Models;

/// <summary>
/// Field vector with spherical components
/// </summary>
/// <param name="R"> Vector radial component </param>
/// <param name="Theta">Vector theta component </param>
/// <param name="Phi"> Vector phi component </param>
/// <param name="CoordinateSystem"> Coordinate system </param>
public readonly record struct SphericalVector<TVector>(double R, double Theta, double Phi, CoordinateSystem CoordinateSystem)
    where TVector : IVectorQuantity;
