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

        double g1 = context.G[1];
        double g2 = context.G[2];
        double h2 = context.H[2];
        double dipmom = Math.Sqrt(g1 * g1 + g2 * g2 + h2 * h2);

        double x2 = location.X * location.X;
        double y2 = location.Y * location.Y;
        double z2 = location.Z * location.Z;
        double v = 3.0 * location.Z * location.X;

        double r2 = x2 + y2 + z2;
        if (r2 <= 0.0)
        {
            throw new InvalidOperationException("Location radius should not be zero.");
        }

        double r5 = r2 * r2 * Math.Sqrt(r2);
        double q = dipmom / r5;

        double bxgsw = q * ((y2 + z2 - 2.0 * x2) * context.SPS - v * context.CPS);
        double bygsw = -3.0 * location.Y * q * (location.X * context.SPS + location.Z * context.CPS);
        double bzgsw = q * ((x2 + y2 - 2.0 * z2) * context.CPS - v * context.SPS);

        return CartesianVector<MagneticField>.New(bxgsw, bygsw, bzgsw, CoordinateSystem.GSW);
    }
}
