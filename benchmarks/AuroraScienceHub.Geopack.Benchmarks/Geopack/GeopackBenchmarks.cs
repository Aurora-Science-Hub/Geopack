using AuroraScienceHub.Geopack.Contracts;
using AuroraScienceHub.Geopack.Contracts.Cartesian;
using AuroraScienceHub.Geopack.Contracts.Coordinates;
using AuroraScienceHub.Geopack.Contracts.Engine;
using AuroraScienceHub.Geopack.Contracts.PhysicalObjects;
using AuroraScienceHub.Geopack.Contracts.PhysicalQuantities;
using AuroraScienceHub.Geopack.Contracts.Spherical;
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
[SimpleJob(RuntimeMoniker.Net80)]
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
    private const double Rad = 57.295779513D;

    private static readonly CartesianVector<Velocity> s_gseVelocityVector = CartesianVector<Velocity>.New(-304.0, 14.78, 4.0, CoordinateSystem.GSE);

    private static readonly CartesianLocation s_testPointNs = CartesianLocation.New(-0.45455707401565865D, 0.4737969930623606D, 0.7542497890011055D, CoordinateSystem.GSW);
    private static readonly CartesianLocation s_testPointSn = CartesianLocation.New(-0.1059965956329907D, 0.41975266827470664D, -0.9014246640527153D, CoordinateSystem.GSW);

    private static readonly CartesianLocation s_gswCartesian = CartesianLocation.New(-1.02D, -1.02D, 1.02D, CoordinateSystem.GSW);
    private static readonly CartesianLocation s_geiCartesian = CartesianLocation.New(-1.02D, -1.02D, 1.02D, CoordinateSystem.GEI);
    private static readonly CartesianLocation s_geoCartesian = CartesianLocation.New(-1.02D, -1.02D, 1.02D, CoordinateSystem.GEO);
    private static readonly CartesianLocation s_magCartesian = CartesianLocation.New(-1.02D, -1.02D, 1.02D, CoordinateSystem.MAG);
    private static readonly CartesianLocation s_smCartesian = CartesianLocation.New(-1.02D, -1.02D, 1.02D, CoordinateSystem.SM);
    private static readonly CartesianVector<MagneticField> s_gswFieldCar = CartesianVector<MagneticField>.New(1.0, 1.0, 1.0, CoordinateSystem.GSW);

    private static readonly SphericalLocation s_igrfGeoLocation = SphericalLocation.New(1.02, (90.0 - 73.0) / Rad, 45.0 / Rad, CoordinateSystem.GEO);

    private static readonly SphericalLocation s_gswSph = SphericalLocation.New(1.02D, 1.0D, 1.0D, CoordinateSystem.GSW);
    private static readonly SphericalVector<MagneticField> s_gswFieldSph = SphericalVector<MagneticField>.New(1.0, 1.0, 1.0, CoordinateSystem.GSW);

    private static readonly GeodeticCoordinates s_geodetic = new(1.04719, 100.0);
    private static readonly GeocentricCoordinates s_geocentric = new(6462.13176, 0.526464);

    private const double XnPd = 5.0D;
    private const double Vel = -350.0D;
    private const double BzImf = 5.0D;

    private static readonly DateTime s_testDate = new(1997, 12, 11, 10, 10, 0, DateTimeKind.Utc);

    private static readonly ComputationContext s_context = s_geopack.Recalc(s_testDate);

    // [Benchmark]
    // public void ToSphericalVector()
    //     => s_gswFieldCar.ToSphericalVector(s_gswCartesian);
    //
    // [Benchmark]
    // public void ToCartesianVector()
    //     => s_gswFieldSph.ToCartesianVector(s_gswSph);
    //
    // [Benchmark]
    // public void ToSpherical()
    //     => s_gswCartesian.ToSpherical();
    //
    // [Benchmark]
    // public void ToCartesian()
    //     => s_gswSph.ToCartesian();
    //
    // [Benchmark]
    // public void GeiToGeo()
    //     => s_geopack.GeiToGeo(s_context, s_geiCartesian);
    //
    // [Benchmark]
    // public void GeoToGei()
    //     => s_geopack.GeoToGei(s_context, s_geoCartesian);
    //
    // [Benchmark]
    // public void ToGeocentric()
    //     => s_geodetic.ToGeocentric();
    //
    // [Benchmark]
    // public void ToGeodetic()
    //     => s_geocentric.ToGeodetic();
    //
    // [Benchmark]
    // public void GeoToGsw()
    //     => s_geopack.GeoToGsw(s_context, s_geoCartesian);
    //
    // [Benchmark]
    // public void GswToGeo()
    //     => s_geopack.GswToGeo(s_context, s_gswCartesian);
    //
    // [Benchmark]
    // public void GeoToMag()
    //     => s_geopack.GeoToMag(s_context, s_geoCartesian);
    //
    // [Benchmark]
    // public void MagToGeo()
    //     => s_geopack.MagToGeo(s_context, s_magCartesian);
    //
    // [Benchmark]
    // public void GswToGse()
    //     => s_geopack.GswToGse(s_context, s_gswFieldCar);
    //
    // [Benchmark]
    // public void GseToGsw()
    //     => s_geopack.GseToGsw(s_context, s_gseVelocityVector);
    //
    // [Benchmark]
    // public void MagToSm()
    //     => s_geopack.MagToSm(s_context, s_magCartesian);
    //
    // [Benchmark]
    // public void SmToMag()
    //     => s_geopack.SmToMag(s_context, s_smCartesian);
    //
    // [Benchmark]
    // public void SmToGsw()
    //     => s_geopack.SmToGsw(s_context, s_smCartesian);
    //
    // [Benchmark]
    // public void GswToSm()
    //     => s_geopack.GswToSm(s_context, s_gswFieldCar);
    //
    // [Benchmark(Baseline = true)]
    // public void Recalc()
    //     => s_geopack.Recalc(s_testDate, s_gseVelocityVector);

    [Benchmark]
    public void ShuMgnp()
        => s_geopack.ShuMgnp(XnPd, Vel, BzImf, s_gswCartesian);

    // [Benchmark]
    // public void T96Mgnp()
    //     => s_geopack.T96Mgnp(XnPd, Vel, s_gswCartesian);
    //
    // [Benchmark]
    // public void IgrfGeo()
    //     => s_geopack.IgrfGeo(s_context, s_igrfGeoLocation);
    //
    // [Benchmark]
    // public void IgrfGsw()
    //     => s_geopack.IgrfGsw(s_context, s_gswCartesian);
    //
    // [Benchmark]
    // public void Dip()
    //     => s_geopack.Dip(s_context, s_gswCartesian);
    //
    // [Benchmark]
    // public void Sun()
    //     => s_geopack.Sun(s_testDate);
    //
    // [Benchmark]
    // public void T89()
    //     => s_t89.Calculate(Iopt, s_parmod, Psi, s_gswCartesian);
    //
    // [Benchmark]
    // public void Trace_NS()
    //     => s_geopack.Trace(s_context,
    //         s_testPointNs,
    //         TraceDirection.AntiParallel, Dsmax, Err, Rlim, R0,
    //         Iopt, s_parmod,
    //         s_t89, s_geopack.IgrfGsw,
    //         Lmax);
    //
    // [Benchmark]
    // public void Trace_SN()
    //     => s_geopack.Trace(s_context,
    //         s_testPointSn,
    //         TraceDirection.Parallel, Dsmax, Err, Rlim, R0,
    //         Iopt, s_parmod,
    //         s_t89, s_geopack.IgrfGsw,
    //         Lmax);

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
