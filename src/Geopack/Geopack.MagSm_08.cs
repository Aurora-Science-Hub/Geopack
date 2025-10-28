using AuroraScienceHub.Geopack.Contracts.Engine;
using AuroraScienceHub.Geopack.Contracts.Models;

namespace AuroraScienceHub.Geopack;

public sealed partial class Geopack
{
    public CartesianLocation MagSm_08(ComputationContext ctx, double xMag, double yMag, double zMag)
    {
        double xSm = xMag * ctx.CFI - yMag * ctx.SFI;
        double ySm = xMag * ctx.SFI + yMag * ctx.CFI;
        double zSm = zMag;
        return new CartesianLocation(xSm, ySm, zSm, CoordinateSystem.SM);
    }

    public CartesianLocation SmMag_08(ComputationContext ctx, double xSm, double ySm, double zSm)
    {
        double xMag = xSm * ctx.CFI + ySm * ctx.SFI;
        double yMag = ySm * ctx.CFI - xSm * ctx.SFI;
        double zMag = zSm;
        return new CartesianLocation(xMag, yMag, zMag, CoordinateSystem.MAG);
    }
}
