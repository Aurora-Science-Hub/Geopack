using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using AuroraScienceHub.Geopack.Contracts;
using AuroraScienceHub.Geopack.Contracts.Interfaces;
using AuroraScienceHub.Geopack.ExternalFieldModels.T89;

namespace AuroraScienceHub.Geopack.Benchmarks.Geopack;

/// <summary>
/// Benchmarks for magnetic field line tracing performance
/// </summary>
[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net90)]
// [SimpleJob(RuntimeMoniker.NativeAot90)]
[MarkdownExporterAttribute.GitHub]
public class MagneticFieldLineTraceBenchmarks
{
    private static readonly AuroraScienceHub.Geopack.Geopack s_geopack = new();
    private static readonly IExternalFieldModel s_t89 = new T89();

    private const TraceDirection DirNs = TraceDirection.AntiParallel;
    private const TraceDirection DirSn = TraceDirection.Parallel;
    private const double Dsmax = 0.1D;
    private const double Err = 0.0001D;
    private const double Rlim = 60.0D;
    private const double R0 = 1.0D;
    private const int Iopt = 1;
    private static readonly double[] s_parmod = new double[10];
    private const int Lmax = 500;

    private const double Vgsex = -304.0D;
    private const double Vgsey = 14.78D;
    private const double Vgsez = 4.0D;

    private static readonly (double X, double Y, double Z) s_testPointNs = (-1.02D, 0.0D, 0.9D);
    private static readonly (double X, double Y, double Z) s_testPointSn = (-1.02D, 0.0D, -0.9D);

    private static readonly DateTime s_testDate = new(2023, 10, 18, 0, 0, 00);

    [GlobalSetup]
    public void Setup()
    {
        s_geopack.Recalc_08(s_testDate, Vgsex, Vgsey, Vgsez);
    }

    [Benchmark(Baseline = true)]
    public void Trace_FieldLineFromNorthToSouthHemisphere()
        => s_geopack.Trace_08(
            s_testPointNs.X, s_testPointNs.Y, s_testPointNs.Z,
            DirNs, Dsmax, Err, Rlim, R0,
            Iopt, s_parmod,
            s_t89, s_geopack.IgrfGsw_08,
            Lmax);

    [Benchmark]
    public void Trace_FieldLineFromSouthToNorthHemisphere()
        => s_geopack.Trace_08(
            s_testPointSn.X, s_testPointSn.Y, s_testPointSn.Z,
            DirSn, Dsmax, Err, Rlim, R0,
            Iopt, s_parmod,
            s_t89, s_geopack.IgrfGsw_08,
            Lmax);
}
