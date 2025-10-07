using AuroraScienceHub.Geopack.Common.Contracts;
using AuroraScienceHub.Geopack.UnitTests.Utils;
using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack;

public partial class GeopackTests
{
    [Fact(DisplayName = "GeoMag: convert to correct values")]
    public async Task GeoMag_ReturnsCorrectValues()
    {
        // Arrange
        string rawData = await EmbeddedResourceReader.ReadTextAsync(GeoMagDatasetFileName);
        string[] lines = rawData.SplitLines();

        _geopack.Recalc(fixture.InputData.DateTime, -304.0D, 13.0D, 4.0D);

        foreach (string line in lines)
        {
            string[] coordinatesString = line.SplitParametersLine();
            double xgeo = coordinatesString[1].ParseDouble();
            double ygeo = coordinatesString[3].ParseDouble();
            double zgeo = coordinatesString[5].ParseDouble();

            double xmag = coordinatesString[7].ParseDouble();
            double ymag = coordinatesString[9].ParseDouble();
            double zmag = coordinatesString[11].ParseDouble();

            // Act
            CartesianLocation location = _geopack.GeoMag(xgeo, ygeo, zgeo);

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
        string rawData = await EmbeddedResourceReader.ReadTextAsync(MagGeoDatasetFileName);
        string[] lines = rawData.SplitLines();

        _geopack.Recalc(fixture.InputData.DateTime, -304.0D, 13.0D, 4.0D);

        foreach (string line in lines)
        {
            string[] coordinatesString = line.SplitParametersLine();
            double xmag = coordinatesString[1].ParseDouble();
            double ymag = coordinatesString[3].ParseDouble();
            double zmag = coordinatesString[5].ParseDouble();

            double xgeo = coordinatesString[7].ParseDouble();
            double ygeo = coordinatesString[9].ParseDouble();
            double zgeo = coordinatesString[11].ParseDouble();

            // Act
            CartesianLocation location = _geopack.MagGeo(xmag, ymag, zmag);

            // Assert
            location.X.ShouldBe(xgeo, MinimalTestsPrecision);
            location.Y.ShouldBe(ygeo, MinimalTestsPrecision);
            location.Z.ShouldBe(zgeo, MinimalTestsPrecision);
            location.CoordinateSystem.ShouldBe(CoordinateSystem.GEO);
        }
    }
}
