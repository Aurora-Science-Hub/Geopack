using System.Globalization;
using AuroraScienceHub.Geopack.UnitTests.Geopack2008.TestData;
using AuroraScienceHub.Geopack.UnitTests.Models;
using AuroraScienceHub.Geopack.UnitTests.Utils;
using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack2008;

public partial class Geopack2008Tests
{
    [Fact(DisplayName = "Spherical to cartesian coordinates conversion")]
    public async Task SphCar_ShouldBeCorrect()
    {
        // Arrange
        var rawData = await EmbeddedResourceReader.ReadTextAsync(SphCarDatasetFileName);
        var line = rawData.Split(new[] { ' ', '=', '\t' }, StringSplitOptions.RemoveEmptyEntries);
        var approvedData = new SphCar();
        approvedData.R = double.Parse(line[1], CultureInfo.InvariantCulture);
        approvedData.Theta = double.Parse(line[3], CultureInfo.InvariantCulture);
        approvedData.Phi = double.Parse(line[5], CultureInfo.InvariantCulture);
        approvedData.X = double.Parse(line[7], CultureInfo.InvariantCulture);
        approvedData.Y = double.Parse(line[9], CultureInfo.InvariantCulture);
        approvedData.Z = double.Parse(line[11], CultureInfo.InvariantCulture);

        // Act
        // Calculate transformation matrix coefficients
        _geopack2008.SPHCAR_08(
            approvedData.R, approvedData.Theta, approvedData.Phi,
            out var x, out var y, out var z);

        // Assert
        x.ShouldBe(approvedData.X, MinimalTestsPrecision);
        y.ShouldBe(approvedData.Y, MinimalTestsPrecision);
        z.ShouldBe(approvedData.Z, MinimalTestsPrecision);
    }
}
