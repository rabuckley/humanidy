using BenchmarkDotNet.Attributes;

namespace Humanidy.Benchmarks;

[MemoryDiagnoser]
public class Utf8SpanFormattableBenchmarks
{
    private static readonly BenchmarkId s_benchmarkId = BenchmarkId.NewId();

    [Benchmark]
    public void TryFormat()
    {
        Span<byte> buffer = stackalloc byte[256];
        s_benchmarkId.TryFormat(buffer, out _);
    }
}
