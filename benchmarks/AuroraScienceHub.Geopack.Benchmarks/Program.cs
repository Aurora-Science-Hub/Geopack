using AuroraScienceHub.Geopack.Benchmarks.Geopack;
using BenchmarkDotNet.Running;

namespace AuroraScienceHub.Geopack.Benchmarks;

public class Program
{
    public static void Main(string[] args)
    {
        BenchmarkSwitcher.FromAssembly(typeof(GeopackBenchmarks).Assembly).Run(args);
    }
}
