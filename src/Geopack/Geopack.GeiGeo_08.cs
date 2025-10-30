using AuroraScienceHub.Geopack.Contracts.Engine;
using AuroraScienceHub.Geopack.Contracts.Models;

namespace AuroraScienceHub.Geopack;

public sealed partial class Geopack
{
    public CartesianLocation GeiGeo_08(ComputationContext context, double xGei, double yGei, double zGei)
    {
        double xGeo = xGei * context.CGST + yGei * context.SGST;
        double yGeo = yGei * context.CGST - xGei * context.SGST;
        double zGeo = zGei;

        return new CartesianLocation(xGeo, yGeo, zGeo, CoordinateSystem.GEO);
    }

    public CartesianLocation GeoGei_08(ComputationContext context, double xGeo, double yGeo, double zGeo)
    {
        double xGei = xGeo * context.CGST - yGeo * context.SGST;
        double yGei = yGeo * context.CGST + xGeo * context.SGST;
        double zGei = zGeo;

        return new CartesianLocation(xGei, yGei, zGei, CoordinateSystem.GEI);
    }
}
