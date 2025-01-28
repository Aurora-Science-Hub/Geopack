namespace AuroraScienceHub.Geopack.Geopack08;

public sealed partial class Geopack08
{
    private Common1 Common1 { get; set; } = new ();
    private Common2 Common2 { get; set; } = new ();

    public (Common1, Common2) RECALC_08(DateTime dateTime, float vgsex, float vgsey, float vgsez)
    {
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

        // Calculate the REC array
        for (int N = 1; N <= 14; N++)
        {
            int N2 = 2 * N - 1;
            N2 *= (N2 - 2);
            for (int M = 1; M <= N; M++)
            {
                int MN = N * (N - 1) / 2 + M;
                Common2.REC[MN - 1] = (float)((N - M) * (N + M - 2)) / N2;
            }
        }

        switch (IY)
        {
            // Interpolation and extrapolation logic
            case < 1970:
                Interpolate(1965, 1970, IY, IDAY, G65, G70, H65, H70);
                break;
            case < 1975:
                Interpolate(1970, 1975, IY, IDAY, G70, G75, H70, H75);
                break;
            case < 1980:
                Interpolate(1975, 1980, IY, IDAY, G75, G80, H75, H80);
                break;
            case < 1985:
                Interpolate(1980, 1985, IY, IDAY, G80, G85, H80, H85);
                break;
            case < 1990:
                Interpolate(1985, 1990, IY, IDAY, G85, G90, H85, H90);
                break;
            case < 1995:
                Interpolate(1990, 1995, IY, IDAY, G90, G95, H90, H95);
                break;
            case < 2000:
                Interpolate(1995, 2000, IY, IDAY, G95, G00, H95, H00);
                break;
            case < 2005:
                Interpolate(2000, 2005, IY, IDAY, G00, G05, H00, H05);
                break;
            case < 2010:
                Interpolate(2005, 2010, IY, IDAY, G05, G10, H05, H10);
                break;
            case < 2015:
                Interpolate(2010, 2015, IY, IDAY, G10, G15, H10, H15);
                break;
            case < 2020:
                Interpolate(2015, 2020, IY, IDAY, G15, G20, H15, H20);
                break;
            case < 2025:
                Interpolate(2020, 2025, IY, IDAY, G20, G25, H20, H25);
                break;
        }

        // Extrapolate beyond 2025
        if (IY >= 2025)
        {
            float DT = IY + (IDAY - 1) / 365.25f - 2025;
            for (int N = 0; N <= 104; N++)
            {
                Common2.G[N] = G25[N];
                Common2.H[N] = H25[N];
                if (N <= 44)
                {
                    Common2.G[N] += DG25[N] * DT;
                    Common2.H[N] += DH25[N] * DT;
                }
            }
        }

        // Schmidt normalization factors
        float S = 1;
        for (int N = 2; N <= 14; N++)
        {
            int MN = N * (N - 1) / 2 + 1;
            S *= (2 * N - 3) / (float)(N - 1);
            Common2.G[MN-1] *= S;
            Common2.H[MN-1] *= S;
            float P = S;
            for (int M = 2; M <= N; M++)
            {
                float AA = (M == 2) ? 2 : 1;
                P *= (float)MathF.Sqrt(AA * (N - M + 1) / (N + M - 2));
                int MNN = MN + M - 1;
                Common2.G[MNN-1] *= P;
                Common2.H[MNN-1] *= P;
            }
        }

        // Calculate GEO components of the unit vector EzMAG
        float G_10 = -Common2.G[1];
        float G_11 = Common2.G[2];
        float H_11 = Common2.H[2];
        float SQ = G_11 * G_11 + H_11 * H_11;
        float SQQ = MathF.Sqrt(SQ);
        float SQR = MathF.Sqrt(G_10 * G_10 + SQ);
        Common1.SL0 = -H_11 / SQQ;
        Common1.CL0 = -G_11 / SQQ;
        Common1.ST0 = SQQ / SQR;
        Common1.CT0 = G_10 / SQR;
        Common1.STCL = Common1.ST0 * Common1.CL0;
        Common1.STSL = Common1.ST0 * Common1.SL0;
        Common1.CTSL = Common1.CT0 * Common1.SL0;
        Common1.CTCL = Common1.CT0 * Common1.CL0;

        // Calculate GEI components of the unit vector S = EX_GSE
        // Call to SUN_08 method is assumed to be implemented elsewhere
        float GST, SLONG, SRASN, SDEC;
        SUN_08(dateTime, out GST, out SLONG, out SRASN, out SDEC);
        float S1 = MathF.Cos(SRASN) * (float)Math.Cos(SDEC);
        float S2 = MathF.Sin(SRASN) * (float)Math.Cos(SDEC);
        float S3 = MathF.Sin(SDEC);

        // Calculate GEI components of the unit vector EZGSE
        float DJ = 365 * (IY - 1900) + (IY - 1901) / 4 + IDAY - 0.5f + (IHOUR * 3600 + MIN * 60 + ISEC) / 86400f;
        float T = DJ / 36525f;
        float OBLIQ = (23.45229f - 0.0130125f * T) / 57.2957795f;
        float DZ1 = 0;
        float DZ2 = -MathF.Sin(OBLIQ);
        float DZ3 = MathF.Cos(OBLIQ);

        // Obtain GEI components of the unit vector EYGSE
        float DY1 = DZ2 * S3 - DZ3 * S2;
        float DY2 = DZ3 * S1 - DZ1 * S3;
        float DY3 = DZ1 * S2 - DZ2 * S1;

        // Calculate GEI components of the unit vector X = EXGSW
        float V = MathF.Sqrt(vgsex * vgsex + vgsey * vgsey + vgsez * vgsez);
        float DX1 = -vgsex / V;
        float DX2 = -vgsey / V;
        float DX3 = -vgsez / V;

        // Then in GEI
        float X1 = DX1 * S1 + DX2 * DY1 + DX3 * DZ1;
        float X2 = DX1 * S2 + DX2 * DY2 + DX3 * DZ2;
        float X3 = DX1 * S3 + DX2 * DY3 + DX3 * DZ3;

        // Calculate GEI components of the unit vector DIP = EZ_SM = EZ_MAG
        Common1.CGST = MathF.Cos(GST);
        Common1.SGST = MathF.Sin(GST);
        float DIP1 = Common1.STCL * Common1.CGST - Common1.STSL * Common1.SGST;
        float DIP2 = Common1.STCL * Common1.SGST + Common1.STSL * Common1.CGST;
        float DIP3 = Common1.CT0;

        // Calculate GEI components of the unit vector Y = EYGSW
        float Y1 = DIP2 * X3 - DIP3 * X2;
        float Y2 = DIP3 * X1 - DIP1 * X3;
        float Y3 = DIP1 * X2 - DIP2 * X1;
        float Y = MathF.Sqrt(Y1 * Y1 + Y2 * Y2 + Y3 * Y3);
        Y1 /= Y;
        Y2 /= Y;
        Y3 /= Y;

        // GEI components of the unit vector Z = EZGSW = EXGSW x EYGSW
        float Z1 = X2 * Y3 - X3 * Y2;
        float Z2 = X3 * Y1 - X1 * Y3;
        float Z3 = X1 * Y2 - X2 * Y1;

        // Elements of the matrix GSE to GSW
        Common1.E11 = S1 * X1 + S2 * X2 + S3 * X3;
        Common1.E12 = S1 * Y1 + S2 * Y2 + S3 * Y3;
        Common1.E13 = S1 * Z1 + S2 * Z2 + S3 * Z3;
        Common1.E21 = DY1 * X1 + DY2 * X2 + DY3 * X3;
        Common1.E22 = DY1 * Y1 + DY2 * Y2 + DY3 * Y3;
        Common1.E23 = DY1 * Z1 + DY2 * Z2 + DY3 * Z3;
        Common1.E31 = DZ1 * X1 + DZ2 * X2 + DZ3 * X3;
        Common1.E32 = DZ1 * Y1 + DZ2 * Y2 + DZ3 * Y3;
        Common1.E33 = DZ1 * Z1 + DZ2 * Z2 + DZ3 * Z3;

        // Geodipole tilt angle in the GSW system
        Common1.SPS = DIP1 * X1 + DIP2 * X2 + DIP3 * X3;
        Common1.CPS = MathF.Sqrt(1 - Common1.SPS * Common1.SPS);
        Common1.PSI = MathF.Asin(Common1.SPS);

        // Elements of the matrix GEO to GSW
        Common1.A11 = X1 * Common1.CGST + X2 * Common1.SGST;
        Common1.A12 = -X1 * Common1.SGST + X2 * Common1.CGST;
        Common1.A13 = X3;
        Common1.A21 = Y1 * Common1.CGST + Y2 * Common1.SGST;
        Common1.A22 = -Y1 * Common1.SGST + Y2 * Common1.CGST;
        Common1.A23 = Y3;
        Common1.A31 = Z1 * Common1.CGST + Z2 * Common1.SGST;
        Common1.A32 = -Z1 * Common1.SGST + Z2 * Common1.CGST;
        Common1.A33 = Z3;

        // Elements of the matrix MAG to SM
        float EXMAGX = Common1.CT0 * (Common1.CL0 * Common1.CGST - Common1.SL0 * Common1.SGST);
        float EXMAGY = Common1.CT0 * (Common1.CL0 * Common1.SGST + Common1.SL0 * Common1.CGST);
        float EXMAGZ = -Common1.ST0;
        float EYMAGX = -(Common1.SL0 * Common1.CGST + Common1.CL0 * Common1.SGST);
        float EYMAGY = -(Common1.SL0 * Common1.SGST - Common1.CL0 * Common1.CGST);
        Common1.CFI = Y1 * EYMAGX + Y2 * EYMAGY;
        Common1.SFI = Y1 * EXMAGX + Y2 * EXMAGY + Y3 * EXMAGZ;

        return (Common1, Common2);
    }

    private void Interpolate(int year1, int year2, int IY, int IDAY, float[] G1, float[] G2, float[] H1,
        float[] H2)
    {
        float F2 = (IY + (IDAY - 1) / 365.25f - year1) / 5f;
        float F1 = 1.0f - F2;
        for (int N = 0; N <= 104; N++)
        {
            Common2.G[N] = G1[N] * F1 + G2[N] * F2;
            Common2.H[N] = H1[N] * F1 + H2[N] * F2;
        }
    }
}
