using AuroraScienceHub.Geopack.Contracts;
using AuroraScienceHub.Geopack.Contracts.Cartesian;
using AuroraScienceHub.Geopack.Contracts.Coordinates;
using AuroraScienceHub.Geopack.Contracts.Engine;
using AuroraScienceHub.Geopack.Contracts.PhysicalObjects;
using AuroraScienceHub.Geopack.Contracts.PhysicalQuantities;
using AuroraScienceHub.Geopack.UnitTests.Extensions;
using AuroraScienceHub.Geopack.UnitTests.Utils;
using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack;

public partial class GeopackTests
{
    [Fact(DisplayName = "Trace field line from North to South")]
    public async Task TraceFieldLineFromNorthToSouth()
    {
        // Arrange
        CartesianVector<Velocity> swVelocity = CartesianVector<Velocity>.New(-304.0D, -16.0D + 29.78D, 4.0D, CoordinateSystem.GSE);
        ComputationContext context = s_geopack.Recalc(fixture.InputData.DateTime, swVelocity);
        InternalFieldModel internalField = s_geopack.IgrfGsw;

        string rawData = await EmbeddedResourceReader.ReadTextAsync(TraceNSResultFileName);
        string[] lines = rawData.SplitLines();

        TraceDirection dir = TraceDirection.AntiParallel;
        double dsmax = 0.1D;
        double err = 0.0001D;
        double rlim = 60.0D;
        double r0 = 1.0D;
        int iopt = 1;
        double[] parmod = new double[10];
        int lmax = 500;

        // Act
        CartesianLocation startingPoint = CartesianLocation.New(-1.02D, 0.8D, 0.9D, CoordinateSystem.GSW);

        FieldLine fieldLine = s_geopack.Trace(context,
            startingPoint,
            dir, dsmax, err, rlim, r0,
            iopt, parmod,
            _t89, internalField,
            lmax);

        // Assert
        fieldLine.ActualPointCount.ShouldBe(lines.Length);

        for (int i = 0; i < lines.Length; i++)
        {
            string[] pars = lines[i].SplitParametersLine();
            fieldLine.Points[i].X.ShouldApproximatelyBe(pars[0].ParseDouble());
            fieldLine.Points[i].Y.ShouldApproximatelyBe(pars[1].ParseDouble());
            fieldLine.Points[i].Z.ShouldApproximatelyBe(pars[2].ParseDouble());
        }
    }

    [Fact(DisplayName = "Trace field line from South to North")]
    public async Task TraceFieldLineFromSouthToNorth()
    {
        // Arrange
        ComputationContext context = s_geopack.Recalc(fixture.InputData.DateTime);
        InternalFieldModel internalField = s_geopack.IgrfGsw;

        string rawData = await EmbeddedResourceReader.ReadTextAsync(TraceSNResultFileName);
        string[] lines = rawData.SplitLines();

        TraceDirection dir = TraceDirection.Parallel;
        double dsmax = 0.1D;
        double err = 0.0001D;
        double rlim = 60.0D;
        double r0 = 1.0D;
        int iopt = 1;
        double[] parmod = new double[10];
        int lmax = 500;

        // Act
        CartesianLocation startingPoint = CartesianLocation.New(-1.02D, 0.8D, -0.9D, CoordinateSystem.GSW);

        FieldLine fieldLine = s_geopack.Trace(context,
            startingPoint,
            dir, dsmax, err, rlim, r0,
            iopt, parmod,
            _t89, internalField,
            lmax);

        // Assert
        fieldLine.ActualPointCount.ShouldBe(lines.Length);

        for (int i = 0; i < lines.Length; i++)
        {
            string[] pars = lines[i].SplitParametersLine();
            fieldLine.Points[i].X.ShouldApproximatelyBe(pars[0].ParseDouble());
            fieldLine.Points[i].Y.ShouldApproximatelyBe(pars[1].ParseDouble());
            fieldLine.Points[i].Z.ShouldApproximatelyBe(pars[2].ParseDouble());
        }
    }

