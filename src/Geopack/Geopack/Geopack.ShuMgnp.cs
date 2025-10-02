using AuroraScienceHub.Geopack.Contracts;

namespace AuroraScienceHub.Geopack.Geopack;

public sealed partial class Geopack
{
        public Magnetopause ShueMgnp(double xnPd, double vel, double bzImf, double xgsw, double ygsw, double zgsw)
        {
            double p;
            if (vel < 0.0)
            {
                p = xnPd;
            }
            else
            {
                p = 1.94e-6 * xnPd * vel * vel; // Solar wind dynamic pressure in nPa
            }

            double phi;
            if (ygsw != 0.0 || zgsw != 0.0)
            {
                phi = Math.Atan2(ygsw, zgsw);
            }
            else
            {
                phi = 0.0;
            }

            int id = -1;
            double r0 = (10.22 + 1.29 * Math.Tanh(0.184 * (bzImf + 8.14))) * Math.Pow(p, -0.15151515);
            double alpha = (0.58 - 0.007 * bzImf) * (1.0 + 0.024 * Math.Log(p));
            double r = Math.Sqrt(xgsw * xgsw + ygsw * ygsw + zgsw * zgsw);
            double rm = r0 * Math.Pow(2.0 / (1.0 + xgsw / r), alpha);

            if (r <= rm) id = +1;

            // Get T96 magnetopause as starting approximation
            var t96Result = T96Mgnp(p, -1.0, xgsw, ygsw, zgsw);
            double xmt96 = t96Result.xMgnp;
            double ymt96 = t96Result.yMgnp;
            double zmt96 = t96Result.zMgnp;

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
                rm_val = r0 * Math.Pow(2.0 / (1.0 + ct), alpha);

                f = r - rm_val;
                gradf_r = 1.0;
                gradf_t = -alpha / r * rm_val * st / (1.0 + ct);
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
                (xgsw - xMgnp) * (xgsw - xMgnp) +
                (ygsw - yMgnp) * (ygsw - yMgnp) +
                (zgsw - zMgnp) * (zgsw - zMgnp));

            return new Magnetopause(xMgnp, yMgnp, zMgnp, dist, id);
        }
    }
}
