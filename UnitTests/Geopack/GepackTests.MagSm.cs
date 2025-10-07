using AuroraScienceHub.Geopack.UnitTests.Utils;
using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack;

public partial class GeopackTests
{
    [Fact(DisplayName = "MagSm: convert to correct values")]
    public async Task MagSm_ReturnsCorrectValues()
    {
        // Arrange
        var rawData = await EmbeddedResourceReader.ReadTextAsync(MagSmDatasetFileName);
        var lines = rawData.SplitLines();

        _geopack.Recalc(fixture.InputData.DateTime, -304.0D, 13.0D, 4.0D);

        foreach (var line in lines)
        {
            var coordinatesString = line.SplitParametersLine();
            var xmag = coordinatesString[1].ParseDouble();
            var ymag = coordinatesString[3].ParseDouble();
            var zmag = coordinatesString[5].ParseDouble();

            var xsm = coordinatesString[7].ParseDouble();
            var ysm = coordinatesString[9].ParseDouble();
            var zsm = coordinatesString[11].ParseDouble();

            // Act
            var location = _geopack.MagSm(xmag, ymag, zmag);

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
        var rawData = await EmbeddedResourceReader.ReadTextAsync(SmMagDatasetFileName);
        var lines = rawData.SplitLines();

        _geopack.Recalc(fixture.InputData.DateTime, -304.0D, 13.0D, 4.0D);

        foreach (var line in lines)
        {
            var coordinatesString = line.SplitParametersLine();
            var xsm = coordinatesString[1].ParseDouble();
            var ysm = coordinatesString[3].ParseDouble();
            var zsm = coordinatesString[5].ParseDouble();

            var xmag = coordinatesString[7].ParseDouble();
            var ymag = coordinatesString[9].ParseDouble();
            var zmag = coordinatesString[11].ParseDouble();

            // Act
            var location = _geopack.SmMag(xsm, ysm, zsm);

            // Assert
            location.X.ShouldBe(xmag, MinimalTestsPrecision);
            location.Y.ShouldBe(ymag, MinimalTestsPrecision);
            location.Z.ShouldBe(zmag, MinimalTestsPrecision);
            location.CoordinateSystem.ShouldBe(CoordinateSystem.MAG);
        }
    }
}
