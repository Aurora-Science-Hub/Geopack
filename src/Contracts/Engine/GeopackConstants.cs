namespace AuroraScienceHub.Geopack.Contracts.Engine;

/// <summary>
/// Constants applied in Geopack calculations
/// </summary>
public static class GeopackConstants
{
    /// <summary>
    /// PI value from original Geopack (lower precision for compatibility)
    /// </summary>
    public const double Pi = 3.141592654D;

    /// <summary>
    /// PI/2 value from original Geopack (lower precision for compatibility)
    /// </summary>
    public const double HalfPi = 1.570796327D;

    /// <summary>
    /// 2π value from original Geopack
    /// </summary>
    public const double TwoPi = 6.283185307D;

    /// <summary>
    /// Radians to degrees conversion factor (180/π)
    /// </summary>
    public const double Rad = 57.295779513D;

    /// <summary>
    /// Equatorial Earth's radius in kilometers
    /// </summary>
    public const double REq = 6378.137D;

    /// <summary>
    /// WGS84 ellipsoid flattening coefficient (Beta = (a-b)/a where a is equatorial radius, b is polar radius)
    /// </summary>
    public const double WGS84Beta = 6.73949674228e-3;

    /// <summary>
    /// Convergence tolerance for iterative coordinate transformations
    /// </summary>
    public const double CoordinateConvergenceTolerance = 1e-6;

    /// <summary>
    /// WGS84 ellipsoid parameter: 1 + Beta
    /// </summary>
    public const double WGS84Ex = 1.0 + WGS84Beta;

    /// <summary>
    /// WGS84 ellipsoid parameter: Beta * (2 + Beta)
    /// </summary>
    public const double WGS84FirstEx = WGS84Beta * (2.0 + WGS84Beta);

    /// <summary>
    /// Earth's axial tilt at J2000 epoch (obliquity of the ecliptic) in degrees
    /// </summary>
    public const double EarthObliquityJ2000 = 23.45229;

    /// <summary>
    /// Rate of change of Earth's obliquity per Julian century in degrees
    /// </summary>
    public const double EarthObliquityRatePerCentury = 0.0130125;

    /// <summary>
    /// Number of days in a Julian century
    /// </summary>
    public const double DaysPerJulianCentury = 36525.0;

    /// <summary>
    /// Solar wind proton mass density conversion factor (converts n*v^2 to pressure in nPa)
    /// Formula: 1.6726e-27 kg (proton mass) * 1e15 (km^3 to m^3) * 1e9 (Pa to nPa) = 1.6726e-6
    /// But empirical value used in models is 1.94e-6
    /// </summary>
    public const double SolarWindDynamicPressureFactor = 1.94e-6;

    /// <summary>
    /// Seconds per hour
    /// </summary>
    public const double SecondsPerHour = 3600.0;

    /// <summary>
    /// Seconds per day
    /// </summary>
    public const double SecondsPerDay = 86400.0;

    /// <summary>
    /// Degrees in a full circle
    /// </summary>
    public const double DegreesPerCircle = 360.0;

    /// <summary>
    /// Degrees in a half circle (semicircle)
    /// </summary>
    public const double DegreesPerSemicircle = 180.0;

    /// <summary>
    /// Average number of days in a year (accounting for leap years)
    /// </summary>
    public const double DaysPerYear = 365.25;

    /// <summary>
    /// Reciprocal of IGRF interpolation interval (1/5 = 0.2) for optimization
    /// </summary>
    public const double IgrfInterpolationIntervalReciprocal = 0.2;

    /// <summary>
    /// Total number of IGRF coefficients
    /// </summary>
    public const int IgrfCoefficientCount = 105;

    /// <summary>
    /// Number of IGRF delta coefficients used in extrapolation
    /// </summary>
    public const int IgrfDeltaCoefficientCount = 45;

    /// <summary>
    /// IGRF extrapolation base year
    /// </summary>
    public const int IgrfExtrapolationBaseYear = 2025;

    /// <summary>
    /// Maximum iterations for Newton's method convergence
    /// </summary>
    public const int NewtonMaxIterations = 1000;

    /// <summary>
    /// Convergence tolerance for Newton's iterative methods
    /// </summary>
    public const double NewtonConvergenceTolerance = 1e-4;
}
