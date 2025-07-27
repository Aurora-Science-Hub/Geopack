using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack2008;

public partial class GeopackTests
{
    [Theory(DisplayName = "BCarSph: Compare with FORTRAN calculation")]
    [InlineData(1, 1, 1, 1, 1, 1, 0, 0, 0)]
    public void BCarSph_ShouldReturnsCorrectValues(
        double x, double y, double z,
        double bx, double by, double bz,
        double br, double btheta, double bphi)
    {
        // Act
        var fieldVector = _geopack2008.BCarSph(x, y, z, bx, by, bz);

        // Assert
        fieldVector.Bx.ShouldBe(bx);
        fieldVector.By.ShouldBe(by);
        fieldVector.Bz.ShouldBe(bz);
        fieldVector.Br.ShouldBe(br);
        fieldVector.Btheta.ShouldBe(btheta);
        fieldVector.Bphi.ShouldBe(bphi);
    }

    // [Fact(DisplayName = "BSphCar: Large angles B-field coordinates conversion")]
    // public void BSphCar_LargeAngles_ReturnsExpectedValues()
    // {
    //     // Act
    //     var fieldVector = _geopack2008.BSphCar(2 * Math.PI, 2 * Math.PI, 1.0, 1.0, 1.0);
    //
    //     // Assert
    //     fieldVector.Br.ShouldBe(1.0);
    //     fieldVector.Btheta.ShouldBe(1.0);
    //     fieldVector.Bphi.ShouldBe(1.0);
    //     fieldVector.Bx.ShouldBe(1.0, MinimalTestsPrecision);
    //     fieldVector.By.ShouldBe(1.0, MinimalTestsPrecision);
    //     fieldVector.Bz.ShouldBe(1.0, MinimalTestsPrecision);
    // }
    //
    // [Fact(DisplayName = "BSphCar: Small angles B-field coordinates conversion")]
    // public void BSphCar_SmallAngles_ReturnsExpectedValues()
    // {
    //     // Act
    //     var fieldVector = _geopack2008.BSphCar(1e-10, 1e-10, 1.0, 1.0, 1.0);
    //
    //     // Assert
    //     fieldVector.Br.ShouldBe(1.0);
    //     fieldVector.Btheta.ShouldBe(1.0);
    //     fieldVector.Bphi.ShouldBe(1.0);
    //     fieldVector.Bx.ShouldBe(1.0, MinimalTestsPrecision);
    //     fieldVector.By.ShouldBe(1.0000000001, MinimalTestsPrecision);
    //     fieldVector.Bz.ShouldBe(0.99999999989999999, MinimalTestsPrecision);
    // }
}
