namespace AuroraScienceHub.Geopack.Contracts.Models;

/// <summary>
/// Magnetic field line
/// </summary>
public class FieldLine
{
    public List<CartesianFieldVector> Points { get; set; }

    public CartesianFieldVector EndPoint { get; set; }

    public int ActualPointCount { get; set; }

    public string TerminationReason { get; set; }

    public FieldLine(
        List<CartesianFieldVector> points,
        CartesianFieldVector endPoint,
        int actualPointCount,
        string terminationReason)
    {
        Points = points;
        EndPoint = endPoint;
        ActualPointCount = actualPointCount;
        TerminationReason = terminationReason;
    }
}
