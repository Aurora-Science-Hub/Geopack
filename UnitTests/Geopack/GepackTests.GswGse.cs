using AuroraScienceHub.Geopack.Contracts;
using AuroraScienceHub.Geopack.UnitTests.Utils;
using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack;

public partial class GeopackTests
{
    [Fact(DisplayName = "GswGse: convert to correct values")]
    public async Task GswGse_ReturnsCorrectValues()
    {
        // Arrange
        var rawData = await EmbeddedResourceReader.ReadTextAsync(GswGseDatasetFileName);
        var lines = rawData.SplitLines();

        _geopack.Recalc(fixture.InputData.DateTime, -304.0D, 13.0D, 4.0D);

        foreach (var line in lines)
        {
            var coordinatesString = line.SplitParametersLine();
            var xgsw = coordinatesString[1].ParseDouble();
            var ygsw = coordinatesString[3].ParseDouble();
            var zgsw = coordinatesString[5].ParseDouble();

            var xgse = coordinatesString[7].ParseDouble();
            var ygse = coordinatesString[9].ParseDouble();
            var zgse = coordinatesString[11].ParseDouble();

            // Act
            var location = _geopack.GswGse(xgsw, ygsw, zgsw);

            // Assert
            location.X.ShouldBe(xgse, MinimalTestsPrecision);
            location.Y.ShouldBe(ygse, MinimalTestsPrecision);
            location.Z.ShouldBe(zgse, MinimalTestsPrecision);
            location.CoordinateSystem.ShouldBe(CoordinateSystem.GSE);
        }
    }

    [Fact(DisplayName = "GseGsw: convert to correct values")]
    public async Task GseGsw_ReturnsCorrectValues()
    {
        // Arrange
        var rawData = await EmbeddedResourceReader.ReadTextAsync(GseGswDatasetFileName);
        var lines = rawData.SplitLines();

        _geopack.Recalc(fixture.InputData.DateTime, -304.0D, 13.0D, 4.0D);

        foreach (var line in lines)
        {
            var coordinatesString = line.SplitParametersLine();
            var xgse = coordinatesString[1].ParseDouble();
            var ygse = coordinatesString[3].ParseDouble();
            var zgse = coordinatesString[5].ParseDouble();

            var xgsw = coordinatesString[7].ParseDouble();
            var ygsw = coordinatesString[9].ParseDouble();
            var zgsw = coordinatesString[11].ParseDouble();

            // Act
            var location = _geopack.GseGsw(xgse, ygse, zgse);

            // Assert
            location.X.ShouldBe(xgsw, MinimalTestsPrecision);
            location.Y.ShouldBe(ygsw, MinimalTestsPrecision);
            location.Z.ShouldBe(zgsw, MinimalTestsPrecision);
            location.CoordinateSystem.ShouldBe(CoordinateSystem.GSW);
        }
    }
}
