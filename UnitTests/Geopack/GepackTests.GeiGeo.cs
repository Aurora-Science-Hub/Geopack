using AuroraScienceHub.Geopack.Contracts.Engine;
using AuroraScienceHub.Geopack.Contracts.Models;
using AuroraScienceHub.Geopack.UnitTests.Utils;
using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack;

public partial class GeopackTests
{
    [Fact(DisplayName = "GeiGeo: convert to correct values")]
    public async Task GeiGeo_ReturnsCorrectValues()
    {
        // Arrange
        CartesianVector<Velocity> swVelocity = CartesianVector<Velocity>.New(-304.0D, 13.0D, 4.0D, CoordinateSystem.GSE);
        ComputationContext ctx = s_geopack.Recalc(fixture.InputData.DateTime, swVelocity);
        string rawData = await EmbeddedResourceReader.ReadTextAsync(GeiGeoDatasetFileName);
        string[] lines = rawData.SplitLines();

        foreach (string line in lines)
        {
            string[] coordinatesString = line.SplitParametersLine();
            CartesianLocation locationGei = CartesianLocation.New(
                coordinatesString[1].ParseDouble(),
                coordinatesString[3].ParseDouble(),
                coordinatesString[5].ParseDouble(),
                CoordinateSystem.GEI);

            CartesianLocation locationGeo = CartesianLocation.New(
                coordinatesString[7].ParseDouble(),
                coordinatesString[9].ParseDouble(),
                coordinatesString[11].ParseDouble(),
                CoordinateSystem.GEO);

            // Act
            CartesianLocation calculatedGeo = s_geopack.GeiToGeo(ctx, locationGei);

            // Assert
            calculatedGeo.X.ShouldBe(locationGeo.X, MinimalTestsPrecision);
            calculatedGeo.Y.ShouldBe(locationGeo.Y, MinimalTestsPrecision);
            calculatedGeo.Z.ShouldBe(locationGeo.Z, MinimalTestsPrecision);
            calculatedGeo.CoordinateSystem.ShouldBe(CoordinateSystem.GEO);
        }
    }

    [Fact(DisplayName = "GeoGei: convert to correct values")]
    public async Task GeoGei_ReturnsCorrectValues()
    {
        // Arrange
        string rawData = await EmbeddedResourceReader.ReadTextAsync(GeoGeiDatasetFileName);
        string[] lines = rawData.SplitLines();

        foreach (string line in lines)
        {
            string[] coordinatesString = line.SplitParametersLine();
            CartesianLocation locationGeo = CartesianLocation.New(
                coordinatesString[1].ParseDouble(),
                coordinatesString[3].ParseDouble(),
                coordinatesString[5].ParseDouble(),
                CoordinateSystem.GSE);

            CartesianLocation locationGei = CartesianLocation.New(
                coordinatesString[7].ParseDouble(),
                coordinatesString[9].ParseDouble(),
                coordinatesString[11].ParseDouble(),
                CoordinateSystem.GSE);

            // Act
            CartesianLocation calculatedLocation = s_geopack.GeoToGei(_context, locationGeo);

            // Assert
            calculatedLocation.X.ShouldBe(locationGei.X, MinimalTestsPrecision);
            calculatedLocation.Y.ShouldBe(locationGei.Y, MinimalTestsPrecision);
            calculatedLocation.Z.ShouldBe(locationGei.Z, MinimalTestsPrecision);
            calculatedLocation.CoordinateSystem.ShouldBe(CoordinateSystem.GEI);
        }
    }
}
