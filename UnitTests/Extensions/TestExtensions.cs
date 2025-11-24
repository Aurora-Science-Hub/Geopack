using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.Extensions;

/// <summary>
/// Test extensions
/// </summary>
public static class TestExtensions
{
    private const double MinimalTestsPrecision = 0.000000000008d;

    internal static void ShouldApproximatelyBe(this double actual, double expected)
        => actual.ShouldBe(expected, MinimalTestsPrecision);
}
