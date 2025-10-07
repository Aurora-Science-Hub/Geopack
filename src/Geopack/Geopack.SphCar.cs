using AuroraScienceHub.Geopack.Common.Contracts;

namespace AuroraScienceHub.Geopack;

public sealed partial class Geopack
{
    public CartesianLocation SphCar(double r, double theta, double phi)
    {
        var sq = r * Math.Sin(theta);
        var x = sq * Math.Cos(phi);
        var y = sq * Math.Sin(phi);
        var z = r * Math.Cos(theta);

        return new CartesianLocation(x, y, z);
    }
}
