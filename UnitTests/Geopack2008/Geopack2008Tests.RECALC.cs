using AuroraScienceHub.Geopack.UnitTests.Geopack2008.Fixtures;
using Shouldly;
using AuroraScienceHub.Geopack.UnitTests.Utils;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack2008;

public partial class Geopack2008Tests
{
    public const string CommonsDataFileName = "AuroraScienceHub.Geopack.UnitTests.Geopack2008.TestData.CommonsDataSet.dat";

    [Fact(DisplayName = "Basic test: Recalc Common1 & Common2 should be correct")]
    public async Task RecalcCommonBlocks_ShouldBeCorrect()
    {
        // Act
        // Calculate transformation matrix coefficients
        var (common1, common2) =_geopack2008.RECALC_08(
            _fixture.InputData.DateTime,
            _fixture.InputData.VGSEX,
            _fixture.InputData.VGSEY,
            _fixture.InputData.VGSEZ);

        // Assert
        var rawData = await EmbeddedResourceReader.ReadTextAsync(CommonsDataFileName);
        var approvedData = GeopackDataParser.ParseRecalcCommons(rawData);

        for (int i = 0; i < common2.G.Length; i++)
        {
            common2.G[i].ShouldBe(approvedData.G![i], MinimalTestsPrecision);
            common2.H[i].ShouldBe(approvedData.H![i], MinimalTestsPrecision);
            common2.REC[i].ShouldBe(approvedData.REC![i], MinimalTestsPrecision);
        }
        common1.ST0.ShouldBe(approvedData.ST0, MinimalTestsPrecision);
        common1.CT0.ShouldBe(approvedData.CT0, MinimalTestsPrecision);
        common1.SL0.ShouldBe(approvedData.SL0, MinimalTestsPrecision);
        common1.CL0.ShouldBe(approvedData.CL0, MinimalTestsPrecision);
        common1.CTCL.ShouldBe(approvedData.CTCL, MinimalTestsPrecision);
        common1.STCL.ShouldBe(approvedData.STCL, MinimalTestsPrecision);
        common1.CTSL.ShouldBe(approvedData.CTSL, MinimalTestsPrecision);
        common1.STSL.ShouldBe(approvedData.STSL, MinimalTestsPrecision);
        common1.SFI.ShouldBe(approvedData.SFI, MinimalTestsPrecision);
        common1.CFI.ShouldBe(approvedData.CFI, MinimalTestsPrecision);
        common1.SPS.ShouldBe(approvedData.SPS, MinimalTestsPrecision);
        common1.CPS.ShouldBe(approvedData.CPS, MinimalTestsPrecision);
        common1.DS3.ShouldBe(approvedData.DS3, MinimalTestsPrecision);
        common1.CGST.ShouldBe(approvedData.CGST, MinimalTestsPrecision);
        common1.SGST.ShouldBe(approvedData.SGST, MinimalTestsPrecision);
        common1.PSI.ShouldBe(approvedData.PSI, MinimalTestsPrecision);
        common1.A11.ShouldBe(approvedData.A11, MinimalTestsPrecision);
        common1.A21.ShouldBe(approvedData.A21, MinimalTestsPrecision);
        common1.A31.ShouldBe(approvedData.A31, MinimalTestsPrecision);
        common1.A12.ShouldBe(approvedData.A12, MinimalTestsPrecision);
        common1.A22.ShouldBe(approvedData.A22, MinimalTestsPrecision);
        common1.A32.ShouldBe(approvedData.A32, MinimalTestsPrecision);
        common1.A13.ShouldBe(approvedData.A13, MinimalTestsPrecision);
        common1.A23.ShouldBe(approvedData.A23, MinimalTestsPrecision);
        common1.A33.ShouldBe(approvedData.A33, MinimalTestsPrecision);
        common1.E11.ShouldBe(approvedData.E11, MinimalTestsPrecision);
        common1.E21.ShouldBe(approvedData.E21, MinimalTestsPrecision);
        common1.E31.ShouldBe(approvedData.E31, MinimalTestsPrecision);
        common1.E12.ShouldBe(approvedData.E12, MinimalTestsPrecision);
        common1.E22.ShouldBe(approvedData.E22, MinimalTestsPrecision);
        common1.E32.ShouldBe(approvedData.E32, MinimalTestsPrecision);
        common1.E13.ShouldBe(approvedData.E13, MinimalTestsPrecision);
        common1.E23.ShouldBe(approvedData.E23, MinimalTestsPrecision);
        common1.E33.ShouldBe(approvedData.E33, MinimalTestsPrecision);
    }

    [Fact(Skip = "Basic test: Trace_08 with T96 external model should construct correct magnetic field line")]
    public Task Trace08_WithT96ExternalModel_ShouldBeCorrect()
    {
        return Task.CompletedTask;
        // // Arrange
        // var rawData = await EmbeddedResourceReader.ReadTextAsync(TraceDatasetFileName);
        // var approvedData = GeopackDataParser.ParseTrace(rawData);
        // approvedData.FillBasicOriginalDataSolarWindVelocity();
        //
        // // T96 test inputs
        // float[] parmod = [approvedData.SolarWindPressure, approvedData.DstIndex, approvedData.ByIMF, approvedData.BzIMF];
        //
        // // The line will be traced from a ground (Re = 1.0) footpoint
        // // with the following geographic coordinates
        // var geoLat = 75.0f;
        // var geoLon = 45.0f;
        // var re = 1.0f;
        //
        // // TRACE inputs
        // var dsmax = 1.0f;
        // var err = 0.0001f;
        // var rlim = 60.0f;
        // var r0 = 1.0f;
        // var iopt = 0;
        //
        // // Act
        // // Calculate transformation matrix coefficients
        // var (common1, common2) =_geopack2008.RECALC_08(approvedData.DateTime, approvedData.VGSEX, approvedData.VGSEY, approvedData.VGSEZ);
        //
        // // Convert Latitude to co-Latitude & Degrees to Radians
        // var coLat = (90.0f - geoLat) * Deg2Rad;
        // var xLon = geoLon * Deg2Rad;
        //
        // // Convert spherical geographic coordinates to Cartesian
        // _geopack2008.SPHCAR_08(
        //     re, coLat, xLon,
        //     out var xgeo, out var ygeo, out var zgeo);
        //
        // // Convert spherical coordinates to Cartesian
        // _geopack2008.GEOGSW_08(
        //     xgeo, ygeo, zgeo,
        //     out var xgsw, out var ygsw, out var zgsw);
        //
        // // Trace the field line
        // _geopack2008.TRACE_NS_08(
        //     xgsw, ygsw, zgsw, dsmax, err, rlim, r0, iopt,
        //     parmod, "T96", "IGRF", 100, out var xx, out var yy, out var zz,
        //     out var xf, out var yf, out var zf, out var l);
        //
        // // Assert
        // // Extract the expected coordinates from FieldLineCoordinates
        // var expectedX = approvedData.FieldLineCoordinates?.Select(coord => coord.X).ToArray();
        // var expectedY = approvedData.FieldLineCoordinates?.Select(coord => coord.Y).ToArray();
        // var expectedZ = approvedData.FieldLineCoordinates?.Select(coord => coord.Z).ToArray();
        //
        // // Assert
    }

    //TODO: Add more tests for smaller procedures
}
