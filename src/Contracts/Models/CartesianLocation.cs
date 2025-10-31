using AuroraScienceHub.Geopack.Contracts.Engine;
using AuroraScienceHub.Geopack.Contracts.Interfaces;

namespace AuroraScienceHub.Geopack.Contracts.Models;

/// <summary>
/// Position in Cartesian coordinates
/// </summary>
/// <param name="X">X-coordinate</param>
/// <param name="Y">Y-coordinate</param>
/// <param name="Z">Z-coordinate</param>
/// <param name="CoordinateSystem">Coordinate system</param>
public readonly record struct CartesianLocation(double X, double Y, double Z, CoordinateSystem CoordinateSystem)
    : ICartesian
{
    /// <summary>
    /// Converts cartesian coordinates to spherical ones
    /// </summary>
    /// <remarks>
    /// Original Geopack-2008 method: CARSPH_08
    /// </remarks>
    public SphericalLocation ToSpherical()
    {
        double phi;
        double theta;
        double sq = X * X + Y * Y;
        double r = Math.Sqrt(sq + Z * Z);

        if (sq != 0.0d)
        {
            sq = Math.Sqrt(sq);
            phi = Math.Atan2(Y, X);
            theta = Math.Atan2(sq, Z);
            if (phi < 0.0d)
            {
                phi += GeopackConstants.TwoPi;
            }
        }
        else
        {
            phi = 0.0;
            theta = Z < 0.0d ? GeopackConstants.Pi : 0.0d;
        }

        return new SphericalLocation(r, theta, phi, CoordinateSystem);
    }

    public T Create<T>(double x, double y, double z, CoordinateSystem coordinateSystem)
        where T : ICartesian
    {
        if (typeof(T) == typeof(CartesianLocation))
        {
            return (T)(ICartesian)new CartesianLocation(x, y, z, coordinateSystem);
        }

        throw new InvalidOperationException(
            $"Cannot create {typeof(T)}. This factory only supports {typeof(CartesianLocation)}");
    }
}
