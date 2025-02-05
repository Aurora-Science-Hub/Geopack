namespace AuroraScienceHub.Geopack.Geopack08;

public sealed partial class Geopack08
{
    public void BSPCAR_08(
        double theta, double phi, double br, double btheta, double bphi,
        out double bx, out double by, out double bz)
    {
        double s = Math.Sin(theta);
        double c = Math.Cos(theta);
        double sf = Math.Sin(phi);
        double cf = Math.Cos(phi);
        double be = br * s + btheta * c;
        bx = be * cf - bphi * sf;
        by = be * sf + bphi * cf;
        bz = br * c - btheta * s;
    }
}
