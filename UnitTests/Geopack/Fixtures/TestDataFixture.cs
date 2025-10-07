using AuroraScienceHub.Geopack.UnitTests.Models;
using AuroraScienceHub.Geopack.UnitTests.Utils;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack.Fixtures;

public class TestDataFixture
{
    private const string InputDataFileName = "AuroraScienceHub.Geopack.UnitTests.Geopack.TestData.InputData.dat";

    public InputGeopackData InputData { get; private set; }

    public TestDataFixture()
    {
        string rawData = ReadTextAsync(InputDataFileName).Result;
        InputData = rawData.ParseInputData();
    }

    private static async Task<string> ReadTextAsync(string resourceName) =>
        await EmbeddedResourceReader.ReadTextAsync(InputDataFileName);
}
