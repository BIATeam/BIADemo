namespace BIA.Net.Analyzer.PresentationLayer
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Composition;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CodeActions;
    using Microsoft.CodeAnalysis.CodeFixes;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Rename;
    using Microsoft.CodeAnalysis.Text;

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(BIANetAnalyzerPresentationLayerCodeFixProvider)), Shared]
    public class BIANetAnalyzerPresentationLayerCodeFixProvider : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds
            => ImmutableArray.Create(BIANetAnalyzerPresentationLayerAnalyzer.DiagnosticId);

        public sealed override FixAllProvider GetFixAllProvider()
            => WellKnownFixAllProviders.BatchFixer;

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            if (root == null)
                return;

            var diagnostic = context.Diagnostics[0];
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            // Find the using directive identified by the analyzer
            var usingDirective = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf()
                .OfType<UsingDirectiveSyntax>().FirstOrDefault();

            if (usingDirective == null)
                return;

            // Register a code action to remove the forbidden using directive
            context.RegisterCodeFix(
                Microsoft.CodeAnalysis.CodeActions.CodeAction.Create(
                    title: CodeFixResources.CodeFixTitle,
                    createChangedDocument: c => RemoveForbiddenUsingDirectiveAsync(context.Document, usingDirective, c),
                    equivalenceKey: CodeFixResources.CodeFixTitle),
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
