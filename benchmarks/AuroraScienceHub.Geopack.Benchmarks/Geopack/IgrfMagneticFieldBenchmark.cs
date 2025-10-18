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
public class IgrfMagneticFieldBenchmark
{
    private readonly AuroraScienceHub.Geopack.Geopack _geopack = new();

    private readonly double _xgsw = -1.02D;
    private readonly double _ygsw = 0.0D;
    private readonly double _zgsw = 0.0D;

    private readonly double _vgsex = -304.0D;
    private readonly double _vgsey = 14.78D;
    private readonly double _vgsez = 4.0D;

    private readonly DateTime _testDate = new(1997, 12, 11, 10, 10, 0, DateTimeKind.Utc);

    [GlobalSetup]
    public void Setup()
    {
    }

    [Benchmark(Baseline=true)]
    public CartesianFieldVector Calculate_IgrfMagneticField()
    {
        _geopack.Recalc_08(_testDate, _vgsex, _vgsey, _vgsez);
        CartesianFieldVector field = _geopack.IgrfGsw_08(_xgsw, _ygsw, _zgsw);

        return field;
    }

    [Benchmark]
    public CartesianFieldVector Calculate_DipMagneticField()
    {
        _geopack.Recalc_08(_testDate, _vgsex, _vgsey, _vgsez);
        CartesianFieldVector field = _geopack.Dip_08(_xgsw, _ygsw, _zgsw);

        return field;
    }

    [Benchmark]
    public Sun Calculate_Sun()
    {
        Sun sun = _geopack.Sun_08(_testDate);

        return sun;
    }
}
