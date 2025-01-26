using AuroraScienceHub.Geopack.UnitTests.Utils;
using FluentAssertions;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack2008;

public class Geopack2008Tests
{
    private const string RecalcDatasetFileName = "AuroraScienceHub.Geopack.UnitTests.Geopack2008.TestData.RecalcDataSet.dat";
    private const string TraceDatasetFileName =  "AuroraScienceHub.Geopack.UnitTests.Geopack2008.TestData.TraceDataSet.dat";
    private const float Deg2Rad = MathF.PI / 180.0f;
    private readonly Geopack08.Geopack08 _geopack2008 = new();

    [Fact(DisplayName = "Basic test: Recalc Common1 & Common2 should be correct")]
    public async Task RecalcCommonBlocks_ShouldBeCorrect()
    {
        // Arrange
        var rawData = await EmbeddedResourceReader.ReadTextAsync(RecalcDatasetFileName);
        var approvedData = GeopackDataParser.ParseRecalcCommons(rawData);

        // Act
        // Calculate transformation matrix coefficients
        var (common1, common2) =_geopack2008.RECALC_08(approvedData.DateTime, approvedData.VGSEX, approvedData.VGSEY, approvedData.VGSEZ);

        // Assert
        common2.G.Should().BeEquivalentTo(approvedData.G, options => options
            .Using<float>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, 0.00013f))
            .WhenTypeIs<float>());
        common2.H.Should().BeEquivalentTo(approvedData.H, options => options
            .Using<float>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, 0.00013f))
            .WhenTypeIs<float>());
        common2.REC.Should().BeEquivalentTo(approvedData.REC, options => options
            .Using<float>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, 0.00005f))
            .WhenTypeIs<float>());
        common1.ST0.Should().BeApproximately(approvedData.ST0, 0.0000001F);
        common1.CT0.Should().BeApproximately(approvedData.CT0, 0.0000001F);
        common1.SL0.Should().BeApproximately(approvedData.SL0, 0.0000001F);
        common1.CL0.Should().BeApproximately(approvedData.CL0, 0.0000001F);
        common1.CTCL.Should().BeApproximately(approvedData.CTCL, 0.0000001F);
        common1.STCL.Should().BeApproximately(approvedData.STCL, 0.0000001F);
        common1.CTSL.Should().BeApproximately(approvedData.CTSL, 0.0000001F);
        common1.STSL.Should().BeApproximately(approvedData.STSL, 0.0000001F);
        common1.SFI.Should().BeApproximately(approvedData.SFI, 0.00004F);
        common1.CFI.Should().BeApproximately(approvedData.CFI, 0.00007F);
        common1.SPS.Should().BeApproximately(approvedData.SPS, 0.000008F);
        common1.CPS.Should().BeApproximately(approvedData.CPS, 0.000003F);
        common1.DS3.Should().BeApproximately(approvedData.DS3, 0.0000001F);
        common1.CGST.Should().BeApproximately(approvedData.CGST, 0.000006F);
        common1.SGST.Should().BeApproximately(approvedData.SGST, 0.000007F);
        common1.PSI.Should().BeApproximately(approvedData.PSI, 0.000009F);
        common1.A11.Should().BeApproximately(approvedData.A11, 0.00005F);
        common1.A21.Should().BeApproximately(approvedData.A21, 0.00005F);
        common1.A31.Should().BeApproximately(approvedData.A31, 0.00003F);
        common1.A12.Should().BeApproximately(approvedData.A12, 0.00005F);
        common1.A22.Should().BeApproximately(approvedData.A22, 0.00005F);
        common1.A32.Should().BeApproximately(approvedData.A32, 0.00005F);
        common1.A13.Should().BeApproximately(approvedData.A13, 0.000004F);
        common1.A23.Should().BeApproximately(approvedData.A23, 0.000006F);
        common1.A33.Should().BeApproximately(approvedData.A33, 0.0000003F);
        common1.E11.Should().BeApproximately(approvedData.E11, 0.000004F);
        common1.E21.Should().BeApproximately(approvedData.E21, 0.00007F);
        common1.E31.Should().BeApproximately(approvedData.E31, 0.0000001F);
        common1.E12.Should().BeApproximately(approvedData.E12, 0.00007F);
        common1.E22.Should().BeApproximately(approvedData.E22, 0.0000007F);
        common1.E32.Should().BeApproximately(approvedData.E32, 0.00002F);
        common1.E13.Should().BeApproximately(approvedData.E13, 0.000008F);
        common1.E23.Should().BeApproximately(approvedData.E23, 0.00002F);
        common1.E33.Should().BeApproximately(approvedData.E33, 0.000003F);
    }

    [Fact(Skip = "Basic test: Trace_08 with T96 external model should construct correct magnetic field line")]
    public async Task Trace08_WithT96ExternalModel_ShouldBeCorrect()
    {
        // Arrange
        var rawData = await EmbeddedResourceReader.ReadTextAsync(TraceDatasetFileName);
        var approvedData = GeopackDataParser.ParseTrace(rawData);
        approvedData.FillBasicOriginalDataSolarWindVelocity();

        // T96 test inputs
        float[] parmod = [approvedData.SolarWindPressure, approvedData.DstIndex, approvedData.ByIMF, approvedData.BzIMF];

        // The line will be traced from a ground (Re = 1.0) footpoint
        // with the following geographic coordinates
        var geoLat = 75.0f;
        var geoLon = 45.0f;
        var re = 1.0f;

        // TRACE inputs
        var dsmax = 1.0f;
        var err = 0.0001f;
        var rlim = 60.0f;
        var r0 = 1.0f;
        var iopt = 0;

        // Act
        // Calculate transformation matrix coefficients
        var (common1, common2) =_geopack2008.RECALC_08(approvedData.DateTime, approvedData.VGSEX, approvedData.VGSEY, approvedData.VGSEZ);

        // Convert Latitude to co-Latitude & Degrees to Radians
        var coLat = (90.0f - geoLat) * Deg2Rad;
        var xLon = geoLon * Deg2Rad;

        // Convert spherical geographic coordinates to Cartesian
        _geopack2008.SPHCAR_08(
            re, coLat, xLon,
            out var xgeo, out var ygeo, out var zgeo);

        // Convert spherical coordinates to Cartesian
        _geopack2008.GEOGSW_08(
            xgeo, ygeo, zgeo,
            out var xgsw, out var ygsw, out var zgsw);

        // Trace the field line
        _geopack2008.TRACE_NS_08(
            xgsw, ygsw, zgsw, dsmax, err, rlim, r0, iopt,
            parmod, "T96", "IGRF", 100, out var xx, out var yy, out var zz,
            out var xf, out var yf, out var zf, out var l);

        // Assert
        // Extract the expected coordinates from FieldLineCoordinates
        var expectedX = approvedData.FieldLineCoordinates?.Select(coord => coord.X).ToArray();
        var expectedY = approvedData.FieldLineCoordinates?.Select(coord => coord.Y).ToArray();
        var expectedZ = approvedData.FieldLineCoordinates?.Select(coord => coord.Z).ToArray();

        // Assert
        xx.Should().AllBeEquivalentTo(expectedX, options => options.WithStrictOrdering());
        yy.Should().AllBeEquivalentTo(expectedY, options => options.WithStrictOrdering());
        zz.Should().AllBeEquivalentTo(expectedZ, options => options.WithStrictOrdering());
    }

    //TODO: Add more tests for smaller procedures
}
