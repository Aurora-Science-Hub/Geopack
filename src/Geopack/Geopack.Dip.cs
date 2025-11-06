using AuroraScienceHub.Geopack.Contracts.Cartesian;
using AuroraScienceHub.Geopack.Contracts.Coordinates;
using AuroraScienceHub.Geopack.Contracts.Engine;
using AuroraScienceHub.Geopack.Contracts.PhysicalQuantities;

namespace AuroraScienceHub.Geopack;

internal sealed partial class Geopack
{
    public CartesianVector<MagneticField> Dip(ComputationContext context, CartesianLocation location)
    {
        if (location.CoordinateSystem is not CoordinateSystem.GSW)
        {
            throw new InvalidOperationException("Location must be in GSW coordinate system.");
        }

        double dipmom = Math.Sqrt(Math.Pow(context.G[1], 2) + Math.Pow(context.G[2], 2) + Math.Pow(context.H[2], 2));

        double p = Math.Pow(location.X, 2);
        double u = Math.Pow(location.Z, 2);
        double v = 3.0D * location.Z * location.X;
        double t = Math.Pow(location.Y, 2);

        if (p + t + u is 0D)
        {
            throw new InvalidOperationException("Location radius should not be zero.");
        }

        double q = dipmom / Math.Pow(p + t + u, 2.5);

        double bxgsw = q * ((t + u - 2.0D * p) * context.SPS - v * context.CPS);
        double bygsw = -3.0D * location.Y * q * (location.X * context.SPS + location.Z * context.CPS);
        double bzgsw = q * ((p + t - 2.0D * u) * context.CPS - v * context.SPS);

        return CartesianVector<MagneticField>.New(bxgsw, bygsw, bzgsw, CoordinateSystem.GSW);
    }
}
