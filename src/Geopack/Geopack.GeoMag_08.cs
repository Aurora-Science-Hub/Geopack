using AuroraScienceHub.Geopack.Contracts.Engine;
using AuroraScienceHub.Geopack.Contracts.Models;

namespace AuroraScienceHub.Geopack;

public sealed partial class Geopack
{
    public CartesianLocation GeoMag_08(ComputationContext context, double xgeo, double ygeo, double zgeo)
    {
        double xmag = xgeo * context.CTCL + ygeo * context.CTSL - zgeo * context.ST0;
        double ymag = ygeo * context.CL0 - xgeo * context.SL0;
        double zmag = xgeo * context.STCL + ygeo * context.STSL + zgeo * context.CT0;

        return new CartesianLocation(xmag, ymag, zmag, CoordinateSystem.MAG);
    }

    public CartesianLocation MagGeo_08(ComputationContext context, double xmag, double ymag, double zmag)
    {
        double xgeo = xmag * context.CTCL - ymag * context.SL0 + zmag * context.STCL;
        double ygeo = xmag * context.CTSL + ymag * context.CL0 + zmag * context.STSL;
        double zgeo = zmag * context.CT0 - xmag * context.ST0;

        return new CartesianLocation(xgeo, ygeo, zgeo, CoordinateSystem.GEO);
    }
}
