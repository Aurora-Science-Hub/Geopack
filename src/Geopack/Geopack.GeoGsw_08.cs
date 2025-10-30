using AuroraScienceHub.Geopack.Contracts.Engine;
using AuroraScienceHub.Geopack.Contracts.Models;

namespace AuroraScienceHub.Geopack;

public sealed partial class Geopack
{
    public CartesianLocation GeoGsw_08(ComputationContext context, double xGeo, double yGeo, double zGeo)
    {
        double xGsw = context.A11 * xGeo + context.A12 * yGeo + context.A13 * zGeo;
        double yGsw = context.A21 * xGeo + context.A22 * yGeo + context.A23 * zGeo;
        double zGsw = context.A31 * xGeo + context.A32 * yGeo + context.A33 * zGeo;
        return new CartesianLocation(xGsw, yGsw, zGsw, CoordinateSystem.GSW);
    }

    public CartesianLocation GswGeo_08(ComputationContext context, double xGsw, double yGsw, double zGsw)
    {
        double xGeo = context.A11 * xGsw + context.A21 * yGsw + context.A31 * zGsw;
        double yGeo = context.A12 * xGsw + context.A22 * yGsw + context.A32 * zGsw;
        double zGeo = context.A13 * xGsw + context.A23 * yGsw + context.A33 * zGsw;
        return new CartesianLocation(xGeo, yGeo, zGeo, CoordinateSystem.GEO);
    }
}
