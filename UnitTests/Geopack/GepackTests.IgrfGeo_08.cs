using AuroraScienceHub.Geopack.Contracts.Models;
using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack;

public partial class GeopackTests
{
    [Theory(DisplayName = "Iterate IGRF setups and verify result")]
    [InlineData(73.0D, 175.0D, 1.02D, -52864.305642992534558289, -8687.997645029174236697, 651.714296303753599204)]
    [InlineData(90.0, 0.0, 1.02, -53078.710753819832461886, -1566.578021744054694864, -997.428703425218827761)]
    [InlineData(90.0, 45.0, 1.02, -53078.710753819832461886, -1813.026542374652080980, 402.449342492993991982)]
    [InlineData(90.0, 180.0, 1.02, -53078.710753819832461886, 1566.578021748556693638, 997.428703418148188575)]
    [InlineData(90.0, 359.0, 1.02, -53078.710753819832461886, -1548.931893208286282970, -1024.617346539782374748)]
    [InlineData(-90.0, 0.0, 1.02, 50101.895603876902896445, -13178.029910924784417148, -7289.356921431653972832)]
    [InlineData(-90.0, 120.0, 1.02, 50101.895603514029062353, 12901.783226819290575804, -7767.830214052999508567)]
    [InlineData(0.0, 0.0, 6.6, -7.466237418809549276, -100.329164113115780310, -17.725359682701444797)]
    [InlineData(-26.0D, 135.0D, 1.02D, 44172.875491145212436095D, -26655.578798915645165835D, 2603.779522109869958513D)]
    [InlineData(50.0, -90.0, 1.02, -53752.810795780664193444, -12788.775549677107846946, -495.322683045213238984)]
    [InlineData(89.9, 0.0, 1.02, -53063.222394293967226986, -1617.228827444984290196, -998.914807598020502155)]
    [InlineData(-89.9999, 0.0, 1.02, 50101.815124357046443038, -13178.076581014527619118, 4176496199.650153636932373047)]
    public void IgrfGeo_ShouldReturnCorrectValues(
        double xLat, double xLon, double r,
        double expectedBr, double expectedBtheta, double expectedBphi)
    {
        // Arrange
        double coLat = (90.0 - xLat) / Rad;
        double lon = xLon / Rad;

        // Act
        SphericalFieldVector resultField = _geopack.IgrfGeo_08(_ctx, r, coLat, lon);

        // Assert
        resultField.Br.ShouldBe(expectedBr, MinimalTestsPrecision);
        resultField.Btheta.ShouldBe(expectedBtheta, MinimalTestsPrecision);
        resultField.Bphi.ShouldBe(expectedBphi, MinimalTestsPrecision);
        resultField.CoordinateSystem.ShouldBe(CoordinateSystem.GEO);
    }

    [Fact(DisplayName = "IGRF_GEO_08 is NaN if Zero coordinates")]
    public void IgrfGeo_ShouldReturnNaNValues_IfZeroCoordinates()
    {
        // Act
        SphericalFieldVector resultField = _geopack.IgrfGeo_08(_ctx, 0.0D, 0.0D, 0.0D);

        // Assert
        resultField.Br.ShouldBe(double.NaN);
        resultField.Btheta.ShouldBe(double.NaN);
        resultField.Bphi.ShouldBe(double.NaN);
        resultField.CoordinateSystem.ShouldBe(CoordinateSystem.GEO);
    }
}
