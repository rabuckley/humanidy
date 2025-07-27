using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Humanidy.Generators;

[Generator]
public sealed class HumanidyGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var provider = context.SyntaxProvider.ForAttributeWithMetadataName(
                "Humanidy.HumanidyAttribute",
                Filter,
                Transform)
            .Where(static spec => spec is not null);

        var source = context.CompilationProvider.Combine(provider.Collect());

        context.RegisterSourceOutput(source, HumanidyEmitter.Execute);
    }

    private static bool Filter(SyntaxNode node, CancellationToken cancellationToken)
    {
        return node is StructDeclarationSyntax;
    }

    private static IdentifierSpecification Transform(
        GeneratorAttributeSyntaxContext context,
        CancellationToken cancellationToken)
    {
        var structDeclarationSyntax = (StructDeclarationSyntax)context.TargetNode;

        // Go through all attributes of the class.
        foreach (var attr in context.Attributes)
        {
            if (context.SemanticModel.GetDeclaredSymbol(structDeclarationSyntax, cancellationToken: cancellationToken)
                is not INamedTypeSymbol namedTypeSymbol)
            {
                continue;
            }

            var namespaceName = namedTypeSymbol.ContainingNamespace.IsGlobalNamespace
                ? null
                : namedTypeSymbol.ContainingNamespace.ToDisplayString();

            var prefixArgument = attr.ConstructorArguments[0];

            if (prefixArgument.Value is not string prefix)
            {
                return new IdentifierSpecification
                {
                    Diagnostics =
                    {
                        Diagnostic.Create(
                            new DiagnosticDescriptor(
                                "HUMANIDY001",
                                "Invalid Prefix",
                                "The prefix must be a non-empty string.",
                                "Usage",
                                DiagnosticSeverity.Error,
                                isEnabledByDefault: true),
                            attr.ApplicationSyntaxReference?.GetSyntax(cancellationToken).GetLocation())
                    }
                };
            }

            // The prefix must contain only alphanumeric ASCII characters or underscores.
            // If it contains any other characters, we return an error diagnostic.
            if (!prefix.All(c => c < 128 && (char.IsLetterOrDigit(c) || c == '_')))
            {
                return new IdentifierSpecification
                {
                    Diagnostics =
                    {
                        Diagnostic.Create(
                            new DiagnosticDescriptor(
                                "HUMANIDY002",
                                "Invalid Prefix",
                                "The prefix must contain only alphanumeric ASCII characters.",
                                "Usage",
                                DiagnosticSeverity.Error,
                                isEnabledByDefault: true),
                            attr.ApplicationSyntaxReference?.GetSyntax(cancellationToken).GetLocation())
                    }
                };
            }

            var randomLengthArg = attr.NamedArguments
                .FirstOrDefault(arg => arg.Key == "RandomLength")
                .Value;

            if (randomLengthArg.Value is not int randomLength)
            {
                // If RandomLength is not provided, use the default value of 14.
                randomLength = 14;
            }

            return new IdentifierSpecification
            {
                Namespace = namespaceName,
                StructName = structDeclarationSyntax.Identifier.Text,
                Prefix = prefix,
                RandomLength = randomLength
            };
        }

        return null!; // `Where(spec => spec is not null)` handles this.
    }
}
