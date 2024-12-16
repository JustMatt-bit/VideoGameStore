using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Rename;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EndpointNaming
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(EndpointNamingCodeFixProvider)), Shared]
    public class EndpointNamingCodeFixProvider : CodeFixProvider
    {
        private const string Title = "Trim HTTP verb prefix";

        public sealed override ImmutableArray<string> FixableDiagnosticIds
            => ImmutableArray.Create(EndpointNamingAnalyzer.DiagnosticId);

        public sealed override FixAllProvider GetFixAllProvider()
            => WellKnownFixAllProviders.BatchFixer;

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            // Find the method declaration identified by the diagnostic.
            var methodDeclaration = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType<MethodDeclarationSyntax>().First();

            // Register a code action that will invoke the fix.
            context.RegisterCodeFix(
                CodeAction.Create(
                    title: Title,
                    createChangedSolution: c => FixEndpointNameAsync(context.Document, methodDeclaration, c),
                    equivalenceKey: Title),
                diagnostic);
        }

        private async Task<Solution> FixEndpointNameAsync(Document document, MethodDeclarationSyntax methodDeclaration, CancellationToken cancellationToken)
        {
            var identifierToken = methodDeclaration.Identifier;
            var methodName = identifierToken.Text;

            // List of HTTP verbs to check for
            var httpVerbs = new[] { "GET", "POST", "PUT", "DELETE" };

            // Find the verb prefix and trim it
            var newName = httpVerbs.FirstOrDefault(verb => methodName.StartsWith(verb, System.StringComparison.OrdinalIgnoreCase));
            if (newName != null)
            {
                newName = methodName.Substring(newName.Length);
            }
            else
            {
                return document.Project.Solution;
            }

            // Produce a new solution that has all references to the method renamed.
            var semanticModel = await document.GetSemanticModelAsync(cancellationToken);
            var methodSymbol = semanticModel.GetDeclaredSymbol(methodDeclaration, cancellationToken);
            var solution = document.Project.Solution;
            var optionSet = solution.Workspace.Options;
            var newSolution = await Renamer.RenameSymbolAsync(solution, methodSymbol, newName, optionSet, cancellationToken).ConfigureAwait(false);

            return newSolution;
        }
    }
}
