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
            {
                -116.53D, -10719D, 42.375D, 59.753D, -11363D, 1.7844D, 30.268D, -0.0035372D, -0.066832D, 0.016456D,
             -1.3024D, 0.0016529D, 0.0020293D, 20.289D, -0.025203D, 224.91D, -9234.8D, 22.788D, 7.8813D, 1.8362D,
             -0.27228D, 8.8184D, 2.8714D, 14.468D, 32.177D, 0.01D, 0.0D, 7.0459D, 4.0D, 20.0D
            },
            {
                -55.553D, -13198D, 60.647D, 61.072D, -16064D, 2.2534D, 34.407D, -0.038887D, -0.094571D, 0.027154D,
             -1.3901D, 0.001346D, 0.0013238D, 23.005D, -0.030565D, 55.047D, -3875.7D, 20.178D, 7.9693D, 1.4575D,
             0.89471D, 9.4039D, 3.5215D, 14.474D, 36.555D, 0.01D, 0.0D, 7.0787D, 4.0D, 20.0D
            },
            {
                -101.34D, -13480D, 111.35D, 12.386D, -24699D, 2.6459D, 38.948D, -0.03408D, -0.12404D, 0.029702D,
             -1.4052D, 0.0012103D, 0.0016381D, 24.49D, -0.037705D, -298.32D, 4400.9D, 18.692D, 7.9064D, 1.3047D,
             2.4541D, 9.7012D, 7.1624D, 14.288D, 33.822D, 0.01D, 0.0D, 6.7442D, 4.0D, 20.0D
            },
            {
                -181.69D, -12320D, 173.79D, -96.664D, -39051D, 3.2633D, 44.968D, -0.046377D, -0.16686D, 0.048298D,
             -1.5473D, 0.0010277D, 0.0031632D, 27.341D, -0.050655D, -514.10D, 12482D, 16.257D, 8.5834D, 1.0194D,
             3.6148D, 8.6042D, 5.5057, 13.778D, 32.373D, 0.01D, 0.0D, 7.3195D, 4.0D, 20.0D
            },
            {
                -436.54D, -9001.0D, 323.66D, -410.08D, -50340D, 3.9932D, 58.524D, -0.038519D, -0.26822D, 0.074528D,
             -1.4268D, -0.0010985D, 0.0096613D, 27.557D, -0.056522D, -867.03D, 20652D, 14.101D, 8.3501D, 0.72996D,
             3.8149D, 9.2908D, 6.4674D, 13.729D, 28.353D, 0.01D, 0.0D, 7.4237D, 4.0D, 20.0D
            },
            {
                -707.77D, -4471.9D, 432.81D, -435.51D, -60400D, 4.6229D, 68.178D, -0.088245D, -0.21002D, 0.11846D,
             -2.6711D, 0.0022305D, 0.001091D, 27.547D, -0.05408D, -424.23D, 1100.2D, 13.954D, 7.5337D, 0.89714D,
             3.7813D, 8.2945D, 5.174D, 14.213D, 25.237D, 0.01D, 0.0D, 7.0037D, 4.0D, 20.0D
            },
            {
                -1190.4D, 2749.9D, 742.56D, -1110.3D, -77193D, 7.6727D, 102.05D, -0.096015D, -0.74507D, 0.11214D,
             -1.3614D, 0.0015157D, 0.0022283D, 23.164D, -0.074146D, -2219.1D, 48253D, 12.714D, 7.6777D, 0.57138D,
             2.9633D, 9.3909D, 9.7263D, 11.123D, 21.558D, 0.01D, 0.0D, 4.4518D, 4.0D, 20.0D
            }
        };

        double A02 = 25D, XLW2 = 170D, YN = 30D, RPI = 0.31830989D, RT = 30D;
        double XD = 0D, XLD2 = 40D;
        double SXC = 4D, XLWC2 = 50D;
        double DXL = 20D;

        if (IOP != IOPT)
        {
            IOP = IOPT;
            for (int i = 0; i < 30; i++)
                A[i] = PARAM[i, IOPT - 1];

            DYC = A[29];
            DYC2 = DYC * DYC;
            DX = A[17];
            HA02 = 0.5D * A02;
            RDX2M = -1.0D / (DX * DX);
            RDX2 = -RDX2M;
            RDYC2 = 1.0D / DYC2;
            HLWC2M = -0.5D * XLWC2;
            DRDYC2 = -2.0D * RDYC2;
            DRDYC3 = 2.0D * RDYC2 * Math.Sqrt(RDYC2);
            HXLW2M = -0.5D * XLW2;
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
            HXLD2M = -0.5D * XLD2;
            ADSL = 0.0D;
            XGHS = 0.0D;
            H = 0.0D;
            HS = 0.0D;
            GAMH = 0.0D;
            W1 = -0.5D / DX;
            DBLDEL = 2.0D * DEL;
            W2 = W1 * 2.0D;
            W4 = -1.0D / 3.0D;
            W3 = W4 / DX;
            W5 = -0.5D;
            W6 = -3.0D;
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
            SXA = 0.0D;
            SYA = 0.0D;
            SZA = 0.0D;
            AK610 = AK6 * W1 + AK10 * W5;
            AK711 = AK7 * W2 - AK11;
            AK812 = AK8 * W2 + AK12 * W6;
            AK913 = AK9 * W3 + AK13 * W4;
            RDXL = 1.0D / DXL;
            HRDXL = 0.5D * RDXL;
            A6H = AK6 * 0.5D;
            A9T = AK9 / 3.0D;
            YNP = RPI / YN * 0.5D;
            YND = 2.0D * YN;
        }

        double SPS = Math.Sin(PS);
        double CPS = Math.Cos(PS);

        double X2 = X * X;
        double Y2 = Y * Y;
        double Z2 = Z * Z;
        double TPS = SPS / CPS;
        double HTP = TPS * 0.5D;
        double GSP = G * SPS;
        double XSM = X * CPS - Z * SPS;
        double ZSM = X * SPS + Z * CPS;

        double XRC = XSM + RC;
        double XRC16 = XRC * XRC + 16.0D;
        double SXRC = Math.Sqrt(XRC16);
        double Y4 = Y2 * Y2;
        double Y410 = Y4 + 10000.0D;
        double SY4 = SPS / Y410;
        double GSY4 = G * SY4;
        double ZS1 = HTP * (XRC - SXRC);
        double DZSX = -ZS1 / SXRC;
        double ZS = ZS1 - GSY4 * Y4;
        double D2ZSGY = -SY4 / Y410 * 40000.0D * Y2 * Y;
        double DZSY = G * D2ZSGY;

        double XSM2 = XSM * XSM;
        double DSQT = Math.Sqrt(XSM2 + A02);
        double FA0 = 0.5D * (1.0D + XSM / DSQT);
        double DDR = D0 + DD * FA0;
        double DFA0 = HA02 / (DSQT * DSQT * DSQT);
        double ZR = ZSM - ZS;
        double TR = Math.Sqrt(ZR * ZR + DDR * DDR);
        double RTR = 1.0D / TR;
        double RO2 = XSM2 + Y2;
        double ADRT = ADR + TR;
        double ADRT2 = ADRT * ADRT;
        double FK = 1.0D / (ADRT2 + RO2);
        double DSFC = Math.Sqrt(FK);
        double FC = FK * FK * DSFC;
        double FACXY = 3.0D * ADRT * FC * RTR;
        double XZR = XSM * ZR;
        double YZR = Y * ZR;
        double DBXDP = FACXY * XZR;
        double DER25 = FACXY * YZR;
        double XZYZ = XSM * DZSX + Y * DZSY;
        double FAQ = ZR * XZYZ - DDR * DD * DFA0 * XSM;
        double DBZDP = FC * (2.0D * ADRT2 - RO2) + FACXY * FAQ;
        double DER15 = DBXDP * CPS + DBZDP * SPS;
        double DER35 = DBZDP * CPS - DBXDP * SPS;

        double DELY2 = DEL * Y2;
        double D = DT + DELY2;
        if (Math.Abs(GAM) < 1e-6)
        {
            H = 0.0D;
            HS = 0.0D;
            GAMH = 0.0D;
            ADSL = 0.0D;
        }
        else
        {
            double XXD = XSM - XD;
            double RQD = 1.0D / (XXD * XXD + XLD2);
            double RQDS = Math.Sqrt(RQD);
            H = 0.5D * (1.0D + XXD * RQDS);
            HS = -HXLD2M * RQD * RQDS;
            GAMH = GAM * H;
            D += GAMH;
            XGHS = XSM * GAM * HS;
            ADSL = -D * XGHS;
        }
        double D2 = D * D;
        double T = Math.Sqrt(ZR * ZR + D2);
        double XSMX = XSM - SX;
        double RDSQ2 = 1.0D / (XSMX * XSMX + XLW2);
        double RDSQ = Math.Sqrt(RDSQ2);
        double V = 0.5D * (1.0D - XSMX * RDSQ);
        double DVX = HXLW2M * RDSQ * RDSQ2;
        double OM = Math.Sqrt(Math.Sqrt(XSM2 + 16.0D) - XSM);
        double OMS = -OM / (OM * OM + XSM) * 0.5D;
        double RDY = 1.0D / (P + Q * OM);
        double OMSV = OMS * V;
        double RDY2 = RDY * RDY;
        double FY = 1.0D / (1.0D + Y2 * RDY2);
        double W = V * FY;
        double YFY1 = 2.0D * FY * Y2 * RDY2;
        double FYPR = YFY1 * RDY;
        double FYDY = FYPR * FY;
        double DWX = DVX * FY + FYDY * Q * OMSV;
        double YDWY = -V * YFY1 * FY;
        double DDY = DBLDEL * Y;
        double ATT = AT + T;
        double S1 = Math.Sqrt(ATT * ATT + RO2);
        double F5 = 1.0D / S1;
        double F7 = 1.0D / (S1 + ATT);
        double F1 = F5 * F7;
        double F3 = F5 * F5 * F5;
        double F9 = ATT * F3;
        double FS = ZR * XZYZ - D * Y * DDY + ADSL;
        double XDWX = XSM * DWX + YDWY;
        double RTT = 1.0D / T;
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
        double RQC2 = 1.0D / (XSXC * XSXC + XLWC2);
        double RQC = Math.Sqrt(RQC2);
        double FYC = 1.0D / (1.0D + Y2 * RDYC2);
        double WC = 0.5D * (1.0D - XSXC * RQC) * FYC;
        double DWCX = HLWC2M * RQC2 * RQC * FYC;
        double DWCY = DRDYC2 * WC * FYC * Y;
        double SZRP = 1.0D / (SPL + ZPL);
        double SZRM = 1.0D / (SMN - ZMN);
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
