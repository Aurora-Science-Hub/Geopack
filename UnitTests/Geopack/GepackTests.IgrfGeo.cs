using AuroraScienceHub.Geopack.Contracts;
using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack;

public partial class GeopackTests
{
    [Fact(DisplayName = "Calculate GEO (X,Y,Z)-components of IGRF model")]
    public void IgrfGeo_ShouldReturnCorrectValues()
    {
        // Arrange
        var xLat = 73.0D;
        var xLon = 175.0D;

        var r = 1.02D;
        var coLat = (90.0 - xLat) * DegRad;
        var lat = xLat + DegRad;
        var lon = xLon * DegRad;

        var approvedData = new MagneticFieldVector(
            52835.667510690909693949,
            -5276.746518637517510797,
            -7147.665100534355588024,
            -52864.306715310063736979,
            -8687.996256471798915300,
            651.711645425813571819);

        // Act
        _geopack.Recalc(fixture.InputData.DateTime, fixture.InputData.VGSEX, fixture.InputData.VGSEY, fixture.InputData.VGSEZ);
        var field = _geopack.IgrfGeo(r, coLat, lon);

        // Assert
        field.Br.ShouldBe(-52864.306715310063736979, MinimalTestsPrecision);
        field.Btheta.ShouldBe(-8687.996256471798915300, MinimalTestsPrecision);
        field.Bphi.ShouldBe(651.711645425813571819, MinimalTestsPrecision);
        field.Bx.ShouldBe(52835.667510690909693949, MinimalTestsPrecision);
        field.By.ShouldBe(-5276.746518637517510797, MinimalTestsPrecision);
        field.Bz.ShouldBe(-7147.665100534355588024, MinimalTestsPrecision);
    }

    // [Theory(DisplayName = "Spherical to cartesian coordinates: zeroes and ones")]
    // [InlineData(0.0, 0.0, 0.0, 0.0, 0.0, 0.0)]
    // [InlineData(1.0, 0.0, 0.0, 0.0, 0.0, 1.0)]
    // [InlineData(0.0, 1.0, 0.0, 0.0, 0.0, 0.0)]
    // [InlineData(0.0, 0.0, 1.0, 0.0, 0.0, 0.0)]
    // [InlineData(1.0, 1.0, 1.0, 0.45464871341284091, 0.70807341827357118, 0.54030230586813977)]
    // [InlineData(-1.0, 0.0, 0.0, 0.0, 0.0, -1.0)]
    // [InlineData(0.0, -1.0, 0.0, 0.0, 0.0, 0.0)]
    // [InlineData(0.0, 0.0, -1.0, 0.0, 0.0, 0.0)]
    // [InlineData(-1.0, -1.0, -1.0, 0.45464871341284091, -0.70807341827357118, -0.54030230586813977)]
    //
    // public void SphCar_Variances_ReturnCorrectValues(double r, double theta, double phi, double x, double y, double z)
    // {
    //     // Act
    //     var point = _geopack.SphCar(r, theta, phi);
    //
    //     // Assert
    //     point.X.ShouldBe(x);
    //     point.Y.ShouldBe(y);
    //     point.Z.ShouldBe(z);
    // }
}
