using AuroraScienceHub.Geopack.Common;
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

        _geopack.Recalc(fixture.InputData.DateTime, -304.0D, 13.0D, 4.0D);

        foreach (string line in lines)
        {
            string[] coordinatesString = line.SplitParametersLine();
            double xsm = coordinatesString[1].ParseDouble();
            double ysm = coordinatesString[3].ParseDouble();
            double zsm = coordinatesString[5].ParseDouble();

            double xgsw = coordinatesString[7].ParseDouble();
            double ygsw = coordinatesString[9].ParseDouble();
            double zgsw = coordinatesString[11].ParseDouble();

            // Act
            CartesianLocation location = _geopack.SmGsw(xsm, ysm, zsm);

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

        _geopack.Recalc(fixture.InputData.DateTime, -304.0D, 13.0D, 4.0D);

        foreach (string line in lines)
        {
            string[] coordinatesString = line.SplitParametersLine();
            double xgsw = coordinatesString[1].ParseDouble();
            double ygsw = coordinatesString[3].ParseDouble();
            double zgsw = coordinatesString[5].ParseDouble();

            double xsm = coordinatesString[7].ParseDouble();
            double ysm = coordinatesString[9].ParseDouble();
            double zsm = coordinatesString[11].ParseDouble();

            // Act
            CartesianLocation location = _geopack.GswSm(xgsw, ygsw, zgsw);

            // Assert
            location.X.ShouldBe(xsm, MinimalTestsPrecision);
            location.Y.ShouldBe(ysm, MinimalTestsPrecision);
            location.Z.ShouldBe(zsm, MinimalTestsPrecision);
            location.CoordinateSystem.ShouldBe(CoordinateSystem.SM);
        }
    }
}
