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
            51372.491600877190649044D,
            66.305873729605991684D,
            -11025.145645735878133564D,
            -52158.723565112122741994D,
            -4417.628027113716598251D,
            -4543.483788135986287671D);

        // Act
        _geopack.Recalc(fixture.InputData.DateTime, fixture.InputData.VGSEX, fixture.InputData.VGSEY, fixture.InputData.VGSEZ);
        var field = _geopack.IgrfGeo(r, coLat, lon);

        // Assert
        field.Br.ShouldBe(approvedData.Br, MinimalTestsPrecision);
        field.Btheta.ShouldBe(approvedData.Btheta, MinimalTestsPrecision);
        field.Bphi.ShouldBe(approvedData.Bphi, MinimalTestsPrecision);
        field.Bx.ShouldBe(approvedData.Bx);
        field.By.ShouldBe(approvedData.By);
        field.Bz.ShouldBe(approvedData.Bz);
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
