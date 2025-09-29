using AuroraScienceHub.Geopack.Contracts;

namespace AuroraScienceHub.Geopack.Geopack;

public sealed partial class Geopack
{
    public GeodeticGeocentricCoordinates GeodGeo(double h, double xmu)
    {
        const double r_eq = 6378.137;
        const double beta = 6.73949674228e-3;

        double cosxmu = Math.Cos(xmu);
        double sinxmu = Math.Sin(xmu);
        double den = Math.Sqrt(cosxmu * cosxmu + (sinxmu / (1.0 + beta)) * (sinxmu / (1.0 + beta)));
        double coslam = cosxmu / den;
        double sinlam = sinxmu / (den * (1.0 + beta));
        double rs = r_eq / Math.Sqrt(1.0 + beta * sinlam * sinlam);
        double x = rs * coslam + h * cosxmu;
        double z = rs * sinlam + h * sinxmu;
        double r = Math.Sqrt(x * x + z * z);
        double theta = Math.Acos(z / r);

        return new GeodeticGeocentricCoordinates(h, xmu, r, theta);
    }

    public GeodeticGeocentricCoordinates GeoGeod(double r, double theta)
    {
        const double r_eq = 6378.137;
        const double beta = 6.73949674228e-3;
        const double tol = 1e-6;

        int n = 0;
        double phi = 1.570796327 - theta;
        double phi1 = phi;

        double xmus, rs, cosfims, h, z, x, rr, dphi;

        do
        {
            double sp = Math.Sin(phi1);
            double arg = sp * (1.0 + beta) / Math.Sqrt(1.0 + beta * (2.0 + beta) * sp * sp);
            xmus = Math.Asin(arg);
            rs = r_eq / Math.Sqrt(1.0 + beta * Math.Sin(phi1) * Math.Sin(phi1));
            cosfims = Math.Cos(phi1 - xmus);
            h = Math.Sqrt((rs * cosfims) * (rs * cosfims) + r * r - rs * rs) - rs * cosfims;
            z = rs * Math.Sin(phi1) + h * Math.Sin(xmus);
            x = rs * Math.Cos(phi1) + h * Math.Cos(xmus);
            rr = Math.Sqrt(x * x + z * z);
            dphi = Math.Asin(z / rr) - phi;
            phi1 = phi1 - dphi;
            n++;
        }
        while (Math.Abs(dphi) > tol && n < 100);

        return new GeodeticGeocentricCoordinates(h, xmus, r, theta);
    }
}
