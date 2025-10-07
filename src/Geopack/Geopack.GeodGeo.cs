using AuroraScienceHub.Geopack.Common.Contracts;

namespace AuroraScienceHub.Geopack;

public sealed partial class Geopack
{
    public GeodeticGeocentricCoordinates GeodGeo(double h, double xmu)
    {
        const double r_eq = 6378.137D;
        const double beta = 6.73949674228e-3;

        double cosxmu = Math.Cos(xmu);
        double sinxmu = Math.Sin(xmu);
        double den = Math.Sqrt(Math.Pow(cosxmu, 2) + Math.Pow(sinxmu / (1.0D + beta), 2));
        double coslam = cosxmu / den;
        double sinlam = sinxmu / (den * (1.0D + beta));
        double rs = r_eq / Math.Sqrt(1.0D + beta * Math.Pow(sinlam, 2));
        double x = rs * coslam + h * cosxmu;
        double z = rs * sinlam + h * sinxmu;
        double r = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(z, 2));
        double theta = Math.Acos(z / r);

        return new GeodeticGeocentricCoordinates(h, xmu, r, theta);
    }

    public GeodeticGeocentricCoordinates GeoGeod(double r, double theta)
    {
        const double r_eq = 6378.137D;
        const double beta = 6.73949674228e-3;
        const double tol = 1e-6;

        int n = 0;
        double phi = 1.570796327D - theta;
        double phi1 = phi;

        double xmus, rs, cosfims, h, z, x, rr, dphi;

        do
        {
            double sp = Math.Sin(phi1);
            double arg = sp * (1.0D + beta) / Math.Sqrt(1.0D + beta * (2.0D + beta) * Math.Pow(sp, 2));
            xmus = Math.Asin(arg);
            rs = r_eq / Math.Sqrt(1.0D + beta * Math.Pow(Math.Sin(phi1), 2));
            cosfims = Math.Cos(phi1 - xmus);
            h = Math.Sqrt(Math.Pow(rs * cosfims, 2) + Math.Pow(r, 2) - Math.Pow(rs, 2)) - rs * cosfims;
            z = rs * Math.Sin(phi1) + h * Math.Sin(xmus);
            x = rs * Math.Cos(phi1) + h * Math.Cos(xmus);
            rr = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(z, 2));
            dphi = Math.Asin(z / rr) - phi;
            phi1 -= dphi;
            n++;
        }
        while (Math.Abs(dphi) > tol && n < 100);

        return new GeodeticGeocentricCoordinates(h, xmus, r, theta);
    }
}
