using AuroraScienceHub.Geopack.Contracts.Cartesian;
using AuroraScienceHub.Geopack.Contracts.Coordinates;

namespace AuroraScienceHub.Geopack.Contracts.Spherical;

/// <summary>
/// Location with spherical coordinates (geocentric)
/// </summary>
public readonly record struct SphericalLocation : ISpherical<SphericalLocation>
{
    public double R { get; }

    public double Theta { get; }

    public double Phi { get; }

    public CoordinateSystem CoordinateSystem { get; }

    private SphericalLocation(double r, double theta, double phi, CoordinateSystem coordinateSystem)
    {
        R = r;
        Theta = theta;
        Phi = phi;
        CoordinateSystem = coordinateSystem;
    }

    public static SphericalLocation New(double r, double theta, double phi, CoordinateSystem coordinateSystem)
        => new(r, theta, phi, coordinateSystem);

    /// <summary>
    /// Convert spherical coordinates to cartesian ones
    /// </summary>
    /// <remarks>
    /// Original Geopack-2008 method: SPHCAR_08
    /// </remarks>
    public CartesianLocation ToCartesian()
    {
        (double sinTheta, double cosTheta) = Math.SinCos(Theta);
        (double sinPhi, double cosPhi) = Math.SinCos(Phi);
        double sq = R * sinTheta;
        double x = sq * cosPhi;
        double y = sq * sinPhi;
        double z = R * cosTheta;

        return CartesianLocation.New(x, y, z, CoordinateSystem);
    }
}
