using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Linq;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class EndpointNamingAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "EndpointNaming";
    private static readonly LocalizableString Title = "Endpoint naming issue";
    private static readonly LocalizableString MessageFormat = "Method name '{0}' does not follow the naming convention";
    private static readonly LocalizableString Description = "Ensure endpoint method names follow the naming convention";
    private const string Category = "Naming";

    private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Method);
    }

    private static void AnalyzeSymbol(SymbolAnalysisContext context)
    {
        var methodSymbol = (IMethodSymbol)context.Symbol;

        // List of HTTP verbs to check for
        var httpVerbs = new[] { "GET", "POST", "PUT", "DELETE" };

        // Check if the method is public and belongs to a class that is likely a controller
        if (methodSymbol.DeclaredAccessibility == Accessibility.Public &&
            methodSymbol.ContainingType.Name.EndsWith("Controller"))
        {
            // Check if the name starts with any of the HTTP verbs
            if (httpVerbs.Any(verb => methodSymbol.Name.StartsWith(verb, System.StringComparison.OrdinalIgnoreCase)))
            {
                var diagnostic = Diagnostic.Create(Rule, methodSymbol.Locations[0], methodSymbol.Name);
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
