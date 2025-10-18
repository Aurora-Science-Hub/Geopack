using AuroraScienceHub.ExternalFieldModels.T89;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using AuroraScienceHub.Geopack.Contracts;
using AuroraScienceHub.Geopack.Contracts.Interfaces;
using AuroraScienceHub.Geopack.Contracts.Models;

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

    private readonly (double X, double Y, double Z)[] _testPoints = new[]
    {
        (1.24D, 6.38D, 1.22D),
        (1.22D, 6.38D, 1.21D),
        (1.19D, 6.39D, 1.21D),
        (1.16D, 6.40D, 1.20D),
        (1.13D, 6.40D, 1.20D),
        (1.10D, 6.41D, 1.19D),
        (1.08D, 6.41D, 1.19D),
        (1.05D, 6.42D, 1.18D),
        (1.02D, 6.42D, 1.18D),
        (0.99D, 6.43D, 1.17D),
        (0.96D, 6.44D, 1.16D),
        (0.94D, 6.44D, 1.16D),
        (0.91D, 6.45D, 1.15D),
        (0.88D, 6.45D, 1.15D),
        (0.85D, 6.46D, 1.14D),
        (0.82D, 6.46D, 1.13D)
    };

    private readonly DateTime[] _testDates = new[]
    {
        new DateTime(2023, 10, 18, 0, 0, 00),
        new DateTime(2023, 10, 18, 0, 01, 00),
        new DateTime(2023, 10, 18, 0, 02, 00),
        new DateTime(2023, 10, 18, 0, 03, 00),
        new DateTime(2023, 10, 18, 0, 04, 00),
        new DateTime(2023, 10, 18, 0, 05, 00),
        new DateTime(2023, 10, 18, 0, 06, 00),
        new DateTime(2023, 10, 18, 0, 07, 00),
        new DateTime(2023, 10, 18, 0, 08, 00),
        new DateTime(2023, 10, 18, 0, 09, 00),
        new DateTime(2023, 10, 18, 0, 10, 00),
        new DateTime(2023, 10, 18, 0, 11, 00),
        new DateTime(2023, 10, 18, 0, 12, 00),
        new DateTime(2023, 10, 18, 0, 13, 00),
        new DateTime(2023, 10, 18, 0, 14, 00),
        new DateTime(2023, 10, 18, 0, 15, 00),
    };

    [GlobalSetup]
    public void Setup()
    {
    }

    [Benchmark]
    public FieldLine[] Trace_SpacecraftFromNorthToSouth()
    {
        FieldLine[] results = new FieldLine[_testPoints.Length];

        for (int i = 0; i < _testPoints.Length; i++)
        {
            _geopack.Recalc_08(_testDates[i], _vgsex, _vgsey, _vgsez);
            (double X, double Y, double Z) point = _testPoints[i];
            results[i] = _geopack.Trace_08(
                point.X, point.Y, point.Z,
                _dirNS, _dsmax, _err, _rlim, _r0,
                _iopt, _parmod,
                _t89, _geopack.IgrfGsw_08,
                _lmax);
        }

        return results;
    }

    [Benchmark(Baseline = true)]
    public FieldLine[] Trace_SpacecraftFromSouthToNorth()
    {
        FieldLine[] results = new FieldLine[_testPoints.Length];

        for (int i = 0; i < _testPoints.Length; i++)
        {
            _geopack.Recalc_08(_testDates[i], _vgsex, _vgsey, _vgsez);
            (double X, double Y, double Z) point = _testPoints[i];
            results[i] = _geopack.Trace_08(
                point.X, point.Y, point.Z,
                _dirSN, _dsmax, _err, _rlim, _r0,
                _iopt, _parmod,
                _t89, _geopack.IgrfGsw_08,
                _lmax);
        }

        return results;
    }
}
