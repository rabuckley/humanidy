using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Humanidy.Generators;

public sealed class HumanidyGeneratorTests
{
    [Fact]
    public Task GenerateReportMethod()
    {
        const string structName = "SampleEntity";

        var compilation = CompilationHelper.CreateCompilation(
            $"""
             using Humanidy;

             namespace Humanidy.Generators.Sample;

             [Humanidy("pref")]
             public partial struct {structName};
             """);

        var result = CompilationHelper.RunValuesSourceGenerator(compilation);

        Assert.NotNull(result);
        Assert.Empty(result.Diagnostics);

        var identifierSource = Assert.Single(result.Compilation.SyntaxTrees,
            tree => tree.FilePath.EndsWith($"{structName}.g.cs", StringComparison.Ordinal));

        var jsonSource = Assert.Single(result.Compilation.SyntaxTrees,
            tree => tree.FilePath.EndsWith($"{structName}JsonConverter.g.cs", StringComparison.Ordinal));

        var expected = string.Join('\n', identifierSource, jsonSource);

        return Verify(expected);
    }

    /// <summary>
    /// Tests that we don't emit unsafe code like `[SkipLocalsInit]` when `allowUnsafe` is false.
    /// </summary>
    [Fact]
    public Task GenerationWithAllowUnsafeFalse()
    {
        const string structName = "SampleEntity";

        var options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, allowUnsafe: false);

        var compilation = CompilationHelper.CreateCompilation(
            $"""
             using Humanidy;

             namespace Humanidy.Generators.Sample;

             [Humanidy("pref")]
             public partial struct {structName};
             """, options);

        var result = CompilationHelper.RunValuesSourceGenerator(compilation);

        Assert.NotNull(result);
        Assert.Empty(result.Diagnostics);

        var identifierSource = Assert.Single(result.Compilation.SyntaxTrees,
            tree => tree.FilePath.EndsWith($"{structName}.g.cs", StringComparison.Ordinal));

        var jsonSource = Assert.Single(result.Compilation.SyntaxTrees,
            tree => tree.FilePath.EndsWith($"{structName}JsonConverter.g.cs", StringComparison.Ordinal));

        var expected = string.Join('\n', identifierSource, jsonSource);

        return Verify(expected);
    }

    private static Task Verify(string source)
    {
        return Verifier.Verify(source).UseDirectory("Snapshots");
    }
}
