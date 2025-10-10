namespace AuroraScienceHub.Geopack.Contracts.Models;

/// <summary>
/// Magnetic field line
/// </summary>
public class FieldLine
{
    public List<CartesianLocation> Points { get; set; }

    public CartesianLocation EndPoint { get; set; }

    public int ActualPointCount { get; set; }

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
