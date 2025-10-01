using AuroraScienceHub.Geopack.Contracts;
using AuroraScienceHub.Geopack.Interfaces;

namespace AuroraScienceHub.Geopack.T89;

public sealed class T89 : IT89
{
    public CartesianFieldVector T89D_SP(int iopt, float[] parmod, float ps, float x, float y, float z)
        => new(0.0D, 0.0D, 0.0D, CoordinateSystem.GSW);
}
