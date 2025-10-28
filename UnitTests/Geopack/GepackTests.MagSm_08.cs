using AuroraScienceHub.Geopack.Contracts.Models;
using AuroraScienceHub.Geopack.UnitTests.Utils;
using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack;

public partial class GeopackTests
{
    [Fact(DisplayName = "MagSm: convert to correct values")]
    public async Task MagSm_ReturnsCorrectValues()
    {
        // Arrange
        string rawData = await EmbeddedResourceReader.ReadTextAsync(MagSmDatasetFileName);
        string[] lines = rawData.SplitLines();

        foreach (string line in lines)
        {
            string[] coordinatesString = line.SplitParametersLine();
            double xmag = coordinatesString[1].ParseDouble();
            double ymag = coordinatesString[3].ParseDouble();
            double zmag = coordinatesString[5].ParseDouble();

            double xsm = coordinatesString[7].ParseDouble();
            double ysm = coordinatesString[9].ParseDouble();
            double zsm = coordinatesString[11].ParseDouble();

            // Act
            CartesianLocation location = _geopack.MagSm_08(_ctx, xmag, ymag, zmag);

            // Assert
            location.X.ShouldBe(xsm, MinimalTestsPrecision);
            location.Y.ShouldBe(ysm, MinimalTestsPrecision);
            location.Z.ShouldBe(zsm, MinimalTestsPrecision);
            location.CoordinateSystem.ShouldBe(CoordinateSystem.SM);
        }
    }

    [Fact(DisplayName = "SmMag: convert to correct values")]
    public async Task SmMag_ReturnsCorrectValues()
    {
        // Arrange
        string rawData = await EmbeddedResourceReader.ReadTextAsync(SmMagDatasetFileName);
        string[] lines = rawData.SplitLines();

        foreach (string line in lines)
        {
            string[] coordinatesString = line.SplitParametersLine();
            double xsm = coordinatesString[1].ParseDouble();
            double ysm = coordinatesString[3].ParseDouble();
            double zsm = coordinatesString[5].ParseDouble();

            double xmag = coordinatesString[7].ParseDouble();
            double ymag = coordinatesString[9].ParseDouble();
            double zmag = coordinatesString[11].ParseDouble();

            // Act
            CartesianLocation location = _geopack.SmMag_08(_ctx, xsm, ysm, zsm);

            // Assert
            location.X.ShouldBe(xmag, MinimalTestsPrecision);
            location.Y.ShouldBe(ymag, MinimalTestsPrecision);
            location.Z.ShouldBe(zmag, MinimalTestsPrecision);
            location.CoordinateSystem.ShouldBe(CoordinateSystem.MAG);
        }
    }
}
