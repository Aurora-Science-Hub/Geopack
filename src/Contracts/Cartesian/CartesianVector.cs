using AuroraScienceHub.Geopack.Contracts.Coordinates;
using AuroraScienceHub.Geopack.Contracts.Spherical;

namespace AuroraScienceHub.Geopack.Contracts.Cartesian;

/// <summary>
/// Cartesian field vector
/// </summary>
public readonly record struct CartesianVector<TVector>
    : ICartesian<CartesianVector<TVector>>
    where TVector : IVectorQuantity
{
    public double X { get; }

    public double Y { get; }

    public double Z { get; }

    public CoordinateSystem CoordinateSystem { get; }

    public static CartesianVector<TVector> New(double x, double y, double z, CoordinateSystem coordinateSystem)
        => new(x, y, z, coordinateSystem);

    private CartesianVector(double x, double y, double z, CoordinateSystem coordinateSystem)
    {
        X = x;
        Y = y;
        Z = z;
        CoordinateSystem = coordinateSystem;
    }

    /// <summary>
    /// Calculates local spherical field components from those in cartesian system.
    /// </summary>
    /// <remarks>
    /// Original Geopack-2008 method: BCARSP_08
    /// </remarks>
    public SphericalVector<TVector> ToSphericalVector(CartesianLocation location)
    {
        double rho2 = location.X * location.X + location.Y * location.Y;
        double rho = Math.Sqrt(rho2);

        double r = Math.Sqrt(rho2 + location.Z * location.Z);

        if (Math.Abs(r) <= double.Epsilon)
        {
            throw new InvalidOperationException("Location radius should not be zero.");
        }

        double cphi;
        double sphi;

        if (rho > 0.0D)
        {
            cphi = location.X / rho;
            sphi = location.Y / rho;
        }
        else
        {
            cphi = 1.0D;
            sphi = 0.0D;
        }

        double ct = location.Z / r;
        double st = rho / r;

        double br = Math.FusedMultiplyAdd(location.X, X, Math.FusedMultiplyAdd(location.Y, Y, location.Z * Z)) / r;
        double btheta = Math.FusedMultiplyAdd(Math.FusedMultiplyAdd(X, cphi, Y * sphi), ct, -Z * st);
        double bphi = Math.FusedMultiplyAdd(Y, cphi, -X * sphi);

        return SphericalVector<TVector>.New(br, btheta, bphi, CoordinateSystem);
    }
};
