using System.Reflection;
using Humanidy.Generators.Tests;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Humanidy.Generators;

public static class CompilationHelper
{
    private static readonly CSharpParseOptions s_parseOptions =
        new(kind: SourceCodeKind.Regular, documentationMode: DocumentationMode.Parse);

    private static readonly Assembly s_systemRuntimeAssembly = Assembly.Load("System.Runtime");

    public static Compilation CreateCompilation(string source)
    {
        List<MetadataReference>? references =
        [
            MetadataReference.CreateFromFile(s_systemRuntimeAssembly.Location),
            MetadataReference.CreateFromFile(typeof(HumanidyAttribute).GetTypeInfo().Assembly.Location),
            .. AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => !assembly.IsDynamic && !string.IsNullOrWhiteSpace(assembly.Location))
                .Select(assembly => MetadataReference.CreateFromFile(assembly.Location))
        ];

        return CSharpCompilation.Create(
            "TestAssembly",
            [CSharpSyntaxTree.ParseText(source)],
            references,
            options: new CSharpCompilationOptions(
                outputKind: OutputKind.DynamicallyLinkedLibrary,
                allowUnsafe: true));
    }

    public static SourceGenerationResult RunValuesSourceGenerator(Compilation compilation)
    {
        var generator = new HumanidyGenerator();

        var driver = CSharpGeneratorDriver.Create(
            generators: [generator.AsSourceGenerator()],
            parseOptions: s_parseOptions,
            driverOptions: new GeneratorDriverOptions(
                disabledOutputs: IncrementalGeneratorOutputKind.None,
                trackIncrementalGeneratorSteps: true)
        );

        _ = driver.RunGeneratorsAndUpdateCompilation(compilation, out var outCompilation, out var diagnostics);

        return new SourceGenerationResult
        {
            Compilation = outCompilation,
            Diagnostics = diagnostics,
        };
    }
}
