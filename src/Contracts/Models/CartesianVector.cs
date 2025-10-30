using AuroraScienceHub.Geopack.Contracts.Interfaces;

namespace AuroraScienceHub.Geopack.Contracts.Models;

/// <summary>
/// Cartesian field vector
/// </summary>
/// <param name="X"> Vector x-component </param>
/// <param name="Y"> Vector y-component </param>
/// <param name="Z"> Vector z-component </param>
/// <param name="CoordinateSystem"> Coordinate system </param>
public readonly record struct CartesianVector<TVector>(double X, double Y, double Z, CoordinateSystem CoordinateSystem)
    where TVector : IVectorQuantity;
