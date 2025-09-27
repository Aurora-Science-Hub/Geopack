using AuroraScienceHub.Geopack.Contracts;

namespace AuroraScienceHub.Geopack.Geopack;

public sealed partial class Geopack
{
    public CartesianLocation GswGse(double xgsw, double ygsw, double zgsw)
    {
        double xgse = xgsw * Common1.E11 + ygsw * Common1.E21 + zgsw * Common1.E31;
        double ygse = xgsw * Common1.E12 + ygsw * Common1.E22 + zgsw * Common1.E32;
        double zgse = xgsw * Common1.E13 + ygsw * Common1.E23 + zgsw * Common1.E33;
        return new CartesianLocation(xgse, ygse, zgse);
    }
}
