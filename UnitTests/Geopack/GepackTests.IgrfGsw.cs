using AuroraScienceHub.Geopack.Contracts;
using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack;

public partial class GeopackTests
{
    [Theory(DisplayName = "Iterate IGRF_GSW_08 setups and verify result")]
    [InlineData(1.0D, 1.0D, 1.0D, -5474.572141126816859469, -3598.502243529560473689, 1833.215273628635031855)]
    public void IgrfGsw_ShouldReturnCorrectValues(
        double x, double y, double z,
        double expectedBx, double expectedBy, double expectedBz)
    {
        // Act
        _geopack.Recalc(fixture.InputData.DateTime, -304.0D, 13.0D, 4.0D);
        var resultField = _geopack.IgrfGsw(x, y, z);

        // Assert
        resultField.Bx.ShouldBe(expectedBx, MinimalTestsPrecision);
        resultField.By.ShouldBe(expectedBy, MinimalTestsPrecision);
        resultField.Bz.ShouldBe(expectedBz, MinimalTestsPrecision);
        resultField.CoordinateSystem.ShouldBe(CoordinateSystem.GSW);
    }

    [Fact(DisplayName = "IGRF_GSW_08 is NaN if Zero coordinates")]
    public void IgrfGsw_ShouldReturnNaNValues_IfZeroCoordinates()
    {
        // Act
        _geopack.Recalc(fixture.InputData.DateTime, -304.0D, 13.0D, 4.0D);
        var resultField = _geopack.IgrfGsw(0.0D, 0.0D, 0.0D);

        // Assert
        resultField.Bx.ShouldBe(double.NaN);
        resultField.By.ShouldBe(double.NaN);
        resultField.Bz.ShouldBe(double.NaN);
        resultField.CoordinateSystem.ShouldBe(CoordinateSystem.GSW);
    }
}
