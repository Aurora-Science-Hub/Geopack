using AuroraScienceHub.Geopack.Contracts.Engine;
using AuroraScienceHub.Geopack.Contracts.Models;

namespace AuroraScienceHub.Geopack;

public sealed partial class Geopack
{
    public CartesianLocation GeoGsw_08(ComputationContext ctx, double xGeo, double yGeo, double zGeo)
    {
        double xGsw = ctx.A11 * xGeo + ctx.A12 * yGeo + ctx.A13 * zGeo;
        double yGsw = ctx.A21 * xGeo + ctx.A22 * yGeo + ctx.A23 * zGeo;
        double zGsw = ctx.A31 * xGeo + ctx.A32 * yGeo + ctx.A33 * zGeo;
        return new CartesianLocation(xGsw, yGsw, zGsw, CoordinateSystem.GSW);
    }

    public CartesianLocation GswGeo_08(ComputationContext ctx, double xGsw, double yGsw, double zGsw)
    {
        double xGeo = ctx.A11 * xGsw + ctx.A21 * yGsw + ctx.A31 * zGsw;
        double yGeo = ctx.A12 * xGsw + ctx.A22 * yGsw + ctx.A32 * zGsw;
        double zGeo = ctx.A13 * xGsw + ctx.A23 * yGsw + ctx.A33 * zGsw;
        return new CartesianLocation(xGeo, yGeo, zGeo, CoordinateSystem.GEO);
    }
}
