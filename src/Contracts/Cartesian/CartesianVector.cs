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
        double r = Math.Sqrt(rho2 + location.Z * location.Z);

        if (r <= double.Epsilon)
        {
            throw new InvalidOperationException("Location radius should not be zero.");
        }

        double rho = Math.Sqrt(rho2);

        double cphi;
        double sphi;

        if (rho > double.Epsilon)
        {
            double rhoInv = 1.0 / rho;
            cphi = location.X * rhoInv;
            sphi = location.Y * rhoInv;
        }
        else
        {
            cphi = 1.0;
            sphi = 0.0;
        }

        double rInv = 1.0 / r;
        double ct = location.Z * rInv;
        double st = rho * rInv;

        double br = (location.X * X + location.Y * Y + location.Z * Z) * rInv;
        double btheta = (X * cphi + Y * sphi) * ct - Z * st;
        double bphi = Y * cphi - X * sphi;

        return SphericalVector<TVector>.New(br, btheta, bphi, CoordinateSystem);
    }
};
