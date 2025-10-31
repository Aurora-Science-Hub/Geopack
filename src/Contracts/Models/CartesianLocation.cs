using AuroraScienceHub.Geopack.Contracts.Engine;
using AuroraScienceHub.Geopack.Contracts.Interfaces;

namespace AuroraScienceHub.Geopack.Contracts.Models;

/// <summary>
/// Position in Cartesian coordinates
/// </summary>
public readonly record struct CartesianLocation : ICartesian<CartesianLocation>
{
    public double X { get; }

    public double Y { get; }

    public double Z  { get; }

    public CoordinateSystem CoordinateSystem  { get; }

    private CartesianLocation(double x, double y, double z, CoordinateSystem coordinateSystem)
    {
        X = x;
        Y = y;
        Z = z;
        CoordinateSystem = coordinateSystem;
    }

    /// <summary>
    /// Create new cartesian location
    /// </summary>
    /// <param name="x">X-coordinate</param>
    /// <param name="y">Y-coordinate</param>
    /// <param name="z">Z-coordinate</param>
    /// <param name="coordinateSystem">Coordinate system</param>
    public static CartesianLocation New(double x, double y, double z, CoordinateSystem coordinateSystem)
        => new(x, y, z, coordinateSystem);

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

        return SphericalLocation.New(r, theta, phi, CoordinateSystem);
    }
}
