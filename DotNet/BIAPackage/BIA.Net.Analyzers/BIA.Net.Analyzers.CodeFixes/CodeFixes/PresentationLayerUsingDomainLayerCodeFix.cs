namespace BIA.Net.Analyzer.CodeFixes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CodeFixes;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    internal sealed class PresentationLayerUsingDomainLayerCodeFix : CodeFixBase
    {
        public override string DiagnosticId => "BIA001";

        public override string Title => "Remove forbidden Domain layer reference";

        public override async Task Register(CodeFixContext codeFixContext)
        {
            var root = await codeFixContext.Document.GetSyntaxRootAsync(codeFixContext.CancellationToken).ConfigureAwait(false);
            if (root == null)
                return;

            var diagnostic = codeFixContext.Diagnostics[0];
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            // Find the using directive identified by the analyzer
            var usingDirective = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf()
                .OfType<UsingDirectiveSyntax>().FirstOrDefault();

            if (usingDirective == null)
                return;

            // Register a code action to remove the forbidden using directive
            codeFixContext.RegisterCodeFix(
                Microsoft.CodeAnalysis.CodeActions.CodeAction.Create(
                    title: Title,
                    createChangedDocument: c => RemoveForbiddenUsingDirectiveAsync(codeFixContext.Document, usingDirective, c),
                    equivalenceKey: Title),
                diagnostic);
        }

        private async Task<Document> RemoveForbiddenUsingDirectiveAsync(Document document, UsingDirectiveSyntax usingDirective, CancellationToken cancellationToken)
        {
            var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            if (root == null)
                return document;

            // Remove the forbidden using directive from the syntax tree
            var newRoot = root.RemoveNode(usingDirective, SyntaxRemoveOptions.KeepNoTrivia);

            // Return the updated document with the forbidden using removed
            return document.WithSyntaxRoot(newRoot);
        }
    }
}
