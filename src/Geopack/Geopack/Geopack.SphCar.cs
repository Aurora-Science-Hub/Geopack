using AuroraScienceHub.Geopack.Contracts;

namespace AuroraScienceHub.Geopack.Geopack;

public sealed partial class Geopack
{
    public Point SphCar(double r, double theta, double phi)
    {
        var sq = r * Math.Sin(theta);
        var x = sq * Math.Cos(phi);
        var y = sq * Math.Sin(phi);
        var z = r * Math.Cos(theta);

        return new Point(x, y, z, r, theta, phi);
    }
}
