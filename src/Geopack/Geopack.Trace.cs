using AuroraScienceHub.Geopack.Contracts.Engine;
using AuroraScienceHub.Geopack.Contracts.Interfaces;
using AuroraScienceHub.Geopack.Contracts.Models;

namespace AuroraScienceHub.Geopack;

public sealed partial class Geopack
{
    public FieldLine Trace(ComputationContext context,
    CartesianLocation location,
    TraceDirection dir,
    double dsMax, double err, double rLim, double r0,
    int iopt, double[] parmod,
    IExternalFieldModel exName,
    InternalFieldModel inName,
    int lMax)
    {
        if (location.CoordinateSystem is not CoordinateSystem.GSW)
        {
            throw new InvalidOperationException("Location should be in GSW system.");
        }

        List<CartesianLocation> points = new();
        double direction = (double)dir;

        int l = 0;
        int nrev = 0;
        double ds3 = direction;

        double ds = 0.5D * direction;
        double x = location.X;
        double y = location.Y;
        double z = location.Z;

        double xr = x, yr = y, zr = z;

        FieldLineRhsVector initialRhs = Rhand(context, location, iopt, parmod, exName, inName, ds3);
        double ad = 0.01D;
        if (x * initialRhs.R1 + y * initialRhs.R2 + z * initialRhs.R3 < 0.0D)
        {
            ad = -0.01D;
        }

        double rr = Math.Sqrt(x * x + y * y + z * z) + ad;
        bool maxPointsExceeded = false;

        while (l <= lMax)
        {
            l++;

            points.Add(CartesianLocation.New(x, y, z, CoordinateSystem.GSW));

            double ryz = y * y + z * z;
            double r2 = x * x + ryz;
            double r = Math.Sqrt(r2);

            // Check outer boundary conditions
            if (r > rLim || ryz > 1600.0D || x > 20.0D)
            {
                break;
            }

            // Check inner boundary crossing from outside
            if (r < r0 && rr > r)
            {
                double r1 = (r0 - r) / (rr - r);
                x -= (x - xr) * r1;
                y -= (y - yr) * r1;
                z -= (z - zr) * r1;
                break;
            }

            // Adaptive step size near Earth
            if (!(r >= rr || r >= 3.0D))
            {
                double fc = 0.2D;
                if (r - r0 < 0.05D)
                    fc = 0.05D;
                double al = fc * (r - r0 + 0.2D);
                ds = direction * al;
            }

            xr = x;
            yr = y;
            zr = z;

            double drp = r - rr;
            rr = r;

            // Make step
            (StepResult stepResult, ds3) = Step(context, x, y, z, ds, dsMax, err, iopt, parmod, exName, inName, ds3);
            x = stepResult.X;
            y = stepResult.Y;
            z = stepResult.Z;
            ds = stepResult.NextStepSize;

            // Check for radial direction reversals
            r = Math.Sqrt(x * x + y * y + z * z);
            double dr = r - rr;

            if (drp * dr < 0.0D)
            {
                nrev++;
            }

            if (nrev > 4)
            {
                break;
            }
        }

        if (l > lMax)
        {
            Console.WriteLine(
                "**** COMPUTATIONS IN THE SUBROUTINE TRACE_08 ARE TERMINATED: THE NUMBER OF POINTS EXCEEDED LMAX ****");
            maxPointsExceeded = true;

            if (points.Count > lMax)
            {
                points.RemoveAt(points.Count - 1);
            }
        }

        if (points.Count > 0)
        {
            points[^1] = CartesianLocation.New(x, y, z, CoordinateSystem.GSW);
        }
        else
        {
            points.Add(CartesianLocation.New(x, y, z, CoordinateSystem.GSW));
        }

        return new FieldLine(
            points,
            CartesianLocation.New(x, y, z, CoordinateSystem.GSW),
            points.Count,
            maxPointsExceeded ? "Maximum points exceeded" : "Boundary reached");
    }

