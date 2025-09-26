using AuroraScienceHub.Geopack.Contracts;

namespace AuroraScienceHub.Geopack.Geopack;

public sealed partial class Geopack
{
    public CartesianFieldVector Dip(double xgsw, double ygsw, double zgsw)
    {
        double dipmom = Math.Sqrt(Math.Pow(Common2.G[1], 2) + Math.Pow(Common2.G[2], 2) + Math.Pow(Common2.H[2], 2));

        double p = Math.Pow(xgsw, 2);
        double u = Math.Pow(zgsw, 2);
        double v = 3.0D * zgsw * xgsw;
        double t = Math.Pow(ygsw, 2);

        double q = dipmom / Math.Pow(Math.Sqrt(p + t + u), 5);

        double bxgsw = q * ((t + u - 2.0D * p) * Common1.SPS - v * Common1.CPS);
        double bygsw = -3.0D * ygsw * q * (xgsw * Common1.SPS + zgsw * Common1.CPS);
        double bzgsw = q * ((p + t - 2.0D * u) * Common1.CPS - v * Common1.SPS);

        return new CartesianFieldVector(bxgsw, bygsw, bzgsw, CoordinateSystem.GSW);
    }
}
