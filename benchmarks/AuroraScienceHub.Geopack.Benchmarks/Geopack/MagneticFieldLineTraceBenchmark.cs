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
[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.NativeAot80)]
[SimpleJob(RuntimeMoniker.Net90)]
[SimpleJob(RuntimeMoniker.NativeAot90)]
[MarkdownExporterAttribute.GitHub]
[HtmlExporter]
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
        (-1.02D, 0.8D, 0.9D),
        (-2.0D, 1.5D, 1.2D),
        (-0.5D, 0.3D, 0.4D),
        (-3.0D, 2.0D, 1.8D)
    };

    [GlobalSetup]
    public void Setup()
    {
        DateTime dateTime = new(2008, 12, 12, 12, 0, 0);
        _geopack.Recalc_08(dateTime, -304.0D, -16.0D + 29.78D, 4.0D);
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
    public FieldLine[] Trace_MultiplePoints()
    {
        FieldLine[] results = new FieldLine[_testPoints.Length];

        for (int i = 0; i < _testPoints.Length; i++)
        {
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
