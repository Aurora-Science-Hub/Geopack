using AuroraScienceHub.Geopack.UnitTests.Models;
using AuroraScienceHub.Geopack.UnitTests.Utils;
using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack2008;

public partial class Geopack2008Tests
{
    [Fact(DisplayName = "Spherical to cartesian coordinates conversion")]
    public async Task SphCar_ShouldReturnCorrectValues()
    {
        // Arrange
        var rawData = await EmbeddedResourceReader.ReadTextAsync(SphCarDatasetFileName);
        var line = rawData.Split([' ', '=', '\t'], StringSplitOptions.RemoveEmptyEntries);
        var approvedData = new SphCar();
        approvedData.R = line[1].ParseDouble();
        approvedData.Theta = line[3].ParseDouble();
        approvedData.Phi = line[5].ParseDouble();
        approvedData.X = line[7].ParseDouble();
        approvedData.Y = line[9].ParseDouble();
        approvedData.Z = line[11].ParseDouble();

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
