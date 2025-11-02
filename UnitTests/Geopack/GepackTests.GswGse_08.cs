using AuroraScienceHub.Geopack.Contracts.Models;
using AuroraScienceHub.Geopack.UnitTests.Utils;
using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack;

public partial class GeopackTests
{
    [Fact(DisplayName = "GswGse: convert to correct values")]
    public async Task GswGse_ReturnsCorrectValues()
    {
        // Arrange
        string rawData = await EmbeddedResourceReader.ReadTextAsync(GswGseDatasetFileName);
        string[] lines = rawData.SplitLines();

        foreach (string line in lines)
        {
            string[] coordinatesString = line.SplitParametersLine();
            CartesianLocation gswLocation = CartesianLocation.New(coordinatesString[1].ParseDouble(),
            coordinatesString[3].ParseDouble(),
            coordinatesString[5].ParseDouble(),
            CoordinateSystem.GSW);

            double xgse = coordinatesString[7].ParseDouble();
            double ygse = coordinatesString[9].ParseDouble();
            double zgse = coordinatesString[11].ParseDouble();

            // Act
            CartesianLocation location = s_geopack.GswToGse(_context, gswLocation);

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
        string rawData = await EmbeddedResourceReader.ReadTextAsync(GseGswDatasetFileName);
        string[] lines = rawData.SplitLines();

        foreach (string line in lines)
        {
            string[] coordinatesString = line.SplitParametersLine();
            CartesianLocation gseLocation = CartesianLocation.New(coordinatesString[1].ParseDouble(),
                coordinatesString[3].ParseDouble(),
                coordinatesString[5].ParseDouble(),
                CoordinateSystem.GSW);

            double xgsw = coordinatesString[7].ParseDouble();
            double ygsw = coordinatesString[9].ParseDouble();
            double zgsw = coordinatesString[11].ParseDouble();

            // Act
            CartesianLocation location = s_geopack.GseToGsw(_context, gseLocation);

            // Assert
            location.X.ShouldBe(xgsw, MinimalTestsPrecision);
            location.Y.ShouldBe(ygsw, MinimalTestsPrecision);
            location.Z.ShouldBe(zgsw, MinimalTestsPrecision);
            location.CoordinateSystem.ShouldBe(CoordinateSystem.GSW);
        }
    }
}
