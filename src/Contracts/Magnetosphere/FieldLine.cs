using AuroraScienceHub.Geopack.Contracts.Coordinates;

namespace AuroraScienceHub.Geopack.Contracts.Magnetosphere;

/// <summary>
/// Magnetic field line
/// </summary>
public class FieldLine
{
    /// <summary>
    /// A set of field line points
    /// </summary>
    public List<CartesianLocation> Points { get; set; }

    /// <summary>
    /// Field line end point
    /// </summary>
    public CartesianLocation EndPoint { get; set; }

    /// <summary>
    /// Field line points count
    /// </summary>
    public int ActualPointCount { get; set; }

    /// <summary>
    /// Field-line state message
    /// </summary>
    public string TerminationReason { get; set; }

    public FieldLine(
        List<CartesianLocation> points,
        CartesianLocation endPoint,
        int actualPointCount,
        string terminationReason)
    {
        Points = points;
        EndPoint = endPoint;
        ActualPointCount = actualPointCount;
        TerminationReason = terminationReason;
    }
}
