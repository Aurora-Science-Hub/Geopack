using AuroraScienceHub.Geopack.UnitTests.Models;
using AuroraScienceHub.Geopack.UnitTests.Utils;
using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack2008;

public partial class Geopack2008Tests
{
    [Fact(DisplayName = "Cartesian to spherical coordinates conversion: Compare with approved data")]
    public async Task CarSph_ShouldBeCorrect()
    {
        // Arrange
        var rawData = await EmbeddedResourceReader.ReadTextAsync(CarSphDatasetFileName);
        var line = rawData.Split(new[] { ' ', '=', '\t' }, StringSplitOptions.RemoveEmptyEntries);
        var approvedData = new SphCar
        {
            R = line[1].ParseDouble(),
            Theta = line[3].ParseDouble(),
            Phi = line[5].ParseDouble(),
            X = line[7].ParseDouble(),
            Y = line[9].ParseDouble(),
            Z = line[11].ParseDouble()
        };

        // Act
        _geopack2008.CARSPH_08(
            approvedData.X, approvedData.Y, approvedData.Z,
            out var r, out var th, out var phi);

        // Assert
        r.ShouldBe(approvedData.R, MinimalTestsPrecision);
        th.ShouldBe(approvedData.Theta, MinimalTestsPrecision);
        phi.ShouldBe(approvedData.Phi, MinimalTestsPrecision);
    }

    [Fact(DisplayName = "Cartesian to spherical coordinates conversion: zero coordinates")]
    public void ConvertCoordinates_ZeroCoordinates_ReturnsZeroValues()
    {
        _geopack2008.CARSPH_08(0.0, 0.0, 0.0, out double r, out double theta, out double phi);

        r.ShouldBe(0.0D);
        theta.ShouldBe(0.0D);
        phi.ShouldBe(0.0D);
    }

    [Fact(DisplayName = "Cartesian to spherical coordinates conversion: positive Z")]
    public void ConvertCoordinates_PositiveZ_ReturnsExpectedValues()
    {
        _geopack2008.CARSPH_08(1.0, 1.0, 1.0, out double r, out double theta, out double phi);

        r.ShouldBe(Math.Sqrt(3), Tolerance);
        theta.ShouldBe(Math.Atan2(Math.Sqrt(2), 1.0), Tolerance);
        phi.ShouldBe(Math.Atan2(1.0, 1.0), Tolerance);
    }

    [Fact(DisplayName = "Cartesian to spherical coordinates conversion: negative Z")]
    public void ConvertCoordinates_NegativeZ_ReturnsExpectedValues()
    {
        _geopack2008.CARSPH_08(1.0, 1.0, -1.0, out double r, out double theta, out double phi);

        r.ShouldBe(Math.Sqrt(3), Tolerance);
        theta.ShouldBe(Math.Atan2(Math.Sqrt(2), -1.0), Tolerance);
        phi.ShouldBe(Math.Atan2(1.0, 1.0), Tolerance);
    }

    [Fact(DisplayName = "Cartesian to spherical coordinates conversion: negative PHI")]
    public void ConvertCoordinates_PhiLessThanZero_AdjustsPhi()
    {
        _geopack2008.CARSPH_08(-1.0, -1.0, 1.0, out double r, out double theta, out double phi);

        r.ShouldBe(Math.Sqrt(3), Tolerance);
        theta.ShouldBe(Math.Atan2(Math.Sqrt(2), 1.0), Tolerance);
        phi.ShouldBe(Math.Atan2(-1.0, -1.0) + 2 * Math.PI, Tolerance);
    }
}
