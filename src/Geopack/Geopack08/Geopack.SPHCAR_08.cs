namespace AuroraScienceHub.Geopack.Geopack08;

public sealed partial class Geopack08
{
    public void SPHCAR_08(
        double r, double theta, double phi,
        out double x, out double y, out double z)
    {
        double sq = r * Math.Sin(theta);
        x = sq * Math.Cos(phi);
        y = sq * Math.Sin(phi);
        z = r * Math.Cos(theta);
    }
}
