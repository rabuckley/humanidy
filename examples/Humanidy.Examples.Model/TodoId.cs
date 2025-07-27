using System.Text.Json.Serialization;
using Humanidy.Text.Json;

namespace Humanidy.Examples.Model;

[Humanidy("todo")]
[JsonConverter(typeof(TodoIdJsonConverter))]
public partial struct TodoId;
