using Common.Contracts;

namespace AuroraScienceHub.Geopack;

public sealed partial class Geopack
{
    public Magnetopause ShuMgnp(
        double xnPd,
        double vel,
        double bzImf,
        double xgsw, double ygsw, double zgsw)
    {
        double p;
        if (vel < 0.0)
        {
            p = xnPd;
        }
        else
        {
            // Solar wind dynamic pressure in nPa
            p = 1.94e-6 * xnPd * Math.Pow(vel, 2);
        }

        double phi;
        if (ygsw is not 0.0 || zgsw is not 0.0)
        {
            phi = Math.Atan2(ygsw, zgsw);
        }
        else
        {
            phi = 0.0D;
        }

        var id = MagnetopausePosition.NotDefined;
        double r0 = (10.22D + 1.29D * Math.Tanh(0.184D * (bzImf + 8.14D))) * Math.Pow(p, -0.15151515D);
        double alpha = (0.58D - 0.007D * bzImf) * (1.0D + 0.024D * Math.Log(p));
        double r = Math.Sqrt(xgsw * xgsw + ygsw * ygsw + zgsw * zgsw);
        double rm = r0 * Math.Pow(2.0D / (1.0D + xgsw / r), alpha);

        if (double.IsFinite(rm))
        {
            id = r <= rm ? MagnetopausePosition.Inside : MagnetopausePosition.Outside;
        }
        else
        {
            id = MagnetopausePosition.NotDefined;
        }

        // Get T96 magnetopause as starting approximation
        var t96Result = T96Mgnp(p, -1.0D, xgsw, ygsw, zgsw);
        double xmt96 = t96Result.X;
        double ymt96 = t96Result.Y;
        double zmt96 = t96Result.Z;

        double rho2 = ymt96 * ymt96 + zmt96 * zmt96;
        r = Math.Sqrt(rho2 + xmt96 * xmt96);
        double st = Math.Sqrt(rho2) / r;
        double ct = xmt96 / r;

        // Newton's iterative method to find nearest boundary point
        int nit = 0;
        double t, rm_val, f, gradf_r, gradf_t, gradf, dr, dt, ds;

        do
        {
            t = Math.Atan2(st, ct);
            rm_val = r0 * Math.Pow(2.0D / (1.0D + ct), alpha);

            f = r - rm_val;
            gradf_r = 1.0D;
            gradf_t = -alpha / r * rm_val * st / (1.0D + ct);
            gradf = Math.Sqrt(gradf_r * gradf_r + gradf_t * gradf_t);

            dr = -f / (gradf * gradf);
            dt = dr / r * gradf_t;

            r += dr;
            t += dt;
            st = Math.Sin(t);
            ct = Math.Cos(t);

            ds = Math.Sqrt(dr * dr + (r * dt) * (r * dt));
            nit++;

            if (nit > 1000)
            {
                throw new InvalidOperationException(
                    "BOUNDARY POINT COULD NOT BE FOUND; ITERATIONS DO NOT CONVERGE");
            }
        }
        while (ds > 1e-4);

        double xMgnp = r * Math.Cos(t);
        double rho = r * Math.Sin(t);
        double yMgnp = rho * Math.Sin(phi);
        double zMgnp = rho * Math.Cos(phi);

        double dist = Math.Sqrt(
            Math.Pow(xgsw - xMgnp, 2) +
            Math.Pow(ygsw - yMgnp, 2) +
            Math.Pow(zgsw - zMgnp, 2));

        return new Magnetopause(xMgnp, yMgnp, zMgnp, dist, id, CoordinateSystem.GSW);
    }
}
