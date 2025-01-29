namespace AuroraScienceHub.Geopack.Geopack08;

public sealed partial class Geopack08
{
    public void SPHCAR_08(
        float r, float theta, float phi,
        out float x, out float y, out float z)
    {
        float sq = r * MathF.Sin(theta);
        x = sq * MathF.Cos(phi);
        y = sq * MathF.Sin(phi);
        z = r * MathF.Cos(theta);
    }
}
