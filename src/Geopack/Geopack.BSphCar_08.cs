using AuroraScienceHub.Geopack.Contracts.Models;

namespace AuroraScienceHub.Geopack;

public sealed partial class Geopack
{
    public CartesianFieldVector BSphCar_08(
        double theta, double phi,
        double br, double btheta, double bphi)
    {
        double s = Math.Sin(theta);
        double c = Math.Cos(theta);
        double sf = Math.Sin(phi);
        double cf = Math.Cos(phi);
        double be = br * s + btheta * c;
        double bx = be * cf - bphi * sf;
        double by = be * sf + bphi * cf;
        double bz = br * c - btheta * s;

        return new CartesianFieldVector(bx, by, bz, null);
    }
}
