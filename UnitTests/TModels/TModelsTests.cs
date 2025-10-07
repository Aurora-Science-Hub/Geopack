using AuroraScienceHub.Geopack.UnitTests.Geopack.Fixtures;

namespace AuroraScienceHub.Geopack.UnitTests.TModels;

[CollectionDefinition("TModels")]
public class TestDataCollection : ICollectionFixture<TestDataFixture>;

[Collection("TModels")]
public partial class TModelsTests
{
    private readonly AuroraScienceHub.TModels.T89.T89 _t89 = new();

    private const double MinimalTestsPrecision = 0.000000000008d;

    // private const string CommonsDataFileName =
    //     "AuroraScienceHub.Geopack.UnitTests.Geopack.TestData.CommonsDataSet.dat";
}
