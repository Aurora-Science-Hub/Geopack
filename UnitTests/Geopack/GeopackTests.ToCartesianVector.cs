using AuroraScienceHub.Geopack.Contracts.Models;
using AuroraScienceHub.Geopack.UnitTests.Utils;
using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack;

public partial class GeopackTests
{
    [Fact(DisplayName = "Iterate BSPCAR_08 setups and verify result")]
    public async Task ToCartesian_ReturnsCorrectValues()
    {
        // Arrange
        string rawData = await EmbeddedResourceReader.ReadTextAsync(BSpCarDatasetFileName);
        string[] line = rawData.SplitParametersLine();

        SphericalLocation sphLocation = SphericalLocation.New(1D, line[1].ParseDouble(), line[3].ParseDouble(), CoordinateSystem.GSW);
        SphericalVector<MagneticField> testSphericalFieldVector = SphericalVector<MagneticField>.New(
            line[5].ParseDouble(),
            line[7].ParseDouble(),
            line[9].ParseDouble(),
            CoordinateSystem.GSW);

        CartesianVector<MagneticField> expectedVector = CartesianVector<MagneticField>.New(
            line[11].ParseDouble(),
            line[13].ParseDouble(),
            line[15].ParseDouble(),
            CoordinateSystem.GSW);

        // Act
        CartesianVector<MagneticField> calculatedVector = testSphericalFieldVector.ToCartesianVector(sphLocation);

        // Assert
        calculatedVector.X.ShouldBe(expectedVector.X, MinimalTestsPrecision);
        calculatedVector.Y.ShouldBe(expectedVector.Y, MinimalTestsPrecision);
        calculatedVector.Z.ShouldBe(expectedVector.Z, MinimalTestsPrecision);
    }

    [Fact(DisplayName = "BSphCar: Zero angles B-field coordinates conversion")]
    public void ToCartesian_ZeroAngles_ReturnsExpectedValues()
    {
        // Arrange
        SphericalLocation location = SphericalLocation.New(0,0,0, CoordinateSystem.GSW);
        SphericalVector<MagneticField> testField = SphericalVector<MagneticField>.New(1, 1, 1, CoordinateSystem.GSW);

        // Act
        CartesianVector<MagneticField> fieldVector = testField.ToCartesianVector(location);

        // Assert
        fieldVector.X.ShouldBe(1.0, MinimalTestsPrecision);
        fieldVector.Y.ShouldBe(1.0, MinimalTestsPrecision);
        fieldVector.Z.ShouldBe(1.0, MinimalTestsPrecision);
    }

    [Fact(DisplayName = "BSphCar: Negative angles B-field coordinates conversion")]
    public void BSphCar_NegativeAngles_ReturnsExpectedValues()
    {
        // Arrange
        SphericalLocation location = SphericalLocation.New(0,-Math.PI / 4, -Math.PI / 4, CoordinateSystem.GSW);
        SphericalVector<MagneticField> testField = SphericalVector<MagneticField>.New(1, 1, 1, CoordinateSystem.GSW);

        // Act
        CartesianVector<MagneticField> fieldVector = testField.ToCartesianVector(location);

        // Assert
        fieldVector.X.ShouldBe(0.70710678118654757, MinimalTestsPrecision);
        fieldVector.Y.ShouldBe(0.70710678118654746, MinimalTestsPrecision);
        fieldVector.Z.ShouldBe(1.4142135623730949, MinimalTestsPrecision);
    }

    [Fact(DisplayName = "BSphCar: Large angles B-field coordinates conversion")]
    public void BSphCar_LargeAngles_ReturnsExpectedValues()
    {
        // Arrange
        SphericalLocation location = SphericalLocation.New(0,2 * Math.PI, 2 * Math.PI, CoordinateSystem.GSW);
        SphericalVector<MagneticField> testField = SphericalVector<MagneticField>.New(1, 1, 1, CoordinateSystem.GSW);

        // Act
        CartesianVector<MagneticField> fieldVector = testField.ToCartesianVector(location);

        // Assert
        fieldVector.X.ShouldBe(1.0, MinimalTestsPrecision);
        fieldVector.Y.ShouldBe(1.0, MinimalTestsPrecision);
        fieldVector.Z.ShouldBe(1.0, MinimalTestsPrecision);
    }

    [Fact(DisplayName = "BSphCar: Small angles B-field coordinates conversion")]
    public void BSphCar_SmallAngles_ReturnsExpectedValues()
    {
        // Arrange
        SphericalLocation location = SphericalLocation.New(0,1e-10, 1e-10, CoordinateSystem.GSW);
        SphericalVector<MagneticField> testField = SphericalVector<MagneticField>.New(1, 1, 1, CoordinateSystem.GSW);

        // Act
        CartesianVector<MagneticField> fieldVector = testField.ToCartesianVector(location);

        // Assert
        fieldVector.X.ShouldBe(1.0, MinimalTestsPrecision);
        fieldVector.Y.ShouldBe(1.0000000001, MinimalTestsPrecision);
        fieldVector.Z.ShouldBe(0.99999999989999999, MinimalTestsPrecision);
    }
}
