using AuroraScienceHub.Geopack.Common.Contracts;

namespace AuroraScienceHub.Geopack;

public sealed partial class Geopack
{
    public CartesianFieldVector IgrfGsw(double xgsw, double ygsw, double zgsw)
    {
        CartesianLocation geoLocation = GswGeo(xgsw, ygsw, zgsw);
        double xgeo = geoLocation.X;
        double ygeo = geoLocation.Y;
        double zgeo = geoLocation.Z;

        double rho2 = xgeo * xgeo + ygeo * ygeo;
        double r = Math.Sqrt(rho2 + zgeo * zgeo);
        double c = zgeo / r;
        double rho = Math.Sqrt(rho2);
        double s = rho / r;

        double cf, sf;
        if (s < 1e-10)
        {
            cf = 1.0;
            sf = 0.0;
        }
        else
        {
            cf = xgeo / rho;
            sf = ygeo / rho;
        }

        double pp = 1.0D / r;
        double p = pp;

        // Calculate optimal expansion order
        int irp3 = (int)r + 2;
        int nm = 3 + 30 / irp3;
        if (nm > 13) nm = 13;

        int k = nm + 1;
        double[] a = new double[k + 1];
        double[] b = new double[k + 1];

        for (int n = 1; n <= k; n++)
        {
            p *= pp;
            a[n-1] = p;
            b[n-1] = p * n;
        }

        p = 1.0D;
        double d = 0.0D;
        double bbr = 0.0D;
        double bbt = 0.0D;
        double bbf = 0.0D;

        double x = 0.0D, y = 0.0D;

        for (int m = 1; m <= k; m++)
        {
            if (m == 1)
            {
                x = 0.0D;
                y = 1.0D;
            }
            else
            {
                double w = x;
                x = w * cf + y * sf;
                y = y * cf - w * sf;
            }

            double q = p;
            double z = d;
            double bi = 0.0D;
            double p2 = 0.0D;
            double d2 = 0.0D;

            for (int n = m; n <= k; n++)
            {
                double an = a[n-1];
                int mn = n * (n - 1) / 2 + m;
                double e = Common2.G[mn-1];
                double hh = Common2.H[mn-1];
                double w_val = e * y + hh * x;
                bbr += b[n-1] * w_val * q;
                bbt -= an * w_val * z;

                if (m != 1)
                {
                    double qq = q;
                    if (s < 1e-10) qq = z;
                    bi += an * (e * x - hh * y) * qq;
                }

                double xk = Common2.REC[mn-1];
                double dp = c * z - s * q - xk * d2;
                double pm = c * q - xk * p2;
                d2 = z;
                p2 = q;
                z = dp;
                q = pm;
            }

            d = s * d + c * p;
            p = s * p;

            if (m != 1)
            {
                bi *= (m - 1);
                bbf += bi;
            }
        }

        double br = bbr;
        double bt = bbt;
        double bf;

        if (s < 1e-10)
        {
            if (c < 0.0) bbf = -bbf;
            bf = bbf;
        }
        else
        {
            bf = bbf / s;
        }

        // Convert spherical to Cartesian in GEO
        double he = br * s + bt * c;
        double hxgeo = he * cf - bf * sf;
        double hygeo = he * sf + bf * cf;
        double hzgeo = br * c - bt * s;

        // Convert GEO to GSW
        CartesianLocation gswField = GeoGsw(hxgeo, hygeo, hzgeo);

        return new CartesianFieldVector(gswField.X, gswField.Y, gswField.Z, CoordinateSystem.GSW);
    }
}
