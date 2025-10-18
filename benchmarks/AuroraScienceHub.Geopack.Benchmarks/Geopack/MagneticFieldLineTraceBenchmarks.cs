using AuroraScienceHub.ExternalFieldModels.T89;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using AuroraScienceHub.Geopack.Contracts;
using AuroraScienceHub.Geopack.Contracts.Interfaces;

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
    private readonly AuroraScienceHub.Geopack.Geopack _geopack = new();
    private readonly IExternalFieldModel _t89 = new T89();

    private readonly TraceDirection _dirNS = TraceDirection.AntiParallel;
    private readonly TraceDirection _dirSN = TraceDirection.Parallel;
    private readonly double _dsmax = 0.1D;
    private readonly double _err = 0.0001D;
    private readonly double _rlim = 60.0D;
    private readonly double _r0 = 1.0D;
    private readonly int _iopt = 1;
    private readonly double[] _parmod = new double[10];
    private readonly int _lmax = 500;

    private readonly double _vgsex = -304.0D;
    private readonly double _vgsey = 14.78D;
    private readonly double _vgsez = 4.0D;

    private readonly (double X, double Y, double Z) _testPoint = (1.24D, 6.38D, 1.22D);

    private readonly DateTime _testDate = new(2023, 10, 18, 0, 0, 00);

    [GlobalSetup]
    public void Setup()
    {
    }

    [Benchmark]
    public void Trace_SpacecraftFromNorthToSouth()
    {
        _geopack.Recalc_08(_testDate, _vgsex, _vgsey, _vgsez);
        (double X, double Y, double Z) point = _testPoint;
        _geopack.Trace_08(
            point.X, point.Y, point.Z,
            _dirNS, _dsmax, _err, _rlim, _r0,
            _iopt, _parmod,
            _t89, _geopack.IgrfGsw_08,
            _lmax);
    }

    [Benchmark(Baseline = true)]
    public void Trace_SpacecraftFromSouthToNorth()
    {
        _geopack.Recalc_08(_testDate, _vgsex, _vgsey, _vgsez);
        (double X, double Y, double Z) point = _testPoint;
        _geopack.Trace_08(
            point.X, point.Y, point.Z,
            _dirSN, _dsmax, _err, _rlim, _r0,
            _iopt, _parmod,
            _t89, _geopack.IgrfGsw_08,
            _lmax);
    }
}
