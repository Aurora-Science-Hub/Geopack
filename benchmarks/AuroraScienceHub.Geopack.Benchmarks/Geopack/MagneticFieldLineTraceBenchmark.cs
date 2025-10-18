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
// [SimpleJob(RuntimeMoniker.Net80)]
//[SimpleJob(RuntimeMoniker.NativeAot80)]
[SimpleJob(RuntimeMoniker.Net90)]
// [SimpleJob(RuntimeMoniker.NativeAot90)]
[MarkdownExporterAttribute.GitHub]
public class MagneticFieldLineTraceBenchmark
{
    private readonly AuroraScienceHub.Geopack.Geopack _geopack = new();
    private readonly IExternalFieldModel _t89 = new T89();

    private readonly TraceDirection _dir = TraceDirection.AntiParallel;
    private readonly double _dsmax = 0.1D;
    private readonly double _err = 0.0001D;
    private readonly double _rlim = 60.0D;
    private readonly double _r0 = 1.0D;
    private readonly int _iopt = 1;
    private readonly double[] _parmod = new double[10];
    private readonly int _lmax = 500;

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
    public FieldLine[] Trace_MultiplePoints()
    {
        FieldLine[] results = new FieldLine[_testPoints.Length];

        for (int i = 0; i < _testPoints.Length; i++)
        {
            _geopack.Recalc_08(_testDates[i], -304.0D, -16.0D + 29.78D, 4.0D);
            (double X, double Y, double Z) point = _testPoints[i];
            results[i] = _geopack.Trace_08(
                point.X, point.Y, point.Z,
                _dir, _dsmax, _err, _rlim, _r0,
                _iopt, _parmod,
                _t89, _geopack.IgrfGsw_08,
                _lmax);
        }

        return results;
    }


    [Benchmark(Baseline = true)]
    [Arguments(-1.02D, 0.8D, 0.9D)]
    public FieldLine Trace_SinglePoint(double x, double y, double z)
    {
        return _geopack.Trace_08(
            x, y, z,
            _dir, _dsmax, _err, _rlim, _r0,
            _iopt, _parmod,
            _t89, _geopack.IgrfGsw_08,
            _lmax);
    }

    [Benchmark]
    public void Trace_WithReinitialization()
    {
        DateTime dateTime = new(2008, 12, 12, 12, 0, 0);

        for (int i = 0; i < 3; i++) // Несколько итераций для стабильности измерений
        {
            _geopack.Recalc_08(dateTime, -304.0D, -16.0D + 29.78D, 4.0D);

            FieldLine result = _geopack.Trace_08(
                _testPoints[0].X, _testPoints[0].Y, _testPoints[0].Z,
                _dir, _dsmax, _err, _rlim, _r0,
                _iopt, _parmod,
                _t89, _geopack.IgrfGsw_08,
                _lmax);
        }
    }

    [Benchmark(Description = "Try to vary some tracing parameters.")]
    public FieldLine Trace_DifferentParameters()
    {
        return _geopack.Trace_08(
            -1.02D, 0.8D, -0.9D,
            TraceDirection.Parallel,
            0.05D,
            0.00001D,
            100.0D,
            _r0, _iopt, _parmod,
            _t89, _geopack.IgrfGsw_08,
            _lmax);
    }
}
