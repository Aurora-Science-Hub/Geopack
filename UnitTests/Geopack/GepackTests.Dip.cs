using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack;

public partial class GeopackTests
{
    [Theory(DisplayName = "Iterate DIP_08 setups and verify result")]
    [InlineData(6.5999999999999996,0.0, 0.0, 70.248846561769155983, 0.000000000000000000, 98.845731875605991945 )]
    [InlineData(0.0,6.5999999999999996, 0.0, -35.124423280884577991, 0.000000000000000000, 98.845731875605991945)]
    [InlineData(0.0,0.0, 6.5999999999999996, -35.124423280884577991, 0.000000000000000000, -197.691463751211983890)]
    [InlineData(1.0D,1.0D, 1.0D, -5468.999024571849076892, -3525.612769882045540726, 1943.386254689803536166)]
    [InlineData(-6.5999999999999996,0.0D, 0.0D, 70.248846561769155983, 0.000000000000000000, 98.845731875605991945)]
    [InlineData(0.0D,-6.5999999999999996D, 0.0D, -35.124423280884577991, 0.000000000000000000, 98.845731875605991945)]
    [InlineData(0.0D, 0.0D, -6.5999999999999996D, -35.124423280884577991, 0.000000000000000000, -197.691463751211983890)]
    public void Dip_ShouldReturnCorrectValues(
        double xgsw, double ygsw, double zgsw,
        double expectedBx, double expectedBy, double expectedBz)
    {
        // Act
        _geopack.Recalc(fixture.InputData.DateTime, -304.0D, 13.0D, 4.0D);
        var resultField = _geopack.Dip(xgsw, ygsw, zgsw);

        // Assert
        resultField.Bx.ShouldBe(expectedBx, MinimalTestsPrecision);
        resultField.By.ShouldBe(expectedBy, MinimalTestsPrecision);
        resultField.Bz.ShouldBe(expectedBz, MinimalTestsPrecision);
    }

    [Fact(DisplayName = "DIP_08 is NaN if Zero coordinates")]
    public void Dip_ShouldReturnNaNValues_IfZeroCoordinates()
    {
        // Act
        _geopack.Recalc(fixture.InputData.DateTime, -304.0D, 13.0D, 4.0D);
        var resultField = _geopack.Dip(0.0D, 0.0D, 0.0D);

        // Assert
        resultField.Bx.ShouldBe(double.NaN);
        resultField.By.ShouldBe(double.NaN);
        resultField.Bz.ShouldBe(double.NaN);
    }
}
