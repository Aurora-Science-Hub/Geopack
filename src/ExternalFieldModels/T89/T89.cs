using AuroraScienceHub.Geopack.Contracts.Cartesian;
using AuroraScienceHub.Geopack.Contracts.Coordinates;
using AuroraScienceHub.Geopack.Contracts.PhysicalQuantities;

namespace AuroraScienceHub.Geopack.ExternalFieldModels.T89;

// T89 holds no mutable state, so one instance serves concurrent callers: the coefficient
// set of every IOPT is precomputed once (s_coefficients) and every intermediate value is a local.
internal sealed partial class T89 : IT89
{
    private const double A02 = 25D;
    private const double XLW2 = 170D;
    private const double RT = 30D;
    private const double XD = 0D;
    private const double XLD2 = 40D;
    private const double SXC = 4D;
    private const double XLWC2 = 50D;

    public CartesianVector<MagneticField> Calculate(int IOPT, double[] PARMOD, double PS, CartesianLocation location)
    {
        if (location.CoordinateSystem is not (CoordinateSystem.GSM or CoordinateSystem.GSW))
        {
            throw new InvalidOperationException("Location must be in GSM or GSW coordinate system.");
        }

        ArgumentOutOfRangeException.ThrowIfLessThan(IOPT, 1);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(IOPT, s_coefficients.Length);

        Coefficients c = s_coefficients[IOPT - 1];

        (double SPS, double CPS) = Math.SinCos(PS);

        double X2 = location.X * location.X;
        double Y2 = location.Y * location.Y;
        double Z2 = location.Z * location.Z;
        double TPS = SPS / CPS;
        double HTP = TPS * 0.5D;
        double GSP = c.G * SPS;
        double XSM = location.X * CPS - location.Z * SPS;
        double ZSM = location.X * SPS + location.Z * CPS;

        double XRC = XSM + c.RC;
        double XRC16 = XRC * XRC + 16.0D;
        double SXRC = Math.Sqrt(XRC16);
        double Y4 = Y2 * Y2;
        double Y410 = Y4 + 10000.0D;
        double SY4 = SPS / Y410;
        double GSY4 = c.G * SY4;
        double ZS1 = HTP * (XRC - SXRC);
        double DZSX = -ZS1 / SXRC;
        double ZS = ZS1 - GSY4 * Y4;
        double D2ZSGY = -SY4 / Y410 * 40000.0D * Y2 * location.Y;
        double DZSY = c.G * D2ZSGY;

        double XSM2 = XSM * XSM;
        double DSQT = Math.Sqrt(XSM2 + A02);
        double FA0 = 0.5D * (1.0D + XSM / DSQT);
        double DDR = c.D0 + c.DD * FA0;
        double DFA0 = c.HA02 / (DSQT * DSQT * DSQT);
        double ZR = ZSM - ZS;
        double TR = Math.Sqrt(ZR * ZR + DDR * DDR);
        double RTR = 1.0D / TR;
        double RO2 = XSM2 + Y2;
        double ADRT = c.ADR + TR;
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
        double FAQ = ZR * XZYZ - DDR * c.DD * DFA0 * XSM;
        double DBZDP = FC * (2.0D * ADRT2 - RO2) + FACXY * FAQ;
        double DER15 = DBXDP * CPS + DBZDP * SPS;
        double DER35 = DBZDP * CPS - DBXDP * SPS;

        double DELY2 = c.DEL * Y2;
        double D = c.DT + DELY2;
        double ADSL;
        if (Math.Abs(c.GAM) < 1e-6)
        {
            ADSL = 0.0D;
        }
        else
        {
            double XXD = XSM - XD;
            double RQD = 1.0D / (XXD * XXD + XLD2);
            double RQDS = Math.Sqrt(RQD);
            double H = 0.5D * (1.0D + XXD * RQDS);
            double HS = -c.HXLD2M * RQD * RQDS;
            double GAMH = c.GAM * H;
            D += GAMH;
            double XGHS = XSM * c.GAM * HS;
            ADSL = -D * XGHS;
        }
        double D2 = D * D;
        double T = Math.Sqrt(ZR * ZR + D2);
        double XSMX = XSM - c.SX;
        double RDSQ2 = 1.0D / (XSMX * XSMX + XLW2);
        double RDSQ = Math.Sqrt(RDSQ2);
        double V = 0.5D * (1.0D - XSMX * RDSQ);
        double DVX = c.HXLW2M * RDSQ * RDSQ2;
        double OM = Math.Sqrt(Math.Sqrt(XSM2 + 16.0D) - XSM);
        double OMS = -OM / (OM * OM + XSM) * 0.5D;
        double RDY = 1.0D / (c.P + c.Q * OM);
        double OMSV = OMS * V;
        double RDY2 = RDY * RDY;
        double FY = 1.0D / (1.0D + Y2 * RDY2);
        double W = V * FY;
        double YFY1 = 2.0D * FY * Y2 * RDY2;
        double FYPR = YFY1 * RDY;
        double FYDY = FYPR * FY;
        double DWX = DVX * FY + FYDY * c.Q * OMSV;
        double YDWY = -V * YFY1 * FY;
        double DDY = c.DBLDEL * location.Y;
        double ATT = c.AT + T;
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
        double FYC = 1.0D / (1.0D + Y2 * c.RDYC2);
        double WC = 0.5D * (1.0D - XSXC * RQC) * FYC;
        double DWCX = c.HLWC2M * RQC2 * RQC * FYC;
        double DWCY = c.DRDYC2 * WC * FYC * location.Y;
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
        double FZPL = Math.FusedMultiplyAdd(WCSP, 1.0, XYWC * SZRP);
        double FZMN = Math.FusedMultiplyAdd(WCSM, 1.0, XYWC * SZRM);
        double DER13 = FXPL + FXMN;
        double DER14 = (FXPL - FXMN) * SPS;
        double DER23 = FYPL + FYMN;
        double DER24 = (FYPL - FYMN) * SPS;
        double DER33 = FZPL + FZMN;
        double DER34 = (FZPL - FZMN) * SPS;

        double EX = Math.Exp(location.X / c.DX);
        double EC = EX * CPS;
        double ES = EX * SPS;
        double ECZ = EC * location.Z;
        double ESZ = ES * location.Z;
        double ESZY2 = ESZ * Y2;
        double ESZZ2 = ESZ * Z2;
        double ECZ2 = ECZ * location.Z;
        double ESY = ES * location.Y;

        double SX1 = c.AK6 * ECZ + c.AK7 * ES + c.AK8 * ESY * location.Y + c.AK9 * ESZ * location.Z;
        double SY1 = c.AK10 * ECZ * location.Y + c.AK11 * ESY + c.AK12 * ESY * Y2 + c.AK13 * ESY * Z2;
        double SZ1 = c.AK14 * EC + c.AK15 * EC * Y2 + c.AK610 * ECZ2 + c.AK711 * ESZ + c.AK812 * ESZY2 + c.AK913 * ESZZ2;
        double BXCL = c.AK3 * DER13 + c.AK4 * DER14;
        double BYCL = c.AK3 * DER23 + c.AK4 * DER24;
        double BZCL = c.AK3 * DER33 + c.AK4 * DER34;
        double BXT = c.AK1 * DER11 + c.AK2 * DER12 + BXCL + c.AK16 * DER116 + c.AK17 * DER117;
        double BYT = c.AK1 * DER21 + c.AK2 * DER22 + BYCL + c.AK16 * DER216 + c.AK17 * DER217;
        double BZT = c.AK1 * DER31 + c.AK2 * DER32 + BZCL + c.AK16 * DER316 + c.AK17 * DER317;
        double BX = BXT + c.AK5 * DER15 + SX1 + c.SXA;
        double BY = BYT + c.AK5 * DER25 + SY1 + c.SYA;
        double BZ = BZT + c.AK5 * DER35 + SZ1 + c.SZA;

        return CartesianVector<MagneticField>.New(BX, BY, BZ, CoordinateSystem.GSM);
    }
}
