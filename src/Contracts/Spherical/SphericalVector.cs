using AuroraScienceHub.Geopack.Contracts.Cartesian;
using AuroraScienceHub.Geopack.Contracts.Coordinates;

namespace AuroraScienceHub.Geopack.Contracts.Spherical;

/// <summary>
/// Field vector with spherical components
/// </summary>
public readonly record struct SphericalVector<TVector>
    : ISpherical<SphericalVector<TVector>>
    where TVector : IVectorQuantity
{
    public double R { get; }

    public double Theta { get; }

    public double Phi { get; }

    public CoordinateSystem CoordinateSystem { get; }

    public static SphericalVector<TVector> New(double r, double theta, double phi, CoordinateSystem coordinateSystem)
        => new(r, theta, phi, coordinateSystem);

    private SphericalVector(double r, double theta, double phi, CoordinateSystem coordinateSystem)
    {
        R = r;
        Theta = theta;
        Phi = phi;
        CoordinateSystem = coordinateSystem;
    }

    /// <summary>
    /// Calculates Cartesian vector components from local spherical ones.
    /// </summary>
    /// <remarks>
    /// Original Geopack-2008 method: BSPCAR_08
    /// </remarks>
    /// <param name="location"> Point spherical coordinates </param>
    public CartesianVector<TVector> ToCartesianVector(SphericalLocation location)
    {
        double s = Math.Sin(location.Theta);
        double c = Math.Cos(location.Theta);
        double sf = Math.Sin(location.Phi);
        double cf = Math.Cos(location.Phi);

        double be = R * s + Theta * c;
        double bx = be * cf - Phi * sf;
        double by = be * sf + Phi * cf;
        double bz = R * c - Theta * s;

        return CartesianVector<TVector>.New(bx, by, bz, CoordinateSystem);
    }
}
