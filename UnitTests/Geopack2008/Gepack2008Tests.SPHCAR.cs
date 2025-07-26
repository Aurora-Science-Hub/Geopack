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
        var line = rawData.Split(new[] { ' ', '=', '\t' }, StringSplitOptions.RemoveEmptyEntries);
        var approvedData = new SphCar();
        approvedData.R = line[1].ParseDouble();
        approvedData.Theta = line[3].ParseDouble();
        approvedData.Phi = line[5].ParseDouble();
        approvedData.X = line[7].ParseDouble();
        approvedData.Y = line[9].ParseDouble();
        approvedData.Z = line[11].ParseDouble();

        // Act
        _geopack2008.SPHCAR_08(
            approvedData.R, approvedData.Theta, approvedData.Phi,
            out var x, out var y, out var z);

        // Assert
        x.ShouldBe(approvedData.X, MinimalTestsPrecision);
        y.ShouldBe(approvedData.Y, MinimalTestsPrecision);
        z.ShouldBe(approvedData.Z, MinimalTestsPrecision);
    }
}
