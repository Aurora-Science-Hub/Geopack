using AuroraScienceHub.Geopack.UnitTests.Geopack2008.Fixtures;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack2008;

[CollectionDefinition("Geopack2008")]
public class TestDataCollection : ICollectionFixture<TestDataFixture>;

[Collection("Geopack2008")]
public partial class Geopack2008Tests
{
    private const double Deg2Rad = MathF.PI / 180.0f;
    private readonly Geopack08.Geopack08 _geopack2008 = new();
    private const double MinimalTestsPrecision = 0.0000000000001d;
    private readonly TestDataFixture _fixture;

    private const string SphCarDatasetFileName = "AuroraScienceHub.Geopack.UnitTests.Geopack2008.TestData.SPHCARDataSet.dat";
    private const string TraceDatasetFileName =  "AuroraScienceHub.Geopack.UnitTests.Geopack2008.TestData.TraceDataSet.dat";

    public Geopack2008Tests(TestDataFixture fixture)
    {
        _fixture = fixture;
    }
}
