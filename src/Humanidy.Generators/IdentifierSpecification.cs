using Microsoft.CodeAnalysis;

namespace Humanidy.Generators;

internal sealed record IdentifierSpecification
{
    /// <summary>
    /// <see langword="null"/> if the struct is in the global namespace.
    /// </summary>
    public string? Namespace { get; set; } = string.Empty;

    public string StructName { get; set; } = string.Empty;

    public string Prefix { get; set; } = string.Empty;

    public int RandomLength { get; set; }

    public ValueList<Diagnostic> Diagnostics { get; } = [];
}
