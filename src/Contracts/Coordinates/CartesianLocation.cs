using AuroraScienceHub.Geopack.Contracts.Engine;

namespace AuroraScienceHub.Geopack.Contracts.Coordinates;

/// <summary>
/// Location with cartesian coordinates
/// </summary>
public readonly record struct CartesianLocation : ICartesian<CartesianLocation>
{
    public double X { get; }

    public double Y { get; }

    public double Z { get; }

    public CoordinateSystem CoordinateSystem { get; }

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
    /// Summ of two cartesian locations
    /// </summary>
    /// <param name="location"> Location to be added </param>
    /// <exception cref="InvalidOperationException">If different location coordinate systems</exception>
    public CartesianLocation SumWith(CartesianLocation location)
        => CoordinateSystem == location.CoordinateSystem
            ? New(X + location.X, Y + location.Y, Z + location.Z, CoordinateSystem)
            : throw new InvalidOperationException("Cannot sum locations in different coordinate systems.");

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
