using AuroraScienceHub.Geopack.Contracts.Cartesian;
using AuroraScienceHub.Geopack.Contracts.Coordinates;
using AuroraScienceHub.Geopack.Contracts.Spherical;
using AuroraScienceHub.Geopack.UnitTests.Extensions;
using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack;

public partial class GeopackTests
{
    [Fact(DisplayName = "Cartesian to spherical coordinates conversion: Compare with approved data")]
    public void CarSph_ShouldReturnCorrectValues()
    {
        // Arrange
        CartesianLocation testData = CartesianLocation.New(1D, 1D, 1D, CoordinateSystem.GSW);
        SphericalLocation approvedData = SphericalLocation.New(1.7320508075689, 0.9553166181245, 0.7853981633974, CoordinateSystem.GSW);

        // Act
        SphericalLocation result = testData.ToSpherical();

        // Assert
        result.R.ShouldApproximatelyBe(approvedData.R);
        result.Theta.ShouldApproximatelyBe(approvedData.Theta);
        result.Phi.ShouldApproximatelyBe(approvedData.Phi);
        result.CoordinateSystem.ShouldBe(approvedData.CoordinateSystem);
    }

    [Theory(DisplayName = "Cartesian to spherical coordinates conversion: zeroes and ones")]
    [InlineData(0.0, 0.0, 0.0, 0.0, 0.0, 0.0)]
    [InlineData(1.0, 0.0, 0.0, 1.0, 1.57079632679489656, 0.0)]
    [InlineData(0.0, 1.0, 0.0, 1.0, 1.57079632679489656, 1.57079632679489656)]
    [InlineData(0.0, 0.0, 1.0, 1.0, 0.0, 0.0)]
    [InlineData(1.0, 1.0, 1.0, 1.73205080756887719, 0.95531661812450930, 0.78539816339744828)]
    [InlineData(-1.0, 0.0, 0.0, 1.0, 1.57079632679489656, 3.14159265358979312)]
    [InlineData(0.0, -1.0, 0.0, 1.0, 1.57079632679489656, 4.71238898020510355)]
    [InlineData(0.0, 0.0, -1.0, 1.0, 3.14159265400000010, 0.0)]
    [InlineData(-1.0, -1.0, -1.0, 1.73205080756887719, 2.18627603546528393, 3.92699081680765527)]
    public void CarSph_ZeroesAndOnes_ReturnsCorrectValues(double x, double y, double z, double r, double theta, double phi)
    {
        // Arrange
        CartesianLocation testLocation = CartesianLocation.New(x, y, z, CoordinateSystem.GSW);

        // Act
        SphericalLocation result = testLocation.ToSpherical();

        // Assert
        result.R.ShouldApproximatelyBe(r);
        result.Theta.ShouldApproximatelyBe(theta);
        result.Phi.ShouldApproximatelyBe(phi);
        result.CoordinateSystem.ShouldBe(testLocation.CoordinateSystem);
    }
}
