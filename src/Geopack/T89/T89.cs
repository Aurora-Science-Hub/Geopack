using AuroraScienceHub.Geopack.Interfaces;

namespace AuroraScienceHub.Geopack.T89;

public sealed class T89 : IT89D_SP
{
    public void T89D_SP(
        int iopt,
        float[] parmod,
        float ps,
        float x, float y, float z,
        out float bx, out float by, out float bz)
        => throw new NotImplementedException();
}
