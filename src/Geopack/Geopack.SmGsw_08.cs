using AuroraScienceHub.Geopack.Contracts.Engine;
using AuroraScienceHub.Geopack.Contracts.Models;

namespace AuroraScienceHub.Geopack;

public sealed partial class Geopack
{
    public CartesianLocation SmGsw_08(ComputationContext ctx, double xSm, double ySm, double zSm)
    {
        double xGsw = xSm * ctx.CPS + zSm * ctx.SPS;
        double yGsw = ySm;
        double zGsw = zSm * ctx.CPS - xSm * ctx.SPS;

        return new CartesianLocation(xGsw, yGsw, zGsw, CoordinateSystem.GSW);
    }

    public CartesianLocation GswSm_08(ComputationContext ctx, double xGsw, double yGsw, double zGsw)
    {
        double xSm = xGsw * ctx.CPS - zGsw * ctx.SPS;
        double ySm = yGsw;
        double zSm = xGsw * ctx.SPS + zGsw * ctx.CPS;

        return new CartesianLocation(xSm, ySm, zSm, CoordinateSystem.SM);
    }
}
