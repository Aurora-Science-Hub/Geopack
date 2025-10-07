using Common.Contracts;

namespace AuroraScienceHub.Geopack;

public sealed partial class Geopack
{
    public CartesianLocation MagSm(double xMag, double yMag, double zMag)
    {
        double xSm = xMag * Common1.CFI - yMag * Common1.SFI;
        double ySm = xMag * Common1.SFI + yMag * Common1.CFI;
        double zSm = zMag;
        return new CartesianLocation(xSm, ySm, zSm, CoordinateSystem.SM);
    }

    public CartesianLocation SmMag(double xSm, double ySm, double zSm)
    {
        double xMag = xSm * Common1.CFI + ySm * Common1.SFI;
        double yMag = ySm * Common1.CFI - xSm * Common1.SFI;
        double zMag = zSm;
        return new CartesianLocation(xMag, yMag, zMag, CoordinateSystem.MAG);
    }
}
