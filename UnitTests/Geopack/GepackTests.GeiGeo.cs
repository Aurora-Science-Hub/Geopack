using AuroraScienceHub.Geopack.UnitTests.Utils;
using Common.Contracts;
using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack;

public partial class GeopackTests
{
    [Fact(DisplayName = "GeiGeo: convert to correct values")]
    public async Task GeiGeo_ReturnsCorrectValues()
    {
        // Arrange
        var rawData = await EmbeddedResourceReader.ReadTextAsync(GeiGeoDatasetFileName);
        var lines = rawData.SplitLines();

        _geopack.Recalc(fixture.InputData.DateTime, -304.0D, 13.0D, 4.0D);

        foreach (var line in lines)
        {
            var coordinatesString = line.SplitParametersLine();
            var xgei = coordinatesString[1].ParseDouble();
            var ygei = coordinatesString[3].ParseDouble();
            var zgei = coordinatesString[5].ParseDouble();

            var xgeo = coordinatesString[7].ParseDouble();
            var ygeo = coordinatesString[9].ParseDouble();
            var zgeo = coordinatesString[11].ParseDouble();

            // Act
            var location = _geopack.GeiGeo(xgei, ygei, zgei);

            // Assert
            location.X.ShouldBe(xgeo, MinimalTestsPrecision);
            location.Y.ShouldBe(ygeo, MinimalTestsPrecision);
            location.Z.ShouldBe(zgeo, MinimalTestsPrecision);
            location.CoordinateSystem.ShouldBe(CoordinateSystem.GEO);
        }
    }

    [Fact(DisplayName = "GeoGei: convert to correct values")]
    public async Task GeoGei_ReturnsCorrectValues()
    {
        // Arrange
        var rawData = await EmbeddedResourceReader.ReadTextAsync(GeoGeiDatasetFileName);
        var lines = rawData.SplitLines();

        _geopack.Recalc(fixture.InputData.DateTime, -304.0D, 13.0D, 4.0D);

        foreach (var line in lines)
        {
            var coordinatesString = line.SplitParametersLine();
            var xgeo = coordinatesString[1].ParseDouble();
            var ygeo = coordinatesString[3].ParseDouble();
            var zgeo = coordinatesString[5].ParseDouble();

            var xgei = coordinatesString[7].ParseDouble();
            var ygei = coordinatesString[9].ParseDouble();
            var zgei = coordinatesString[11].ParseDouble();

            // Act
            var location = _geopack.GeoGei(xgeo, ygeo, zgeo);

            // Assert
            location.X.ShouldBe(xgei, MinimalTestsPrecision);
            location.Y.ShouldBe(ygei, MinimalTestsPrecision);
            location.Z.ShouldBe(zgei, MinimalTestsPrecision);
            location.CoordinateSystem.ShouldBe(CoordinateSystem.GEI);
        }
    }
}
