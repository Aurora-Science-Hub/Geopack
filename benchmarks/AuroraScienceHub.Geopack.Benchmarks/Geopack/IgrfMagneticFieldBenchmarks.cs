using AuroraScienceHub.Geopack.Contracts.Engine;
using AuroraScienceHub.Geopack.Contracts.Models;
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
[SimpleJob(RuntimeMoniker.NativeAot90)]
[MarkdownExporterAttribute.GitHub]
[Config(typeof(NativeAotConfig))]
public class IgrfMagneticFieldBenchmarks
{
    private readonly ComputationContext _ctx;
    private readonly AuroraScienceHub.Geopack.Geopack _geopack = new();
    private static readonly CartesianLocation s_testLocation = CartesianLocation.New(-1.02D, 0D, 0D, CoordinateSystem.GSW);
    private static readonly CartesianVector<Velocity> s_testVelocity = CartesianVector<Velocity>.New(-304.0D, 14.78D, 4.0D, CoordinateSystem.GSE);
    private static readonly DateTime s_testDate = new(1997, 12, 11, 10, 10, 0, DateTimeKind.Utc);

    public IgrfMagneticFieldBenchmarks()
    {
        _ctx = _geopack.Recalc(s_testDate, s_testVelocity);
    }

    [Benchmark(Baseline = true)]
    public void Calculate_IgrfMagneticField()
        => _geopack.IgrfGsw(_ctx, s_testLocation);

    [Benchmark]
    public void Calculate_DipMagneticField()
        => _geopack.Dip(_ctx, s_testLocation);

    [Benchmark]
    public void Calculate_Sun()
        => _geopack.Sun(s_testDate);

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
