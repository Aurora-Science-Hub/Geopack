namespace AuroraScienceHub.Geopack.UnitTests.Geopack2008;

public partial class Geopack2008Tests
{
    private const string RecalcDatasetFileName = "AuroraScienceHub.Geopack.UnitTests.Geopack2008.TestData.RecalcDataSet.dat";
    private const string SphCarDatasetFileName = "AuroraScienceHub.Geopack.UnitTests.Geopack2008.TestData.SPHCARDataSet.dat";
    private const string TraceDatasetFileName =  "AuroraScienceHub.Geopack.UnitTests.Geopack2008.TestData.TraceDataSet.dat";
    private const double Deg2Rad = MathF.PI / 180.0f;
    private readonly Geopack08.Geopack08 _geopack2008 = new();
    private const double MinimalTestsPrecision = 0.0000000000001d;
}
