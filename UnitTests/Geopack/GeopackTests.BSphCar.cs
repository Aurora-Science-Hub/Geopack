using AuroraScienceHub.Geopack.Common.Contracts;
using AuroraScienceHub.Geopack.UnitTests.Utils;
using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack;

public partial class GeopackTests
{
    [Fact(DisplayName = "Iterate BSPCAR_08 setups and verify result")]
    public async Task BSphCar_ReturnsCorrectValues()
    {
        // Arrange
        string rawData = await EmbeddedResourceReader.ReadTextAsync(BSpCarDatasetFileName);
        string[] line = rawData.SplitParametersLine();

        double theta = line[1].ParseDouble();
        double phi = line[3].ParseDouble();
        SphericalFieldVector approvedSphericalFieldVector = new SphericalFieldVector(
            line[5].ParseDouble(),
            line[7].ParseDouble(),
            line[9].ParseDouble(),
            null);

        CartesianFieldVector expectedVector = new CartesianFieldVector(
            line[11].ParseDouble(),
            line[13].ParseDouble(),
            line[15].ParseDouble(),
            null);

        // Act
        CartesianFieldVector fieldVector = _geopack.BSphCar(
            theta, phi,
            approvedSphericalFieldVector.Br, approvedSphericalFieldVector.Btheta, approvedSphericalFieldVector.Bphi);

        // Assert
        fieldVector.Bx.ShouldBe(expectedVector.Bx, MinimalTestsPrecision);
        fieldVector.By.ShouldBe(expectedVector.By, MinimalTestsPrecision);
        fieldVector.Bz.ShouldBe(expectedVector.Bz, MinimalTestsPrecision);
    }

    [Fact(DisplayName = "BSphCar: Zero angles B-field coordinates conversion")]
    public void BSphCar_ZeroAngles_ReturnsExpectedValues()
    {
        // Act
        CartesianFieldVector fieldVector = _geopack.BSphCar(0.0, 0.0, 1.0, 1.0, 1.0);

        // Assert
        fieldVector.Bx.ShouldBe(1.0, MinimalTestsPrecision);
        fieldVector.By.ShouldBe(1.0, MinimalTestsPrecision);
        fieldVector.Bz.ShouldBe(1.0, MinimalTestsPrecision);
    }

    [Fact(DisplayName = "BSphCar: Negative angles B-field coordinates conversion")]
    public void BSphCar_NegativeAngles_ReturnsExpectedValues()
    {
        // Act
        CartesianFieldVector fieldVector = _geopack.BSphCar(-Math.PI / 4, -Math.PI / 4, 1.0, 1.0, 1.0);

        // Assert
        fieldVector.Bx.ShouldBe(0.70710678118654757, MinimalTestsPrecision);
        fieldVector.By.ShouldBe(0.70710678118654746, MinimalTestsPrecision);
        fieldVector.Bz.ShouldBe(1.4142135623730949, MinimalTestsPrecision);
    }

    [Fact(DisplayName = "BSphCar: Large angles B-field coordinates conversion")]
    public void BSphCar_LargeAngles_ReturnsExpectedValues()
    {
        // Act
        CartesianFieldVector fieldVector = _geopack.BSphCar(2 * Math.PI, 2 * Math.PI, 1.0, 1.0, 1.0);

        // Assert
        fieldVector.Bx.ShouldBe(1.0, MinimalTestsPrecision);
        fieldVector.By.ShouldBe(1.0, MinimalTestsPrecision);
        fieldVector.Bz.ShouldBe(1.0, MinimalTestsPrecision);
    }

    [Fact(DisplayName = "BSphCar: Small angles B-field coordinates conversion")]
    public void BSphCar_SmallAngles_ReturnsExpectedValues()
    {
        // Act
        CartesianFieldVector fieldVector = _geopack.BSphCar(1e-10, 1e-10, 1.0, 1.0, 1.0);

        // Assert
        fieldVector.Bx.ShouldBe(1.0, MinimalTestsPrecision);
        fieldVector.By.ShouldBe(1.0000000001, MinimalTestsPrecision);
        fieldVector.Bz.ShouldBe(0.99999999989999999, MinimalTestsPrecision);
    }
}
