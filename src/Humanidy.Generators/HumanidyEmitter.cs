using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace Humanidy.Generators;

internal static class HumanidyEmitter
{
    public static void Execute(
        SourceProductionContext context,
        (Compilation Left, ImmutableArray<IdentifierSpecification> Right) data)
    {
        var (compilation, specifications) = data;

        // If there are no specifications, do nothing.
        if (specifications.IsDefaultOrEmpty)
        {
            return;
        }

        // Generate code for each specification.
        GenerateCode(context, compilation, specifications);
    }

    private static void GenerateCode(
        SourceProductionContext context,
        Compilation compilation,
        ImmutableArray<IdentifierSpecification> specifications)
    {
        var compilationOptions = new HumanidyCompilationOptions
        {
            AllowUnsafe = compilation.Options is CSharpCompilationOptions { AllowUnsafe: true },
        };

        // Go through all filtered class declarations.
        foreach (var spec in specifications)
        {
            var anyErrors = false;

            foreach (var diagnostic in spec.Diagnostics)
            {
                context.ReportDiagnostic(diagnostic);

                // Use effective severity, rather than defaults.
                if (diagnostic.Severity is DiagnosticSeverity.Error)
                {
                    anyErrors = true;
                }
            }

            if (anyErrors)
            {
                // If there are any errors, skip generating code for this specification.
                continue;
            }

            var identifierSource = HumanidyIdentifierEmitter.Emit(spec, compilationOptions);
            var jsonConverterSource = HumanidyJsonConverterEmitter.Emit(spec, compilationOptions);

            context.AddSource($"{spec.StructName}.g.cs", identifierSource);
            context.AddSource($"{spec.StructName}JsonConverter.g.cs", jsonConverterSource);
        }
    }
}
