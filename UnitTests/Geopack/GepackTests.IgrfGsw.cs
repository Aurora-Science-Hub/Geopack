using AuroraScienceHub.Geopack.Contracts;
using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack;

public partial class GeopackTests
{
    [Theory(DisplayName = "Iterate IGRF_GSW_08 setups and verify result")]
    [InlineData(1.0D, 1.0D, 1.0D, -5474.572141126816859469, -3598.502243529560473689, 1833.215273628635031855)]
    [InlineData(-6.6D, 0.0D, 0.0D, 69.106253595272519874, -3.195943937178284955, 99.368806132912794737)]
    [InlineData(6.6D, 0.0D, 0.0D, 71.769903387016739771, 2.701657129752604192, 98.759878146673571564)]
    [InlineData(0.0D, 6.6D, 0.0D, -32.015610653569630983, 0.868906002339342010, 95.911657058327008940)]
    [InlineData(0.0D, -6.6D, 0.0D, -37.971249476612108253, -0.209072020223322497, 102.021488966220218231)]
    [InlineData(0.0D, 0.0D, 6.6D, -35.363226591977408475, -2.562520059996487021, -199.159374390148741440)]
    [InlineData(0.0D, 0.0D, -6.6D, -34.869248473030353352, 3.554189099127370355, -195.320739944871291982)]
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
