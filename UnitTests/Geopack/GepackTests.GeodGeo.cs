using AuroraScienceHub.Geopack.UnitTests.Utils;
using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack;

public partial class GeopackTests
{
    [Fact(DisplayName = "GeodGeo: convert to correct values")]
    public async Task GeodGeo_ReturnsCorrectValues()
    {
        // Arrange
        var rawData = await EmbeddedResourceReader.ReadTextAsync(GeodGeoDatasetFileName);
        var lines = rawData.SplitLines();

        _geopack.Recalc(fixture.InputData.DateTime, -304.0D, 13.0D, 4.0D);

        foreach (var line in lines)
        {
            var coordinatesString = line.SplitParametersLine();
            var h = coordinatesString[1].ParseDouble();
            var xmu = coordinatesString[3].ParseDouble();

            var r = coordinatesString[5].ParseDouble();
            var theta = coordinatesString[7].ParseDouble();

            // Act
            var location = _geopack.GeodGeo(h, xmu);

            // Assert
            location.H.ShouldBe(h, MinimalTestsPrecision);
            location.Xmu.ShouldBe(xmu, MinimalTestsPrecision);
            location.R.ShouldBe(r, MinimalTestsPrecision);
            location.Theta.ShouldBe(theta, MinimalTestsPrecision);
        }
    }

    [Fact(DisplayName = "GeoGeod: convert to correct values")]
    public async Task GeoGeod_ReturnsCorrectValues()
    {
        // Arrange
        var rawData = await EmbeddedResourceReader.ReadTextAsync(GeoGeodDatasetFileName);
        var lines = rawData.SplitLines();

        _geopack.Recalc(fixture.InputData.DateTime, -304.0D, 13.0D, 4.0D);

        foreach (var line in lines)
        {
            var coordinatesString = line.SplitParametersLine();
            var r = coordinatesString[1].ParseDouble();
            var theta = coordinatesString[3].ParseDouble();

            var h = coordinatesString[5].ParseDouble();
            var xmu = coordinatesString[7].ParseDouble();

            // Act
            var location = _geopack.GeoGeod(r, theta);

            // Assert
            location.H.ShouldBe(h, MinimalTestsPrecision);
            location.Xmu.ShouldBe(xmu, MinimalTestsPrecision);
            location.R.ShouldBe(r, MinimalTestsPrecision);
            location.Theta.ShouldBe(theta, MinimalTestsPrecision);
        }
    }
}
