using AuroraScienceHub.Geopack.Contracts.Coordinates;
using AuroraScienceHub.Geopack.Contracts.Magnetosphere;
using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack;

public partial class GeopackTests
{
    [Theory(DisplayName = "Iterate BCARSP_08 setups and verify result")]
    [InlineData(-1.2, 1, 1.5, 1, 11, -21, -10.020128994909539344, 19.492502934797215630, -9.090618475235615392)]
    [InlineData(1, 0, 0, 1, 1, 1, 1, -1, 1)]
    [InlineData(0, 1, 0, 1, 1, 1, 1, -1, -1)]
    [InlineData(0, 0, 1, 1, 1, 1, 1, 1, 1)]
    [InlineData(1, 1, 1, 1, 1, 1, 1.7320508075688776, 0, 0)]
    [InlineData(1, 1, 1, 1, 0, 0, 0.577350269189625842, 0.408248290463863017, -0.707106781186547462)]
    [InlineData(1, 1, 1, 0, 1, 0, 0.577350269189625842, 0.408248290463863017, 0.707106781186547462)]
    [InlineData(1, 1, 1, 0, 0, 1, 0.577350269189625842, -0.816496580927726145, 0)]
    [InlineData(1, 1, 1, 0, 0, 0, 0, 0, 0)]
    public void ToSphericalVector_ShouldReturnsCorrectValues(double x, double y, double z,
        double bx, double by, double bz,
        double br, double btheta, double bphi)
    {
        // Arrange
        CartesianLocation location = CartesianLocation.New(x, y, z, CoordinateSystem.GSW);
        CartesianVector<MagneticField> cartesianVector = CartesianVector<MagneticField>.New(bx, by, bz, CoordinateSystem.GSW);

        // Act
        SphericalVector<MagneticField> sphericalVector = cartesianVector.ToSphericalVector(location);

        // Assert
        sphericalVector.R.ShouldBe(br, MinimalTestsPrecision);
        sphericalVector.Theta.ShouldBe(btheta, MinimalTestsPrecision);
        sphericalVector.Phi.ShouldBe(bphi, MinimalTestsPrecision);
    }

    [Fact(DisplayName = "BCarSph: NaN check (identical to original Geopack-2008 behavior)")]
    public void ToSphericalVector_ShouldReturnNaND_IfDivideByZero()
    {
        // Arrange
        CartesianLocation location = CartesianLocation.New(0, 0, 0, CoordinateSystem.GSW);
        CartesianVector<MagneticField> cartesianVector = CartesianVector<MagneticField>.New(1, 1, 1, CoordinateSystem.GSW);

        // Act
        Action act = () => cartesianVector.ToSphericalVector(location);

        // Assert
        act.ShouldThrow<DivideByZeroException>();
    }
}
