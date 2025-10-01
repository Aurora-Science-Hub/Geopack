using AuroraScienceHub.Geopack.Contracts;
using AuroraScienceHub.Geopack.Interfaces;

namespace AuroraScienceHub.Geopack.T89;

public sealed class T89 : IT89
{
    private static double[] A = new double[30];
    private static int IOP = 10;
    private static double DYC, DYC2, DX, HA02, RDX2M, RDX2, RDYC2, HLWC2M, DRDYC2, DRDYC3;
    private static double HXLW2M, ADR, D0, DD, RC, G, AT, DT, DEL, P, Q, SX, GAM;
    private static double HXLD2M, ADSL, XGHS, H, HS, GAMH, W1, DBLDEL, W2, W4, W3, W5, W6;
    private static double AK1, AK2, AK3, AK4, AK5, AK6, AK7, AK8, AK9, AK10, AK11, AK12;
    private static double AK13, AK14, AK15, AK16, AK17, SXA, SYA, SZA, AK610, AK711, AK812;
    private static double AK913, RDXL, HRDXL, A6H, A9T, YNP, YND;

    public CartesianFieldVector Calculate(int IOPT, double[] PARMOD, double PS, double X, double Y, double Z)
    {
        double[,] PARAM = {
            {-116.53, -10719, 42.375, 59.753, -11363, 1.7844, 30.268, -0.0035372, -0.066832, 0.016456,
             -1.3024, 0.0016529, 0.0020293, 20.289, -0.025203, 224.91, -9234.8, 22.788, 7.8813, 1.8362,
             -0.27228, 8.8184, 2.8714, 14.468, 32.177, 0.01, 0.0, 7.0459, 4.0, 20.0},
            {-55.553, -13198, 60.647, 61.072, -16064, 2.2534, 34.407, -0.038887, -0.094571, 0.027154,
             -1.3901, 0.001346, 0.0013238, 23.005, -0.030565, 55.047, -3875.7, 20.178, 7.9693, 1.4575,
             0.89471, 9.4039, 3.5215, 14.474, 36.555, 0.01, 0.0, 7.0787, 4.0, 20.0},
            {-101.34, -13480, 111.35, 12.386, -24699, 2.6459, 38.948, -0.03408, -0.12404, 0.029702,
             -1.4052, 0.0012103, 0.0016381, 24.49, -0.037705, -298.32, 4400.9, 18.692, 7.9064, 1.3047,
             2.4541, 9.7012, 7.1624, 14.288, 33.822, 0.01, 0.0, 6.7442, 4.0, 20.0},
            {-181.69, -12320, 173.79, -96.664, -39051, 3.2633, 44.968, -0.046377, -0.16686, 0.048298,
             -1.5473, 0.0010277, 0.0031632, 27.341, -0.050655, -514.10, 12482, 16.257, 8.5834, 1.0194,
             3.6148, 8.6042, 5.5057, 13.778, 32.373, 0.01, 0.0, 7.3195, 4.0, 20.0},
            {-436.54, -9001.0, 323.66, -410.08, -50340, 3.9932, 58.524, -0.038519, -0.26822, 0.074528,
             -1.4268, -0.0010985, 0.0096613, 27.557, -0.056522, -867.03, 20652, 14.101, 8.3501, 0.72996,
             3.8149, 9.2908, 6.4674, 13.729, 28.353, 0.01, 0.0, 7.4237, 4.0, 20.0},
            {-707.77, -4471.9, 432.81, -435.51, -60400, 4.6229, 68.178, -0.088245, -0.21002, 0.11846,
             -2.6711, 0.0022305, 0.001091, 27.547, -0.05408, -424.23, 1100.2, 13.954, 7.5337, 0.89714,
             3.7813, 8.2945, 5.174, 14.213, 25.237, 0.01, 0.0, 7.0037, 4.0, 20.0},
            {-1190.4, 2749.9, 742.56, -1110.3, -77193, 7.6727, 102.05, -0.096015, -0.74507, 0.11214,
             -1.3614, 0.0015157, 0.0022283, 23.164, -0.074146, -2219.1, 48253, 12.714, 7.6777, 0.57138,
             2.9633, 9.3909, 9.7263, 11.123, 21.558, 0.01, 0.0, 4.4518, 4.0, 20.0}
        };

        double A02 = 25, XLW2 = 170, YN = 30, RPI = 0.31830989, RT = 30;
        double XD = 0, XLD2 = 40;
        double SXC = 4, XLWC2 = 50;
        double DXL = 20;

        if (IOP != IOPT)
        {
            IOP = IOPT;
            for (int i = 0; i < 30; i++)
                A[i] = PARAM[i, IOPT - 1];

            DYC = A[29];
            DYC2 = DYC * DYC;
            DX = A[17];
            HA02 = 0.5 * A02;
            RDX2M = -1.0 / (DX * DX);
            RDX2 = -RDX2M;
            RDYC2 = 1.0 / DYC2;
            HLWC2M = -0.5 * XLWC2;
            DRDYC2 = -2.0 * RDYC2;
            DRDYC3 = 2.0 * RDYC2 * Math.Sqrt(RDYC2);
            HXLW2M = -0.5 * XLW2;
            ADR = A[18];
            D0 = A[19];
            DD = A[20];
            RC = A[21];
            G = A[22];
            AT = A[23];
            DT = D0;
            DEL = A[25];
            P = A[24];
            Q = A[26];
            SX = A[27];
            GAM = A[28];
            HXLD2M = -0.5 * XLD2;
            ADSL = 0.0;
            XGHS = 0.0;
            H = 0.0;
            HS = 0.0;
            GAMH = 0.0;
            W1 = -0.5 / DX;
            DBLDEL = 2.0 * DEL;
            W2 = W1 * 2.0;
            W4 = -1.0 / 3.0;
            W3 = W4 / DX;
            W5 = -0.5;
            W6 = -3.0;
            AK1 = A[0];
            AK2 = A[1];
            AK3 = A[2];
            AK4 = A[3];
            AK5 = A[4];
            AK6 = A[5];
            AK7 = A[6];
            AK8 = A[7];
            AK9 = A[8];
            AK10 = A[9];
            AK11 = A[10];
            AK12 = A[11];
            AK13 = A[12];
            AK14 = A[13];
            AK15 = A[14];
            AK16 = A[15];
            AK17 = A[16];
            SXA = 0.0;
            SYA = 0.0;
            SZA = 0.0;
            AK610 = AK6 * W1 + AK10 * W5;
            AK711 = AK7 * W2 - AK11;
            AK812 = AK8 * W2 + AK12 * W6;
            AK913 = AK9 * W3 + AK13 * W4;
            RDXL = 1.0 / DXL;
            HRDXL = 0.5 * RDXL;
            A6H = AK6 * 0.5;
            A9T = AK9 / 3.0;
            YNP = RPI / YN * 0.5;
            YND = 2.0 * YN;
        }

        double SPS = Math.Sin(PS);
        double CPS = Math.Cos(PS);

        double X2 = X * X;
        double Y2 = Y * Y;
        double Z2 = Z * Z;
        double TPS = SPS / CPS;
        double HTP = TPS * 0.5;
        double GSP = G * SPS;
        double XSM = X * CPS - Z * SPS;
        double ZSM = X * SPS + Z * CPS;

        double XRC = XSM + RC;
        double XRC16 = XRC * XRC + 16.0;
        double SXRC = Math.Sqrt(XRC16);
        double Y4 = Y2 * Y2;
        double Y410 = Y4 + 10000.0;
        double SY4 = SPS / Y410;
        double GSY4 = G * SY4;
        double ZS1 = HTP * (XRC - SXRC);
        double DZSX = -ZS1 / SXRC;
        double ZS = ZS1 - GSY4 * Y4;
        double D2ZSGY = -SY4 / Y410 * 40000.0 * Y2 * Y;
        double DZSY = G * D2ZSGY;

        double XSM2 = XSM * XSM;
        double DSQT = Math.Sqrt(XSM2 + A02);
        double FA0 = 0.5 * (1.0 + XSM / DSQT);
        double DDR = D0 + DD * FA0;
        double DFA0 = HA02 / (DSQT * DSQT * DSQT);
        double ZR = ZSM - ZS;
        double TR = Math.Sqrt(ZR * ZR + DDR * DDR);
        double RTR = 1.0 / TR;
        double RO2 = XSM2 + Y2;
        double ADRT = ADR + TR;
        double ADRT2 = ADRT * ADRT;
        double FK = 1.0 / (ADRT2 + RO2);
        double DSFC = Math.Sqrt(FK);
        double FC = FK * FK * DSFC;
        double FACXY = 3.0 * ADRT * FC * RTR;
        double XZR = XSM * ZR;
        double YZR = Y * ZR;
        double DBXDP = FACXY * XZR;
        double DER25 = FACXY * YZR;
        double XZYZ = XSM * DZSX + Y * DZSY;
        double FAQ = ZR * XZYZ - DDR * DD * DFA0 * XSM;
        double DBZDP = FC * (2.0 * ADRT2 - RO2) + FACXY * FAQ;
        double DER15 = DBXDP * CPS + DBZDP * SPS;
        double DER35 = DBZDP * CPS - DBXDP * SPS;

        double DELY2 = DEL * Y2;
        double D = DT + DELY2;
        if (Math.Abs(GAM) < 1e-6)
        {
            H = 0.0;
            HS = 0.0;
            GAMH = 0.0;
            ADSL = 0.0;
        }
        else
        {
            double XXD = XSM - XD;
            double RQD = 1.0 / (XXD * XXD + XLD2);
            double RQDS = Math.Sqrt(RQD);
            H = 0.5 * (1.0 + XXD * RQDS);
            HS = -HXLD2M * RQD * RQDS;
            GAMH = GAM * H;
            D += GAMH;
            XGHS = XSM * GAM * HS;
            ADSL = -D * XGHS;
        }
        double D2 = D * D;
        double T = Math.Sqrt(ZR * ZR + D2);
        double XSMX = XSM - SX;
        double RDSQ2 = 1.0 / (XSMX * XSMX + XLW2);
        double RDSQ = Math.Sqrt(RDSQ2);
        double V = 0.5 * (1.0 - XSMX * RDSQ);
        double DVX = HXLW2M * RDSQ * RDSQ2;
        double OM = Math.Sqrt(Math.Sqrt(XSM2 + 16.0) - XSM);
        double OMS = -OM / (OM * OM + XSM) * 0.5;
        double RDY = 1.0 / (P + Q * OM);
        double OMSV = OMS * V;
        double RDY2 = RDY * RDY;
        double FY = 1.0 / (1.0 + Y2 * RDY2);
        double W = V * FY;
        double YFY1 = 2.0 * FY * Y2 * RDY2;
        double FYPR = YFY1 * RDY;
        double FYDY = FYPR * FY;
        double DWX = DVX * FY + FYDY * Q * OMSV;
        double YDWY = -V * YFY1 * FY;
        double DDY = DBLDEL * Y;
        double ATT = AT + T;
        double S1 = Math.Sqrt(ATT * ATT + RO2);
        double F5 = 1.0 / S1;
        double F7 = 1.0 / (S1 + ATT);
        double F1 = F5 * F7;
        double F3 = F5 * F5 * F5;
        double F9 = ATT * F3;
        double FS = ZR * XZYZ - D * Y * DDY + ADSL;
        double XDWX = XSM * DWX + YDWY;
        double RTT = 1.0 / T;
        double WT = W * RTT;
        double BRRZ1 = WT * F1;
        double BRRZ2 = WT * F3;
        double DBXC1 = BRRZ1 * XZR;
        double DBXC2 = BRRZ2 * XZR;

        double TLT2 = PS * PS;

        double DER21 = BRRZ1 * YZR;
        double DER22 = BRRZ2 * YZR;
        double DER216 = DER21 * TLT2;
        double DER217 = DER22 * TLT2;
        double WTFS = WT * FS;
        double DBZC1 = W * F5 + XDWX * F7 + WTFS * F1;
        double DBZC2 = W * F9 + XDWX * F1 + WTFS * F3;
        double DER11 = DBXC1 * CPS + DBZC1 * SPS;
        double DER12 = DBXC2 * CPS + DBZC2 * SPS;
        double DER31 = DBZC1 * CPS - DBXC1 * SPS;
        double DER32 = DBZC2 * CPS - DBXC2 * SPS;
        double DER116 = DER11 * TLT2;
        double DER117 = DER12 * TLT2;
        double DER316 = DER31 * TLT2;
        double DER317 = DER32 * TLT2;

        double ZPL = Z + RT;
        double ZMN = Z - RT;
        double ROGSM2 = X2 + Y2;
        double SPL = Math.Sqrt(ZPL * ZPL + ROGSM2);
        double SMN = Math.Sqrt(ZMN * ZMN + ROGSM2);
        double XSXC = X - SXC;
        double RQC2 = 1.0 / (XSXC * XSXC + XLWC2);
        double RQC = Math.Sqrt(RQC2);
        double FYC = 1.0 / (1.0 + Y2 * RDYC2);
        double WC = 0.5 * (1.0 - XSXC * RQC) * FYC;
        double DWCX = HLWC2M * RQC2 * RQC * FYC;
        double DWCY = DRDYC2 * WC * FYC * Y;
        double SZRP = 1.0 / (SPL + ZPL);
        double SZRM = 1.0 / (SMN - ZMN);
        double XYWC = X * DWCX + Y * DWCY;
        double WCSP = WC / SPL;
        double WCSM = WC / SMN;
        double FXYP = WCSP * SZRP;
        double FXYM = WCSM * SZRM;
        double FXPL = X * FXYP;
        double FXMN = -X * FXYM;
        double FYPL = Y * FXYP;
        double FYMN = -Y * FXYM;
        double FZPL = WCSP + XYWC * SZRP;
        double FZMN = WCSM + XYWC * SZRM;
        double DER13 = FXPL + FXMN;
        double DER14 = (FXPL - FXMN) * SPS;
        double DER23 = FYPL + FYMN;
        double DER24 = (FYPL - FYMN) * SPS;
        double DER33 = FZPL + FZMN;
        double DER34 = (FZPL - FZMN) * SPS;

        double EX = Math.Exp(X / DX);
        double EC = EX * CPS;
        double ES = EX * SPS;
        double ECZ = EC * Z;
        double ESZ = ES * Z;
        double ESZY2 = ESZ * Y2;
        double ESZZ2 = ESZ * Z2;
        double ECZ2 = ECZ * Z;
        double ESY = ES * Y;

        double SX1 = AK6 * ECZ + AK7 * ES + AK8 * ESY * Y + AK9 * ESZ * Z;
        double SY1 = AK10 * ECZ * Y + AK11 * ESY + AK12 * ESY * Y2 + AK13 * ESY * Z2;
        double SZ1 = AK14 * EC + AK15 * EC * Y2 + AK610 * ECZ2 + AK711 * ESZ + AK812 * ESZY2 + AK913 * ESZZ2;
        double BXCL = AK3 * DER13 + AK4 * DER14;
        double BYCL = AK3 * DER23 + AK4 * DER24;
        double BZCL = AK3 * DER33 + AK4 * DER34;
        double BXT = AK1 * DER11 + AK2 * DER12 + BXCL + AK16 * DER116 + AK17 * DER117;
        double BYT = AK1 * DER21 + AK2 * DER22 + BYCL + AK16 * DER216 + AK17 * DER217;
        double BZT = AK1 * DER31 + AK2 * DER32 + BZCL + AK16 * DER316 + AK17 * DER317;
        double BX = BXT + AK5 * DER15 + SX1 + SXA;
        double BY = BYT + AK5 * DER25 + SY1 + SYA;
        double BZ = BZT + AK5 * DER35 + SZ1 + SZA;

        return new CartesianFieldVector(BX, BY, BZ, CoordinateSystem.GSM);
    }
}
