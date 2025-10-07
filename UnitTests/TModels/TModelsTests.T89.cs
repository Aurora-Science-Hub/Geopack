using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.TModels;

public partial class TModelsTests
{
    // [Theory(DisplayName = "Iterate T89 setups and verify result")]
    // [InlineData(0, 0.1D, -6.6D, 0.0D, 0.0D)]
    // public void T89_ShouldReturnCorrectValues(
    //     int iopt,
    //     double ps,
    //     double x, double y, double z)
    // {
    //     // Act
    //     var resultField = _geopack.T96Mgnp(xnPd, vel, x, y, z);
    //
    //     // Assert
    //     resultField.X.ShouldBe(xmgnp, Geopack.GeopackTests.MinimalTestsPrecision);
    //     resultField.Y.ShouldBe(ymgnp, Geopack.GeopackTests.MinimalTestsPrecision);
    //     resultField.Z.ShouldBe(zmgnp, Geopack.GeopackTests.MinimalTestsPrecision);
    //     resultField.Dist.ShouldBe(dist, Geopack.GeopackTests.MinimalTestsPrecision);
    //     resultField.Position.ShouldBe(position);
    // }
    //
    // [Theory(DisplayName = "Iterate T89 setups for NaNs")]
    // [InlineData(0.0D, 0.0D, 9.0D, 0.0D, 0.0D)]
    // [InlineData(0.0D, 99999999.0D, 9.0D, 0.0D, 0.0D)]
    // [InlineData(99999999.0D, 0.0D, 9.0D, 0.0D, 0.0D)]
    // public void T89Mgnp_ShouldNaN(
    //     double xnPd, double vel,
    //     double x, double y, double z)
    // {
    //     // Act
    //     var resultField = _geopack.T96Mgnp(xnPd, vel, x, y, z);
    //
    //     // Assert
    //     resultField.X.ShouldBe(double.NaN);
    //     resultField.Y.ShouldBe(double.NaN);
    //     resultField.Z.ShouldBe(double.NaN);
    //     resultField.Dist.ShouldBe(double.NaN);
    //     resultField.Position.ShouldBe(MagnetopausePosition.NotDefined);
    // }
}
