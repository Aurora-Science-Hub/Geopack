using AuroraScienceHub.Geopack.Contracts.Models;
using AuroraScienceHub.Geopack.UnitTests.Utils;
using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack;

public partial class GeopackTests
{
    [Fact(DisplayName = "SmGsw: convert to correct values")]
    public async Task SmGsw_ReturnsCorrectValues()
    {
        // Arrange
        string rawData = await EmbeddedResourceReader.ReadTextAsync(SmGswDatasetFileName);
        string[] lines = rawData.SplitLines();

        foreach (string line in lines)
        {
            string[] coordinatesString = line.SplitParametersLine();
            CartesianLocation smLocation = CartesianLocation.New(coordinatesString[1].ParseDouble(),
                coordinatesString[3].ParseDouble(),
                coordinatesString[5].ParseDouble(),
                CoordinateSystem.SM);

            double xgsw = coordinatesString[7].ParseDouble();
            double ygsw = coordinatesString[9].ParseDouble();
            double zgsw = coordinatesString[11].ParseDouble();

            // Act
            CartesianLocation location = s_geopack.SmToGsw(_context, smLocation);

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
        string rawData = await EmbeddedResourceReader.ReadTextAsync(GswSmDatasetFileName);
        string[] lines = rawData.SplitLines();

        foreach (string line in lines)
        {
            string[] coordinatesString = line.SplitParametersLine();
            CartesianLocation gswLocation = CartesianLocation.New(coordinatesString[1].ParseDouble(),
                coordinatesString[3].ParseDouble(),
                coordinatesString[5].ParseDouble(),
                CoordinateSystem.GSW);

            double xsm = coordinatesString[7].ParseDouble();
            double ysm = coordinatesString[9].ParseDouble();
            double zsm = coordinatesString[11].ParseDouble();

            // Act
            CartesianLocation location = s_geopack.GswToSm(_context, gswLocation);

            // Assert
            location.X.ShouldBe(xsm, MinimalTestsPrecision);
            location.Y.ShouldBe(ysm, MinimalTestsPrecision);
            location.Z.ShouldBe(zsm, MinimalTestsPrecision);
            location.CoordinateSystem.ShouldBe(CoordinateSystem.SM);
        }
    }
}
