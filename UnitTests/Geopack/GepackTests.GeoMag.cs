using AuroraScienceHub.Geopack.UnitTests.Utils;
using Common.Contracts;
using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack;

public partial class GeopackTests
{
    [Fact(DisplayName = "GeoMag: convert to correct values")]
    public async Task GeoMag_ReturnsCorrectValues()
    {
        // Arrange
        var rawData = await EmbeddedResourceReader.ReadTextAsync(GeoMagDatasetFileName);
        var lines = rawData.SplitLines();

        _geopack.Recalc(fixture.InputData.DateTime, -304.0D, 13.0D, 4.0D);

        foreach (var line in lines)
        {
            var coordinatesString = line.SplitParametersLine();
            var xgeo = coordinatesString[1].ParseDouble();
            var ygeo = coordinatesString[3].ParseDouble();
            var zgeo = coordinatesString[5].ParseDouble();

            var xmag = coordinatesString[7].ParseDouble();
            var ymag = coordinatesString[9].ParseDouble();
            var zmag = coordinatesString[11].ParseDouble();

            // Act
            var location = _geopack.GeoMag(xgeo, ygeo, zgeo);

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
        var rawData = await EmbeddedResourceReader.ReadTextAsync(MagGeoDatasetFileName);
        var lines = rawData.SplitLines();

        _geopack.Recalc(fixture.InputData.DateTime, -304.0D, 13.0D, 4.0D);

        foreach (var line in lines)
        {
            var coordinatesString = line.SplitParametersLine();
            var xmag = coordinatesString[1].ParseDouble();
            var ymag = coordinatesString[3].ParseDouble();
            var zmag = coordinatesString[5].ParseDouble();

            var xgeo = coordinatesString[7].ParseDouble();
            var ygeo = coordinatesString[9].ParseDouble();
            var zgeo = coordinatesString[11].ParseDouble();

            // Act
            var location = _geopack.MagGeo(xmag, ymag, zmag);

            // Assert
            location.X.ShouldBe(xgeo, MinimalTestsPrecision);
            location.Y.ShouldBe(ygeo, MinimalTestsPrecision);
            location.Z.ShouldBe(zgeo, MinimalTestsPrecision);
            location.CoordinateSystem.ShouldBe(CoordinateSystem.GEO);
        }
    }
}
