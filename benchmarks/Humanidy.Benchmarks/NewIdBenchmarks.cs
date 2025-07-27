using BenchmarkDotNet.Attributes;

namespace Humanidy.Benchmarks;

[MemoryDiagnoser]
public class NewIdBenchmarks
{
    [Benchmark]
    public BenchmarkId NewId()
    {
        return BenchmarkId.NewId();
    }
}
