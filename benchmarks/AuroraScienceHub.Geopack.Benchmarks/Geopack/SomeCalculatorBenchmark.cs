// using BenchmarkDotNet.Attributes;
// using BenchmarkDotNet.Jobs;

// namespace AuroraScienceHub.Geopack.Benchmarks.Geopack;

// /// <summary>
// /// Benchmarks for <see cref="SomeCalculator"/>.
// /// </summary>
// [MemoryDiagnoser(false)]
// [SimpleJob(RuntimeMoniker.Net60, baseline: true)]
// [SimpleJob(RuntimeMoniker.Net70)]
// [SimpleJob(RuntimeMoniker.Net80)]
// public class SomeCalculatorBenchmark
// {
//     private SomeCalculator? _calculator;
//
//     [GlobalSetup]
//     public void Setup()
//     {
//         _calculator = new SomeCalculator();
//     }
//
//     [Benchmark]
//     public void Parse()
//     {
//         _calculator!.Calculate();
//     }
// }
