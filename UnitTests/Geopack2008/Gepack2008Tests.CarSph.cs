using AuroraScienceHub.Geopack.UnitTests.Models;
using AuroraScienceHub.Geopack.UnitTests.Utils;
using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack2008;

public partial class Geopack2008Tests
{
    [Fact(DisplayName = "Cartesian to spherical coordinates conversion: Compare with approved data")]
    public async Task CarSph_ShouldReturnCorrectValues()
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
        var point = _geopack2008.CarSph_08(approvedData.X, approvedData.Y, approvedData.Z);

        // Assert
        point.X.ShouldBe(approvedData.X);
        point.Y.ShouldBe(approvedData.Y);
        point.Z.ShouldBe(approvedData.Z);
        point.R.ShouldBe(approvedData.R, MinimalTestsPrecision);
        point.Theta.ShouldBe(approvedData.Theta, MinimalTestsPrecision);
        point.Phi.ShouldBe(approvedData.Phi, MinimalTestsPrecision);
    }

    [Fact(DisplayName = "Cartesian to spherical coordinates conversion: zero coordinates")]
    public void ConvertCoordinates_ZeroCoordinates_ReturnsZeroValues()
    {
        // Act
        var point = _geopack2008.CarSph_08(0.0, 0.0, 0.0);

        // Assert
        point.R.ShouldBe(0.0D);
        point.Theta.ShouldBe(0.0D);
        point.Phi.ShouldBe(0.0D);
    }

    [Fact(DisplayName = "Cartesian to spherical coordinates conversion: positive Z")]
    public void ConvertCoordinates_PositiveZ_ReturnsExpectedValues()
    {
        // Act
        var point = _geopack2008.CarSph_08(1.0, 1.0, 1.0);

        // Assert
        point.R.ShouldBe(Math.Sqrt(3), Tolerance);
        point.Theta.ShouldBe(Math.Atan2(Math.Sqrt(2), 1.0), Tolerance);
        point.Phi.ShouldBe(Math.Atan2(1.0, 1.0), Tolerance);
    }

    [Fact(DisplayName = "Cartesian to spherical coordinates conversion: negative Z")]
    public void ConvertCoordinates_NegativeZ_ReturnsExpectedValues()
    {
        // Act
        var point = _geopack2008.CarSph_08(1.0, 1.0, -1.0);

        // Assert
        point.R.ShouldBe(Math.Sqrt(3), Tolerance);
        point.Theta.ShouldBe(Math.Atan2(Math.Sqrt(2), -1.0), Tolerance);
        point.Phi.ShouldBe(Math.Atan2(1.0, 1.0), Tolerance);
    }

    [Fact(DisplayName = "Cartesian to spherical coordinates conversion: negative PHI")]
    public void ConvertCoordinates_PhiLessThanZero_AdjustsPhi()
    {
        // Act
        var point = _geopack2008.CarSph_08(-1.0, -1.0, 1.0);

        // Assert
        point.R.ShouldBe(Math.Sqrt(3), Tolerance);
        point.Theta.ShouldBe(Math.Atan2(Math.Sqrt(2), 1.0), Tolerance);
        point.Phi.ShouldBe(Math.Atan2(-1.0, -1.0) + 2 * Math.PI, Tolerance);
    }
}
