 namespace AuroraScienceHub.Geopack.Contracts.Models;

/// <summary>
/// Magnetopause location relative to any point in space
/// </summary>
/// <param name="X"> X-coordinate of the boundary point </param>
/// <param name="Y"> Y-coordinate of the boundary point </param>
/// <param name="Z"> Z-coordinate of the boundary point </param>
/// <param name="Dist"> Distance (in Re) between observation point and the model magnetopause </param>
/// <param name="Position"> Position flag </param>
/// <param name="CoordinateSystem"> Coordinate system </param>
public sealed record Magnetopause(
    double X, double Y, double Z, double Dist,
    MagnetopausePosition Position,
    CoordinateSystem CoordinateSystem);