    private FieldLineRhsVector Rhand(ComputationContext context,
        CartesianLocation location,
        int iopt, double[] parmod,
        IExternalFieldModel exName,
        InternalFieldModel inName,
        double ds3)
    {
        CartesianVector<MagneticField> externalField = exName.Calculate(iopt, parmod, context.PSI, location);
        CartesianVector<MagneticField> internalField = inName(context, location);

        double bx = externalField.X + internalField.X;
        double by = externalField.Y + internalField.Y;
        double bz = externalField.Z + internalField.Z;

        double b = ds3 / Math.Sqrt(bx * bx + by * by + bz * bz);

        double r1 = bx * b;
        double r2 = by * b;
        double r3 = bz * b;

        return new FieldLineRhsVector(r1, r2, r3);
    }

    private (StepResult, double) Step(ComputationContext context,
        CartesianLocation location,
        double ds, double dsMax, double errIn,
        int iopt, double[] parmod,
        IExternalFieldModel exName,
        InternalFieldModel inName,
        double ds3)
    {
        double currentDs = ds;

        while (true)
        {
            ds3 = -currentDs / 3.0D;

            FieldLineRhsVector r1 = Rhand(context, location, iopt, parmod, exName, inName, ds3);
            FieldLineRhsVector r2 = Rhand(context,
                CartesianLocation.New(location.X + r1.R1, location.Y + r1.R2, location.Z + r1.R3, CoordinateSystem.GSW),
                iopt, parmod, exName, inName, ds3);
            FieldLineRhsVector r3 = Rhand(context,
                x + 0.5D * (r1.R1 + r2.R1),
                y + 0.5D * (r1.R2 + r2.R2),
                z + 0.5D * (r1.R3 + r2.R3),
                iopt, parmod, exName, inName,
                ds3);
            FieldLineRhsVector r4 = Rhand(context,
                x + 0.375D * (r1.R1 + 3.0D * r3.R1),
                y + 0.375D * (r1.R2 + 3.0D * r3.R2),
                z + 0.375D * (r1.R3 + 3.0D * r3.R3),
                iopt, parmod, exName, inName,
                ds3);
            FieldLineRhsVector r5 = Rhand(context,
                x + 1.5D * (r1.R1 - 3.0D * r3.R1 + 4.0D * r4.R1),
                y + 1.5D * (r1.R2 - 3.0D * r3.R2 + 4.0D * r4.R2),
                z + 1.5D * (r1.R3 - 3.0D * r3.R3 + 4.0D * r4.R3),
                iopt, parmod, exName, inName,
                ds3);

            double errCur = Math.Abs(r1.R1 - 4.5D * r3.R1 + 4.0D * r4.R1 - 0.5D * r5.R1)
                            + Math.Abs(r1.R2 - 4.5D * r3.R2 + 4.0D * r4.R2 - 0.5D * r5.R2)
                            + Math.Abs(r1.R3 - 4.5D * r3.R3 + 4.0D * r4.R3 - 0.5D * r5.R3);

            if (errCur > errIn)
            {
                currentDs *= 0.5D;
                continue;
            }

            if (Math.Abs(currentDs) > dsMax)
            {
                currentDs = Math.Sign(currentDs) * dsMax;
                continue;
            }

            double newX = x + 0.5D * (r1.R1 + 4.0D * r4.R1 + r5.R1);
            double newY = y + 0.5D * (r1.R2 + 4.0D * r4.R2 + r5.R2);
            double newZ = z + 0.5D * (r1.R3 + 4.0D * r4.R3 + r5.R3);

            double nextDs = currentDs;
            if (errCur < errIn * 0.04D && currentDs < dsMax / 1.5D)
            {
                nextDs *= 1.5D;
            }

            return (new StepResult(newX, newY, newZ, nextDs), ds3);
        }
    }

    private record struct StepResult(double X, double Y, double Z, double NextStepSize);

    private record struct FieldLineRhsVector(double R1, double R2, double R3);
}
