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
        const double r_eq = 6378.137;
        const double beta = 6.73949674228e-3;
        const double tol = 1e-6;
        const double beta1 = 1.0 + beta;
        const double beta2 = beta * (2.0 + beta);

        int n = 0;
        double phi = 1.570796327 - Theta;
        double phi1 = phi;

        double xmus, rs, cosfims, h, z, x, rr, dphi;
        double r2 = R * R;

        do
        {
            (double sinPhi1, double cosPhi1) = Math.SinCos(phi1);
            double sp2 = sinPhi1 * sinPhi1;

            double arg = sinPhi1 * beta1 / Math.Sqrt(1.0 + beta2 * sp2);
            xmus = Math.Asin(arg);

            rs = r_eq / Math.Sqrt(1.0 + beta * sp2);
            cosfims = Math.Cos(phi1 - xmus);

            double rsCosfims = rs * cosfims;
            double rs2 = rs * rs;
            h = Math.Sqrt(rsCosfims * rsCosfims + r2 - rs2) - rsCosfims;

            (double sinXmus, double cosXmus) = Math.SinCos(xmus);

            z = rs * sinPhi1 + h * sinXmus;
            x = rs * cosPhi1 + h * cosXmus;

            rr = Math.Sqrt(x * x + z * z);
            dphi = Math.Asin(z / rr) - phi;
            phi1 -= dphi;
            n++;
        }
        while (Math.Abs(dphi) > tol && n < 100);

        return new GeodeticCoordinates(xmus, h);
    }
};
