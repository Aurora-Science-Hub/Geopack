namespace AuroraScienceHub.Geopack.Contracts;

/// <summary>
/// Specifies the direction of tracing relative to the Earth's main magnetic field.
/// </summary>
public enum TraceDirection
{
    /// <summary>
    /// Antiparallel to the main Earth's magnetic field
    /// </summary>
    AntiParallel = 1,

    /// <summary>
    /// Parallel to the main Earth's magnetic field
    /// </summary>
    Parallel = -1
}
