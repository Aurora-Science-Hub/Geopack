using AuroraScienceHub.Geopack.Contracts.Models;
using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack;

public partial class GeopackTests
{
    [Theory(DisplayName = "Iterate ShuMgnp setups and verify result")]
    [InlineData(5.0D, -350.0D, 5.0D, 9.0D, 0.0D, 0.0D, 9.003326462780140815, 0.000000000000000000, 0.000000000000000000, 0.003326462780140815, MagnetopausePosition.Inside)]
    [InlineData(5.0D, -350.0D, 5.0D, 15.0D, 0.0D, 0.0D, 9.003326462780140815, 0.000000000000000000, 0.000000000000000000, 5.996673537219859185, MagnetopausePosition.Outside)]
    [InlineData(5.0D, 350.0D, 5.0D, 9.0D, 0.0D, 0.0D, 11.193293023217856685, 0.000000000000000000, 0.000000000000000000, 2.193293023217856685, MagnetopausePosition.Inside)]
    [InlineData(99990.0D, 999990.0D, 999.0D, 9.0D, 0.0D, 0.0D, 0.224290174048649371, 0.000000000000000000, 0.000000000000000000, 8.775709825951350851, MagnetopausePosition.Outside)]
    public void ShuMgnp_ShouldReturnCorrectValues(
        double xnPd, double vel, double bzimf,
        double x, double y, double z,
        double xmgnp, double ymgnp, double zmgnp,
        double dist,
        MagnetopausePosition position)
    {
        // Act
        Magnetopause resultField = _geopack.ShuMgnp_08(xnPd, vel, bzimf, x, y, z);

        // Assert
        resultField.X.ShouldBe(xmgnp, MinimalTestsPrecision);
        resultField.Y.ShouldBe(ymgnp, MinimalTestsPrecision);
        resultField.Z.ShouldBe(zmgnp, MinimalTestsPrecision);
        resultField.Dist.ShouldBe(dist, MinimalTestsPrecision);
        resultField.Position.ShouldBe(position);
    }

    [Theory(DisplayName = "Iterate ShuMgnp setups for NaNs")]
    [InlineData(0.0D, 0.0D, 0.0D, 9.0D, 0.0D, 0.0D)]
    [InlineData(0.0D, 0.0D, 999.0D, 9.0D, 0.0D, 0.0D)]
    [InlineData(0.0D, 9990.0D, 999.0D, 9.0D, 0.0D, 0.0D)]
    [InlineData(99990.0D, 0.0D, 999.0D, 9.0D, 0.0D, 0.0D)]
    public void ShuMgnp_ShouldNaN(
        double xnPd, double vel, double bzImf,
        double x, double y, double z)
    {
        // Act
        Magnetopause resultField = _geopack.ShuMgnp_08(xnPd, vel, bzImf, x, y, z);

        // Assert
        resultField.X.ShouldBe(double.NaN);
        resultField.Y.ShouldBe(double.NaN);
        resultField.Z.ShouldBe(double.NaN);
        resultField.Dist.ShouldBe(double.NaN);
        resultField.Position.ShouldBe(MagnetopausePosition.NotDefined);
    }
}
