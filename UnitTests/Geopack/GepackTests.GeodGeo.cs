using AuroraScienceHub.Geopack.Common;
using AuroraScienceHub.Geopack.UnitTests.Utils;
using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack;

public partial class GeopackTests
{
    [Fact(DisplayName = "GeodGeo: convert to correct values")]
    public async Task GeodGeo_ReturnsCorrectValues()
    {
        // Arrange
        string rawData = await EmbeddedResourceReader.ReadTextAsync(GeodGeoDatasetFileName);
        string[] lines = rawData.SplitLines();

        _geopack.Recalc(fixture.InputData.DateTime, -304.0D, 13.0D, 4.0D);

        foreach (string line in lines)
        {
            string[] coordinatesString = line.SplitParametersLine();
            double h = coordinatesString[1].ParseDouble();
            double xmu = coordinatesString[3].ParseDouble();

            double r = coordinatesString[5].ParseDouble();
            double theta = coordinatesString[7].ParseDouble();

            // Act
            GeodeticGeocentricCoordinates location = _geopack.GeodGeo(h, xmu);

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
        string rawData = await EmbeddedResourceReader.ReadTextAsync(GeoGeodDatasetFileName);
        string[] lines = rawData.SplitLines();

        _geopack.Recalc(fixture.InputData.DateTime, -304.0D, 13.0D, 4.0D);

        foreach (string line in lines)
        {
            string[] coordinatesString = line.SplitParametersLine();
            double r = coordinatesString[1].ParseDouble();
            double theta = coordinatesString[3].ParseDouble();

            double h = coordinatesString[5].ParseDouble();
            double xmu = coordinatesString[7].ParseDouble();

            // Act
            GeodeticGeocentricCoordinates location = _geopack.GeoGeod(r, theta);

            // Assert
            location.H.ShouldBe(h, MinimalTestsPrecision);
            location.Xmu.ShouldBe(xmu, MinimalTestsPrecision);
            location.R.ShouldBe(r, MinimalTestsPrecision);
            location.Theta.ShouldBe(theta, MinimalTestsPrecision);
        }
    }
}
