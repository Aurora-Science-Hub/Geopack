using AuroraScienceHub.Geopack.Contracts.Interfaces;
using AuroraScienceHub.Geopack.Contracts.Models;
using AuroraScienceHub.Geopack.ExternalFieldModels.T89;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;

namespace AuroraScienceHub.Geopack.Benchmarks.Geopack;

/// <summary>
/// Benchmarks for magnetic field calculation performance
/// </summary>
[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net80, baseline: true)]
[SimpleJob(RuntimeMoniker.Net10_0)]
[MarkdownExporterAttribute.GitHub]
[Config(typeof(NativeAotConfig))]
public class GeopackBenchmarks
{
    private static readonly AuroraScienceHub.Geopack.Geopack s_geopack = new();
    private static readonly IExternalFieldModel s_t89 = new T89();

    private const double Dsmax = 0.1D;
    private const double Err = 0.0001D;
    private const double Rlim = 60.0D;
    private const double R0 = 1.0D;
    private const int Iopt = 1;
    private const double Psi = 1;
    private static readonly double[] s_parmod = new double[10];
    private const int Lmax = 500;

    private const double Vgsex = -304.0D;
    private const double Vgsey = 14.78D;
    private const double Vgsez = 4.0D;

    private static readonly (double X, double Y, double Z) s_testPointNs = (-0.45455707401565865D, 0.4737969930623606D, 0.7542497890011055D);
    private static readonly (double X, double Y, double Z) s_testPointSn = (-0.1059965956329907D, 0.41975266827470664D, -0.9014246640527153D);

    private const double X = -1.02D;
    private const double Y = 1.02D;
    private const double Z = 1.02D;
    private const double BX = 1.0D;
    private const double BY = 1.0D;
    private const double BZ = 1.0D;

    private const double Theta = 0.7D;
    private const double Phi = 1.4D;
    private const double R = 1.02D;
    private const double BR = 1.0D;
    private const double BTh = 1.0D;
    private const double BPhi = 1.0D;

    private const double H = 100.0D;
    private const double XMU = 1.04719D;
    private const double RR = 6462.13176D;
    private const double THETAR = 0.526464D;

    private const double XnPd = 5.0D;
    private const double Vel = -350.0D;
    private const double BzImf = 5.0D;

    private static readonly DateTime s_testDate = new(1997, 12, 11, 10, 10, 0, DateTimeKind.Utc);

    [GlobalSetup]
    public void Setup()
        => s_geopack.Recalc_08(s_testDate, Vgsex, Vgsey, Vgsez);

    [Benchmark]
    public void ToSphericalVector()
        => s_geopack.BCarSph_08(X, Y, Z, BX, BY, BZ);

    [Benchmark]
    public void ToCartesianVector()
        => s_geopack.BSphCar_08(Theta, Phi, BR, BTh, BPhi);

    [Benchmark]
    public void ToSpherical()
        => s_geopack.CarSph_08(X, Y, Z);

    [Benchmark]
    public void ToCartesian()
        => s_geopack.SphCar_08(R, Theta, Phi);

    [Benchmark]
    public void GeiToGeo()
        => s_geopack.GeiGeo_08(X, Y, Z);

    [Benchmark]
    public void GeoToGei()
        => s_geopack.GeoGei_08(X, Y, Z);

    [Benchmark]
    public void ToGeocentric()
        => s_geopack.GeodGeo_08(H, XMU);

    [Benchmark]
    public void ToGeodetic()
        => s_geopack.GeoGeod_08(RR, THETAR);

    [Benchmark]
    public void GeoToGsw()
        => s_geopack.GeoGsw_08(X, Y, Z);

    [Benchmark]
    public void GswToGeo()
        => s_geopack.GswGeo_08(X, Y, Z);

    [Benchmark]
    public void GeoToMag()
        => s_geopack.GeoMag_08(X, Y, Z);

    [Benchmark]
    public void MagToGeo()
        => s_geopack.MagGeo_08(X, Y, Z);

    [Benchmark]
    public void GswToGse()
        => s_geopack.GswGse_08(X, Y, Z);

    [Benchmark]
    public void GseToGsw()
        => s_geopack.GseGsw_08(X, Y, Z);

    [Benchmark]
    public void MagToSm()
        => s_geopack.MagSm_08(X, Y, Z);

    [Benchmark]
    public void SmToMag()
        => s_geopack.SmMag_08(X, Y, Z);

    [Benchmark]
    public void SmToGsw()
        => s_geopack.SmGsw_08(X, Y, Z);

    [Benchmark]
    public void GswToSm()
        => s_geopack.GswSm_08(X, Y, Z);

    [Benchmark(Baseline = true)]
    public void Recalc()
        => s_geopack.Recalc_08(s_testDate, Vgsex, Vgsey, Vgsez);

    [Benchmark]
    public void ShuMgnp()
        => s_geopack.ShuMgnp_08(XnPd, Vel, BzImf, X, Y, Z);

    [Benchmark]
    public void T96Mgnp()
        => s_geopack.T96Mgnp_08(XnPd, Vel, X, Y, Z);

    [Benchmark]
    public void IgrfGeo()
        => s_geopack.IgrfGeo_08(R, Theta, Phi);

    [Benchmark]
    public void IgrfGsw()
        => s_geopack.IgrfGsw_08(X, Y, Z);

    [Benchmark]
    public void Dip()
        => s_geopack.Dip_08(X, Y, Z);

    [Benchmark]
    public void Sun()
        => s_geopack.Sun_08(s_testDate);

    [Benchmark]
    public void T89()
        => s_t89.Calculate(Iopt, s_parmod, Psi, X, Y, Z);

    [Benchmark]
    public void Trace_NS()
        => s_geopack.Trace_08(
            s_testPointNs.X, s_testPointNs.Y, s_testPointNs.Z,
            TraceDirection.AntiParallel, Dsmax, Err, Rlim, R0,
            Iopt, s_parmod,
            s_t89, s_geopack.IgrfGsw_08,
            Lmax);

    [Benchmark]
    public void Trace_SN()
        => s_geopack.Trace_08(
            s_testPointSn.X, s_testPointSn.Y, s_testPointSn.Z,
            TraceDirection.Parallel, Dsmax, Err, Rlim, R0,
            Iopt, s_parmod,
            s_t89, s_geopack.IgrfGsw_08,
            Lmax);

    private class NativeAotConfig : ManualConfig
    {
        public NativeAotConfig()
        {
            ArtifactsPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "BenchmarkDotNet.Artifacts");
        }
    }
}
