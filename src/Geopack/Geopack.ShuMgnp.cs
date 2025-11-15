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
            phi = 0.0;
        }

        MagnetopausePosition id;
        double r0 = (10.22 + 1.29 * Math.Tanh(0.184 * (bzImf + 8.14))) * Math.Pow(p, -0.15151515);
        double alpha = (0.58 - 0.007 * bzImf) * (1.0 + 0.024 * Math.Log(p));
        double x2 = location.X * location.X;
        double y2 = location.Y * location.Y;
        double z2 = location.Z * location.Z;
        double r = Math.Sqrt(x2 + y2 + z2);
        double rInv = 1.0 / r;
        double rm = r0 * Math.Pow(2.0 / (1.0 + location.X * rInv), alpha);

        if (double.IsFinite(rm))
        {
            id = r <= rm ? MagnetopausePosition.Inside : MagnetopausePosition.Outside;
        }
        else
        {
            id = MagnetopausePosition.NotDefined;
        }

        // Get T96 magnetopause as starting approximation
        Magnetopause t96Magnetopause = T96Mgnp(p, -1.0, location);
        double xmt96 = t96Magnetopause.BoundaryLocation.X;
        double ymt96 = t96Magnetopause.BoundaryLocation.Y;
        double zmt96 = t96Magnetopause.BoundaryLocation.Z;

        double rho2 = ymt96 * ymt96 + zmt96 * zmt96;
        r = Math.Sqrt(rho2 + xmt96 * xmt96);
        rInv = 1.0 / r;
        double st = Math.Sqrt(rho2) * rInv;
        double ct = xmt96 * rInv;

        // Newton's iterative method to find nearest boundary point
        int nit = 0;
        double t, rm_val, f, gradf_r, gradf_t, gradf, dr, dt, ds;

        do
        {
            t = Math.Atan2(st, ct);
            rm_val = r0 * Math.Pow(2.0 / (1.0 + ct), alpha);

            f = r - rm_val;
            gradf_r = 1.0;
            gradf_t = -alpha * rInv * rm_val * st / (1.0 + ct);
            gradf = Math.Sqrt(gradf_r * gradf_r + gradf_t * gradf_t);

            dr = -f / (gradf * gradf);
            dt = dr * rInv * gradf_t;

            r += dr;
            t += dt;
            (st, ct) = Math.SinCos(t);
            rInv = 1.0 / r;

            double rdt = r * dt;
            ds = Math.Sqrt(dr * dr + rdt * rdt);
            nit++;

            if (nit > 1000)
            {
                throw new InvalidOperationException(
                    "BOUNDARY POINT COULD NOT BE FOUND; ITERATIONS DO NOT CONVERGE");
            }
        }
        while (ds > 1e-4);

        (double sinPhi, double cosPhi) = Math.SinCos(phi);
        (double sinT, double cosT) = Math.SinCos(t);
        double xMgnp = r * cosT;
        double rho = r * sinT;
        double yMgnp = rho * sinPhi;
        double zMgnp = rho * cosPhi;

        double dx = location.X - xMgnp;
        double dy = location.Y - yMgnp;
        double dz = location.Z - zMgnp;
        double dist = Math.Sqrt(dx * dx + dy * dy + dz * dz);

        return new Magnetopause(CartesianLocation.New(xMgnp, yMgnp, zMgnp, CoordinateSystem.GSW), dist, id);
    }
}
