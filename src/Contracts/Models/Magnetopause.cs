namespace AuroraScienceHub.Geopack.Contracts.Models;

/// <summary>
/// Magnetopause location relative to any point in space
/// </summary>
/// <param name="BoundaryLocation"></param>
/// <param name="Dist"> Distance (in Re) between observation point and the model magnetopause </param>
/// <param name="Position"> Position flag </param>
public readonly record struct Magnetopause(CartesianLocation BoundaryLocation, double Dist, MagnetopausePosition Position);
