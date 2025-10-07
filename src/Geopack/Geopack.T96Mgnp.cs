using Common.Contracts;

namespace AuroraScienceHub.Geopack;

public sealed partial class Geopack
{
    public Magnetopause T96Mgnp(
        double xnPd, double vel,
        double xgsw, double ygsw, double zgsw)
    {
        double pd;
        if (vel < 0.0)
        {
            pd = xnPd;
        }
        else
        {
            pd = 1.94e-6 * xnPd * vel * vel;
        }

        double rat = pd / 2.0D;
        double rat16 = Math.Pow(rat, 0.14);

        // Magnetopause parameters for PD = 2 nPa
        double a0 = 70.0D;
        double s00 = 1.08D;
        double x00 = 5.48D;

        // Scaled parameters for actual pressure
        double a = a0 / rat16;
        double s0 = s00;
        double x0 = x00 / rat16;
        double xm = x0 - a;

        double phi;
        if (ygsw is not 0.0 || zgsw is not 0.0)
        {
            phi = Math.Atan2(ygsw, zgsw);
        }
        else
        {
            phi = 0.0D;
        }

        double rho = Math.Sqrt(Math.Pow(ygsw, 2) + Math.Pow(zgsw, 2));

        if (xgsw < xm)
        {
            double xMgnp = xgsw;
            double rhomGnp = a * Math.Sqrt(Math.Pow(s0, 2) - 1.0D);
            double yMgnp = rhomGnp * Math.Sin(phi);
            double zMgnp = rhomGnp * Math.Cos(phi);
            double dist = Math.Sqrt(
                (xgsw - xMgnp) * (xgsw - xMgnp) +
                (ygsw - yMgnp) * (ygsw - yMgnp) +
                (zgsw - zMgnp) * (zgsw - zMgnp));

            var position = double.IsNaN(rhomGnp) ? MagnetopausePosition.NotDefined
                : rhomGnp > rho
                    ? MagnetopausePosition.Inside
                    : MagnetopausePosition.Outside;

            return new Magnetopause(xMgnp, yMgnp, zMgnp, dist, position, CoordinateSystem.GSW);
        }

        double xksi = (xgsw - x0) / a + 1.0D;
        double xdzt = rho / a;
        double sq1 = Math.Sqrt((1.0D + xksi) * (1.0D + xksi) + xdzt * xdzt);
        double sq2 = Math.Sqrt((1.0D - xksi) * (1.0D - xksi) + xdzt * xdzt);
        double sigma = 0.5D * (sq1 + sq2);
        double tau = 0.5D * (sq1 - sq2);

        // Calculate closest point at magnetopause
        double xMgnpOut = x0 - a * (1.0D - s0 * tau);
        double arg = (s0 * s0 - 1.0D) * (1.0D - tau * tau);
        if (arg < 0.0)
        {
            arg = 0.0D;
        }

        double rhomGnpOut = a * Math.Sqrt(arg);
        double yMgnpOut = rhomGnpOut * Math.Sin(phi);
        double zMgnpOut = rhomGnpOut * Math.Cos(phi);

        double distOut = Math.Sqrt(
            (xgsw - xMgnpOut) * (xgsw - xMgnpOut) +
            (ygsw - yMgnpOut) * (ygsw - yMgnpOut) +
            (zgsw - zMgnpOut) * (zgsw - zMgnpOut));

        var idOut = double.IsNaN(sigma)
            ? MagnetopausePosition.NotDefined
            : sigma > s0
                ? MagnetopausePosition.Outside
                : MagnetopausePosition.Inside;

        return new Magnetopause(xMgnpOut, yMgnpOut, zMgnpOut, distOut, idOut, CoordinateSystem.GSW);
    }
}
