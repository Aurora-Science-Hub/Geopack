using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack;

public partial class GeopackTests
{
    [Theory(DisplayName = "Iterate T96Mgnp setups and verify result")]
    [InlineData(5.0D, 350.0D, 9.0D, 0.0D, 0.0D, 11.917821173671217849D, 0.000000000000000000D, 0.000000000000000000D, 2.917821173671217849D, MagnetopausePosition.Inside)]
    [InlineData(5.0D, 350.0D, 12.0D, 1.0D, 0.0D, 11.875615424737137715, 0.989433928116373318, 0.000000000000000061, 0.124832545589572186, MagnetopausePosition.Outside)]
    [InlineData(1.0D, -1350.0D, 9.0D, 0.0D, 0.0D, 12.209108683912852200, 0.000000000000000000, 0.000000000000000000, 3.209108683912852200, MagnetopausePosition.Inside)]
    [InlineData(1.0D, -1350.0D, 15.0D, 0.0D, 0.0D, 12.209108683912852200, 0.000000000000000000, 0.000000000000000000, 2.790891316087147800, MagnetopausePosition.Outside)]
    [InlineData(5.0D, 350.0D, 0.0D, 0.0D, 0.0D, 5.551897632673411742, 0.000000000000000000, 11.912899382845374419, 13.143087119451134726, MagnetopausePosition.Inside)]
    public void T96Mgnp_ShouldReturnCorrectValues(
        double xnPd, double vel,
        double x, double y, double z,
        double xmgnp, double ymgnp, double zmgnp,
        double dist, MagnetopausePosition position)
    {
        // Act
        Magnetopause resultField = _geopack.T96Mgnp(xnPd, vel, x, y, z);

        // Assert
        resultField.X.ShouldBe(xmgnp, MinimalTestsPrecision);
        resultField.Y.ShouldBe(ymgnp, MinimalTestsPrecision);
        resultField.Z.ShouldBe(zmgnp, MinimalTestsPrecision);
        resultField.Dist.ShouldBe(dist, MinimalTestsPrecision);
        resultField.Position.ShouldBe(position);
    }

    [Theory(DisplayName = "Iterate T96Mgnp setups for NaNs")]
    [InlineData(0.0D, 0.0D, 9.0D, 0.0D, 0.0D)]
    [InlineData(0.0D, 99999999.0D, 9.0D, 0.0D, 0.0D)]
    [InlineData(99999999.0D, 0.0D, 9.0D, 0.0D, 0.0D)]
    public void T96Mgnp_ShouldNaN(
        double xnPd, double vel,
        double x, double y, double z)
    {
        // Act
        Magnetopause resultField = _geopack.T96Mgnp(xnPd, vel, x, y, z);

        // Assert
        resultField.X.ShouldBe(double.NaN);
        resultField.Y.ShouldBe(double.NaN);
        resultField.Z.ShouldBe(double.NaN);
        resultField.Dist.ShouldBe(double.NaN);
        resultField.Position.ShouldBe(MagnetopausePosition.NotDefined);
    }
}
