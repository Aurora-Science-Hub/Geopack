using AuroraScienceHub.Geopack.Contracts.Models;
using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.ExternalFieldModels;

public partial class TModelsTests
{
    [Theory(DisplayName = "Iterate T89 setups and verify result")]
    [InlineData(1, 0.5D, -6.6D, 0.0D, 0.0D, -21.7428582008425, 0.000000000000000E+000, -16.1753178949799)]
    [InlineData(1, 0.5D, 6.6D, 0.0D, 0.0D, 2.77460435954486, 0.000000000000000E+000, 10.4563908886324)]
    [InlineData(7, 1.0D, -1.02D, -1.02D, -1.02D, -40.3174033286441, -4.28475694981233, -21.8189293247679)]
    [InlineData(6, 1.0D, -1.02D, -1.02D, -1.02D, -77.7524838720541, -1.48818282095091, -46.0916345034569)]
    [InlineData(5, 1.0D, -1.02D, -1.02D, -1.02D, -35.7991349979912, -1.12022295377554, -19.4019675803310)]
    [InlineData(4, 1.0D, -1.02D, -1.02D, -1.02D, -31.0211249343846, -0.446400971617370, -18.2770347314801)]
    [InlineData(3, 1.0D, -1.02D, -1.02D, -1.02D, -33.8409563532569, -0.947163635685137, -22.4120161221729)]
    [InlineData(2, 1.0D, -1.02D, -1.02D, -1.02D, -30.3828134463662, -1.13707597340196, -21.7515334042755)]
    public void T89_ShouldReturnCorrectValues(
        int iopt,
        double ps,
        double x, double y, double z,
        double expectedBx, double expectedBy, double expectedBz)
    {
        // Arrange
        CartesianLocation location = CartesianLocation.New(x, y, z, CoordinateSystem.GSW);

        // Act
        CartesianVector<MagneticField> resultField = _t89.Calculate(iopt, new double[10], ps, location);

        // Assert
        resultField.X.ShouldBe(expectedBx, MinimalTestsPrecision);
        resultField.Y.ShouldBe(expectedBy, MinimalTestsPrecision);
        resultField.Z.ShouldBe(expectedBz, MinimalTestsPrecision);
    }
}
