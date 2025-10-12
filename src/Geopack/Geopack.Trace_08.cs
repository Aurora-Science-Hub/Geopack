using AuroraScienceHub.Geopack.Contracts;
using AuroraScienceHub.Geopack.Contracts.Interfaces;
using AuroraScienceHub.Geopack.Contracts.Models;

namespace AuroraScienceHub.Geopack;

public sealed partial class Geopack
{
    public FieldLine Trace_08(
    double xi, double yi, double zi,
    TraceDirection dir,
    double dsMax, double err, double rLim, double r0,
    int iopt, double[] parmod,
    IExternalFieldModel exName,
    InternalFieldModel inName,
    int lMax)
{
    List<CartesianLocation> points = new();
    double direction = (double)dir;

    int l = 0;
    int nrev = 0;
    Common1.DS3 = direction;

    double ds = 0.5D * direction;
    double x = xi;
    double y = yi;
    double z = zi;

    // Сохраняем предыдущую точку для интерполяции
    double xPrev = x, yPrev = y, zPrev = z;

    // Determine initial tracing direction
    FieldLineRhsVector initialRhs = Rhand_08(x, y, z, iopt, parmod, exName, inName);
    double ad = 0.01D;
    if (x * initialRhs.R1 + y * initialRhs.R2 + z * initialRhs.R3 < 0.0D)
    {
        ad = -0.01D;
    }

    double rr = Math.Sqrt(x * x + y * y + z * z) + ad;

    while (l <= lMax)
    {
        l++;

        // Сохраняем предыдущую точку перед шагом
        xPrev = x;
        yPrev = y;
        zPrev = z;

        points.Add(new CartesianLocation(x, y, z, CoordinateSystem.GSW));

        double ryz = y * y + z * z;
        double r2 = x * x + ryz;
        double r = Math.Sqrt(r2);

        // Check outer boundary conditions (метка 8 в FORTRAN)
        if (r > rLim
            || ryz > 1600.0D
            || x > 20.0D)
        {
            break;
        }

        // Check inner boundary crossing from outside (метка 6 в FORTRAN)
        if (r < r0 && rr > r)
        {
            // ИНТЕРПОЛЯЦИЯ как в FORTRAN метка 6
            double r1 = (r0 - r) / (rr - r);
            x -= (x - xPrev) * r1;
            y -= (y - yPrev) * r1;
            z -= (z - zPrev) * r1;
            break;
        }

        // Adaptive step size near Earth (условие исправлено)
        // В FORTRAN: IF (R.GE.RR.OR.R.GE.3.D0) GOTO 4
        if (!(r >= rr || r >= 3.0D)) // Если НЕ движемся наружу И R < 3Re
        {
            double fc = 0.2D;
            if (r - r0 < 0.05D) fc = 0.05D;
            double al = fc * (r - r0 + 0.2D);
            ds = direction * al;
        }

        // Сохраняем координаты для возможной интерполяции (метка 4 в FORTRAN)
        double xr = x;
        double yr = y;
        double zr = z;

        double drp = r - rr;  // Изменение радиуса на предыдущем шаге
        rr = r;               // Сохраняем текущий радиус как предыдущий

        // Make step
        StepResult stepResult = Step_08(x, y, z, ds, dsMax, err, iopt, parmod, exName, inName);
        x = stepResult.X;
        y = stepResult.Y;
        z = stepResult.Z;
        ds = stepResult.NextStepSize;

        // Check for radial direction reversals
        r = Math.Sqrt(x * x + y * y + z * z);
        double dr = r - rr;  // Изменение радиуса на текущем шаге

        // Если направление изменения радиуса поменялось
        if (drp * dr < 0.0D) nrev++;

        if (nrev > 4) break;
    }

    // Обработка превышения максимального количества точек (метка 7 в FORTRAN)
    if (l >= lMax)
    {
        Console.WriteLine(
            "**** COMPUTATIONS IN THE SUBROUTINE TRACE_08 ARE TERMINATED: THE NUMBER OF POINTS EXCEEDED LMAX ****");
        l = lMax;

        // Заменяем последнюю точку на текущее положение как в FORTRAN
        if (points.Count > 0)
        {
            points[points.Count - 1] = new CartesianLocation(x, y, z, CoordinateSystem.GSW);
        }
    }

    return new FieldLine(
        points,
        new CartesianLocation(x, y, z, CoordinateSystem.GSW),
        l,
        l >= lMax ? "Maximum points exceeded" : "Boundary reached");
}

    private FieldLineRhsVector Rhand_08(
        double x, double y, double z,
        int iopt, double[] parmod,
        IExternalFieldModel exName,
        InternalFieldModel inName)
    {
        CartesianFieldVector externalField = exName.Calculate(iopt, parmod, Common1.PSI, x, y, z);
        CartesianFieldVector internalField = inName(x, y, z);

        double bx = externalField.Bx + internalField.Bx;
        double by = externalField.By + internalField.By;
        double bz = externalField.Bz + internalField.Bz;

        double b = Common1.DS3 / Math.Sqrt(bx * bx + by * by + bz * bz);

        double r1 = bx * b;
        double r2 = by * b;
        double r3 = bz * b;

        return new FieldLineRhsVector(r1, r2, r3);
    }

    private StepResult Step_08(
        double x, double y, double z,
        double ds, double dsMax, double errIn,
        int iopt, double[] parmod,
        IExternalFieldModel exName,
        InternalFieldModel inName)
    {
        double currentDs = ds;

        while (true)
        {
            Common1.DS3 = -currentDs / 3.0D;

            FieldLineRhsVector r1 = Rhand_08(x, y, z, iopt, parmod, exName, inName);
            FieldLineRhsVector r2 = Rhand_08(x + r1.R1, y + r1.R2, z + r1.R3, iopt, parmod, exName, inName);
            FieldLineRhsVector r3 = Rhand_08(
                x + 0.5D * (r1.R1 + r2.R1),
                y + 0.5D * (r1.R2 + r2.R2),
                z + 0.5D * (r1.R3 + r2.R3),
                iopt, parmod, exName, inName);
            FieldLineRhsVector r4 = Rhand_08(
                x + 0.375D * (r1.R1 + 3.0D * r3.R1),
                y + 0.375D * (r1.R2 + 3.0D * r3.R2),
                z + 0.375D * (r1.R3 + 3.0D * r3.R3),
                iopt, parmod, exName, inName);
            FieldLineRhsVector r5 = Rhand_08(
                x + 1.5D * (r1.R1 - 3.0D * r3.R1 + 4.0D * r4.R1),
                y + 1.5D * (r1.R2 - 3.0D * r3.R2 + 4.0D * r4.R2),
                z + 1.5D * (r1.R3 - 3.0D * r3.R3 + 4.0D * r4.R3),
                iopt, parmod, exName, inName);

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

            return new StepResult(newX, newY, newZ, nextDs);
        }
    }

    private sealed record StepResult(double X, double Y, double Z, double NextStepSize);

    private sealed record FieldLineRhsVector(double R1, double R2, double R3);
}
