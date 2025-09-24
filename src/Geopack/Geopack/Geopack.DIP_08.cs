using AuroraScienceHub.Geopack.Contracts;

namespace AuroraScienceHub.Geopack.Geopack;

public sealed partial class Geopack
{
    public CartesianFieldVector Dip(float xgsw, float ygsw, float zgsw)
    {
        double dipmom = Math.Sqrt(Common2.G[2] * Common2.G[2]
                                  + Common2.G[3] * Common2.G[3]
                                  + Common2.H[3] * Common2.H[3]);

        double p = xgsw * xgsw;
        double u = zgsw * zgsw;
        double v = 3.0 * zgsw * xgsw;
        double t = ygsw * ygsw;

        double q = dipmom / Math.Pow(Math.Sqrt(p + t + u), 5);

        double bxgsw = q * ((t + u - 2.0 * p) * Common1.SPS - v * Common1.CPS);
        double bygsw = -3.0 * ygsw * q * (xgsw * Common1.SPS + zgsw * Common1.CPS);
        double bzgsw = q * ((p + t - 2.0 * u) * Common1.CPS - v * Common1.SPS);

        return new CartesianFieldVector(bxgsw, bygsw, bzgsw, CoordinateSystem.GSW);
    }
}
