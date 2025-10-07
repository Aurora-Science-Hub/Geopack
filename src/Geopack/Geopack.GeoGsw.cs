using AuroraScienceHub.Geopack.Common;

namespace AuroraScienceHub.Geopack;

public sealed partial class Geopack
{
    public CartesianLocation GeoGsw(double xGeo, double yGeo, double zGeo)
    {
        double xGsw = Common1.A11 * xGeo + Common1.A12 * yGeo + Common1.A13 * zGeo;
        double yGsw = Common1.A21 * xGeo + Common1.A22 * yGeo + Common1.A23 * zGeo;
        double zGsw = Common1.A31 * xGeo + Common1.A32 * yGeo + Common1.A33 * zGeo;
        return new CartesianLocation(xGsw, yGsw, zGsw, CoordinateSystem.GSW);
    }

    public CartesianLocation GswGeo(double xGsw, double yGsw, double zGsw)
    {
        double xGeo = Common1.A11 * xGsw + Common1.A21 * yGsw + Common1.A31 * zGsw;
        double yGeo = Common1.A12 * xGsw + Common1.A22 * yGsw + Common1.A32 * zGsw;
        double zGeo = Common1.A13 * xGsw + Common1.A23 * yGsw + Common1.A33 * zGsw;
        return new CartesianLocation(xGeo, yGeo, zGeo, CoordinateSystem.GEO);
    }
}
