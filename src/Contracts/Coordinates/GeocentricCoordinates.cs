using AuroraScienceHub.Geopack.Contracts.Engine;

namespace AuroraScienceHub.Geopack.Contracts.Coordinates;

/// <summary>
/// Represents (R, Theta) slice of ECEF spherical coordinates
/// </summary>
public readonly record struct GeocentricCoordinates
{
    /// <summary>
    /// Geocentric distance (in km, ECEF radial)
    /// </summary>
    public double R { get; }

    /// <summary>
    /// Spherical co-latitude (in radians, ECEF polar angle)
    /// </summary>
    public double Theta { get; }

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="r">Geocentric distance (in km, ECEF radial)</param>
    /// <param name="theta">Spherical co-latitude (in radians, ECEF polar angle)</param>
    public GeocentricCoordinates(double r, double theta)
    {
        R = r;
        Theta = theta;
    }

    /// <summary>
    /// Converts geocentric coordinates R and THETA into vertical local height (altitude) H and geodetic latitude.
    /// </summary>
    /// <remarks>
    /// Original Geopack-2008 method: GEODGEO_08.
    /// The subroutine uses World Geodetic System WGS84 parameters for the Earth's ellipsoid. The angular quantities
    /// (geo co-latitude THETA and geodetic latitude XMU) are in radians, and the distances (geocentric radius R and
    /// altitude H above the Earth's ellipsoid) are in kilometers.
    /// </remarks>
    public GeodeticCoordinates ToGeodetic()
    {
        int n = 0;
        double phi = GeopackConstants.HalfPi - Theta;
        double phi1 = phi;
        double r2 = R * R;

        double xmus, h, dphi;

        do
        {
            (double sinPhi, double cosPhi) = Math.SinCos(phi1);
            double sp2 = sinPhi * sinPhi;
            double arg = sinPhi * GeopackConstants.WGS84Ex / Math.Sqrt(1.0D + GeopackConstants.WGS84FirstEx * sp2);
            double rs = GeopackConstants.REq / Math.Sqrt(1.0D + GeopackConstants.WGS84Beta * sp2);
            double rs2 = rs * rs;
            xmus = Math.Asin(arg);
            (double sinXmus, double cosXmus) = Math.SinCos(xmus);

            double cosfims = Math.Cos(phi1 - xmus);
            h = Math.Sqrt(rs2 * cosfims * cosfims + r2 - rs2) - rs * cosfims;
            double z = rs * sinPhi + h * sinXmus;
            double x = rs * cosPhi + h * cosXmus;
            double rr = Math.Sqrt(x * x + z * z);
            dphi = Math.Asin(z / rr) - phi;
            phi1 -= dphi;
            n++;
        }
        while (Math.Abs(dphi) > GeopackConstants.CoordinateConvergenceTolerance && n < 100);

        return new GeodeticCoordinates(xmus, h);
    }
};
