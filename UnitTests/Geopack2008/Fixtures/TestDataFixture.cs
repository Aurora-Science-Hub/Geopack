using AuroraScienceHub.Geopack.UnitTests.Models;
using AuroraScienceHub.Geopack.UnitTests.Utils;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack2008.Fixtures;

public class TestDataFixture
{
    private const string InputDataFileName = "AuroraScienceHub.Geopack.UnitTests.Geopack2008.TestData.InputData.dat";

    public InputGeopackData InputData { get; private set; }

    public TestDataFixture()
    {
        var rawData = ReadTextAsync(InputDataFileName).Result;
        InputData = rawData.ParseInputData();
    }

    private static async Task<string> ReadTextAsync(string resourceName) =>
        await EmbeddedResourceReader.ReadTextAsync(InputDataFileName);
}
