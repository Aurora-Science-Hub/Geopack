namespace AuroraScienceHub.Geopack.ExternalFieldModels.T89;

internal sealed partial class T89
{
    private static readonly double[,] s_parameters =
    {
        {
            -116.53E0, -10719.0E0, 42.375E0, 59.753E0, -11363.0E0, 1.7844E0, 30.268E0, -0.35372E-01, -0.66832E-01,
            0.16456E-01, -1.3024E0, 0.16529E-02, 0.20293E-02, 20.289E0, -0.25203E-01, 224.91E0, -9234.8E0, 22.788E0,
            7.8813E0, 1.8362E0, -0.27228E0, 8.8184E0, 2.8714E0, 14.468E0, 32.177E0, 0.01E0, 0.0E0, 7.0459E0, 4.0E0,
            20.0E0
        },
        {
            -55.553E0, -13198.0E0, 60.647E0, 61.072E0, -16064.0E0, 2.2534E0, 34.407E0, -0.38887E-01, -0.94571E-01,
            0.27154E-01, -1.3901E0, 0.13460E-02, 0.13238E-02, 23.005E0, -0.30565E-01, 55.047E0, -3875.7E0, 20.178E0,
            7.9693E0, 1.4575E0, 0.89471E0, 9.4039E0, 3.5215E0, 14.474E0, 36.555E0, 0.01E0, 0.0E0, 7.0787E0, 4.0E0,
            20.0E0
        },
        {
            -101.34E0, -13480.0E0, 111.35E0, 12.386E0, -24699.0E0, 2.6459E0, 38.948E0, -0.34080E-01, -0.12404E0,
            0.29702E-01, -1.4052E0, 0.12103E-02, 0.16381E-02, 24.49E0, -0.37705E-01, -298.32E0, 4400.9E0, 18.692E0,
            7.9064E0, 1.3047E0, 2.4541E0, 9.7012E0, 7.1624E0, 14.288E0, 33.822E0, 0.01E0, 0.0E0, 6.7442E0, 4.0E0, 20.0E0
        },
        {
            -181.69E0, -12320.0E0, 173.79E0, -96.664E0, -39051.0E0, 3.2633E0, 44.968E0, -0.46377E-01, -0.16686E0,
            0.048298E0, -1.5473E0, 0.10277E-02, 0.31632E-02, 27.341E0, -0.50655E-01, -514.10E0, 12482.0E0, 16.257E0,
            8.5834E0, 1.0194E0, 3.6148E0, 8.6042E0, 5.5057E0, 13.778E0, 32.373E0, 0.01E0, 0.0E0, 7.3195E0, 4.0E0, 20.0E0
        },
        {
            -436.54E0, -9001.0E0, 323.66E0, -410.08E0, -50340.0E0, 3.9932E0, 58.524E0, -0.38519E-01, -0.26822E0,
            0.74528E-01, -1.4268E0, -0.10985E-02, 0.96613E-02, 27.557E0, -0.56522E-01, -867.03E0, 20652.0E0, 14.101E0,
            8.3501E0, 0.72996E0, 3.8149E0, 9.2908E0, 6.4674E0, 13.729E0, 28.353E0, 0.01E0, 0.0E0, 7.4237E0, 4.0E0,
            20.0E0
        },
        {
            -707.77E0, -4471.9E0, 432.81E0, -435.51E0, -60400.0E0, 4.6229E0, 68.178E0, -0.88245E-01, -0.21002E0,
            0.11846E0, -2.6711E0, 0.22305E-02, 0.10910E-01, 27.547E0, -0.54080E-01, -424.23E0, 1100.2E0, 13.954E0,
            7.5337E0, 0.89714E0, 3.7813E0, 8.2945E0, 5.174E0, 14.213E0, 25.237E0, 0.01E0, 0.0E0, 7.0037E0, 4.0E0, 20.0E0
        },
        {
            -1190.4E0, 2749.9E0, 742.56E0, -1110.3E0, -77193.0E0, 7.6727E0, 102.05E0, -0.96015E-01, -0.74507E0,
            0.11214E0, -1.3614E0, 0.15157E-02, 0.22283E-01, 23.164E0, -0.74146E-01, -2219.1E0, 48253.0E0, 12.714E0,
            7.6777E0, 0.57138E0, 2.9633E0, 9.3909E0, 9.7263E0, 11.123E0, 21.558E0, 0.01E0, 0.0E0, 4.4518E0, 4.0E0,
            20.0E0
        }
    };

