namespace AuroraScienceHub.Geopack.Geopack08;

public sealed partial class Geopack08
{
    public void CARSPH_08(
        double x, double y, double z,
        out double r, out double theta, out double phi)
    {
        double sq = x * x + y * y;
        r = Math.Sqrt(sq + z * z);
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
            phi = 0.0d;
            theta = z < 0.0d ? Pi : 0.0d;
        }
    }
}
