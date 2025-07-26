using AuroraScienceHub.Geopack.Contracts;

namespace AuroraScienceHub.Geopack.Geopack08;

public sealed partial class Geopack08
{
    public Point CarSph_08(double x, double y, double z)
    {
        double phi;
        double theta;
        var sq = x * x + y * y;
        var r = Math.Sqrt(sq + z * z);

        if (sq != 0.0d)
        {
            sq = Math.Sqrt(sq);
            phi = Math.Atan2(y, x);
            theta = Math.Atan2(sq, z);
            if (phi < 0.0d)
            {
                phi += TwoPi;
            }
        }
        else
        {
            phi = 0.0;
            theta = z < 0.0d ? Pi : 0.0d;
        }

        return new Point(x, y, x, r, theta, phi);
    }
}