    private static readonly Coefficients[] s_coefficients = CreateCoefficients();

    private static Coefficients[] CreateCoefficients()
    {
        Coefficients[] coefficients = new Coefficients[s_parameters.GetLength(0)];
        for (int i = 0; i < coefficients.Length; i++)
        {
            coefficients[i] = new Coefficients(i);
        }

        return coefficients;
    }

    /// <summary>
    /// Immutable T89 coefficients of one IOPT, derived from a row of <see cref="s_parameters"/>
    /// </summary>
    /// <remarks>
    /// Each value is computed with the same expression as the IOPT branch of the Fortran
    /// subroutine T89C, so the field values stay bitwise identical to the original port
    /// </remarks>
    private sealed class Coefficients
    {
        public Coefficients(int row)
        {
            double DYC = s_parameters[row, 29];
            double DYC2 = DYC * DYC;
            DX = s_parameters[row, 17];
            HA02 = 0.5D * A02;
            RDYC2 = 1.0D / DYC2;
            HLWC2M = -0.5D * XLWC2;
            DRDYC2 = -2.0D * RDYC2;
            HXLW2M = -0.5D * XLW2;
            ADR = s_parameters[row, 18];
            D0 = s_parameters[row, 19];
            DD = s_parameters[row, 20];
            RC = s_parameters[row, 21];
            G = s_parameters[row, 22];
            AT = s_parameters[row, 23];
            DT = D0;
            DEL = s_parameters[row, 25];
            P = s_parameters[row, 24];
            Q = s_parameters[row, 26];
            SX = s_parameters[row, 27];
            GAM = s_parameters[row, 28];
            HXLD2M = -0.5D * XLD2;
            double W1 = -0.5D / DX;
            DBLDEL = 2.0D * DEL;
            double W2 = W1 * 2.0D;
            double W4 = -1.0D / 3.0D;
            double W3 = W4 / DX;
            double W5 = -0.5D;
            double W6 = -3.0D;
            AK1 = s_parameters[row, 0];
            AK2 = s_parameters[row, 1];
            AK3 = s_parameters[row, 2];
            AK4 = s_parameters[row, 3];
            AK5 = s_parameters[row, 4];
            AK6 = s_parameters[row, 5];
            AK7 = s_parameters[row, 6];
            AK8 = s_parameters[row, 7];
            AK9 = s_parameters[row, 8];
            AK10 = s_parameters[row, 9];
            AK11 = s_parameters[row, 10];
            AK12 = s_parameters[row, 11];
            AK13 = s_parameters[row, 12];
            AK14 = s_parameters[row, 13];
            AK15 = s_parameters[row, 14];
            AK16 = s_parameters[row, 15];
            AK17 = s_parameters[row, 16];
            SXA = 0.0D;
            SYA = 0.0D;
            SZA = 0.0D;
            AK610 = AK6 * W1 + AK10 * W5;
            AK711 = AK7 * W2 - AK11;
            AK812 = AK8 * W2 + AK12 * W6;
            AK913 = AK9 * W3 + AK13 * W4;
        }

        public double DX { get; }
        public double HA02 { get; }
        public double RDYC2 { get; }
        public double HLWC2M { get; }
        public double DRDYC2 { get; }
        public double HXLW2M { get; }
        public double ADR { get; }
        public double D0 { get; }
        public double DD { get; }
        public double RC { get; }
        public double G { get; }
        public double AT { get; }
        public double DT { get; }
        public double DEL { get; }
        public double P { get; }
        public double Q { get; }
        public double SX { get; }
        public double GAM { get; }
        public double HXLD2M { get; }
        public double DBLDEL { get; }
        public double AK1 { get; }
        public double AK2 { get; }
        public double AK3 { get; }
        public double AK4 { get; }
        public double AK5 { get; }
        public double AK6 { get; }
        public double AK7 { get; }
        public double AK8 { get; }
        public double AK9 { get; }
        public double AK10 { get; }
        public double AK11 { get; }
        public double AK12 { get; }
        public double AK13 { get; }
        public double AK14 { get; }
        public double AK15 { get; }
        public double AK16 { get; }
        public double AK17 { get; }
        public double SXA { get; }
        public double SYA { get; }
        public double SZA { get; }
        public double AK610 { get; }
        public double AK711 { get; }
        public double AK812 { get; }
        public double AK913 { get; }
    }
}
