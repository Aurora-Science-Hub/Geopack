using AuroraScienceHub.Geopack.Contracts;

namespace AuroraScienceHub.Geopack.Geopack;

public sealed partial class Geopack
{
    public GeodeticGeocentricCoordinates GeodGeo(double h, double xmu)
    {
        const double r_eq = 6378.137D;
        const double beta = 6.73949674228e-3D;

        double cosxmu = Math.Cos(xmu);
        double sinxmu = Math.Sin(xmu);
        double den = Math.Sqrt(cosxmu * cosxmu + (sinxmu / (1.0D + beta)) * (sinxmu / (1.0D + beta)));
        double coslam = cosxmu / den;
        double sinlam = sinxmu / (den * (1.0D + beta));
        double rs = r_eq / Math.Sqrt(1.0D + beta * sinlam * sinlam);
        double x = rs * coslam + h * cosxmu;
        double z = rs * sinlam + h * sinxmu;
        double r = Math.Sqrt(x * x + z * z);
        double theta = Math.Acos(z / r);

        return new GeodeticGeocentricCoordinates(h, xmu, r, theta);
    }

    public GeodeticGeocentricCoordinates GeoGeod(double r, double theta)
    {
        const double r_eq = 6378.137D;
        const double beta = 6.73949674228e-3D;
        const double tol = 1e-6D;

        int n = 0;
        double phi = 1.570796327D - theta;
        double phi1 = phi;

        double xmus, rs, cosfims, h, z, x, rr, dphi;

        do
        {
            double sp = Math.Sin(phi1);
            double arg = sp * (1.0D + beta) / Math.Sqrt(1.0D + beta * (2.0D + beta) * sp * sp);
            xmus = Math.Asin(arg);
            rs = r_eq / Math.Sqrt(1.0D + beta * Math.Sin(phi1) * Math.Sin(phi1));
            cosfims = Math.Cos(phi1 - xmus);
            h = Math.Sqrt((rs * cosfims) * (rs * cosfims) + r * r - rs * rs) - rs * cosfims;
            z = rs * Math.Sin(phi1) + h * Math.Sin(xmus);
            x = rs * Math.Cos(phi1) + h * Math.Cos(xmus);
            rr = Math.Sqrt(x * x + z * z);
            dphi = Math.Asin(z / rr) - phi;
            phi1 -= dphi;
            n++;
        }
        while (Math.Abs(dphi) > tol && n < 100);

        return new GeodeticGeocentricCoordinates(h, xmus, r, theta);
    }
}
