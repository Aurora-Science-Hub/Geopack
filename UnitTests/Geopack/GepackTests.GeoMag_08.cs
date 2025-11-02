using AuroraScienceHub.Geopack.Contracts.Models;
using AuroraScienceHub.Geopack.UnitTests.Utils;
using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack;

public partial class GeopackTests
{
    [Fact(DisplayName = "GeoMag: convert to correct values")]
    public async Task GeoMag_ReturnsCorrectValues()
    {
        // Arrange
        string rawData = await EmbeddedResourceReader.ReadTextAsync(GeoMagDatasetFileName);
        string[] lines = rawData.SplitLines();

        foreach (string line in lines)
        {
            string[] coordinatesString = line.SplitParametersLine();
            CartesianLocation geoLocation = CartesianLocation.New(coordinatesString[1].ParseDouble(),
                coordinatesString[3].ParseDouble(),
                coordinatesString[5].ParseDouble(),
                CoordinateSystem.GSW);

            double xmag = coordinatesString[7].ParseDouble();
            double ymag = coordinatesString[9].ParseDouble();
            double zmag = coordinatesString[11].ParseDouble();

            // Act
            CartesianLocation location = s_geopack.GeoToMag(_context, geoLocation);

            // Assert
            location.X.ShouldBe(xmag, MinimalTestsPrecision);
            location.Y.ShouldBe(ymag, MinimalTestsPrecision);
            location.Z.ShouldBe(zmag, MinimalTestsPrecision);
            location.CoordinateSystem.ShouldBe(CoordinateSystem.MAG);
        }
    }

    [Fact(DisplayName = "MagGeo: convert to correct values")]
    public async Task MagGeo_ReturnsCorrectValues()
    {
        // Arrange
        string rawData = await EmbeddedResourceReader.ReadTextAsync(MagGeoDatasetFileName);
        string[] lines = rawData.SplitLines();

        foreach (string line in lines)
        {
            string[] coordinatesString = line.SplitParametersLine();
            CartesianLocation magLocation = CartesianLocation.New(coordinatesString[1].ParseDouble(),
                coordinatesString[3].ParseDouble(),
                coordinatesString[5].ParseDouble(),
                CoordinateSystem.GSW);

            double xgeo = coordinatesString[7].ParseDouble();
            double ygeo = coordinatesString[9].ParseDouble();
            double zgeo = coordinatesString[11].ParseDouble();

            // Act
            CartesianLocation location = s_geopack.MagToGeo(_context, magLocation);

            // Assert
            location.X.ShouldBe(xgeo, MinimalTestsPrecision);
            location.Y.ShouldBe(ygeo, MinimalTestsPrecision);
            location.Z.ShouldBe(zgeo, MinimalTestsPrecision);
            location.CoordinateSystem.ShouldBe(CoordinateSystem.GEO);
        }
    }
}
