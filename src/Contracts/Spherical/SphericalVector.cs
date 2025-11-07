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
        (double sinTheta, double cosTheta) = Math.SinCos(location.Theta);
        (double sinPhi, double cosPhi) = Math.SinCos(location.Phi);

        double be = R * sinTheta + Theta * cosTheta;
        double bx = be * cosPhi - Phi * sinPhi;
        double by = be * sinPhi + Phi * cosPhi;
        double bz = R * cosTheta - Theta * sinTheta;

        return CartesianVector<TVector>.New(bx, by, bz, CoordinateSystem);
    }
}
