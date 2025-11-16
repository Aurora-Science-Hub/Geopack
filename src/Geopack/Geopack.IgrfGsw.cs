using AuroraScienceHub.Geopack.Contracts.Cartesian;
using AuroraScienceHub.Geopack.Contracts.Coordinates;
using AuroraScienceHub.Geopack.Contracts.Engine;
using AuroraScienceHub.Geopack.Contracts.PhysicalQuantities;

namespace AuroraScienceHub.Geopack;

internal sealed partial class Geopack
{
    public CartesianVector<MagneticField> IgrfGsw(ComputationContext context, CartesianLocation location)
    {
        if (location.CoordinateSystem is not CoordinateSystem.GSW)
        {
            throw new InvalidOperationException("Location must be in GSW coordinate system.");
        }

        CartesianLocation geoLocation = GswToGeo(context, location);

        double geoX = geoLocation.X;
        double geoY = geoLocation.Y;
        double geoZ = geoLocation.Z;

        double x2 = geoX * geoX;
        double y2 = geoY * geoY;
        double z2 = geoZ * geoZ;
        double rho2 = x2 + y2;
        double r = Math.Sqrt(rho2 + z2);

        if (r <= double.Epsilon)
        {
            throw new InvalidOperationException("Location radius vector should not be zero.");
        }

        double rInv = 1.0 / r;
        double c = geoZ * rInv;
        double rho = Math.Sqrt(rho2);
        double s = rho * rInv;

        double cf, sf;
        if (s < 1e-10)
        {
            cf = 1.0;
            sf = 0.0;
        }
        else
        {
            double rhoInv = 1.0 / rho;
            cf = geoX * rhoInv;
            sf = geoY * rhoInv;
        }

        double p = rInv;

        // Calculate optimal expansion order
        int irp3 = (int)r + 2;
        int nm = 3 + 30 / irp3;
        if (nm > 13)
        {
            nm = 13;
        }

        int k = nm + 1;

        // Use stack allocation for small arrays instead of heap allocation
        Span<double> a = stackalloc double[k + 1];
        Span<double> b = stackalloc double[k + 1];

        for (int n = 1; n <= k; n++)
        {
            p *= rInv;
            a[n - 1] = p;
            b[n - 1] = p * n;
        }

        p = 1.0;
        double d = 0.0;
        double bbr = 0.0;
        double bbt = 0.0;
        double bbf = 0.0;

        double x = 0.0, y = 0.0;

        // Cache context arrays
        double[] contextG = context.G;
        double[] contextH = context.H;
        double[] contextREC = context.REC;

        bool sIsSmall = s < 1e-10;

        for (int m = 1; m <= k; m++)
        {
            if (m == 1)
            {
                x = 0.0;
                y = 1.0;
            }
            else
            {
                double w = x;
                x = w * cf + y * sf;
                y = y * cf - w * sf;
            }

            double q = p;
            double z = d;
            double bi = 0.0;
            double p2 = 0.0;
            double d2 = 0.0;

            for (int n = m; n <= k; n++)
            {
                double an = a[n - 1];
                int mnIdx = n * (n - 1) / 2 + m - 1;
                double e = contextG[mnIdx];
                double hh = contextH[mnIdx];
                double wVal = e * y + hh * x;
                double bnVal = b[n - 1];
                bbr += bnVal * wVal * q;
                bbt -= an * wVal * z;

                if (m != 1)
                {
                    double qq = sIsSmall ? z : q;
                    bi += an * (e * x - hh * y) * qq;
                }

                double xk = contextREC[mnIdx];
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

        double bf;
        if (s < 1e-10)
        {
            if (c < 0.0)
                bbf = -bbf;
            bf = bbf;
        }
        else
        {
            bf = bbf / s;
        }

        double he = bbr * s + bbt * c;
        double bx = he * cf - bf * sf;
        double by = he * sf + bf * cf;
        double bz = bbr * c - bbt * s;

        return GeoToGsw(context, CartesianVector<MagneticField>.New(bx, by, bz, CoordinateSystem.GEO));
    }
}
