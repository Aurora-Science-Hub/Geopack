using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack;

public partial class GeopackTests
{
    [Fact(DisplayName = "Sun: Year out of range should return zero values")]
    public void SUN_08_YearOutOfRange_ReturnsZeroValues()
    {
        // Act
        var sun = _geopack.Sun(new DateTime(1800, 1, 1));

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
        var sun = _geopack.Sun(new DateTime(2000, 1, 1, 12, 0, 0));

        // Assert
        sun.DateTime.ShouldBe(new DateTime(2000, 1, 1, 12, 0, 0));
        sun.Gst.ShouldBe(4.894961212735792, MinimalTestsPrecision);
        sun.Slong.ShouldBe(4.894961212735792, MinimalTestsPrecision);
        sun.Srasn.ShouldBe(1.752831, MinimalTestsPrecision);
        sun.Sdec.ShouldBe(-0.402449, MinimalTestsPrecision);
    }

    [Fact(DisplayName = "Sun: Leap year should return expected values")]
    public void SUN_08_LeapYear_ReturnsExpectedValues()
    {
        // Act
        var sun = _geopack.Sun(new DateTime(2004, 2, 29, 0, 0, 0));

        // Assert
        sun.DateTime.ShouldBe(new DateTime(2004, 2, 29, 0, 0, 0));
        sun.Gst.ShouldBe(1.752831, MinimalTestsPrecision);
        sun.Slong.ShouldBe(1.752831, MinimalTestsPrecision);
        sun.Srasn.ShouldBe(1.752831, MinimalTestsPrecision);
        sun.Sdec.ShouldBe(-0.402449, MinimalTestsPrecision);
    }

    [Fact(DisplayName = "Sun: End of year should return expected values")]
    public void SUN_08_EndOfYear_ReturnsExpectedValues()
    {
        // Act
        var sun = _geopack.Sun(new DateTime(1999, 12, 31, 23, 59, 59));

        // Assert
        sun.DateTime.ShouldBe(new DateTime(1999, 12, 31, 23, 59, 59));
        sun.Gst.ShouldBe(6.283185307, MinimalTestsPrecision);
        sun.Slong.ShouldBe(6.283185307, MinimalTestsPrecision);
        sun.Srasn.ShouldBe(1.752831, MinimalTestsPrecision);
        sun.Sdec.ShouldBe(-0.402449, MinimalTestsPrecision);
    }
}
