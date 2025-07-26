using AuroraScienceHub.Geopack.Contracts;
using AuroraScienceHub.Geopack.UnitTests.Utils;
using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack2008;

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
        var point = _geopack2008.SphCar(approvedData.R, approvedData.Theta, approvedData.Phi);

        // Assert
        point.R.ShouldBe(approvedData.R);
        point.Theta.ShouldBe(approvedData.Theta);
        point.Phi.ShouldBe(approvedData.Phi);
        point.X.ShouldBe(approvedData.X, MinimalTestsPrecision);
        point.Y.ShouldBe(approvedData.Y, MinimalTestsPrecision);
        point.Z.ShouldBe(approvedData.Z, MinimalTestsPrecision);
    }
}
