using AuroraScienceHub.Geopack.Contracts.Models;
using AuroraScienceHub.Geopack.UnitTests.Utils;
using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack;

public partial class GeopackTests
{
    [Fact(DisplayName = "GeoGsw: convert to correct values")]
    public async Task GeoGsw_ReturnsCorrectValues()
    {
        // Arrange
        string rawData = await EmbeddedResourceReader.ReadTextAsync(GeoGswDatasetFileName);
        string[] lines = rawData.SplitLines();

        foreach (string line in lines)
        {
            string[] coordinatesString = line.SplitParametersLine();
            CartesianLocation geoLocation = CartesianLocation.New(coordinatesString[1].ParseDouble(),
                coordinatesString[3].ParseDouble(),
                coordinatesString[5].ParseDouble(),
                CoordinateSystem.GSW);

            double xgsw = coordinatesString[7].ParseDouble();
            double ygsw = coordinatesString[9].ParseDouble();
            double zgsw = coordinatesString[11].ParseDouble();

            // Act
            CartesianLocation location = s_geopack.GeoToGsw(_context, geoLocation);

            // Assert
            location.X.ShouldBe(xgsw, MinimalTestsPrecision);
            location.Y.ShouldBe(ygsw, MinimalTestsPrecision);
            location.Z.ShouldBe(zgsw, MinimalTestsPrecision);
            location.CoordinateSystem.ShouldBe(CoordinateSystem.GSW);
        }
    }

    [Fact(DisplayName = "GswGeo: convert to correct values")]
    public async Task GswGeo_ReturnsCorrectValues()
    {
        // Arrange
        string rawData = await EmbeddedResourceReader.ReadTextAsync(GswGeoDatasetFileName);
        string[] lines = rawData.SplitLines();

        foreach (string line in lines)
        {
            string[] coordinatesString = line.SplitParametersLine();
            CartesianLocation gswLocation = CartesianLocation.New(coordinatesString[1].ParseDouble(),
                coordinatesString[3].ParseDouble(),
                coordinatesString[5].ParseDouble(),
                CoordinateSystem.GSW);

            double xgeo = coordinatesString[7].ParseDouble();
            double ygeo = coordinatesString[9].ParseDouble();
            double zgeo = coordinatesString[11].ParseDouble();

            // Act
            CartesianLocation location = s_geopack.GswToGeo(_context, gswLocation);

            // Assert
            location.X.ShouldBe(xgeo, MinimalTestsPrecision);
            location.Y.ShouldBe(ygeo, MinimalTestsPrecision);
            location.Z.ShouldBe(zgeo, MinimalTestsPrecision);
            location.CoordinateSystem.ShouldBe(CoordinateSystem.GEO);
        }
    }
}
