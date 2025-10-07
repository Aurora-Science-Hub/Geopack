using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.TModels;

public partial class TModelsTests
{
    [Theory(DisplayName = "Iterate T89 setups and verify result")]
    [InlineData(1, 0.5D, -6.6D, 0.0D, 0.0D, -21.7428582008425, 0.000000000000000E+000, -16.1753178949799)]
    public void T89_ShouldReturnCorrectValues(
        int iopt,
        double ps,
        double x, double y, double z,
        double expectedBx, double expectedBy, double expectedBz)
    {
        // Act
        var resultField = _t89.Calculate(iopt, new double[10], ps, x, y, z);

        // Assert
        resultField.Bx.ShouldBe(expectedBx, MinimalTestsPrecision);
        resultField.By.ShouldBe(expectedBy, MinimalTestsPrecision);
        resultField.Bz.ShouldBe(expectedBz, MinimalTestsPrecision);
    }

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
