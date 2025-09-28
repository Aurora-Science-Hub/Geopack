using AuroraScienceHub.Geopack.Contracts;
using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack;

public partial class GeopackTests
{
    [Theory(DisplayName = "GeoMag: convert to correct values")]
    [InlineData(-6.5999999999999996, 0.0, 0.0, -2.058161851859945823, -6.259163618566939036, -0.383197843882835232)]
    // [InlineData(1, 1, 1, 1.058072250173702544, 1.071148866358210050, 0.856226149857503227)]
    // [InlineData(-1, -1, -1, -1.058072250173702544, -1.071148866358210050, -0.856226149857503227)]
    // [InlineData(4.5678, 4.5678, 4.5678, 4.833062424343439467, 4.892793791751032018, 3.911069807319103475)]
    [InlineData(0, 0, 0, 0, 0, 0)]
    public void GeoMag_ReturnsCorrectValues(double xgeo, double ygeo, double zgeo, double xmag, double ymag, double zmag)
    {
        // Act
        _geopack.Recalc(fixture.InputData.DateTime, -304.0D, 13.0D, 4.0D);
        var location = _geopack.GeoMag(xgeo, ygeo, zgeo);

        // Assert
        location.X.ShouldBe(xmag, MinimalTestsPrecision);
        location.Y.ShouldBe(ymag, MinimalTestsPrecision);
        location.Z.ShouldBe(zmag, MinimalTestsPrecision);
        location.CoordinateSystem.ShouldBe(CoordinateSystem.MAG);
    }

    [Theory(DisplayName = "MagGeo: convert to correct values")]
    [InlineData( -6.5999999999999996, 0, 0, -2.058161851859945823, 6.153419091567352339, 1.208057645595963070)]
    // [InlineData(1, 1, 1, 0.943135426752761630, 0.909404539846109805, 1.132907299790544497)]
    // [InlineData(-1, -1, -1, -0.943135426752761630, -0.909404539846109805, -1.132907299790544497)]
    // [InlineData(4.5678, 4.5678, 4.5678, 4.308054002321264342, 4.153978057109060984, 5.174893963983248746)]
    [InlineData(0, 0, 0, 0, 0, 0)]
    public void MagGeo_ReturnsCorrectValues(double xmag, double ymag, double zmag, double xgeo, double ygeo, double zgeo)
    {
        // Act
        _geopack.Recalc(fixture.InputData.DateTime, -304.0D, 13.0D, 4.0D);
        var location = _geopack.MagGeo(xmag, ymag, zmag);

        // Assert
        location.X.ShouldBe(xgeo, MinimalTestsPrecision);
        location.Y.ShouldBe(ygeo, MinimalTestsPrecision);
        location.Z.ShouldBe(zgeo, MinimalTestsPrecision);
        location.CoordinateSystem.ShouldBe(CoordinateSystem.GEO);
    }
}
