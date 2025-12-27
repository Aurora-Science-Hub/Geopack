using AuroraScienceHub.Geopack.Contracts.Cartesian;
using AuroraScienceHub.Geopack.Contracts.Coordinates;
using AuroraScienceHub.Geopack.Contracts.PhysicalQuantities;

namespace AuroraScienceHub.Geopack.ExternalFieldModels.T89;

internal sealed partial class T89 : IT89
{
    private static double[] A = new double[30];
    private static int IOP = 10;
    private static double DYC, DYC2, DX, HA02, RDX2M, RDX2, RDYC2, HLWC2M, DRDYC2, DRDYC3;
    private static double HXLW2M, ADR, D0, DD, RC, G, AT, DT, DEL, P, Q, SX, GAM;
    private static double HXLD2M, ADSL, XGHS, H, HS, GAMH, W1, DBLDEL, W2, W4, W3, W5, W6;
    private static double AK1, AK2, AK3, AK4, AK5, AK6, AK7, AK8, AK9, AK10, AK11, AK12;
    private static double AK13, AK14, AK15, AK16, AK17, SXA, SYA, SZA, AK610, AK711, AK812;
    private static double AK913, RDXL, HRDXL, A6H, A9T, YNP, YND;

    public CartesianVector<MagneticField> Calculate(int IOPT, double[] PARMOD, double PS, CartesianLocation location)
    {
        if (location.CoordinateSystem is not (CoordinateSystem.GSM or CoordinateSystem.GSW))
        {
            throw new InvalidOperationException("Location must be in GSM or GSW coordinate system.");
        }

        double A02 = 25D, XLW2 = 170D, YN = 30D, RPI = 0.31830989D, RT = 30D;
        double XD = 0D, XLD2 = 40D;
        double SXC = 4D, XLWC2 = 50D;
        double DXL = 20D;

        if (IOP != IOPT)
        {
            IOP = IOPT;
            for (int i = 0; i < 30; i++)
                A[i] = PARAM[IOPT - 1, i];

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

        (double SPS, double CPS) = Math.SinCos(PS);

        double X2 = location.X * location.X;
        double Y2 = location.Y * location.Y;
        double Z2 = location.Z * location.Z;
        double TPS = SPS / CPS;
        double HTP = TPS * 0.5D;
        double GSP = G * SPS;
        double XSM = location.X * CPS - location.Z * SPS;
        double ZSM = location.X * SPS + location.Z * CPS;

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
        double D2ZSGY = -SY4 / Y410 * 40000.0D * Y2 * location.Y;
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
        double YZR = location.Y * ZR;
        double DBXDP = FACXY * XZR;
        double DER25 = FACXY * YZR;
        double XZYZ = XSM * DZSX + location.Y * DZSY;
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
        double DDY = DBLDEL * location.Y;
        double ATT = AT + T;
        double S1 = Math.Sqrt(ATT * ATT + RO2);
        double F5 = 1.0D / S1;
        double F7 = 1.0D / (S1 + ATT);
        double F1 = F5 * F7;
        double F3 = F5 * F5 * F5;
        double F9 = ATT * F3;
        double FS = ZR * XZYZ - D * location.Y * DDY + ADSL;
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

        double ZPL = location.Z + RT;
        double ZMN = location.Z - RT;
        double ROGSM2 = X2 + Y2;
        double SPL = Math.Sqrt(ZPL * ZPL + ROGSM2);
        double SMN = Math.Sqrt(ZMN * ZMN + ROGSM2);
        double XSXC = location.X - SXC;
        double RQC2 = 1.0D / (XSXC * XSXC + XLWC2);
        double RQC = Math.Sqrt(RQC2);
        double FYC = 1.0D / (1.0D + Y2 * RDYC2);
        double WC = 0.5D * (1.0D - XSXC * RQC) * FYC;
        double DWCX = HLWC2M * RQC2 * RQC * FYC;
        double DWCY = DRDYC2 * WC * FYC * location.Y;
        double SZRP = 1.0D / (SPL + ZPL);
        double SZRM = 1.0D / (SMN - ZMN);
        double XYWC = location.X * DWCX + location.Y * DWCY;
        double WCSP = WC / SPL;
        double WCSM = WC / SMN;
        double FXYP = WCSP * SZRP;
        double FXYM = WCSM * SZRM;
        double FXPL = location.X * FXYP;
        double FXMN = -location.X * FXYM;
        double FYPL = location.Y * FXYP;
        double FYMN = -location.Y * FXYM;
        double FZPL = WCSP + XYWC * SZRP;
        double FZMN = WCSM + XYWC * SZRM;
        double DER13 = FXPL + FXMN;
        double DER14 = (FXPL - FXMN) * SPS;
        double DER23 = FYPL + FYMN;
        double DER24 = (FYPL - FYMN) * SPS;
        double DER33 = FZPL + FZMN;
        double DER34 = (FZPL - FZMN) * SPS;

        double EX = Math.Exp(location.X / DX);
        double EC = EX * CPS;
        double ES = EX * SPS;
        double ECZ = EC * location.Z;
        double ESZ = ES * location.Z;
        double ESZY2 = ESZ * Y2;
        double ESZZ2 = ESZ * Z2;
        double ECZ2 = ECZ * location.Z;
        double ESY = ES * location.Y;

        double SX1 = AK6 * ECZ + AK7 * ES + AK8 * ESY * location.Y + AK9 * ESZ * location.Z;
        double SY1 = AK10 * ECZ * location.Y + AK11 * ESY + AK12 * ESY * Y2 + AK13 * ESY * Z2;
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

        return CartesianVector<MagneticField>.New(BX, BY, BZ, CoordinateSystem.GSM);
    }
}
