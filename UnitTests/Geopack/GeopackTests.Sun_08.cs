using AuroraScienceHub.Geopack.Contracts.Models;
using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack;

public partial class GeopackTests
{
    [Theory(DisplayName = "Sun: Various dates return expected values")]
    [InlineData(1800, 1, 1, 0, 0, 0, 0, 0, 0, 0)]
    [InlineData(2000, 1, 1, 12, 0, 0, 4.894948822912354558, 4.893575238075353440, 4.909361453634409678, -0.402014132081864151)]
    [InlineData(2004, 2, 29, 0, 0, 0, 2.760256269651100602, 5.929696758033518478, 5.956663000518048534, -0.138172813779450315)]
    [InlineData(1999, 12, 31, 23, 59, 59, 1.744681852526303700, 4.884680534002757035, 4.899722720606541237, -0.402689816598403527)]
    public void SUN_08_VariousDates_ReturnsExpectedValues(
        int year, int month, int day, int hour, int minute, int second,
        double gst, double slong, double srasn, double sdec)
    {
        // Arrange
        DateTime date = new(year, month, day, hour, minute, second);

        // Act
        Sun sun = _geopack.Sun_08(date);

        // Assert
        sun.DateTime.ShouldBe(date);
        sun.Gst.ShouldBe(gst, MinimalTestsPrecision);
        sun.Slong.ShouldBe(slong, MinimalTestsPrecision);
        sun.Srasn.ShouldBe(srasn, MinimalTestsPrecision);
        sun.Sdec.ShouldBe(sdec, MinimalTestsPrecision);
    }
}
