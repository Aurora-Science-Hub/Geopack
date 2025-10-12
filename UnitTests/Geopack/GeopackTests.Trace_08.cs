using AuroraScienceHub.Geopack.Contracts;
using AuroraScienceHub.Geopack.Contracts.Interfaces;
using AuroraScienceHub.Geopack.Contracts.Models;
using AuroraScienceHub.Geopack.UnitTests.Utils;
using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack;

public partial class GeopackTests
{
    [Fact(DisplayName = "Trace field line from North to South")]
    public async Task TraceFieldLineFromNorthToSouth()
    {
        // Arrange
        InternalFieldModel internalField = _geopack.IgrfGsw;

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
        _geopack.Recalc(fixture.InputData.DateTime, -304.0D, -16.0D+29.78D, 4.0D);
        double XGSW = -1.02D;
        double YGSW = 0.8D;
        double ZGSW = 0.9D;

        FieldLine fieldLine = _geopack.Trace_08(
            XGSW, YGSW, ZGSW,
            dir, dsmax, err, rlim, r0,
            iopt, parmod,
            _t89, internalField,
            lmax);

        // Assert
        fieldLine.ActualPointCount.ShouldBe(lines.Length);

        for(int i=0; i < lines.Length ; i++)
        {
            string[] pars = lines[i].SplitParametersLine();
            fieldLine.Points[i].X.ShouldBe(pars[0].ParseDouble(), MinimalTestsPrecision);
            fieldLine.Points[i].Y.ShouldBe(pars[1].ParseDouble(), MinimalTestsPrecision);
            fieldLine.Points[i].Z.ShouldBe(pars[2].ParseDouble(), MinimalTestsPrecision);
        }
    }

    [Fact(DisplayName = "Trace field line from South to North")]
    public async Task TraceFieldLineFromSouthToNorth()
    {
        // Arrange
        InternalFieldModel internalField = _geopack.IgrfGsw;

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
        _geopack.Recalc(fixture.InputData.DateTime);
        double XGSW = -1.02D;
        double YGSW = 0.8D;
        double ZGSW = -0.9D;

        FieldLine fieldLine = _geopack.Trace_08(
            XGSW, YGSW, ZGSW,
            dir, dsmax, err, rlim, r0,
            iopt, parmod,
            _t89, internalField,
            lmax);

        // Assert
        fieldLine.ActualPointCount.ShouldBe(lines.Length);

        for (int i = 0; i < lines.Length; i++)
        {
            string[] pars = lines[i].SplitParametersLine();
            fieldLine.Points[i].X.ShouldBe(pars[0].ParseDouble(), MinimalTestsPrecision);
            fieldLine.Points[i].Y.ShouldBe(pars[1].ParseDouble(), MinimalTestsPrecision);
            fieldLine.Points[i].Z.ShouldBe(pars[2].ParseDouble(), MinimalTestsPrecision);
        }
    }
}
