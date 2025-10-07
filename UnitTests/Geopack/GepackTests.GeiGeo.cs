using AuroraScienceHub.Geopack.Common.Contracts;
using AuroraScienceHub.Geopack.UnitTests.Utils;
using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack;

public partial class GeopackTests
{
    [Fact(DisplayName = "GeiGeo: convert to correct values")]
    public async Task GeiGeo_ReturnsCorrectValues()
    {
        // Arrange
        string rawData = await EmbeddedResourceReader.ReadTextAsync(GeiGeoDatasetFileName);
        string[] lines = rawData.SplitLines();

        _geopack.Recalc(fixture.InputData.DateTime, -304.0D, 13.0D, 4.0D);

        foreach (string line in lines)
        {
            string[] coordinatesString = line.SplitParametersLine();
            double xgei = coordinatesString[1].ParseDouble();
            double ygei = coordinatesString[3].ParseDouble();
            double zgei = coordinatesString[5].ParseDouble();

            double xgeo = coordinatesString[7].ParseDouble();
            double ygeo = coordinatesString[9].ParseDouble();
            double zgeo = coordinatesString[11].ParseDouble();

            // Act
            CartesianLocation location = _geopack.GeiGeo(xgei, ygei, zgei);

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
        string rawData = await EmbeddedResourceReader.ReadTextAsync(GeoGeiDatasetFileName);
        string[] lines = rawData.SplitLines();

        _geopack.Recalc(fixture.InputData.DateTime, -304.0D, 13.0D, 4.0D);

        foreach (string line in lines)
        {
            string[] coordinatesString = line.SplitParametersLine();
            double xgeo = coordinatesString[1].ParseDouble();
            double ygeo = coordinatesString[3].ParseDouble();
            double zgeo = coordinatesString[5].ParseDouble();

            double xgei = coordinatesString[7].ParseDouble();
            double ygei = coordinatesString[9].ParseDouble();
            double zgei = coordinatesString[11].ParseDouble();

            // Act
            CartesianLocation location = _geopack.GeoGei(xgeo, ygeo, zgeo);

            // Assert
            location.X.ShouldBe(xgei, MinimalTestsPrecision);
            location.Y.ShouldBe(ygei, MinimalTestsPrecision);
            location.Z.ShouldBe(zgei, MinimalTestsPrecision);
            location.CoordinateSystem.ShouldBe(CoordinateSystem.GEI);
        }
    }
}
