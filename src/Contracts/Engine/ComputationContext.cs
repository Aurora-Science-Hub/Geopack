namespace AuroraScienceHub.Geopack.Contracts.Engine;

/// <summary>
/// Immutable computation context containing pre-calculated coefficients
/// for specific datetime and solar wind conditions
/// </summary>
public sealed record ComputationContext(
    double ST0, double CT0, double SL0, double CL0,
    double CTCL, double STCL, double CTSL, double STSL,
    double SFI, double CFI,
    double SPS, double CPS, double PSI,
    double CGST, double SGST,
    double A11, double A21, double A31, double A12, double A22, double A32, double A13, double A23, double A33,
    double E11, double E21, double E31, double E12, double E22, double E32, double E13, double E23, double E33,
    double[] H, double[] G, double[] REC);
