using AuroraScienceHub.Geopack.Contracts.Engine;
using AuroraScienceHub.Geopack.Contracts.Models;

namespace AuroraScienceHub.Geopack;

public sealed partial class Geopack
{
    public CartesianLocation SmGsw_08(ComputationContext context, double xSm, double ySm, double zSm)
    {
        double xGsw = xSm * context.CPS + zSm * context.SPS;
        double yGsw = ySm;
        double zGsw = zSm * context.CPS - xSm * context.SPS;

        return new CartesianLocation(xGsw, yGsw, zGsw, CoordinateSystem.GSW);
    }

    public CartesianLocation GswSm_08(ComputationContext context, double xGsw, double yGsw, double zGsw)
    {
        double xSm = xGsw * context.CPS - zGsw * context.SPS;
        double ySm = yGsw;
        double zSm = xGsw * context.SPS + zGsw * context.CPS;

        return new CartesianLocation(xSm, ySm, zSm, CoordinateSystem.SM);
    }
}
