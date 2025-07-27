using AuroraScienceHub.Geopack.Contracts;
using AuroraScienceHub.Geopack.UnitTests.Utils;
using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack;

public partial class GeopackTests
{
    [Fact(DisplayName = "Spherical to cartesian coordinates conversion")]
    public async Task SphCar_ShouldReturnCorrectValues()
    {
        // Arrange
        var rawData = await EmbeddedResourceReader.ReadTextAsync(SphCarDatasetFileName);
        var line = rawData.Split([' ', '=', '\t'], StringSplitOptions.RemoveEmptyEntries);
        var approvedData = new Point(
            line[7].ParseDouble(),
            line[9].ParseDouble(),
            line[11].ParseDouble(),
            line[1].ParseDouble(),
            line[3].ParseDouble(),
            line[5].ParseDouble());

        // Act
        var point = _geopack.SphCar(approvedData.R, approvedData.Theta, approvedData.Phi);

        // Assert
        point.R.ShouldBe(approvedData.R);
        point.Theta.ShouldBe(approvedData.Theta);
        point.Phi.ShouldBe(approvedData.Phi);
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
