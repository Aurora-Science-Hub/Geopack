using AuroraScienceHub.Geopack.UnitTests.Geopack.Fixtures;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack;

[CollectionDefinition("Geopack")]
public class TestDataCollection : ICollectionFixture<TestDataFixture>;

[Collection("Geopack")]
public partial class GeopackTests(TestDataFixture fixture)
{
    private readonly AuroraScienceHub.Geopack.Geopack.Geopack _geopack = new();

    private const double DegRad = 0.01745329D;
    private const double Pi = 3.141592654D;
    private const double TwoPi = 6.283185307D;
    private const double Rad = 57.295779513D;

    private const double MinimalTestsPrecision = 0.00000000001d;

    private const string CommonsDataFileName =
        "AuroraScienceHub.Geopack.UnitTests.Geopack.TestData.CommonsDataSet.dat";

    private const string SphCarDatasetFileName =
        "AuroraScienceHub.Geopack.UnitTests.Geopack.TestData.SphCarDataSet.dat";

    private const string BSpCarDatasetFileName =
        "AuroraScienceHub.Geopack.UnitTests.Geopack.TestData.BSpCarDataSet.dat";
}
