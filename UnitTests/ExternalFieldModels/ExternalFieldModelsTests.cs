using AuroraScienceHub.Geopack.UnitTests.Geopack.Fixtures;

namespace AuroraScienceHub.Geopack.UnitTests.ExternalFieldModels;

[CollectionDefinition("ExternalFieldModels")]
public class TestDataCollection : ICollectionFixture<TestDataFixture>;

[Collection("ExternalFieldModels")]
public partial class TModelsTests
{
    private readonly AuroraScienceHub.ExternalFieldModels.T89.T89 _t89 = new();

    private const double MinimalTestsPrecision = 0.0000000000001d;
}
