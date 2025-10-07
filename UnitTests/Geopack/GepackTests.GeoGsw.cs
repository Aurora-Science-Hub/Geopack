using AuroraScienceHub.Geopack.UnitTests.Utils;
using Common.Contracts;
using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack;

public partial class GeopackTests
{
    [Fact(DisplayName = "GeoGsw: convert to correct values")]
    public async Task GeoGsw_ReturnsCorrectValues()
    {
        // Arrange
        var rawData = await EmbeddedResourceReader.ReadTextAsync(GeoGswDatasetFileName);
        var lines = rawData.SplitLines();

        _geopack.Recalc(fixture.InputData.DateTime, -304.0D, 13.0D, 4.0D);

        foreach (var line in lines)
        {
            var coordinatesString = line.SplitParametersLine();
            var xgeo = coordinatesString[1].ParseDouble();
            var ygeo = coordinatesString[3].ParseDouble();
            var zgeo = coordinatesString[5].ParseDouble();

            var xgsw = coordinatesString[7].ParseDouble();
            var ygsw = coordinatesString[9].ParseDouble();
            var zgsw = coordinatesString[11].ParseDouble();

            // Act
            var location = _geopack.GeoGsw(xgeo, ygeo, zgeo);

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
        var rawData = await EmbeddedResourceReader.ReadTextAsync(GswGeoDatasetFileName);
        var lines = rawData.SplitLines();

        _geopack.Recalc(fixture.InputData.DateTime, -304.0D, 13.0D, 4.0D);

        foreach (var line in lines)
        {
            var coordinatesString = line.SplitParametersLine();
            var xgsw = coordinatesString[1].ParseDouble();
            var ygsw = coordinatesString[3].ParseDouble();
            var zgsw = coordinatesString[5].ParseDouble();

            var xgeo = coordinatesString[7].ParseDouble();
            var ygeo = coordinatesString[9].ParseDouble();
            var zgeo = coordinatesString[11].ParseDouble();

            // Act
            var location = _geopack.GswGeo(xgsw, ygsw, zgsw);

            // Assert
            location.X.ShouldBe(xgeo, MinimalTestsPrecision);
            location.Y.ShouldBe(ygeo, MinimalTestsPrecision);
            location.Z.ShouldBe(zgeo, MinimalTestsPrecision);
            location.CoordinateSystem.ShouldBe(CoordinateSystem.GEO);
        }
    }
}
