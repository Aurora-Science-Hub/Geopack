using AuroraScienceHub.Geopack.Contracts.Cartesian;
using AuroraScienceHub.Geopack.Contracts.Coordinates;
using AuroraScienceHub.Geopack.UnitTests.Extensions;
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
            CartesianLocation magLocation = CartesianLocation.New(coordinatesString[1].ParseDouble(),
                coordinatesString[3].ParseDouble(),
                coordinatesString[5].ParseDouble(),
                CoordinateSystem.MAG);

            double xsm = coordinatesString[7].ParseDouble();
            double ysm = coordinatesString[9].ParseDouble();
            double zsm = coordinatesString[11].ParseDouble();

            // Act
            CartesianLocation location = s_geopack.MagToSm(_context, magLocation);

            // Assert
            location.X.ShouldApproximatelyBe(xsm);
            location.Y.ShouldApproximatelyBe(ysm);
            location.Z.ShouldApproximatelyBe(zsm);
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
            CartesianLocation smLocation = CartesianLocation.New(coordinatesString[1].ParseDouble(),
                coordinatesString[3].ParseDouble(),
                coordinatesString[5].ParseDouble(),
                CoordinateSystem.SM);

            double xmag = coordinatesString[7].ParseDouble();
            double ymag = coordinatesString[9].ParseDouble();
            double zmag = coordinatesString[11].ParseDouble();

            // Act
            CartesianLocation location = s_geopack.SmToMag(_context, smLocation);

            // Assert
            location.X.ShouldApproximatelyBe(xmag);
            location.Y.ShouldApproximatelyBe(ymag);
            location.Z.ShouldApproximatelyBe(zmag);
            location.CoordinateSystem.ShouldBe(CoordinateSystem.MAG);
        }
    }
}
