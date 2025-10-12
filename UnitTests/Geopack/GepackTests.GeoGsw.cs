using AuroraScienceHub.Geopack.Contracts.Models;
using AuroraScienceHub.Geopack.UnitTests.Utils;
using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack;

public partial class GeopackTests
{
    [Fact(DisplayName = "GeoGsw: convert to correct values")]
    public async Task GeoGsw_ReturnsCorrectValues()
    {
        // Arrange
        string rawData = await EmbeddedResourceReader.ReadTextAsync(GeoGswDatasetFileName);
        string[] lines = rawData.SplitLines();

        _geopack.Recalc(fixture.InputData.DateTime, -304.0D, 13.0D, 4.0D);

        foreach (string line in lines)
        {
            string[] coordinatesString = line.SplitParametersLine();
            double xgeo = coordinatesString[1].ParseDouble();
            double ygeo = coordinatesString[3].ParseDouble();
            double zgeo = coordinatesString[5].ParseDouble();

            double xgsw = coordinatesString[7].ParseDouble();
            double ygsw = coordinatesString[9].ParseDouble();
            double zgsw = coordinatesString[11].ParseDouble();

            // Act
            CartesianLocation location = _geopack.GeoGsw(xgeo, ygeo, zgeo);

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
        string rawData = await EmbeddedResourceReader.ReadTextAsync(GswGeoDatasetFileName);
        string[] lines = rawData.SplitLines();

        _geopack.Recalc(fixture.InputData.DateTime, -304.0D, 13.0D, 4.0D);

        foreach (string line in lines)
        {
            string[] coordinatesString = line.SplitParametersLine();
            double xgsw = coordinatesString[1].ParseDouble();
            double ygsw = coordinatesString[3].ParseDouble();
            double zgsw = coordinatesString[5].ParseDouble();

            double xgeo = coordinatesString[7].ParseDouble();
            double ygeo = coordinatesString[9].ParseDouble();
            double zgeo = coordinatesString[11].ParseDouble();

            // Act
            CartesianLocation location = _geopack.GswGeo(xgsw, ygsw, zgsw);

            // Assert
            location.X.ShouldBe(xgeo, MinimalTestsPrecision);
            location.Y.ShouldBe(ygeo, MinimalTestsPrecision);
            location.Z.ShouldBe(zgeo, MinimalTestsPrecision);
            location.CoordinateSystem.ShouldBe(CoordinateSystem.GEO);
        }
    }
}
