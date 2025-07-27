using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace Humanidy.Generators.Tests;

public sealed record SourceGenerationResult
{
    public required Compilation Compilation { get; init; }

    public required ImmutableArray<Diagnostic> Diagnostics { get; init; }
}
