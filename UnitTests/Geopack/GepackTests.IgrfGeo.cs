using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack;

public partial class GeopackTests
{
    [Theory(DisplayName = "Iterate IGRF setups and verify result")]
    [InlineData(73.0D, 175.0D, 1.02D, -52864.306715310063736979, -8687.996256471798915300, 651.711645425813571819)]
    [InlineData(90.0, 0.0, 1.02, -53078.710753819832461886, -1566.578021744054694864,-997.428703425218827761)]
    [InlineData(90.0, 45.0, 1.02, -53078.710753819832461886, -1813.026588011821786495, 402.449136898363747150)]
    [InlineData(90.0, 180.0, 1.02, -53078.710753819832461886, 1566.577569320412749221, 997.429414008917888168)]
    [InlineData(90.0, 359.0, 1.02, -53078.710753819832461886, -1548.930966268464999303, -1024.618747809479373245)]
    public void IgrfGeo_ShouldReturnCorrectValues(
        double xLat, double xLon, double r,
        double expectedBr, double expectedBtheta, double expectedBphi)
    {
        // Arrange
        var coLat = (90.0 - xLat) * DegRad;
        var lon = xLon * DegRad;

        // Act
        _geopack.Recalc(fixture.InputData.DateTime, fixture.InputData.VGSEX, fixture.InputData.VGSEY, fixture.InputData.VGSEZ);
        var resultField = _geopack.IgrfGeo(r, coLat, lon);

        // Assert
        resultField.Br.ShouldBe(expectedBr, MinimalTestsPrecision);
        resultField.Btheta.ShouldBe(expectedBtheta, MinimalTestsPrecision);
        resultField.Bphi.ShouldBe(expectedBphi, MinimalTestsPrecision);
    }
}
