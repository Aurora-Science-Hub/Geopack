using Common.Contracts;

namespace AuroraScienceHub.Geopack;

public sealed partial class Geopack
{
    public CartesianLocation GeiGeo(double xGei, double yGei, double zGei)
    {
        double xGeo = xGei * Common1.CGST + yGei * Common1.SGST;
        double yGeo = yGei * Common1.CGST - xGei * Common1.SGST;
        double zGeo = zGei;

        return new CartesianLocation(xGeo, yGeo, zGeo, CoordinateSystem.GEO);
    }

    public CartesianLocation GeoGei(double xGeo, double yGeo, double zGeo)
    {
        double xGei = xGeo * Common1.CGST - yGeo * Common1.SGST;
        double yGei = yGeo * Common1.CGST + xGeo * Common1.SGST;
        double zGei = zGeo;

        return new CartesianLocation(xGei, yGei, zGei, CoordinateSystem.GEI);
    }
}
