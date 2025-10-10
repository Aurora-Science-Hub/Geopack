using AuroraScienceHub.Geopack.Contracts;
using AuroraScienceHub.Geopack.Contracts.Interfaces;
using AuroraScienceHub.Geopack.Contracts.Models;
using AuroraScienceHub.Geopack.UnitTests.Utils;
using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack;

public partial class GeopackTests
{
    [Fact(DisplayName = "Trace field line from geostationary orbit")]
    public async Task TraceFieldLineFromGeostationaryOrbit()
    {
        // Arrange
        InternalFieldModel internalField = _geopack.IgrfGsw;

        string rawData = await EmbeddedResourceReader.ReadTextAsync(TraceResultFileName);
        string[] lines = rawData.SplitLines();

        TraceDirection dir = TraceDirection.AntiParallel;
        double dsmax = 1.0D;
        double err = 0.0001D;
        double rlim = 60.0D;
        double r0 = 1.0D;
        int iopt = 1;
        double[] parmod = new double[10];
        int lmax = 500;

        // Act
        _geopack.Recalc(fixture.InputData.DateTime, -304.0D, -16.0D, 4.0D+29.78D);
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
        fieldLine.ActualPointCount.ShouldBePositive();
    }
}
