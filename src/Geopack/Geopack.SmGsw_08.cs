using AuroraScienceHub.Geopack.Contracts.Models;

namespace AuroraScienceHub.Geopack;

public sealed partial class Geopack
{
    public CartesianLocation SmGsw_08(double xSm, double ySm, double zSm)
    {
        double xGsw = xSm * Common1.CPS + zSm * Common1.SPS;
        double yGsw = ySm;
        double zGsw = zSm * Common1.CPS - xSm * Common1.SPS;

        return new CartesianLocation(xGsw, yGsw, zGsw, CoordinateSystem.GSW);
    }

    public CartesianLocation GswSm_08(double xGsw, double yGsw, double zGsw)
    {
        double xSm = xGsw * Common1.CPS - zGsw * Common1.SPS;
        double ySm = yGsw;
        double zSm = xGsw * Common1.SPS + zGsw * Common1.CPS;

        return new CartesianLocation(xSm, ySm, zSm, CoordinateSystem.SM);
    }
}
