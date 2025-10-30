using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;

namespace AuroraScienceHub.Geopack.Benchmarks.Geopack;

/// <summary>
/// Benchmarks for magnetic field calculation performance
/// </summary>
[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net90)]
// [SimpleJob(RuntimeMoniker.NativeAot90)]
[MarkdownExporterAttribute.GitHub]
[Config(typeof(NativeAotConfig))]
public class IgrfMagneticFieldBenchmarks
{
    private readonly AuroraScienceHub.Geopack.Geopack _geopack = new();

    private const double Xgsw = -1.02D;
    private const double Ygsw = 0.0D;
    private const double Zgsw = 0.0D;

    private const double Vgsex = -304.0D;
    private const double Vgsey = 14.78D;
    private const double Vgsez = 4.0D;

    private static readonly DateTime s_testDate = new(1997, 12, 11, 10, 10, 0, DateTimeKind.Utc);

    [GlobalSetup]
    public void Setup()
        => _geopack.Recalc_08(s_testDate, Vgsex, Vgsey, Vgsez);

    [Benchmark(Baseline = true)]
    public void Calculate_IgrfMagneticField()
        => _geopack.IgrfGsw_08(Xgsw, Ygsw, Zgsw);

    [Benchmark]
    public void Calculate_DipMagneticField()
        => _geopack.Dip_08(Xgsw, Ygsw, Zgsw);

    [Benchmark]
    public void Calculate_Sun()
        => _geopack.Sun_08(s_testDate);

    private class NativeAotConfig : ManualConfig
    {
        public NativeAotConfig()
        {
            AddJob(Job.Default
                .WithRuntime(NativeAotRuntime.Net90)
                .WithId("NativeAOT"));
            ArtifactsPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "BenchmarkDotNet.Artifacts");
        }
    }
}
