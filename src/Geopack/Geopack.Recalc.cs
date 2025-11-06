using AuroraScienceHub.Geopack.Contracts.Cartesian;
using AuroraScienceHub.Geopack.Contracts.Coordinates;
using AuroraScienceHub.Geopack.Contracts.Engine;
using AuroraScienceHub.Geopack.Contracts.PhysicalObjects;
using AuroraScienceHub.Geopack.Contracts.PhysicalQuantities;
using AuroraScienceHub.Geopack.Utilities;

namespace AuroraScienceHub.Geopack;

public sealed partial class Geopack
{
    public ComputationContext Recalc(DateTime dateTime, CartesianVector<Velocity>? swVelocity = null)
    {
        swVelocity ??= CartesianVector<Velocity>.New(-400D, 0D, 0D, CoordinateSystem.GSE);

        // if (swVelocity.Required().CoordinateSystem is not CoordinateSystem.GSE)
        if (swVelocity.Required().CoordinateSystem is not CoordinateSystem.GSE)
        {
            throw new InvalidOperationException("Solar wind velocity must be in GSE coordinate system.");
        }

        int IY = dateTime.Year;
        int IDAY = dateTime.DayOfYear;
        int IHOUR = dateTime.Hour;
        int MIN = dateTime.Minute;
        int ISEC = dateTime.Second;

        if (IY < 1965)
        {
            IY = 1965;
            Console.WriteLine($"Warning: Year {dateTime.Year} is out of range. Using {IY} instead.");
        }

        if (IY > 2030)
        {
            IY = 2030;
            Console.WriteLine($"Warning: Year {dateTime.Year} is out of range. Using {IY} instead.");
        }

        double[] REC = new double[105];
        for (int N = 1; N <= 14; N++)
        {
            int N2 = 2 * N - 1;
            N2 *= (N2 - 2);
            for (int M = 1; M <= N; M++)
            {
                int MN = N * (N - 1) / 2 + M;
                REC[MN - 1] = (double)((N - M) * (N + M - 2)) / N2;
            }
        }

        double[] G, H;
        switch (IY)
        {
            case < 1970:
                (G, H) = Interpolate(1965, IY, IDAY, IgrfCoefficients.G65, IgrfCoefficients.G70, IgrfCoefficients.H65, IgrfCoefficients.H70);
                break;
            case < 1975:
                (G, H) = Interpolate(1970, IY, IDAY, IgrfCoefficients.G70, IgrfCoefficients.G75, IgrfCoefficients.H70, IgrfCoefficients.H75);
                break;
            case < 1980:
                (G, H) = Interpolate(1975, IY, IDAY, IgrfCoefficients.G75, IgrfCoefficients.G80, IgrfCoefficients.H75, IgrfCoefficients.H80);
                break;
            case < 1985:
                (G, H) = Interpolate(1980, IY, IDAY, IgrfCoefficients.G80, IgrfCoefficients.G85, IgrfCoefficients.H80, IgrfCoefficients.H85);
                break;
            case < 1990:
                (G, H) = Interpolate(1985, IY, IDAY, IgrfCoefficients.G85, IgrfCoefficients.G90, IgrfCoefficients.H85, IgrfCoefficients.H90);
                break;
            case < 1995:
                (G, H) = Interpolate(1990, IY, IDAY, IgrfCoefficients.G90, IgrfCoefficients.G95, IgrfCoefficients.H90, IgrfCoefficients.H95);
                break;
            case < 2000:
                (G, H) = Interpolate(1995, IY, IDAY, IgrfCoefficients.G95, IgrfCoefficients.G00, IgrfCoefficients.H95, IgrfCoefficients.H00);
                break;
            case < 2005:
                (G, H) = Interpolate(2000, IY, IDAY, IgrfCoefficients.G00, IgrfCoefficients.G05, IgrfCoefficients.H00, IgrfCoefficients.H05);
                break;
            case < 2010:
                (G, H) = Interpolate(2005, IY, IDAY, IgrfCoefficients.G05, IgrfCoefficients.G10, IgrfCoefficients.H05, IgrfCoefficients.H10);
                break;
            case < 2015:
                (G, H) = Interpolate(2010, IY, IDAY, IgrfCoefficients.G10, IgrfCoefficients.G15, IgrfCoefficients.H10, IgrfCoefficients.H15);
                break;
            case < 2020:
                (G, H) = Interpolate(2015, IY, IDAY, IgrfCoefficients.G15, IgrfCoefficients.G20, IgrfCoefficients.H15, IgrfCoefficients.H20);
                break;
            case < 2025:
                (G, H) = Interpolate(2020, IY, IDAY, IgrfCoefficients.G20, IgrfCoefficients.G25, IgrfCoefficients.H20, IgrfCoefficients.H25);
                break;
            case >= 2025:
                (G, H) = Extrapolate(IY, IDAY);
                break;
        }

        double S = 1;
        for (int N = 2; N <= 14; N++)
        {
            int MN = N * (N - 1) / 2 + 1;
            S *= (2 * N - 3) / (double)(N - 1);
            G[MN - 1] *= S;
            H[MN - 1] *= S;
            double P = S;
            for (int M = 2; M <= N; M++)
            {
                double aa = M == 2 ? 2 : 1;
                P *= Math.Sqrt(aa * (N - M + 1) / (N + M - 2));
                int MNN = MN + M - 1;
                G[MNN - 1] *= P;
                H[MNN - 1] *= P;
            }
        }

        double G_10 = -G[1];
        double G_11 = G[2];
        double H_11 = H[2];
        double SQ = G_11 * G_11 + H_11 * H_11;
        double SQQ = Math.Sqrt(SQ);
        double SQR = Math.Sqrt(G_10 * G_10 + SQ);
        double SL0 = -H_11 / SQQ;
        double CL0 = -G_11 / SQQ;
        double ST0 = SQQ / SQR;
        double CT0 = G_10 / SQR;
        double STCL = ST0 * CL0;
        double STSL = ST0 * SL0;
        double CTSL = CT0 * SL0;
        double CTCL = CT0 * CL0;

        Sun sun = Sun(dateTime);
        double S1 = Math.Cos(sun.Srasn) * Math.Cos(sun.Sdec);
        double S2 = Math.Sin(sun.Srasn) * Math.Cos(sun.Sdec);
        double S3 = Math.Sin(sun.Sdec);

        double DJ = 365d * (IY - 1900) + (IY - 1901) / 4d + IDAY - 0.5d + (IHOUR * 3600 + MIN * 60 + ISEC) / 86400d;
        double T = DJ / 36525d;
        double OBLIQ = (23.45229d - 0.0130125d * T) / 57.2957795d;
        double DZ1 = 0;
        double DZ2 = -Math.Sin(OBLIQ);
        double DZ3 = Math.Cos(OBLIQ);

        double DY1 = DZ2 * S3 - DZ3 * S2;
        double DY2 = DZ3 * S1 - DZ1 * S3;
        double DY3 = DZ1 * S2 - DZ2 * S1;

        double V = Math.Sqrt(
            Math.Pow(swVelocity.Required().X, 2)
            + Math.Pow(swVelocity.Required().Y, 2)
            + Math.Pow(swVelocity.Required().Z, 2));

        double DX1 = -swVelocity.Required().X / V;
        double DX2 = -swVelocity.Required().Y / V;
        double DX3 = -swVelocity.Required().Z / V;

        double X1 = DX1 * S1 + DX2 * DY1 + DX3 * DZ1;
        double X2 = DX1 * S2 + DX2 * DY2 + DX3 * DZ2;
        double X3 = DX1 * S3 + DX2 * DY3 + DX3 * DZ3;

        double CGST = Math.Cos(sun.Gst);
        double SGST = Math.Sin(sun.Gst);
        double DIP1 = STCL * CGST - STSL * SGST;
        double DIP2 = STCL * SGST + STSL * CGST;
        double DIP3 = CT0;

        double Y1 = DIP2 * X3 - DIP3 * X2;
        double Y2 = DIP3 * X1 - DIP1 * X3;
        double Y3 = DIP1 * X2 - DIP2 * X1;
        double Y = Math.Sqrt(Y1 * Y1 + Y2 * Y2 + Y3 * Y3);
        Y1 /= Y;
        Y2 /= Y;
        Y3 /= Y;

        double Z1 = X2 * Y3 - X3 * Y2;
        double Z2 = X3 * Y1 - X1 * Y3;
        double Z3 = X1 * Y2 - X2 * Y1;

        double E11 = S1 * X1 + S2 * X2 + S3 * X3;
        double E12 = S1 * Y1 + S2 * Y2 + S3 * Y3;
        double E13 = S1 * Z1 + S2 * Z2 + S3 * Z3;
        double E21 = DY1 * X1 + DY2 * X2 + DY3 * X3;
        double E22 = DY1 * Y1 + DY2 * Y2 + DY3 * Y3;
        double E23 = DY1 * Z1 + DY2 * Z2 + DY3 * Z3;
        double E31 = DZ1 * X1 + DZ2 * X2 + DZ3 * X3;
        double E32 = DZ1 * Y1 + DZ2 * Y2 + DZ3 * Y3;
        double E33 = DZ1 * Z1 + DZ2 * Z2 + DZ3 * Z3;

        double SPS = DIP1 * X1 + DIP2 * X2 + DIP3 * X3;
        double CPS = Math.Sqrt(1 - SPS * SPS);
        double PSI = Math.Asin(SPS);

        double A11 = X1 * CGST + X2 * SGST;
        double A12 = -X1 * SGST + X2 * CGST;
        double A13 = X3;
        double A21 = Y1 * CGST + Y2 * SGST;
        double A22 = -Y1 * SGST + Y2 * CGST;
        double A23 = Y3;
        double A31 = Z1 * CGST + Z2 * SGST;
        double A32 = -Z1 * SGST + Z2 * CGST;
        double A33 = Z3;

        double EXMAGX = CT0 * (CL0 * CGST - SL0 * SGST);
        double EXMAGY = CT0 * (CL0 * SGST + SL0 * CGST);
        double EXMAGZ = -ST0;
        double EYMAGX = -(SL0 * CGST + CL0 * SGST);
        double EYMAGY = -(SL0 * SGST - CL0 * CGST);
        double CFI = Y1 * EYMAGX + Y2 * EYMAGY;
        double SFI = Y1 * EXMAGX + Y2 * EXMAGY + Y3 * EXMAGZ;

        return new ComputationContext(
            ST0: ST0, CT0: CT0, SL0: SL0, CL0: CL0,
            CTCL: CTCL, STCL: STCL, CTSL: CTSL, STSL: STSL,
            SFI: SFI, CFI: CFI,
            SPS: SPS, CPS: CPS, PSI: PSI,
            CGST: CGST, SGST: SGST,
            A11: A11, A21: A21, A31: A31, A12: A12, A22: A22, A32: A32, A13: A13, A23: A23, A33: A33,
            E11: E11, E21: E21, E31: E31, E12: E12, E22: E22, E32: E32, E13: E13, E23: E23, E33: E33,
            H: H, G: G, REC: REC);
    }

    private static (double[], double[]) Interpolate(
        int year1, int IY, int IDAY,
        double[] G1, double[] G2, double[] H1, double[] H2)
    {
        double[] G = new double[105];
        double[] H = new double[105];

        double F2 = (IY + (IDAY - 1) / 365.25d - year1) / 5d;
        double F1 = 1.0d - F2;
        for (int N = 0; N <= 104; N++)
        {
            G[N] = G1[N] * F1 + G2[N] * F2;
            H[N] = H1[N] * F1 + H2[N] * F2;
        }

        return (G, H);
    }

    private static (double[], double[]) Extrapolate(int iy, int iday)
    {
        double DT = iy + (iday - 1) / 365.25d - 2025;
        double[] G = new double[105];
        double[] H = new double[105];

        for (int N = 0; N <= 104; N++)
        {
            G[N] = IgrfCoefficients.G25[N];
            H[N] = IgrfCoefficients.H25[N];
            if (N > 44)
            {
                continue;
            }

            G[N] += IgrfCoefficients.DG25[N] * DT;
            H[N] += IgrfCoefficients.DH25[N] * DT;
        }

        return (G, H);
    }

}