    [Fact(DisplayName = "Trace stopped by the point limit ends at the position after lMax steps")]
    public async Task Trace_WhenPointLimitReached_EndsAtPositionAfterLMaxSteps()
    {
        // Arrange
        string[] lines = await ReadTraceNSReferenceLinesAsync();
        int lmax = 20;

        // Act
        FieldLine fieldLine = TraceNorthToSouth(lmax);

        // Assert
        fieldLine.TerminationReason.ShouldBe("Maximum points exceeded");
        fieldLine.ActualPointCount.ShouldBe(lmax);
        fieldLine.Points.Count.ShouldBe(lmax);

        for (int i = 0; i < lmax - 1; i++)
        {
            fieldLine.Points[i].ShouldMatchReference(lines[i]);
        }

        fieldLine.Points[^1].ShouldMatchReference(lines[lmax]);
        fieldLine.EndPoint.ShouldMatchReference(lines[lmax]);
    }

    [Fact(DisplayName = "Trace reaching the boundary exactly at the lMax-th point reports the boundary")]
    public async Task Trace_WhenBoundaryReachedAtLastAllowedPoint_ReportsBoundaryReached()
    {
        // Arrange
        string[] lines = await ReadTraceNSReferenceLinesAsync();
        int lmax = lines.Length;

        // Act
        FieldLine fieldLine = TraceNorthToSouth(lmax);

        // Assert
        fieldLine.TerminationReason.ShouldBe("Boundary reached");
        fieldLine.ActualPointCount.ShouldBe(lines.Length);

        for (int i = 0; i < lines.Length; i++)
        {
            fieldLine.Points[i].ShouldMatchReference(lines[i]);
        }
    }

    [Fact(DisplayName = "Trace with lMax one point short of the boundary stops by the point limit")]
    public async Task Trace_WhenBoundaryIsOnePointPastLimit_ReportsMaximumPointsExceeded()
    {
        // Arrange
        string[] lines = await ReadTraceNSReferenceLinesAsync();
        int lmax = lines.Length - 1;

        // Act
        FieldLine fieldLine = TraceNorthToSouth(lmax);

        // Assert
        fieldLine.TerminationReason.ShouldBe("Maximum points exceeded");
        fieldLine.ActualPointCount.ShouldBe(lmax);

        for (int i = 0; i < lmax - 1; i++)
        {
            fieldLine.Points[i].ShouldMatchReference(lines[i]);
        }
    }

    [Theory(DisplayName = "Trace with non-positive lMax throws")]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-2)]
    public void Trace_WhenLMaxIsNotPositive_ShouldThrow(int lmax)
    {
        // Act & Assert
        Should.Throw<ArgumentOutOfRangeException>(() => TraceNorthToSouth(lmax));
    }

    private static async Task<string[]> ReadTraceNSReferenceLinesAsync()
    {
        string rawData = await EmbeddedResourceReader.ReadTextAsync(TraceNSResultFileName);
        return rawData.SplitLines();
    }

    private FieldLine TraceNorthToSouth(int lmax)
    {
        CartesianVector<Velocity> swVelocity = CartesianVector<Velocity>.New(-304.0D, -16.0D + 29.78D, 4.0D, CoordinateSystem.GSE);
        ComputationContext context = s_geopack.Recalc(fixture.InputData.DateTime, swVelocity);
        CartesianLocation startingPoint = CartesianLocation.New(-1.02D, 0.8D, 0.9D, CoordinateSystem.GSW);

        return s_geopack.Trace(context,
            startingPoint,
            TraceDirection.AntiParallel, 0.1D, 0.0001D, 60.0D, 1.0D,
            1, new double[10],
            _t89, s_geopack.IgrfGsw,
            lmax);
    }
}

file static class TraceReferenceAssertions
{
    public static void ShouldMatchReference(this CartesianLocation point, string referenceLine)
    {
        string[] pars = referenceLine.SplitParametersLine();
        point.X.ShouldApproximatelyBe(pars[0].ParseDouble());
        point.Y.ShouldApproximatelyBe(pars[1].ParseDouble());
        point.Z.ShouldApproximatelyBe(pars[2].ParseDouble());
    }
}
