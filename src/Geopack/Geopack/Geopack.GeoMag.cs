using AuroraScienceHub.Geopack.Contracts;

namespace AuroraScienceHub.Geopack.Geopack;

public sealed partial class Geopack
{
    public CartesianLocation GeoMag(double xgeo, double ygeo, double zgeo)
    {
        double xmag = xgeo * Common1.CTCL + ygeo * Common1.CTSL - zgeo * Common1.ST0;
        double ymag = ygeo * Common1.CL0 - xgeo * Common1.SL0;
        double zmag = xgeo * Common1.STCL + ygeo * Common1.STSL + zgeo * Common1.CT0;

        return new CartesianLocation(xmag, ymag, zmag, CoordinateSystem.MAG);
    }

    public CartesianLocation MagGeo(double xmag, double ymag, double zmag)
    {
        double xgeo = xmag * Common1.CTCL - ymag * Common1.SL0 + zmag * Common1.STCL;
        double ygeo = xmag * Common1.CTSL + ymag * Common1.CL0 + zmag * Common1.STSL;
        double zgeo = zmag * Common1.CT0 - xmag * Common1.ST0;

        return new CartesianLocation(xgeo, ygeo, zgeo, CoordinateSystem.GEO);
    }
}
