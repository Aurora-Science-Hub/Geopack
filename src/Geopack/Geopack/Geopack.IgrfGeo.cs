using AuroraScienceHub.Geopack.Contracts;

namespace AuroraScienceHub.Geopack.Geopack;

public sealed partial class Geopack
{
    public SphericalFieldVector IgrfGeo(double r, double coLatitude, double phi)
    {
        double c = Math.Cos(coLatitude);
        double s = Math.Sin(coLatitude);
        double cf = Math.Cos(phi);
        double sf = Math.Sin(phi);

        double pp = 1.0D / r;
        double p = pp;

        // IN THIS NEW VERSION, THE OPTIMAL VALUE OF THE PARAMETER NM (MAXIMAL ORDER OF THE SPHERICAL
        // HARMONIC EXPANSION) IS NOT USER-PRESCRIBED, BUT CALCULATED INSIDE THE SUBROUTINE, BASED
        // ON THE VALUE OF THE RADIAL DISTANCE R:

        int irp3 = (int)r + 2;
        int nm = 3 + 30 / irp3;
        if (nm > 13)
        {
            nm = 13;
        }

        int k = nm + 1;
        var a = new double[k];
        var b = new double[k];

        for (var n = 1; n <= k; n++)
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
            if (m is 1)
            {
                x = 0.0D;
                y = 1.0D;
            }
            else
            {
                int mm = m - 1;
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
                double xk = Common2.REC[mn-1];

                double w = e * y + hh * x;
                bbr += b[n-1] * w * q;
                bbt -= an * w * z;

                if (m is not 1)
                {
                    double qq = q;
                    if (s < 1e-5) qq = z;
                    bi += an * (e * x - hh * y) * qq;
                }

                double dp = c * z - s * q - xk * d2;
                double pm = c * q - xk * p2;
                d2 = z;
                p2 = q;
                z = dp;
                q = pm;
            }

            d = s * d + c * p;
            p = s * p;

            if (m is 1)
            {
                continue;
            }

            bi *= (m - 1);
            bbf += bi;
        }

        double br = bbr;
        double btheta = bbt;
        double bphi;

        if (s < 1e-10)
        {
            if (c < 0.0) bbf = -bbf;
            bphi = bbf;
        }
        else
        {
            bphi = bbf / s;
        }

        return new SphericalFieldVector(br, btheta, bphi, CoordinateSystem.GEO);
    }
}
