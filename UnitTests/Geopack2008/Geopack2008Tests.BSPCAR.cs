using AuroraScienceHub.Geopack.UnitTests.Models;
using AuroraScienceHub.Geopack.UnitTests.Utils;
using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack2008;

public partial class Geopack2008Tests
{
    [Fact(DisplayName = "BSPCAR_08: Compare with FORTRAN calculation")]
    public async Task BSPCAR_08_ReturnsExpectedValues()
    {
        // Arrange
        var rawData = await EmbeddedResourceReader.ReadTextAsync(BSpCarDatasetFileName);
        var line = rawData.SplitParametersLine();
        var bspcar = new SphCar
        {
            Theta = line[1].ParseDouble(),
            Phi = line[3].ParseDouble(),
            Br = line[5].ParseDouble(),
            Btheta = line[7].ParseDouble(),
            Bphi = line[9].ParseDouble(),
            Bx = line[11].ParseDouble(),
            By = line[13].ParseDouble(),
            Bz = line[15].ParseDouble()
        };

        // Act
        _geopack2008.BSPCAR_08(
            bspcar.Theta, bspcar.Phi, bspcar.Br, bspcar.Btheta, bspcar.Bphi,
            out double bx, out double by, out double bz);

        // Assert
        bx.ShouldBe(bspcar.Bx, MinimalTestsPrecision);
        by.ShouldBe(bspcar.By, MinimalTestsPrecision);
        bz.ShouldBe(bspcar.Bz, MinimalTestsPrecision);
    }

    [Fact(DisplayName = "BSPCAR_08: Zero angles B-field coordinates conversion")]
    public void BSPCAR_08_ZeroAngles_ReturnsExpectedValues()
    {
        // Act
        _geopack2008.BSPCAR_08(
            0.0, 0.0, 1.0, 1.0, 1.0,
            out double bx, out double by, out double bz);

        // Assert
        bx.ShouldBe(1.0, MinimalTestsPrecision);
        by.ShouldBe(1.0, MinimalTestsPrecision);
        bz.ShouldBe(1.0, MinimalTestsPrecision);
    }

    [Fact(DisplayName = "BSPCAR_08: Negative angles B-field coordinates conversion")]
    public void BSPCAR_08_NegativeAngles_ReturnsExpectedValues()
    {
        // Act
        _geopack2008.BSPCAR_08(
            -Math.PI / 4, -Math.PI / 4, 1.0, 1.0, 1.0,
            out double bx, out double by, out double bz);

        // Assert
        bx.ShouldBe(0.70710678118654757, MinimalTestsPrecision);
        by.ShouldBe(0.70710678118654746, MinimalTestsPrecision);
        bz.ShouldBe(1.4142135623730949, MinimalTestsPrecision);
    }

    [Fact(DisplayName = "BSPCAR_08: Large angles B-field coordinates conversion")]
    public void BSPCAR_08_LargeAngles_ReturnsExpectedValues()
    {
        // Act
        _geopack2008.BSPCAR_08(
            2 * Math.PI, 2 * Math.PI, 1.0, 1.0, 1.0,
            out double bx, out double by, out double bz);

        // Assert
        bx.ShouldBe(1.0, MinimalTestsPrecision);
        by.ShouldBe(1.0, MinimalTestsPrecision);
        bz.ShouldBe(1.0, MinimalTestsPrecision);
    }

    [Fact(DisplayName = "BSPCAR_08: Small angles B-field coordinates conversion")]
    public void BSPCAR_08_SmallAngles_ReturnsExpectedValues()
    {
        // Act
        _geopack2008.BSPCAR_08(
            1e-10, 1e-10, 1.0, 1.0, 1.0,
            out double bx, out double by, out double bz);

        // Assert
        bx.ShouldBe(1.0, MinimalTestsPrecision);
        by.ShouldBe(1.0000000001, MinimalTestsPrecision);
        bz.ShouldBe(0.99999999989999999, MinimalTestsPrecision);
    }
}
