using AuroraScienceHub.Geopack.UnitTests.Utils;
using Common.Contracts;
using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack;

public partial class GeopackTests
{
    [Fact(DisplayName = "Spherical to cartesian coordinates conversion")]
    public void SphCar_ShouldReturnCorrectValues()
    {
        // Arrange
        var approvedData = new CartesianLocation(0.4637416876811D, 0.7222348866390D, 0.5511083519855D);

        // Act
        var point = _geopack.SphCar(1.02D, 1.0D, 1.0D);

        // Assert
        point.X.ShouldBe(approvedData.X, MinimalTestsPrecision);
        point.Y.ShouldBe(approvedData.Y, MinimalTestsPrecision);
        point.Z.ShouldBe(approvedData.Z, MinimalTestsPrecision);
    }

    [Theory(DisplayName = "Spherical to cartesian coordinates: zeroes and ones")]
    [InlineData(0.0, 0.0, 0.0, 0.0, 0.0, 0.0)]
    [InlineData(1.0, 0.0, 0.0, 0.0, 0.0, 1.0)]
    [InlineData(0.0, 1.0, 0.0, 0.0, 0.0, 0.0)]
    [InlineData(0.0, 0.0, 1.0, 0.0, 0.0, 0.0)]
    [InlineData(1.0, 1.0, 1.0, 0.45464871341284091, 0.70807341827357118, 0.54030230586813977)]
    [InlineData(-1.0, 0.0, 0.0, 0.0, 0.0, -1.0)]
    [InlineData(0.0, -1.0, 0.0, 0.0, 0.0, 0.0)]
    [InlineData(0.0, 0.0, -1.0, 0.0, 0.0, 0.0)]
    [InlineData(-1.0, -1.0, -1.0, 0.45464871341284091, -0.70807341827357118, -0.54030230586813977)]

    public void SphCar_Variances_ReturnCorrectValues(double r, double theta, double phi, double x, double y, double z)
    {
        // Act
        var point = _geopack.SphCar(r, theta, phi);

        // Assert
        point.X.ShouldBe(x);
        point.Y.ShouldBe(y);
        point.Z.ShouldBe(z);
    }
}
