namespace AuroraScienceHub.Geopack.Contracts.Models;

/// <summary>
/// Geodetic coordinates for 2D meridian plane transformations.
/// Contains only altitude and geodetic latitude
/// </summary>
public readonly record struct GeodeticCoordinates
{
    /// <summary> Geodetic latitude (in radians) </summary>
    public double CoLatitude { get; }

    /// <summary> Altitude above ellipsoid (in km) </summary>
    public double Altitude { get; }

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="latitude">Geodetic latitude</param>
    /// <param name="altitude">Geodetic altitude</param>
    public GeodeticCoordinates(double latitude, double altitude)
    {
        CoLatitude = latitude;
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
    public PolarCoordinates ToPolar()
    {
        const double rEq = 6378.137D;
        const double beta = 6.73949674228e-3;

        double cosxmu = Math.Cos(CoLatitude);
        double sinxmu = Math.Sin(CoLatitude);
        double den = Math.Sqrt(Math.Pow(cosxmu, 2) + Math.Pow(sinxmu / (1.0D + beta), 2));
        double coslam = cosxmu / den;
        double sinlam = sinxmu / (den * (1.0D + beta));
        double rs = rEq / Math.Sqrt(1.0D + beta * Math.Pow(sinlam, 2));
        double x = rs * coslam + Altitude * cosxmu;
        double z = rs * sinlam + Altitude * sinxmu;
        double r = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(z, 2));
        double theta = Math.Acos(z / r);

        return new PolarCoordinates(r, theta);
    }
}
