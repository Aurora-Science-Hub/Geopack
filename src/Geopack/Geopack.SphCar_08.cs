using AuroraScienceHub.Geopack.Contracts.Models;

namespace AuroraScienceHub.Geopack;

public sealed partial class Geopack
{
    public CartesianLocation SphCar_08(double r, double theta, double phi)
    {
        double sq = r * Math.Sin(theta);
        double x = sq * Math.Cos(phi);
        double y = sq * Math.Sin(phi);
        double z = r * Math.Cos(theta);

        return new CartesianLocation(x, y, z);
    }

    public SphericalLocation CarSph_08(double x, double y, double z)
    {
        double phi;
        double theta;
        double sq = x * x + y * y;
        double r = Math.Sqrt(sq + z * z);

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

        return new SphericalLocation(r, theta, phi);
    }
}
