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
    [InlineData(1, 1, 1, 0, 1, 0, 0.577350269189625842, 0.408248290463863017,  0.707106781186547462)]
    [InlineData(1, 1, 1, 0, 0, 1, 0.577350269189625842, -0.816496580927726145,  0)]
    [InlineData(1, 1, 1, 0, 0, 0, 0, 0,  0)]
    public void BCarSph_ShouldReturnsCorrectValues(
        double x, double y, double z,
        double bx, double by, double bz,
        double br, double btheta, double bphi)
    {
        // Act
        var fieldVector = _geopack.BCarSph(x, y, z, bx, by, bz);

        // Assert
        fieldVector.Br.ShouldBe(br, MinimalTestsPrecision);
        fieldVector.Btheta.ShouldBe(btheta, MinimalTestsPrecision);
        fieldVector.Bphi.ShouldBe(bphi, MinimalTestsPrecision);
    }

    [Fact(DisplayName = "BCarSph: NaN check (identical to original Geopack-2008 behavior)")]
    public void BCarSph_ShouldReturnNaND_IfDivideByZero()
    {
        // Act
        var fieldVector = _geopack.BCarSph(0, 0, 0, 1, 1, 1);

        // Assert
        fieldVector.Br.ShouldBe(double.NaN);
        fieldVector.Btheta.ShouldBe(double.NaN);
        fieldVector.Bphi.ShouldBe(1);
    }
}
