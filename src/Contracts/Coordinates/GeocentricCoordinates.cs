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
        const double r_eq = 6378.137D;
        const double beta = 6.73949674228e-3;
        const double tol = 1e-6;

        int n = 0;
        double phi = 1.570796327D - Theta;
        double phi1 = phi;

        double xmus, rs, cosfims, h, z, x, rr, dphi;

        do
        {
            double sp = Math.Sin(phi1);
            double arg = sp * (1.0D + beta) / Math.Sqrt(1.0D + beta * (2.0D + beta) * Math.Pow(sp, 2));
            xmus = Math.Asin(arg);
            rs = r_eq / Math.Sqrt(1.0D + beta * Math.Pow(Math.Sin(phi1), 2));
            cosfims = Math.Cos(phi1 - xmus);
            h = Math.Sqrt(rs * cosfims * rs * cosfims + R * R - rs * rs) - rs * cosfims;
            z = rs * Math.Sin(phi1) + h * Math.Sin(xmus);
            x = rs * Math.Cos(phi1) + h * Math.Cos(xmus);
            rr = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(z, 2));
            dphi = Math.Asin(z / rr) - phi;
            phi1 -= dphi;
            n++;
        }
        while (Math.Abs(dphi) > tol && n < 100);

        return new GeodeticCoordinates(xmus, h);
    }
};
