using AuroraScienceHub.Geopack.Contracts;

namespace AuroraScienceHub.Geopack.Geopack;

public sealed partial class Geopack
{
    public MagneticFieldVector IgrfGeo(double r, double theta, double phi)
    {
        var c = Math.Cos(theta);
        var s = Math.Sin(theta);
        var cf = Math.Cos(phi);
        var sf = Math.Sin(phi);

        var pp = 1.0 / r;
        var p = pp;

        // IN THIS NEW VERSION, THE OPTIMAL VALUE OF THE PARAMETER NM (MAXIMAL ORDER OF THE SPHERICAL
        // HARMONIC EXPANSION) IS NOT USER-PRESCRIBED, BUT CALCULATED INSIDE THE SUBROUTINE, BASED
        // ON THE VALUE OF THE RADIAL DISTANCE R:

        var irp3 = (int)r + 2;
        var nm = 3 + 30 / irp3;
        if (nm > 13)
        {
            nm = 13;
        }

        var k = nm + 1;
        var a = new double[k + 1]; // ?????
        var b = new double[k + 1]; // ?????

        for (var n = 1; n <= k; n++)
        {
            p *= pp;
            a[n] = p;
            b[n] = p * n;
        }

        p = 1.0;
        var d = 0.0;
        var bbr = 0.0;
        var bbt = 0.0;
        var bbf = 0.0;

        double x = 0.0, y = 0.0;

        for (var m = 1; m <= k; m++)
        {
            if (m is 1)
            {
                x = 0.0;
                y = 1.0;
            }
            else
            {
                var mm = m - 1;
                var w = x;
                x = w * cf + y * sf;
                y = y * cf - w * sf;
            }

            var q = p;
            var z = d;
            var bi = 0.0;
            var p2 = 0.0;
            var d2 = 0.0;

            for (var n = m; n <= k; n++)
            {
                var an = a[n];
                var mn = n * (n - 1) / 2 + m;
                var e = Common2.G[mn];
                var hh = Common2.H[mn];
                var xk = Common2.REC[mn];

                var w = e * y + hh * x;
                bbr += b[n] * w * q;
                bbt -= an * w * z;

                if (m is not 1)
                {
                    var qq = q;
                    if (s < 1e-5) qq = z;
                    bi += an * (e * x - hh * y) * qq;
                }

                var dp = c * z - s * q - xk * d2;
                var pm = c * q - xk * p2;
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

        var br = bbr;
        var btheta = bbt;
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

        return BSphCar(theta, phi, br, btheta, bphi);
    }
}
