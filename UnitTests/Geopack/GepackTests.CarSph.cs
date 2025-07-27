using AuroraScienceHub.Geopack.Contracts;
using AuroraScienceHub.Geopack.UnitTests.Utils;
using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack;

public partial class GeopackTests
{
    [Fact(DisplayName = "Cartesian to spherical coordinates conversion: Compare with approved data")]
    public async Task CarSph_ShouldReturnCorrectValues()
    {
        // Arrange
        var rawData = await EmbeddedResourceReader.ReadTextAsync(CarSphDatasetFileName);
        var line = rawData.Split([' ', '=', '\t'], StringSplitOptions.RemoveEmptyEntries);
        var approvedData = new Point(
            line[7].ParseDouble(),
            line[9].ParseDouble(),
            line[11].ParseDouble(),
            line[1].ParseDouble(),
            line[3].ParseDouble(),
            line[5].ParseDouble());

        // Act
        var point = _geopack.CarSph(approvedData.X, approvedData.Y, approvedData.Z);

        // Assert
        point.X.ShouldBe(approvedData.X);
        point.Y.ShouldBe(approvedData.Y);
        point.Z.ShouldBe(approvedData.Z);
        point.R.ShouldBe(approvedData.R, MinimalTestsPrecision);
        point.Theta.ShouldBe(approvedData.Theta, MinimalTestsPrecision);
        point.Phi.ShouldBe(approvedData.Phi, MinimalTestsPrecision);
    }

    [Theory(DisplayName = "Cartesian to spherical coordinates conversion: zeroes and ones")]
    [InlineData(0.0, 0.0, 0.0, 0.0, 0.0, 0.0)]
    [InlineData(1.0, 0.0, 0.0, 1.0,1.57079632679489656, 0.0)]
    [InlineData(0.0, 1.0, 0.0, 1.0, 1.57079632679489656, 1.57079632679489656)]
    [InlineData(0.0, 0.0, 1.0, 1.0, 0.0, 0.0)]
    [InlineData(1.0, 1.0, 1.0, 1.73205080756887719, 0.95531661812450930, 0.78539816339744828)]
    [InlineData(-1.0, 0.0, 0.0, 1.0, 1.57079632679489656, 3.14159265358979312)]
    [InlineData(0.0, -1.0, 0.0, 1.0, 1.57079632679489656, 4.71238898020510355)]
    [InlineData(0.0, 0.0, -1.0, 1.0, 3.14159265400000010, 0.0)]
    [InlineData(-1.0, -1.0, -1.0, 1.73205080756887719, 2.18627603546528393, 3.92699081680765527)]
    public void CarSph_ZeroesAndOnes_ReturnsCorrectValues(double x, double y, double z, double r, double theta, double phi)
    {
        // Act
        var point = _geopack.CarSph(x, y, z);

        // Assert
        point.R.ShouldBe(r);
        point.Theta.ShouldBe(theta);
        point.Phi.ShouldBe(phi);
    }
}
