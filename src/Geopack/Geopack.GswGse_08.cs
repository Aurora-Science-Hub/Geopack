using AuroraScienceHub.Geopack.Contracts.Engine;
using AuroraScienceHub.Geopack.Contracts.Models;

namespace AuroraScienceHub.Geopack;

public sealed partial class Geopack
{
    public CartesianLocation GswGse_08(ComputationContext ctx, double xgsw, double ygsw, double zgsw)
    {
        double xgse = xgsw * ctx.E11 + ygsw * ctx.E12 + zgsw * ctx.E13;
        double ygse = xgsw * ctx.E21 + ygsw * ctx.E22 + zgsw * ctx.E23;
        double zgse = xgsw * ctx.E31 + ygsw * ctx.E32 + zgsw * ctx.E33;

        return new CartesianLocation(xgse, ygse, zgse, CoordinateSystem.GSE);
    }

    public CartesianLocation GseGsw_08(ComputationContext ctx, double xgse, double ygse, double zgse)
    {
        double xgsw = xgse * ctx.E11 + ygse * ctx.E21 + zgse * ctx.E31;
        double ygsw = xgse * ctx.E12 + ygse * ctx.E22 + zgse * ctx.E32;
        double zgsw = xgse * ctx.E13 + ygse * ctx.E23 + zgse * ctx.E33;

        return new CartesianLocation(xgsw, ygsw, zgsw, CoordinateSystem.GSW);
    }
}
