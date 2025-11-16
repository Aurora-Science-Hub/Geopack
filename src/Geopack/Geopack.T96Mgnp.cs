using AuroraScienceHub.Geopack.Contracts.Cartesian;
using AuroraScienceHub.Geopack.Contracts.Coordinates;
using AuroraScienceHub.Geopack.Contracts.PhysicalObjects;

namespace AuroraScienceHub.Geopack;

internal sealed partial class Geopack
{
    public Magnetopause T96Mgnp(
        double xnPd, double vel,
        CartesianLocation location)
    {
        if (location.CoordinateSystem is not CoordinateSystem.GSW)
        {
            throw new InvalidOperationException("Location must be in GSW system.");
        }

        double pd;
        if (vel < 0.0)
        {
            pd = xnPd;
        }
        else
        {
            pd = 1.94e-6 * xnPd * vel * vel;
        }

        if (pd is 0.0)
        {
            throw new InvalidOperationException("Dynamic pressure should not be zero.");
        }

        double rat = pd / 2.0;
        double rat16 = Math.Pow(rat, 0.14);

        // Magnetopause parameters for PD = 2 nPa
        double a0 = 70.0;
        double s00 = 1.08;
        double x00 = 5.48;

        // Scaled parameters for actual pressure
        double a = a0 / rat16;
        double s0 = s00;
        double x0 = x00 / rat16;
        double xm = x0 - a;

        double phi;
        if (location.Y is not 0.0 || location.Z is not 0.0)
        {
            phi = Math.Atan2(location.Y, location.Z);
        }
        else
        {
            phi = 0.0;
        }

        double y2 = location.Y * location.Y;
        double z2 = location.Z * location.Z;
        double rho = Math.Sqrt(y2 + z2);

        if (location.X < xm)
        {
            double xMgnp = location.X;
            double s02 = s0 * s0;
            double rhomGnp = a * Math.Sqrt(s02 - 1.0);
            (double sinPhi, double cosPhi) = Math.SinCos(phi);
            double yMgnp = rhomGnp * sinPhi;
            double zMgnp = rhomGnp * cosPhi;

            double dx = location.X - xMgnp;
            double dy = location.Y - yMgnp;
            double dz = location.Z - zMgnp;
            double dist = Math.Sqrt(dx * dx + dy * dy + dz * dz);

            MagnetopausePosition position = double.IsNaN(rhomGnp) ? MagnetopausePosition.NotDefined
                : rhomGnp > rho
                    ? MagnetopausePosition.Inside
                    : MagnetopausePosition.Outside;

            return new Magnetopause(CartesianLocation.New(xMgnp, yMgnp, zMgnp, CoordinateSystem.GSW), dist, position);
        }

        double aInv = 1.0 / a;
        double xksi = (location.X - x0) * aInv + 1.0;
        double xdzt = rho * aInv;
        double xksi1p = 1.0 + xksi;
        double xksi1m = 1.0 - xksi;
        double xdzt2 = xdzt * xdzt;
        double sq1 = Math.Sqrt(xksi1p * xksi1p + xdzt2);
        double sq2 = Math.Sqrt(xksi1m * xksi1m + xdzt2);
        double sigma = 0.5 * (sq1 + sq2);
        double tau = 0.5 * (sq1 - sq2);

        // Calculate closest point at magnetopause
        double xMgnpOut = x0 - a * (1.0 - s0 * tau);
        double s02_out = s0 * s0;
        double tau2 = tau * tau;
        double arg = (s02_out - 1.0) * (1.0 - tau2);
        if (arg < 0.0)
        {
            arg = 0.0;
        }

        double rhomGnpOut = a * Math.Sqrt(arg);
        (double sinPhiOut, double cosPhiOut) = Math.SinCos(phi);
        double yMgnpOut = rhomGnpOut * sinPhiOut;
        double zMgnpOut = rhomGnpOut * cosPhiOut;

        double dxOut = location.X - xMgnpOut;
        double dyOut = location.Y - yMgnpOut;
        double dzOut = location.Z - zMgnpOut;
        double distOut = Math.Sqrt(dxOut * dxOut + dyOut * dyOut + dzOut * dzOut);

        MagnetopausePosition idOut = double.IsNaN(sigma)
            ? MagnetopausePosition.NotDefined
            : sigma > s0
                ? MagnetopausePosition.Outside
                : MagnetopausePosition.Inside;

        return new Magnetopause(CartesianLocation.New(xMgnpOut, yMgnpOut, zMgnpOut, CoordinateSystem.GSW), distOut, idOut);
    }
}
