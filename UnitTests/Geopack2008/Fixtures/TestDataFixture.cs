using AuroraScienceHub.Geopack.UnitTests.Geopack2008.TestData;
using AuroraScienceHub.Geopack.UnitTests.Utils;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack2008.Fixtures;

public class TestDataFixture
{
    public const string InputDataFileName = "AuroraScienceHub.Geopack.UnitTests.Geopack2008.TestData.InputData.dat";
    public const string CommonsDataFileName = "AuroraScienceHub.Geopack.UnitTests.Geopack2008.TestData.CommonsDataSet.dat";
    private const string SphCarDatasetFileName = "AuroraScienceHub.Geopack.UnitTests.Geopack2008.TestData.SPHCARDataSet.dat";
    private const string TraceDatasetFileName =  "AuroraScienceHub.Geopack.UnitTests.Geopack2008.TestData.TraceDataSet.dat";

    public InputGeopackData InputData { get; private set; }

    public TestDataFixture()
    {
        var rawData = ReadTextAsync(InputDataFileName).Result;
        InputData = rawData.ParseInputData();
    }

    private static async Task<string> ReadTextAsync(string resourceName) =>
        await EmbeddedResourceReader.ReadTextAsync(InputDataFileName);
}
