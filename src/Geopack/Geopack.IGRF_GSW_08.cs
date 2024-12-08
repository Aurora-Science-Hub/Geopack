namespace AuroraScienceHub.Geopack;

public sealed partial class Geopack
{
    /// <summary>
    /// Calculates components of the main (internal) geomagnetic field in the geocentric solar-wind (GSW) coordinate system.
    /// </summary>
    /// <param name="XGSW">Cartesian geocentric solar-wind coordinate X (in units RE=6371.2 km).</param>
    /// <param name="YGSW">Cartesian geocentric solar-wind coordinate Y (in units RE=6371.2 km).</param>
    /// <param name="ZGSW">Cartesian geocentric solar-wind coordinate Z (in units RE=6371.2 km).</param>
    /// <param name="HXGSW">Output Cartesian geocentric solar-wind component HX of the main geomagnetic field in nanotesla.</param>
    /// <param name="HYGSW">Output Cartesian geocentric solar-wind component HY of the main geomagnetic field in nanotesla.</param>
    /// <param name="HZGSW">Output Cartesian geocentric solar-wind component HZ of the main geomagnetic field in nanotesla.</param>
    public void IGRF_GSW_08(double XGSW, double YGSW, double ZGSW, out double HXGSW, out double HYGSW, out double HZGSW)
    {
        // Common block variables
        var G = new double[105];
        var H = new double[105];
        var REC = new double[105];

        // Local variables
        double[] A = new double[14];
        double[] B = new double[14];

        // Call to GEOGSW_08 (implementation not provided)
        double XGEO = 0, YGEO = 0, ZGEO = 0;
        GEOGSW_08(XGEO, YGEO, ZGEO, XGSW, YGSW, ZGSW, -1);

        double RHO2 = XGEO * XGEO + YGEO * YGEO;
        double R = Math.Sqrt(RHO2 + ZGEO * ZGEO);
        double C = ZGEO / R;
        double RHO = Math.Sqrt(RHO2);
        double S = RHO / R;

        double CF, SF;
        if (S < 1e-10)
        {
            CF = 1.0;
            SF = 0.0;
        }
        else
        {
            CF = XGEO / RHO;
            SF = YGEO / RHO;
        }

        double PP = 1.0 / R;
        double P = PP;

        // Calculate the optimal value of NM based on the value of the radial distance R
        int IRP3 = (int)(R + 2);
        int NM = 3 + 30 / IRP3;
        if (NM > 13) NM = 13;

        int K = NM + 1;
        for (int N = 1; N <= K; N++)
        {
            P *= PP;
            A[N] = P;
            B[N] = P * N;
        }

        P = 1.0;
        double D = 0.0;
        double BBR = 0.0, BBT = 0.0, BBF = 0.0;

        for (int M = 1; M <= K; M++)
        {
            double X, Y;
            if (M == 1)
            {
                X = 0.0;
                Y = 1.0;
            }
            else
            {
                int MM = M - 1;
                double W = X;
                X = W * CF + Y * SF;
                Y = Y * CF - W * SF;
            }

            double Q = P;
            double Z = D;
            double BI = 0.0;
            double P2 = 0.0, D2 = 0.0;

            for (int N = M; N <= K; N++)
            {
                double AN = A[N];
                int MN = N * (N - 1) / 2 + M;
                double E = G[MN];
                double HH = H[MN];
                double W = E * Y + HH * X;
                BBR += B[N] * W * Q;
                BBT -= AN * W * Z;

                if (M != 1)
                {
                    double QQ = Q;
                    if (S < 1e-10) QQ = Z;
                    BI += AN * (E * X - HH * Y) * QQ;
                }

                double XK = REC[MN];
                double DP = C * Z - S * Q - XK * D2;
                double PM = C * Q - XK * P2;
                D2 = Z;
                P2 = Q;
                Z = DP;
                Q = PM;
            }

            D = S * D + C * P;
            P = S * P;

            if (M != 1)
            {
                BI *= (M - 1);
                BBF += BI;
            }
        }

        double BR = BBR;
        double BT = BBT;
        double BF;
        if (S < 1e-10)
        {
            if (C < 0) BBF = -BBF;
            BF = BBF;
        }
        else
        {
            BF = BBF / S;
        }

        double HE = BR * S + BT * C;
        double HXGEO = HE * CF - BF * SF;
        double HYGEO = HE * SF + BF * CF;
        double HZGEO = BR * C - BT * S;

        // Call to GEOGSW_08 (implementation not provided)
        GEOGSW_08(HXGEO, HYGEO, HZGEO, out HXGSW, out HYGSW, out HZGSW, 1);
    }
}
