using AuroraScienceHub.Geopack.Common;

namespace AuroraScienceHub.Geopack;

public sealed partial class Geopack
{
    public CartesianLocation SphCar(double r, double theta, double phi)
    {
        double sq = r * Math.Sin(theta);
        double x = sq * Math.Cos(phi);
        double y = sq * Math.Sin(phi);
        double z = r * Math.Cos(theta);

        return new CartesianLocation(x, y, z);
    }
}
