using AuroraScienceHub.Geopack.Contracts;
using AuroraScienceHub.Geopack.UnitTests.Utils;
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
            var pars = line.SplitParametersLine();
            var xgeo = pars[1].ParseDouble();
            var ygeo = pars[3].ParseDouble();
            var zgeo = pars[5].ParseDouble();

            // Act
            var location = _geopack.GeoMag(xgeo, ygeo, zgeo);

            // Assert
            location.X.ShouldBe(pars[7].ParseDouble(), MinimalTestsPrecision);
            location.Y.ShouldBe(pars[9].ParseDouble(), MinimalTestsPrecision);
            location.Z.ShouldBe(pars[11].ParseDouble(), MinimalTestsPrecision);
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
            var pars = line.SplitParametersLine();
            var xmag = pars[1].ParseDouble();
            var ymag = pars[3].ParseDouble();
            var zmag = pars[5].ParseDouble();

            // Act
            var location = _geopack.MagGeo(xmag, ymag, zmag);

            // Assert
            location.X.ShouldBe(pars[7].ParseDouble(), MinimalTestsPrecision);
            location.Y.ShouldBe(pars[9].ParseDouble(), MinimalTestsPrecision);
            location.Z.ShouldBe(pars[11].ParseDouble(), MinimalTestsPrecision);
            location.CoordinateSystem.ShouldBe(CoordinateSystem.GEO);
        }
    }
}
