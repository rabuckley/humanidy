using BenchmarkDotNet.Attributes;

namespace Humanidy.Benchmarks;

[MemoryDiagnoser]
public class SpanParsableBenchmarks
{
    private static readonly string s_str = BenchmarkId.NewId().ToString();

    [Benchmark]
    public BenchmarkId Parse()
    {
        return BenchmarkId.Parse(s_str);
    }

    [Benchmark]
    public BenchmarkId TryParse()
    {
        _ = BenchmarkId.TryParse(s_str, out var id);
        return id;
    }
}

[MemoryDiagnoser]
public class Utf8SpanParsableBenchmarks
{
    private static ReadOnlySpan<byte> Bytes => "bench_ilR6rtfrSOuZo"u8;

    [Benchmark]
    public BenchmarkId Parse()
    {
        return BenchmarkId.Parse(Bytes);
    }

    [Benchmark]
    public BenchmarkId TryParse()
    {
        _ = BenchmarkId.TryParse(Bytes, out var id);
        return id;
    }
}
