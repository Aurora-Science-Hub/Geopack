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
            double xgsw = coordinatesString[1].ParseDouble();
            double ygsw = coordinatesString[3].ParseDouble();
            double zgsw = coordinatesString[5].ParseDouble();

            double xgse = coordinatesString[7].ParseDouble();
            double ygse = coordinatesString[9].ParseDouble();
            double zgse = coordinatesString[11].ParseDouble();

            // Act
            CartesianLocation location = s_geopack.GswGse_08(_context, xgsw, ygsw, zgsw);

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
            double xgse = coordinatesString[1].ParseDouble();
            double ygse = coordinatesString[3].ParseDouble();
            double zgse = coordinatesString[5].ParseDouble();

            double xgsw = coordinatesString[7].ParseDouble();
            double ygsw = coordinatesString[9].ParseDouble();
            double zgsw = coordinatesString[11].ParseDouble();

            // Act
            CartesianLocation location = s_geopack.GseGsw_08(_context, xgse, ygse, zgse);

            // Assert
            location.X.ShouldBe(xgsw, MinimalTestsPrecision);
            location.Y.ShouldBe(ygsw, MinimalTestsPrecision);
            location.Z.ShouldBe(zgsw, MinimalTestsPrecision);
            location.CoordinateSystem.ShouldBe(CoordinateSystem.GSW);
        }
    }
}
