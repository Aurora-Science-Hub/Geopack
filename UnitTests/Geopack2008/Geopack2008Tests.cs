using AuroraScienceHub.Geopack.Geopack08;
using UnitTests.Geopack2008.TestData;
using FluentAssertions;

namespace UnitTests.Geopack2008;

public class Geopack2008
{
    private readonly Geopack08 _geopack2008 = new();

    [Fact]
    public void Test1()
    {
        // Arrange
        var data = TestDataParser
            .ReadDataFromFile("../../../Geopack2008/TestData/OriginalDataSet.dat");

        var testData = TestDataParser.Parse(data);
        TestDataParser.FillSolarWindVelocity(testData);

        // T96 test inputs
        var parmod = new List<float> { testData.SolarWindPressure, testData.DstIndex, testData.ByIMF, testData.BzIMF };

        // Act
        _geopack2008.RECALC_08(testData.DateTime, testData.VGSEX, testData.VGSEY, testData.VGSEZ);

        // Assert
        data.Should().NotBeNull();
    }
}
