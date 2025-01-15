using AuroraScienceHub.Geopack.UnitTests.Utils;
using FluentAssertions;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack2008;

public class Geopack2008
{
    private const string ResourceName = "AuroraScienceHub.Geopack.UnitTests.Geopack2008.TestData.OriginalDataSet.dat";
    private const float Deg2Rad = MathF.PI / 180.0f;
    private readonly Geopack08.Geopack08 _geopack2008 = new();

    [Fact(DisplayName = "Basic test: Trace_08 with T96 external model should construct correct magnetic field line")]
    public async Task Trace08_WithT96ExternalModel_ShouldBeCorrect()
    {
        // Arrange
        var rawData = await EmbeddedResourceReader.ReadTextAsync(ResourceName);
        var approvedData = GeopackDataParser.Parse(rawData);
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
        _geopack2008.RECALC_08(approvedData.DateTime, approvedData.VGSEX, approvedData.VGSEY, approvedData.VGSEZ);

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
        var expectedX = approvedData.FieldLineCoordinates.Select(coord => coord.X).ToArray();
        var expectedY = approvedData.FieldLineCoordinates.Select(coord => coord.Y).ToArray();
        var expectedZ = approvedData.FieldLineCoordinates.Select(coord => coord.Z).ToArray();

        // Assert
        xx.Should().AllBeEquivalentTo(expectedX, options => options.WithStrictOrdering());
        yy.Should().AllBeEquivalentTo(expectedY, options => options.WithStrictOrdering());
        zz.Should().AllBeEquivalentTo(expectedZ, options => options.WithStrictOrdering());
    }

    //TODO: Add more tests for smaller procedures
}
