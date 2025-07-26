using AuroraScienceHub.Geopack.Contracts;

namespace AuroraScienceHub.Geopack.Geopack08;

public sealed partial class Geopack08
{
    public MagneticFieldVector BSphCar(
        double theta, double phi,
        double br, double btheta, double bphi)
    {
        var s = Math.Sin(theta);
        var c = Math.Cos(theta);
        var sf = Math.Sin(phi);
        var cf = Math.Cos(phi);
        var be = br * s + btheta * c;
        var bx = be * cf - bphi * sf;
        var by = be * sf + bphi * cf;
        var bz = br * c - btheta * s;

        return new MagneticFieldVector(bx, by, bz, br, btheta, bphi);
    }
}
