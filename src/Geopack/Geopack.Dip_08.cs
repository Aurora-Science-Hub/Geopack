using AuroraScienceHub.Geopack.Contracts.Engine;
using AuroraScienceHub.Geopack.Contracts.Models;

namespace AuroraScienceHub.Geopack;

public sealed partial class Geopack
{
    public CartesianFieldVector Dip_08(ComputationContext ctx, double xgsw, double ygsw, double zgsw)
    {
        double dipmom = Math.Sqrt(Math.Pow(ctx.G[1], 2) + Math.Pow(ctx.G[2], 2) + Math.Pow(ctx.H[2], 2));

        double p = Math.Pow(xgsw, 2);
        double u = Math.Pow(zgsw, 2);
        double v = 3.0D * zgsw * xgsw;
        double t = Math.Pow(ygsw, 2);

        double q = dipmom / Math.Pow(p + t + u, 2.5);

        double bxgsw = q * ((t + u - 2.0D * p) * ctx.SPS - v * ctx.CPS);
        double bygsw = -3.0D * ygsw * q * (xgsw * ctx.SPS + zgsw * ctx.CPS);
        double bzgsw = q * ((p + t - 2.0D * u) * ctx.CPS - v * ctx.SPS);

        return new CartesianFieldVector(bxgsw, bygsw, bzgsw, CoordinateSystem.GSW);
    }
}
