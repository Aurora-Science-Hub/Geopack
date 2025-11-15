namespace AuroraScienceHub.Geopack.Contracts.Coordinates;

/// <summary>
/// Geodetic coordinates for 2D meridian plane transformations.
/// Contains only altitude and geodetic latitude
/// </summary>
public readonly record struct GeodeticCoordinates
{
    /// <summary> Geodetic latitude (in radians) </summary>
    public double Latitude { get; }

    /// <summary> Altitude above ellipsoid (in km) </summary>
    public double Altitude { get; }

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="geodLatitude">Geodetic latitude</param>
    /// <param name="altitude">Geodetic altitude</param>
    public GeodeticCoordinates(double geodLatitude, double altitude)
    {
        Latitude = geodLatitude;
        Altitude = altitude;
    }

    /// <summary>
    /// Converts vertical local height (altitude) altitude and geodetic latitude into geocentric coordinates R and THETA.
    /// </summary>
    /// <remarks>
    /// Original Geopack-2008 method: GEODGEO_08.
    /// The subroutine uses World Geodetic System WGS84 parameters for the Earth's ellipsoid. The angular quantities
    /// (geo co-latitude and geodetic latitude) are in radians, and the distances (geocentric radius R and
    /// altitude H above the Earth's ellipsoid) are in kilometers.
    /// </remarks>
    public GeocentricCoordinates ToGeocentric()
    {
        const double r_eq = 6378.137;
        const double beta = 6.73949674228e-3;
        const double beta1 = 1.0 + beta;

        (double sinxmu, double cosxmu) = Math.SinCos(Latitude);

        double sinxmu_beta1 = sinxmu / beta1;
        double den = Math.Sqrt(cosxmu * cosxmu + sinxmu_beta1 * sinxmu_beta1);
        double denInv = 1.0 / den;

        double coslam = cosxmu * denInv;
        double sinlam = sinxmu_beta1 * denInv;

        double sinlam2 = sinlam * sinlam;
        double rs = r_eq / Math.Sqrt(1.0 + beta * sinlam2);

        double x = rs * coslam + Altitude * cosxmu;
        double z = rs * sinlam + Altitude * sinxmu;

        double r = Math.Sqrt(x * x + z * z);
        double theta = Math.Acos(z / r);

        return new GeocentricCoordinates(r, theta);
    }
}
