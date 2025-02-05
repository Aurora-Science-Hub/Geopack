using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack2008;

public partial class Geopack2008Tests
{
    [Fact(DisplayName = "SUN_08: Year out of range should return zero values")]
    public void SUN_08_YearOutOfRange_ReturnsZeroValues()
    {
        _geopack2008.SUN_08(
            new DateTime(1800, 1, 1),
            out double gst, out double slong, out double srasn, out double sdec);

        gst.ShouldBe(0);
        slong.ShouldBe(0);
        srasn.ShouldBe(0);
        sdec.ShouldBe(0);
    }

    [Fact(DisplayName = "SUN_08: Valid date should return expected values")]
    public void SUN_08_ValidDate_ReturnsExpectedValues()
    {
        _geopack2008.SUN_08(
            new DateTime(2000, 1, 1, 12, 0, 0),
            out double gst, out double slong, out double srasn, out double sdec);

        gst.ShouldBe(4.894961212735792, Tolerance);
        slong.ShouldBe(4.894961212735792, Tolerance);
        srasn.ShouldBe(1.752831, Tolerance);
        sdec.ShouldBe(-0.402449, Tolerance);
    }

    [Fact(DisplayName = "SUN_08: Leap year should return expected values")]
    public void SUN_08_LeapYear_ReturnsExpectedValues()
    {
        _geopack2008.SUN_08(
            new DateTime(2004, 2, 29, 0, 0, 0),
            out double gst, out double slong, out double srasn, out double sdec);

        gst.ShouldBe(1.752831, Tolerance);
        slong.ShouldBe(1.752831, Tolerance);
        srasn.ShouldBe(1.752831, Tolerance);
        sdec.ShouldBe(-0.402449, Tolerance);
    }

    [Fact(DisplayName = "SUN_08: End of year should return expected values")]
    public void SUN_08_EndOfYear_ReturnsExpectedValues()
    {
        _geopack2008.SUN_08(
            new DateTime(1999, 12, 31, 23, 59, 59),
            out double gst, out double slong, out double srasn, out double sdec);

        gst.ShouldBe(6.283185307, Tolerance);
        slong.ShouldBe(6.283185307, Tolerance);
        srasn.ShouldBe(1.752831, Tolerance);
        sdec.ShouldBe(-0.402449, Tolerance);
    }
}
