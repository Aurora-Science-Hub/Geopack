using AuroraScienceHub.Geopack.Contracts.Cartesian;
using AuroraScienceHub.Geopack.Contracts.Coordinates;
using AuroraScienceHub.Geopack.Contracts.PhysicalObjects;

namespace AuroraScienceHub.Geopack;

internal sealed partial class Geopack
{
    public Magnetopause ShuMgnp(
        double xnPd,
        double vel,
        double bzImf,
        CartesianLocation location)
    {
        if (Math.Abs(xnPd) <= double.Epsilon
            || Math.Abs(vel) <= double.Epsilon)
        {
            throw new InvalidOperationException("Solar wind plasma parameters should not be zero.");
        }

        if (location.CoordinateSystem is not CoordinateSystem.GSW)
        {
            throw new InvalidOperationException("Location must be in GSW system.");
        }

        double p;
        if (vel < 0.0)
        {
            p = xnPd;
        }
        else
        {
            // Solar wind dynamic pressure in nPa
            p = 1.94e-6 * xnPd * vel * vel;
        }

        double phi;
        if (location.Y is not 0.0 || location.Z is not 0.0)
        {
            phi = Math.Atan2(location.Y, location.Z);
        }
        else
        {
            phi = 0.0D;
        }

        MagnetopausePosition id;
        double r0 = (10.22D + 1.29D * Math.Tanh(0.184D * (bzImf + 8.14D))) * Math.Pow(p, -0.15151515D);
        double alpha = (0.58D - 0.007D * bzImf) * (1.0D + 0.024D * Math.Log(p));
        double r = Math.Sqrt(location.X * location.X + location.Y * location.Y + location.Z * location.Z);
        double rm = r0 * Math.Pow(2.0D / (1.0D + location.X / r), alpha);

        if (double.IsFinite(rm))
        {
            id = r <= rm ? MagnetopausePosition.Inside : MagnetopausePosition.Outside;
        }
        else
        {
            id = MagnetopausePosition.NotDefined;
        }

        // Get T96 magnetopause as starting approximation
        Magnetopause t96Magnetopause = T96Mgnp(p, -1.0D, location);
        double xmt96 = t96Magnetopause.BoundaryLocation.X;
        double ymt96 = t96Magnetopause.BoundaryLocation.Y;
        double zmt96 = t96Magnetopause.BoundaryLocation.Z;

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

        double dx = location.X - xMgnp;
        double dy = location.Y - yMgnp;
        double dz = location.Z - zMgnp;
        double dist = Math.Sqrt(dx * dx + dy * dy + dz * dz);

        return new Magnetopause(CartesianLocation.New(xMgnp, yMgnp, zMgnp, CoordinateSystem.GSW), dist, id);
    }
}
