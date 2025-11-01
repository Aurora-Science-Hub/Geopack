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
    /// 2π value from original Geopack
    /// </summary>
    public const double TwoPi = 6.283185307D;

    /// <summary>
    /// Radians to degrees conversion factor (180/π)
    /// </summary>
    public const double Rad = 57.295779513D;
}
