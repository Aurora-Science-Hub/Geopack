using AuroraScienceHub.Geopack.Contracts.Engine;
using AuroraScienceHub.Geopack.Contracts.Interfaces;
using AuroraScienceHub.Geopack.Contracts.Models;
using AuroraScienceHub.Geopack.ExternalFieldModels.T89;
using AuroraScienceHub.Geopack.UnitTests.Geopack.Fixtures;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack;

[CollectionDefinition("Geopack")]
public class TestDataCollection : ICollectionFixture<TestDataFixture>;

[Collection("Geopack")]
public partial class GeopackTests(TestDataFixture fixture)
{
    private static readonly AuroraScienceHub.Geopack.Geopack s_geopack = new();

    private readonly ComputationContext _context = s_geopack.Recalc(
        fixture.InputData.DateTime, CartesianVector<Velocity>.New(-304.0D, 13.0D, 4.0D, CoordinateSystem.GSW));

    private readonly IExternalFieldModel _t89 = new T89();

    private const double Rad = 57.295779513D;

    private const double MinimalTestsPrecision = 0.000000000008d;

    private const string CommonsDataFileName =
        "AuroraScienceHub.Geopack.UnitTests.Geopack.TestData.CommonsDataSet.dat";

    private const string BSpCarDatasetFileName =
        "AuroraScienceHub.Geopack.UnitTests.Geopack.TestData.BSpCarDataSet.dat";

    private const string GeoMagDatasetFileName =
        "AuroraScienceHub.Geopack.UnitTests.Geopack.TestData.GeoMag.dat";

    private const string MagGeoDatasetFileName =
        "AuroraScienceHub.Geopack.UnitTests.Geopack.TestData.MagGeo.dat";

    private const string GswGseDatasetFileName =
        "AuroraScienceHub.Geopack.UnitTests.Geopack.TestData.GswGse.dat";

    private const string GseGswDatasetFileName =
        "AuroraScienceHub.Geopack.UnitTests.Geopack.TestData.GseGsw.dat";

    private const string MagSmDatasetFileName =
        "AuroraScienceHub.Geopack.UnitTests.Geopack.TestData.MagSm.dat";

    private const string SmMagDatasetFileName =
        "AuroraScienceHub.Geopack.UnitTests.Geopack.TestData.SmMag.dat";

    private const string GeiGeoDatasetFileName =
        "AuroraScienceHub.Geopack.UnitTests.Geopack.TestData.GeiGeo.dat";

    private const string GeoGeiDatasetFileName =
        "AuroraScienceHub.Geopack.UnitTests.Geopack.TestData.GeoGei.dat";

    private const string SmGswDatasetFileName =
        "AuroraScienceHub.Geopack.UnitTests.Geopack.TestData.SmGsw.dat";

    private const string GswSmDatasetFileName =
        "AuroraScienceHub.Geopack.UnitTests.Geopack.TestData.GswSm.dat";

    private const string GeoGswDatasetFileName =
        "AuroraScienceHub.Geopack.UnitTests.Geopack.TestData.GeoGsw.dat";

    private const string GswGeoDatasetFileName =
        "AuroraScienceHub.Geopack.UnitTests.Geopack.TestData.GswGeo.dat";

    private const string GeodGeoDatasetFileName =
        "AuroraScienceHub.Geopack.UnitTests.Geopack.TestData.GeodGeo.dat";

    private const string GeoGeodDatasetFileName =
        "AuroraScienceHub.Geopack.UnitTests.Geopack.TestData.GeoGeod.dat";

    private const string TraceNSResultFileName =
        "AuroraScienceHub.Geopack.UnitTests.Geopack.TestData.TraceNSResult.dat";

    private const string TraceSNResultFileName =
        "AuroraScienceHub.Geopack.UnitTests.Geopack.TestData.TraceSNResult.dat";
}
