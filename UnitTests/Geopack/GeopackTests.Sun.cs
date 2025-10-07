using AuroraScienceHub.Geopack.Common.Contracts;
using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack;

public partial class GeopackTests
{
    [Fact(DisplayName = "Sun: Year out of range should return zero values")]
    public void SUN_08_YearOutOfRange_ReturnsZeroValues()
    {
        // Act
        Sun sun = _geopack.Sun(new DateTime(1800, 1, 1));

        // Assert
        sun.DateTime.ShouldBe(new DateTime(1800, 1, 1));
        sun.Gst.ShouldBe(0);
        sun.Slong.ShouldBe(0);
        sun.Srasn.ShouldBe(0);
        sun.Sdec.ShouldBe(0);
    }

    [Fact(DisplayName = "Sun: Valid date should return expected values")]
    public void SUN_08_ValidDate_ReturnsExpectedValues()
    {
        // Act
        Sun sun = _geopack.Sun(new DateTime(2000, 1, 1, 12, 0, 0));

        // Assert
        sun.DateTime.ShouldBe(new DateTime(2000, 1, 1, 12, 0, 0));
        sun.Gst.ShouldBe(4.894948822912354558, MinimalTestsPrecision);
        sun.Slong.ShouldBe(4.893575238075353440, MinimalTestsPrecision);
        sun.Srasn.ShouldBe(4.909361453634409678, MinimalTestsPrecision);
        sun.Sdec.ShouldBe(-0.402014132081864151, MinimalTestsPrecision);
    }

    [Fact(DisplayName = "Sun: Leap year should return expected values")]
    public void SUN_08_LeapYear_ReturnsExpectedValues()
    {
        // Act
        Sun sun = _geopack.Sun(new DateTime(2004, 2, 29, 0, 0, 0));

        // Assert
        sun.DateTime.ShouldBe(new DateTime(2004, 2, 29, 0, 0, 0));
        sun.Gst.ShouldBe(2.760256269651100602, MinimalTestsPrecision);
        sun.Slong.ShouldBe(5.929696758033518478, MinimalTestsPrecision);
        sun.Srasn.ShouldBe(5.956663000518048534, MinimalTestsPrecision);
        sun.Sdec.ShouldBe(-0.138172813779450315, MinimalTestsPrecision);
    }

    [Fact(DisplayName = "Sun: End of year should return expected values")]
    public void SUN_08_EndOfYear_ReturnsExpectedValues()
    {
        // Act
        Sun sun = _geopack.Sun(new DateTime(1999, 12, 31, 23, 59, 59));

        // Assert
        sun.DateTime.ShouldBe(new DateTime(1999, 12, 31, 23, 59, 59));
        sun.Gst.ShouldBe(1.744681852526303700, MinimalTestsPrecision);
        sun.Slong.ShouldBe(4.884680534002757035, MinimalTestsPrecision);
        sun.Srasn.ShouldBe(4.899722720606541237, MinimalTestsPrecision);
        sun.Sdec.ShouldBe(-0.402689816598403527, MinimalTestsPrecision);
    }
}
