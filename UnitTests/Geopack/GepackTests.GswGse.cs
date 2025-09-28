using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack;

public partial class GeopackTests
{
    [Theory(DisplayName = "GswGse: convert to correct values")]
    [InlineData(-6.5999999999999996, 0, 0.0, -6.593403902518470971, 0.281954772147567645, 0.086755313802676370)]
    [InlineData(1, 1, 1, 1.058072250173702544, 1.071148866358210050, 0.856226149857503227)]
    [InlineData(-1, -1, -1, -1.058072250173702544, -1.071148866358210050, -0.856226149857503227)]
    [InlineData(4.5678, 4.5678, 4.5678, 4.833062424343439467, 4.892793791751032018, 3.911069807319103475)]
    public void GswGse_ReturnsCorrectValues(double xgsw, double ygsw, double zgsw, double xgse, double ygse, double zgse)
    {
        // Act
        _geopack.Recalc(fixture.InputData.DateTime, -304.0D, 13.0D, 4.0D);
        var location = _geopack.GswGse(xgsw, ygsw, zgsw);

        // Assert
        location.X.ShouldBe(xgse, MinimalTestsPrecision);
        location.Y.ShouldBe(ygse, MinimalTestsPrecision);
        location.Z.ShouldBe(zgse, MinimalTestsPrecision);
    }

    [Theory(DisplayName = "GswGse: convert to correct values")]
    [InlineData( -6.5999999999999996, 0, 0, -6.593403902518470971, -0.269181070401027012, -0.120691878226939539)]
    [InlineData(1, 1, 1, 0.943135426752761630, 0.909404539846109805, 1.132907299790544497)]
    [InlineData(-1, -1, -1, -0.943135426752761630, -0.909404539846109805, -1.132907299790544497)]
    [InlineData(4.5678, 4.5678, 4.5678, 4.308054002321264342, 4.153978057109060984, 5.174893963983248746)]
    public void GseGsw_ReturnsCorrectValues(double xgse, double ygse, double zgse, double xgsw, double ygsw, double zgsw)
    {
        // Act
        _geopack.Recalc(fixture.InputData.DateTime, -304.0D, 13.0D, 4.0D);
        var location = _geopack.GseGsw(xgse, ygse, zgse);

        // Assert
        location.X.ShouldBe(xgsw, MinimalTestsPrecision);
        location.Y.ShouldBe(ygsw, MinimalTestsPrecision);
        location.Z.ShouldBe(zgsw, MinimalTestsPrecision);
    }
}
