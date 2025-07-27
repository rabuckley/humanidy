using System.Text.Json.Serialization;
using Humanidy.Examples.Model;

namespace Humanidy.WebApiExample;

[JsonSourceGenerationOptions(
    WriteIndented = true,
    PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower)]
[JsonSerializable(typeof(Todo))]
[JsonSerializable(typeof(IEnumerable<Todo>))]
internal sealed partial class AppJsonSerializerContext : JsonSerializerContext;
