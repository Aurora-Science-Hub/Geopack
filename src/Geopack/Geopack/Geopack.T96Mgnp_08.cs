using AuroraScienceHub.Geopack.Contracts;

namespace AuroraScienceHub.Geopack.Geopack;

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
        if (ygsw != 0.0 || zgsw != 0.0)
        {
            phi = Math.Atan2(ygsw, zgsw);
        }
        else
        {
            phi = 0.0;
        }

        double rho = Math.Sqrt(ygsw * ygsw + zgsw * zgsw);

        if (xgsw < xm)
        {
            double xMgnp = xgsw;
            double rhomGnp = a * Math.Sqrt(s0 * s0 - 1.0);
            double yMgnp = rhomGnp * Math.Sin(phi);
            double zMgnp = rhomGnp * Math.Cos(phi);
            double dist = Math.Sqrt(
                (xgsw - xMgnp) * (xgsw - xMgnp) +
                (ygsw - yMgnp) * (ygsw - yMgnp) +
                (zgsw - zMgnp) * (zgsw - zMgnp));

            int id = (rhomGnp > rho) ? +1 : -1;
            return new Magnetopause(xMgnp, yMgnp, zMgnp, dist, id);
        }

        double xksi = (xgsw - x0) / a + 1.0;
        double xdzt = rho / a;
        double sq1 = Math.Sqrt((1.0 + xksi) * (1.0 + xksi) + xdzt * xdzt);
        double sq2 = Math.Sqrt((1.0 - xksi) * (1.0 - xksi) + xdzt * xdzt);
        double sigma = 0.5 * (sq1 + sq2);
        double tau = 0.5 * (sq1 - sq2);

        // Calculate closest point at magnetopause
        double xMgnpOut = x0 - a * (1.0 - s0 * tau);
        double arg = (s0 * s0 - 1.0) * (1.0 - tau * tau);
        if (arg < 0.0) arg = 0.0;
        double rhomGnpOut = a * Math.Sqrt(arg);
        double yMgnpOut = rhomGnpOut * Math.Sin(phi);
        double zMgnpOut = rhomGnpOut * Math.Cos(phi);

        double distOut = Math.Sqrt(
            (xgsw - xMgnpOut) * (xgsw - xMgnpOut) +
            (ygsw - yMgnpOut) * (ygsw - yMgnpOut) +
            (zgsw - zMgnpOut) * (zgsw - zMgnpOut));

        int idOut = (sigma > s0) ? -1 : +1;

        return new Magnetopause(xMgnpOut, yMgnpOut, zMgnpOut, distOut, idOut);
    }
}
