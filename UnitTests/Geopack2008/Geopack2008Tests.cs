using AuroraScienceHub.Geopack.UnitTests.Geopack2008.Fixtures;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack2008;

[CollectionDefinition("Geopack2008")]
public class TestDataCollection : ICollectionFixture<TestDataFixture>;

[Collection("Geopack2008")]
public partial class Geopack2008Tests(TestDataFixture fixture)
{
    private const string SphCarDatasetFileName =
        "AuroraScienceHub.Geopack.UnitTests.Geopack2008.TestData.SphCarDataSet.dat";

    private const string CarSphDatasetFileName =
        "AuroraScienceHub.Geopack.UnitTests.Geopack2008.TestData.CarSphDataSet.dat";

    private const string BSpCarDatasetFileName =
        "AuroraScienceHub.Geopack.UnitTests.Geopack2008.TestData.BSpCarDataSet.dat";

    private const string TraceDatasetFileName =
        "AuroraScienceHub.Geopack.UnitTests.Geopack2008.TestData.TraceDataSet.dat";

    private readonly Geopack08.Geopack08 _geopack2008 = new();
    private const double MinimalTestsPrecision = 0.0000000000001d;
    private const double Tolerance = 13;
}
