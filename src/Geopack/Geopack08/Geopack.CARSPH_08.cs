namespace AuroraScienceHub.Geopack.Geopack08;

public sealed partial class Geopack08
{
    public void CARSPH_08(
        float x, float y, float z,
        out float r, out float theta, out float phi)
    {
        throw new NotImplementedException("CARSPH_08 is not implemented");
        // float sq = x * x + y * y;
        // r = MathF.Sqrt(sq + z * z);
        // if (sq != 0.0f)
        // {
        //     sq = MathF.Sqrt(sq);
        //     phi = MathF.Atan2(y, x);
        //     theta = MathF.Atan2(sq, z);
        //     if (phi < 0.0f)
        //     {
        //         phi += TWO_PI;
        //     }
        // }
        // else
        // {
        //     phi = 0.0f;
        //     theta = (z < 0.0f) ? PI : 0.0f;
        // }
    }
}
