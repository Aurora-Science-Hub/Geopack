using AuroraScienceHub.Geopack.Contracts;
using AuroraScienceHub.Geopack.UnitTests.Utils;
using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack2008;

public partial class Geopack2008Tests
{
    [Fact(DisplayName = "BSphCar: Compare with FORTRAN calculation")]
    public async Task BSphCar_ReturnsCorrectValues()
    {
        // Arrange
        var rawData = await EmbeddedResourceReader.ReadTextAsync(BSpCarDatasetFileName);
        var line = rawData.SplitParametersLine();

        var theta = line[1].ParseDouble();
        var phi = line[3].ParseDouble();
        var approvedFieldVector = new MagneticFieldVector(
            line[11].ParseDouble(),
            line[13].ParseDouble(),
            line[15].ParseDouble(),
            line[5].ParseDouble(),
            line[7].ParseDouble(),
            line[9].ParseDouble());

        // Act
        var fieldVector = _geopack2008.BSphCar(
            theta, phi,
            approvedFieldVector.Br, approvedFieldVector.Btheta, approvedFieldVector.Bphi);

        // Assert
        fieldVector.Bx.ShouldBe(approvedFieldVector.Bx, MinimalTestsPrecision);
        fieldVector.By.ShouldBe(approvedFieldVector.By, MinimalTestsPrecision);
        fieldVector.Bz.ShouldBe(approvedFieldVector.Bz, MinimalTestsPrecision);
        fieldVector.Br.ShouldBe(approvedFieldVector.Br, MinimalTestsPrecision);
        fieldVector.Btheta.ShouldBe(approvedFieldVector.Btheta, MinimalTestsPrecision);
        fieldVector.Bphi.ShouldBe(approvedFieldVector.Bphi, MinimalTestsPrecision);
    }

    [Fact(DisplayName = "BSphCar: Zero angles B-field coordinates conversion")]
    public void BSphCar_ZeroAngles_ReturnsExpectedValues()
    {
        // Act
        var fieldVector = _geopack2008.BSphCar(0.0, 0.0, 1.0, 1.0, 1.0);

        // Assert
        fieldVector.Br.ShouldBe(1.0);
        fieldVector.Btheta.ShouldBe(1.0);
        fieldVector.Bphi.ShouldBe(1.0);
        fieldVector.Bx.ShouldBe(1.0, MinimalTestsPrecision);
        fieldVector.By.ShouldBe(1.0, MinimalTestsPrecision);
        fieldVector.Bz.ShouldBe(1.0, MinimalTestsPrecision);
    }

    [Fact(DisplayName = "BSphCar: Negative angles B-field coordinates conversion")]
    public void BSphCar_NegativeAngles_ReturnsExpectedValues()
    {
        // Act
        var fieldVector = _geopack2008.BSphCar(-Math.PI / 4, -Math.PI / 4, 1.0, 1.0, 1.0);

        // Assert
        fieldVector.Br.ShouldBe(1.0);
        fieldVector.Btheta.ShouldBe(1.0);
        fieldVector.Bphi.ShouldBe(1.0);
        fieldVector.Bx.ShouldBe(0.70710678118654757, MinimalTestsPrecision);
        fieldVector.By.ShouldBe(0.70710678118654746, MinimalTestsPrecision);
        fieldVector.Bz.ShouldBe(1.4142135623730949, MinimalTestsPrecision);
    }

    [Fact(DisplayName = "BSphCar: Large angles B-field coordinates conversion")]
    public void BSphCar_LargeAngles_ReturnsExpectedValues()
    {
        // Act
        var fieldVector = _geopack2008.BSphCar(2 * Math.PI, 2 * Math.PI, 1.0, 1.0, 1.0);

        // Assert
        fieldVector.Br.ShouldBe(1.0);
        fieldVector.Btheta.ShouldBe(1.0);
        fieldVector.Bphi.ShouldBe(1.0);
        fieldVector.Bx.ShouldBe(1.0, MinimalTestsPrecision);
        fieldVector.By.ShouldBe(1.0, MinimalTestsPrecision);
        fieldVector.Bz.ShouldBe(1.0, MinimalTestsPrecision);
    }

    [Fact(DisplayName = "BSphCar: Small angles B-field coordinates conversion")]
    public void BSphCar_SmallAngles_ReturnsExpectedValues()
    {
        // Act
        var fieldVector = _geopack2008.BSphCar(1e-10, 1e-10, 1.0, 1.0, 1.0);

        // Assert
        fieldVector.Br.ShouldBe(1.0);
        fieldVector.Btheta.ShouldBe(1.0);
        fieldVector.Bphi.ShouldBe(1.0);
        fieldVector.Bx.ShouldBe(1.0, MinimalTestsPrecision);
        fieldVector.By.ShouldBe(1.0000000001, MinimalTestsPrecision);
        fieldVector.Bz.ShouldBe(0.99999999989999999, MinimalTestsPrecision);
    }
}
