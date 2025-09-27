using AuroraScienceHub.Geopack.Contracts;

namespace AuroraScienceHub.Geopack.Geopack;

public sealed partial class Geopack
{
    public CartesianLocation GswGse(float xgsw, float ygsw, float zgsw)
    {
        double xgse = xgsw * E11 + ygsw * E21 + z * E31;
        double ygse = xgsw * E12 + y * E22 + z * E32;
        double zgse = xgsw * E13 + y * E23 + z * E33;
        return new CartesianLocation(xgse, ygse, zgse);
    }
}
