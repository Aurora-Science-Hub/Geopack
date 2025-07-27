using AuroraScienceHub.Geopack.UnitTests.Geopack.Fixtures;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack;

[CollectionDefinition("Geopack")]
public class TestDataCollection : ICollectionFixture<TestDataFixture>;

[Collection("Geopack")]
public partial class GeopackTests(TestDataFixture fixture)
{
    private const string CommonsDataFileName =
        "AuroraScienceHub.Geopack.UnitTests.Geopack.TestData.CommonsDataSet.dat";

    private const string SphCarDatasetFileName =
        "AuroraScienceHub.Geopack.UnitTests.Geopack.TestData.SphCarDataSet.dat";

    private const string CarSphDatasetFileName =
        "AuroraScienceHub.Geopack.UnitTests.Geopack.TestData.CarSphDataSet.dat";

    private const string BSpCarDatasetFileName =
        "AuroraScienceHub.Geopack.UnitTests.Geopack.TestData.BSpCarDataSet.dat";

    private readonly AuroraScienceHub.Geopack.Geopack.Geopack _geopack2008 = new();
    private const double MinimalTestsPrecision = 0.0000000000001d;
    private const double Tolerance = 13;
}
