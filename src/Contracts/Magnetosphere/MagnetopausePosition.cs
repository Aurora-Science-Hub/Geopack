namespace AuroraScienceHub.Geopack.Contracts.Magnetosphere;

/// <summary>
/// Object location relative to magnetopause
/// </summary>
public enum MagnetopausePosition
{
    /// <summary>
    /// Not defined magnetopause
    /// </summary>
    NotDefined,

    /// <summary>
    /// Location inside magnetosphere
    /// </summary>
    Inside,

    /// <summary>
    /// Location outside magnetosphere
    /// </summary>
    Outside
}
