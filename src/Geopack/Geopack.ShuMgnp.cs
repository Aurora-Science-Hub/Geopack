using AuroraScienceHub.Geopack.Contracts.Cartesian;
using AuroraScienceHub.Geopack.Contracts.Coordinates;
using AuroraScienceHub.Geopack.Contracts.Engine;
using AuroraScienceHub.Geopack.Contracts.PhysicalObjects;

namespace AuroraScienceHub.Geopack;

internal sealed partial class Geopack
{
    /// <summary>
    /// Shu magnetopause model coefficient 1
    /// </summary>
    private const double ShuMgnpCoeff1 = 10.22D;

    /// <summary>
    /// Shu magnetopause model coefficient 2
    /// </summary>
    private const double ShuMgnpCoeff2 = 1.29D;

    /// <summary>
    /// Shu magnetopause model coefficient 3
    /// </summary>
    private const double ShuMgnpCoeff3 = 0.184D;

    /// <summary>
    /// Shu magnetopause model coefficient 4
    /// </summary>
    private const double ShuMgnpCoeff4 = 8.14D;

    /// <summary>
    /// Shu magnetopause model pressure exponent (-1/6.6)
    /// </summary>
    private const double ShuMgnpPressureExponent = -0.15151515D;

    /// <summary>
    /// Shu magnetopause model alpha coefficient 1
    /// </summary>
    private const double ShuMgnpAlphaCoeff1 = 0.58D;

    /// <summary>
    /// Shu magnetopause model alpha coefficient 2
    /// </summary>
    private const double ShuMgnpAlphaCoeff2 = 0.007D;

    /// <summary>
    /// Shu magnetopause model alpha coefficient 3
    /// </summary>
    private const double ShuMgnpAlphaCoeff3 = 0.024D;

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
            p = GeopackConstants.SolarWindDynamicPressureFactor * xnPd * vel * vel;
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
        double r0 = (ShuMgnpCoeff1 + ShuMgnpCoeff2 * Math.Tanh(ShuMgnpCoeff3 * (bzImf + ShuMgnpCoeff4))) * Math.Pow(p, ShuMgnpPressureExponent);
        double alpha = (ShuMgnpAlphaCoeff1 - ShuMgnpAlphaCoeff2 * bzImf) * (1.0D + ShuMgnpAlphaCoeff3 * Math.Log(p));
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
            (st, ct) = Math.SinCos(t);

            ds = Math.Sqrt(dr * dr + (r * dt) * (r * dt));
            nit++;

            if (nit > 1000)
            {
                throw new InvalidOperationException(
                    "BOUNDARY POINT COULD NOT BE FOUND; ITERATIONS DO NOT CONVERGE");
            }
        }
        while (ds > 1e-4);

        (double sinT, double cosT) = Math.SinCos(t);
        double xMgnp = r * cosT;
        double rho = r * sinT;
        (double sinPhi, double cosPhi) = Math.SinCos(phi);
        double yMgnp = rho * sinPhi;
        double zMgnp = rho * cosPhi;

        double dx = location.X - xMgnp;
        double dy = location.Y - yMgnp;
        double dz = location.Z - zMgnp;
        double dist = Math.Sqrt(dx * dx + dy * dy + dz * dz);

        return new Magnetopause(CartesianLocation.New(xMgnp, yMgnp, zMgnp, CoordinateSystem.GSW), dist, id);
    }
}
