using AuroraScienceHub.Geopack.Contracts.Engine;
using AuroraScienceHub.Geopack.Contracts.Models;

namespace AuroraScienceHub.Geopack;

public sealed partial class Geopack
{
    public CartesianLocation MagSm_08(ComputationContext context, double xMag, double yMag, double zMag)
    {
        double xSm = xMag * context.CFI - yMag * context.SFI;
        double ySm = xMag * context.SFI + yMag * context.CFI;
        double zSm = zMag;
        return new CartesianLocation(xSm, ySm, zSm, CoordinateSystem.SM);
    }

    public CartesianLocation SmMag_08(ComputationContext context, double xSm, double ySm, double zSm)
    {
        double xMag = xSm * context.CFI + ySm * context.SFI;
        double yMag = ySm * context.CFI - xSm * context.SFI;
        double zMag = zSm;
        return new CartesianLocation(xMag, yMag, zMag, CoordinateSystem.MAG);
    }
}
