using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace AuroraScienceHub.Geopack.Benchmarks.Geopack;

/// <summary>
/// Benchmarks for ...
/// </summary>
[MemoryDiagnoser(false)]
[SimpleJob(RuntimeMoniker.Net90)]
[SimpleJob(RuntimeMoniker.NativeAot90)]
public class SomeCalculatorBenchmark
{
    private readonly SomeCalculator _calculator = new();

    [GlobalSetup]
    public void Setup()
    {
        // Initialize the calculator with some data
    }

    [Benchmark]
    public void Calculate()
    {
        _calculator.Calculate();
    }
}
