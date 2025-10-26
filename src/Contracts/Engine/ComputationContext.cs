namespace AuroraScienceHub.Geopack.Contracts.Engine;

/// <summary>
/// Immutable computation context containing pre-calculated coefficients
/// for specific datetime and solar wind conditions
/// </summary>
public sealed record ComputationContext(double[] H, double[] G, double[] REC);

/// <summary>
/// Elements of the matrix MAG → SM
/// </summary>
public sealed record MagSmCoefficients(double SFI, double CFI);

/// <summary>
/// Dipole tilt angle in the GSW system
/// </summary>
public sealed record DipoleTilt(double SPS, double CPS, double PSI);

/// <summary>
///  Geomagnetic dipole orientation (GEO coordinates)
/// </summary>
public sealed record DipoleOrientation(
    double SL0, double CL0, double ST0, double CT0,
    double STCL, double STSL, double CTSL, double CTCL);

// Greenwich Sidereal Time
public sealed record SiderealTime(double CGST, double SGST);

/// <summary>
/// Gauss coefficients (cosine terms)
/// </summary>
public sealed record G(double[] GaussG);

/// <summary>
/// Gauss coefficients (sine terms)
/// </summary>
public sealed record H(double[] GaussH);

/// <summary>
/// Recursion coefficients for Legendre polynomials
/// </summary>
public sealed record Rec(double[] Recursion);

/// <summary>
///  Matrix (3×3)
/// </summary>
public sealed record Matrix3x3(
    double M11, double M12, double M13,
    double M21, double M22, double M23,
    double M31, double M32, double M33);
