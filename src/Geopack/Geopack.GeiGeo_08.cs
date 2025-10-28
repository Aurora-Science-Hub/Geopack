using AuroraScienceHub.Geopack.Contracts.Engine;
using AuroraScienceHub.Geopack.Contracts.Models;

namespace AuroraScienceHub.Geopack;

public sealed partial class Geopack
{
    public CartesianLocation GeiGeo_08(ComputationContext ctx, double xGei, double yGei, double zGei)
    {
        double xGeo = xGei * ctx.CGST + yGei * ctx.SGST;
        double yGeo = yGei * ctx.CGST - xGei * ctx.SGST;
        double zGeo = zGei;

        return new CartesianLocation(xGeo, yGeo, zGeo, CoordinateSystem.GEO);
    }

    public CartesianLocation GeoGei_08(ComputationContext ctx, double xGeo, double yGeo, double zGeo)
    {
        double xGei = xGeo * ctx.CGST - yGeo * ctx.SGST;
        double yGei = yGeo * ctx.CGST + xGeo * ctx.SGST;
        double zGei = zGeo;

        return new CartesianLocation(xGei, yGei, zGei, CoordinateSystem.GEI);
    }
}
