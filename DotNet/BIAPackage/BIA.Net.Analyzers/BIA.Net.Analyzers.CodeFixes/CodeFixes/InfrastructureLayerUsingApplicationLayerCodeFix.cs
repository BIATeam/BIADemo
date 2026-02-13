// <copyright file="InfrastructureLayerUsingApplicationLayerCodeFix.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Analyzer.CodeFixes
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CodeFixes;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Code fix when detecting using of Application layer into Infrastructure layer.
    /// </summary>
    internal sealed class InfrastructureLayerUsingApplicationLayerCodeFix : ICodeFix
    {
        /// <inheritdoc/>
        public string DiagnosticId => "BIA008";

        /// <inheritdoc/>
        public string Title => "BIA.Net - remove forbidden Application layer reference";

        /// <inheritdoc/>
        public async Task Register(CodeFixContext codeFixContext)
        {
            var root = await codeFixContext.Document.GetSyntaxRootAsync(codeFixContext.CancellationToken).ConfigureAwait(false);
            if (root == null)
            {
                return;
            }

            var diagnostic = codeFixContext.Diagnostics[0];
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            var usingDirective = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType<UsingDirectiveSyntax>().FirstOrDefault();
            if (usingDirective == null)
            {
                return;
            }

            codeFixContext.RegisterCodeFix(
            Microsoft.CodeAnalysis.CodeActions.CodeAction.Create(
                title: this.Title,
                createChangedDocument: c => Common.RemoveUsingDirectiveAsync(codeFixContext.Document, usingDirective, c),
                equivalenceKey: this.Title),
            diagnostic);
        }
    }
}
