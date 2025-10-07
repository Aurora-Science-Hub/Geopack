using AuroraScienceHub.Geopack.Common;

namespace AuroraScienceHub.Geopack;

public sealed partial class Geopack
{
    public SphericalFieldVector BCarSph(
        double x, double y, double z,
        double bx, double by, double bz)
    {
        double rho2 = Math.Pow(x, 2.0D) +  Math.Pow(y, 2.0D);
        double rho = Math.Sqrt(rho2);

        double r = Math.Sqrt(rho2 + Math.Pow(z, 2.0D));

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

        double ct = z / r;
        double st = rho / r;

        double br = (x * bx + y * by + z * bz) / r;
        double btheta = (bx * cphi + by * sphi) * ct - bz * st;
        double bphi = by * cphi - bx * sphi;

        return new SphericalFieldVector(br, btheta, bphi, null);
    }
}
