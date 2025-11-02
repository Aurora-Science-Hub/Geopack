using AuroraScienceHub.Geopack.Contracts.Models;
using AuroraScienceHub.Geopack.UnitTests.Utils;
using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack;

public partial class GeopackTests
{
    [Fact(DisplayName = "ToPolar: convert to correct values")]
    public async Task ToPolar_ReturnsCorrectValues()
    {
        // Arrange
        string rawData = await EmbeddedResourceReader.ReadTextAsync(GeodGeoDatasetFileName);
        string[] lines = rawData.SplitLines();

        foreach (string line in lines)
        {
            string[] coordinatesString = line.SplitParametersLine();
            GeodeticCoordinates testLocation = new(coordinatesString[3].ParseDouble(), coordinatesString[1].ParseDouble());
            PolarCoordinates approvedLocation = new(coordinatesString[5].ParseDouble(), coordinatesString[7].ParseDouble());

            // Act
            PolarCoordinates result = testLocation.ToPolar();

            // Assert
            result.R.ShouldBe(approvedLocation.R, MinimalTestsPrecision);
            result.Theta.ShouldBe(approvedLocation.Theta, MinimalTestsPrecision);
        }
    }

    [Fact(DisplayName = "ToGeodetic: convert to correct values")]
    public async Task ToGeodetic_ReturnsCorrectValues()
    {
        // Arrange
        string rawData = await EmbeddedResourceReader.ReadTextAsync(GeoGeodDatasetFileName);
        string[] lines = rawData.SplitLines();

        foreach (string line in lines)
        {
            string[] coordinatesString = line.SplitParametersLine();
            PolarCoordinates testLocation = new(coordinatesString[1].ParseDouble(), coordinatesString[3].ParseDouble());
            GeodeticCoordinates approvedLocation = new(coordinatesString[7].ParseDouble(), coordinatesString[5].ParseDouble());

            // Act
            GeodeticCoordinates result = testLocation.ToGeodetic();

            // Assert
            result.Altitude.ShouldBe(approvedLocation.Altitude, MinimalTestsPrecision);
            result.GeodLatitude.ShouldBe(approvedLocation.GeodLatitude, MinimalTestsPrecision);
        }
    }
}
