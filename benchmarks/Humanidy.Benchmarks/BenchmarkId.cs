using System.Text.Json.Serialization;
using Humanidy.Text.Json;

namespace Humanidy.Benchmarks;

[Humanidy("bench")]
[JsonConverter(typeof(BenchmarkIdJsonConverter))]
public partial struct BenchmarkId;
