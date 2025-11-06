using AuroraScienceHub.Geopack.Contracts.Cartesian;
using AuroraScienceHub.Geopack.Contracts.Coordinates;
using AuroraScienceHub.Geopack.Contracts.Spherical;
using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack;

public partial class GeopackTests
{
    [Fact(DisplayName = "Spherical to cartesian coordinates conversion")]
    public void SphCar_ShouldReturnCorrectValues()
    {
        // Arrange
        SphericalLocation testLocation = SphericalLocation.New(1.02D, 1.0D, 1.0D, CoordinateSystem.GSW);
        CartesianLocation approvedData = CartesianLocation.New(0.4637416876811D, 0.7222348866390D, 0.5511083519855D, CoordinateSystem.GSW);

        // Act
        CartesianLocation result = testLocation.ToCartesian();

        // Assert
        result.X.ShouldBe(approvedData.X, MinimalTestsPrecision);
        result.Y.ShouldBe(approvedData.Y, MinimalTestsPrecision);
        result.Z.ShouldBe(approvedData.Z, MinimalTestsPrecision);
        result.CoordinateSystem.ShouldBe(approvedData.CoordinateSystem);
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
        // Arrange
        SphericalLocation testLocation = SphericalLocation.New(r, theta, phi, CoordinateSystem.GSW);

        // Act
        CartesianLocation result = testLocation.ToCartesian();

        // Assert
        result.X.ShouldBe(x);
        result.Y.ShouldBe(y);
        result.Z.ShouldBe(z);
    }
}
