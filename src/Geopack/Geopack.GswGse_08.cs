using AuroraScienceHub.Geopack.Contracts.Engine;
using AuroraScienceHub.Geopack.Contracts.Models;

namespace AuroraScienceHub.Geopack;

public sealed partial class Geopack
{
    public CartesianLocation GswGse_08(ComputationContext context, double xgsw, double ygsw, double zgsw)
    {
        double xgse = xgsw * context.E11 + ygsw * context.E12 + zgsw * context.E13;
        double ygse = xgsw * context.E21 + ygsw * context.E22 + zgsw * context.E23;
        double zgse = xgsw * context.E31 + ygsw * context.E32 + zgsw * context.E33;

        return new CartesianLocation(xgse, ygse, zgse, CoordinateSystem.GSE);
    }

    public CartesianLocation GseGsw_08(ComputationContext context, double xgse, double ygse, double zgse)
    {
        double xgsw = xgse * context.E11 + ygse * context.E21 + zgse * context.E31;
        double ygsw = xgse * context.E12 + ygse * context.E22 + zgse * context.E32;
        double zgsw = xgse * context.E13 + ygse * context.E23 + zgse * context.E33;

        return new CartesianLocation(xgsw, ygsw, zgsw, CoordinateSystem.GSW);
    }
}
