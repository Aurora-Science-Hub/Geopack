using AuroraScienceHub.Geopack.Contracts;

namespace AuroraScienceHub.Geopack.Geopack;

public sealed partial class Geopack
{
    public MagneticFieldVector BCarSph(
        double x, double y, double z,
        double bx, double by, double bz)
    {
        var rho2 = Math.Pow(x, 2.0D) +  Math.Pow(y, 2.0D);
        var rho = Math.Sqrt(rho2);

        var r = Math.Sqrt(rho2 + Math.Pow(z, 2.0D));

        double cphi;
        double sphi;

        if (rho > 0.0D)
        {
            cphi = x / rho;
            sphi = y / rho;
        }
        else
        {
            cphi = 1.0D;
            sphi = 0.0D;
        }

        var ct = z / r;
        var st = rho / r;

        var br = (x * bx + y * by + z * bz) / r;
        var btheta = (bx * cphi + by * sphi) * ct - bz * st;
        var bphi = by * cphi - bx * sphi;

        return new MagneticFieldVector(bx, by, bz, br, btheta, bphi);
    }
}
