using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;

namespace Humanidy.Benchmarks;

[MemoryDiagnoser]
public class SpanFormattableBenchmarks
{
    private static readonly BenchmarkId s_benchmarkId = BenchmarkId.NewId();

    [Benchmark]
    public string IdToString()
    {
        return s_benchmarkId.ToString();
    }

    [Benchmark]
    [SkipLocalsInit]
    public void TryFormat()
    {
        Span<char> buffer = stackalloc char[256];
        s_benchmarkId.TryFormat(buffer, out _);
    }
}
