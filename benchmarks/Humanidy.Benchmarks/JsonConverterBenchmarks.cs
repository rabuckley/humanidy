using System.Text.Json;
using BenchmarkDotNet.Attributes;

namespace Humanidy.Benchmarks;

[MemoryDiagnoser]
public class JsonConverterBenchmarks
{
    private static readonly BenchmarkId s_benchmarkId = BenchmarkId.NewId();

    private static readonly string s_string = s_benchmarkId.ToString();

    private static readonly Guid s_guid = Guid.NewGuid();

    [Benchmark(Baseline = true)]
    public string SerializeString()
    {
        return JsonSerializer.Serialize(s_string);
    }

    [Benchmark]
    public string SerializeGuid()
    {
        return JsonSerializer.Serialize(s_guid);
    }

    [Benchmark]
    public string SerializeBenchmarkId()
    {
        return JsonSerializer.Serialize(s_benchmarkId);
    }
}
