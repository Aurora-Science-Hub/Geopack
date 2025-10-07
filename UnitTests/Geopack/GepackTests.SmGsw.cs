using AuroraScienceHub.Geopack.UnitTests.Utils;
using Common.Contracts;
using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack;

public partial class GeopackTests
{
    [Fact(DisplayName = "SmGsw: convert to correct values")]
    public async Task SmGsw_ReturnsCorrectValues()
    {
        // Arrange
        var rawData = await EmbeddedResourceReader.ReadTextAsync(SmGswDatasetFileName);
        var lines = rawData.SplitLines();

        _geopack.Recalc(fixture.InputData.DateTime, -304.0D, 13.0D, 4.0D);

        foreach (var line in lines)
        {
            var coordinatesString = line.SplitParametersLine();
            var xsm = coordinatesString[1].ParseDouble();
            var ysm = coordinatesString[3].ParseDouble();
            var zsm = coordinatesString[5].ParseDouble();

            var xgsw = coordinatesString[7].ParseDouble();
            var ygsw = coordinatesString[9].ParseDouble();
            var zgsw = coordinatesString[11].ParseDouble();

            // Act
            var location = _geopack.SmGsw(xsm, ysm, zsm);

            // Assert
            location.X.ShouldBe(xgsw, MinimalTestsPrecision);
            location.Y.ShouldBe(ygsw, MinimalTestsPrecision);
            location.Z.ShouldBe(zgsw, MinimalTestsPrecision);
            location.CoordinateSystem.ShouldBe(CoordinateSystem.GSW);
        }
    }

    [Fact(DisplayName = "GswSm: convert to correct values")]
    public async Task GswSm_ReturnsCorrectValues()
    {
        // Arrange
        var rawData = await EmbeddedResourceReader.ReadTextAsync(GswSmDatasetFileName);
        var lines = rawData.SplitLines();

        _geopack.Recalc(fixture.InputData.DateTime, -304.0D, 13.0D, 4.0D);

        foreach (var line in lines)
        {
            var coordinatesString = line.SplitParametersLine();
            var xgsw = coordinatesString[1].ParseDouble();
            var ygsw = coordinatesString[3].ParseDouble();
            var zgsw = coordinatesString[5].ParseDouble();

            var xsm = coordinatesString[7].ParseDouble();
            var ysm = coordinatesString[9].ParseDouble();
            var zsm = coordinatesString[11].ParseDouble();

            // Act
            var location = _geopack.GswSm(xgsw, ygsw, zgsw);

            // Assert
            location.X.ShouldBe(xsm, MinimalTestsPrecision);
            location.Y.ShouldBe(ysm, MinimalTestsPrecision);
            location.Z.ShouldBe(zsm, MinimalTestsPrecision);
            location.CoordinateSystem.ShouldBe(CoordinateSystem.SM);
        }
    }
}
