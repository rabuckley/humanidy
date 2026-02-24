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

    [Fact]
    public void PrefixWithUnderscore_ProducesDiagnostic()
    {
        var compilation = CompilationHelper.CreateCompilation(
            """
            using Humanidy;

            namespace Test;

            [Humanidy("my_app")]
            public partial struct BadId;
            """);

        var result = CompilationHelper.RunValuesSourceGenerator(compilation);

        var diagnostic = Assert.Single(result.Diagnostics);
        Assert.Equal("HUMANIDY002", diagnostic.Id);
    }

    [Fact]
    public void PrefixWithDigits_ProducesDiagnostic()
    {
        var compilation = CompilationHelper.CreateCompilation(
            """
            using Humanidy;

            namespace Test;

            [Humanidy("org2")]
            public partial struct BadId;
            """);

        var result = CompilationHelper.RunValuesSourceGenerator(compilation);

        var diagnostic = Assert.Single(result.Diagnostics);
        Assert.Equal("HUMANIDY002", diagnostic.Id);
    }

    [Fact]
    public void PrefixWithUppercase_ProducesDiagnostic()
    {
        var compilation = CompilationHelper.CreateCompilation(
            """
            using Humanidy;

            namespace Test;

            [Humanidy("Test")]
            public partial struct BadId;
            """);

        var result = CompilationHelper.RunValuesSourceGenerator(compilation);

        var diagnostic = Assert.Single(result.Diagnostics);
        Assert.Equal("HUMANIDY002", diagnostic.Id);
    }

    [Fact]
    public void PrefixTooShort_ProducesDiagnostic()
    {
        var compilation = CompilationHelper.CreateCompilation(
            """
            using Humanidy;

            namespace Test;

            [Humanidy("x")]
            public partial struct BadId;
            """);

        var result = CompilationHelper.RunValuesSourceGenerator(compilation);

        var diagnostic = Assert.Single(result.Diagnostics);
        Assert.Equal("HUMANIDY003", diagnostic.Id);
    }

    [Fact]
    public void RandomLengthTooSmall_ProducesDiagnostic()
    {
        var compilation = CompilationHelper.CreateCompilation(
            """
            using Humanidy;

            namespace Test;

            [Humanidy("test", RandomLength = 2)]
            public partial struct BadId;
            """);

        var result = CompilationHelper.RunValuesSourceGenerator(compilation);

        var diagnostic = Assert.Single(result.Diagnostics);
        Assert.Equal("HUMANIDY004", diagnostic.Id);
    }

    private static Task Verify(string source)
    {
        return Verifier.Verify(source).UseDirectory("Snapshots");
    }
}
