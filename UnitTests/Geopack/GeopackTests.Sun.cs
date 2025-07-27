using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack;

public partial class GeopackTests
{
    [Fact(DisplayName = "SUN_08: Year out of range should return zero values")]
    public void SUN_08_YearOutOfRange_ReturnsZeroValues()
    {
        // Act
        var sun = _geopack2008.Sun(new DateTime(1800, 1, 1));

        // Assert
        sun.DateTime.ShouldBe(new DateTime(1800, 1, 1));
        sun.Gst.ShouldBe(0);
        sun.Slong.ShouldBe(0);
        sun.Srasn.ShouldBe(0);
        sun.Sdec.ShouldBe(0);
    }

    [Fact(DisplayName = "SUN_08: Valid date should return expected values")]
    public void SUN_08_ValidDate_ReturnsExpectedValues()
    {
        // Act
        var sun = _geopack2008.Sun(new DateTime(2000, 1, 1, 12, 0, 0));

        // Assert
        sun.DateTime.ShouldBe(new DateTime(2000, 1, 1, 12, 0, 0));
        sun.Gst.ShouldBe(4.894961212735792, Tolerance);
        sun.Slong.ShouldBe(4.894961212735792, Tolerance);
        sun.Srasn.ShouldBe(1.752831, Tolerance);
        sun.Sdec.ShouldBe(-0.402449, Tolerance);
    }

    [Fact(DisplayName = "SUN_08: Leap year should return expected values")]
    public void SUN_08_LeapYear_ReturnsExpectedValues()
    {
        // Act
        var sun = _geopack2008.Sun(new DateTime(2004, 2, 29, 0, 0, 0));

        // Assert
        sun.DateTime.ShouldBe(new DateTime(2004, 2, 29, 0, 0, 0));
        sun.Gst.ShouldBe(1.752831, Tolerance);
        sun.Slong.ShouldBe(1.752831, Tolerance);
        sun.Srasn.ShouldBe(1.752831, Tolerance);
        sun.Sdec.ShouldBe(-0.402449, Tolerance);
    }

    [Fact(DisplayName = "SUN_08: End of year should return expected values")]
    public void SUN_08_EndOfYear_ReturnsExpectedValues()
    {
        // Act
        var sun = _geopack2008.Sun(new DateTime(1999, 12, 31, 23, 59, 59));

        // Assert
        sun.DateTime.ShouldBe(new DateTime(1999, 12, 31, 23, 59, 59));
        sun.Gst.ShouldBe(6.283185307, Tolerance);
        sun.Slong.ShouldBe(6.283185307, Tolerance);
        sun.Srasn.ShouldBe(1.752831, Tolerance);
        sun.Sdec.ShouldBe(-0.402449, Tolerance);
    }
}
