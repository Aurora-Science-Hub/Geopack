using AuroraScienceHub.Geopack.Contracts.Engine;
using AuroraScienceHub.Geopack.Contracts.Models;

namespace AuroraScienceHub.Geopack;

public sealed partial class Geopack
{
    public CartesianLocation GeoMag_08(ComputationContext ctx, double xgeo, double ygeo, double zgeo)
    {
        double xmag = xgeo * ctx.CTCL + ygeo * ctx.CTSL - zgeo * ctx.ST0;
        double ymag = ygeo * ctx.CL0 - xgeo * ctx.SL0;
        double zmag = xgeo * ctx.STCL + ygeo * ctx.STSL + zgeo * ctx.CT0;

        return new CartesianLocation(xmag, ymag, zmag, CoordinateSystem.MAG);
    }

    public CartesianLocation MagGeo_08(ComputationContext ctx, double xmag, double ymag, double zmag)
    {
        double xgeo = xmag * ctx.CTCL - ymag * ctx.SL0 + zmag * ctx.STCL;
        double ygeo = xmag * ctx.CTSL + ymag * ctx.CL0 + zmag * ctx.STSL;
        double zgeo = zmag * ctx.CT0 - xmag * ctx.ST0;

        return new CartesianLocation(xgeo, ygeo, zgeo, CoordinateSystem.GEO);
    }
}
