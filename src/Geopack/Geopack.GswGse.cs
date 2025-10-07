using AuroraScienceHub.Geopack.Common;

namespace AuroraScienceHub.Geopack;

public sealed partial class Geopack
{
    public CartesianLocation GswGse(double xgsw, double ygsw, double zgsw)
    {
        double xgse = xgsw * Common1.E11 + ygsw * Common1.E12 + zgsw * Common1.E13;
        double ygse = xgsw * Common1.E21 + ygsw * Common1.E22 + zgsw * Common1.E23;
        double zgse = xgsw * Common1.E31 + ygsw * Common1.E32 + zgsw * Common1.E33;

        return new CartesianLocation(xgse, ygse, zgse, CoordinateSystem.GSE);
    }

    public CartesianLocation GseGsw(double xgse, double ygse, double zgse)
    {
        double xgsw = xgse * Common1.E11 + ygse * Common1.E21 + zgse * Common1.E31;
        double ygsw = xgse * Common1.E12 + ygse * Common1.E22 + zgse * Common1.E32;
        double zgsw = xgse * Common1.E13 + ygse * Common1.E23 + zgse * Common1.E33;

        return new CartesianLocation(xgsw, ygsw, zgsw, CoordinateSystem.GSW);
    }
}
