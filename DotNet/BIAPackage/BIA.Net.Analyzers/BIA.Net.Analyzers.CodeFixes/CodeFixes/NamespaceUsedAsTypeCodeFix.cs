// <copyright file="NamespaceUsedAsTypeCodeFix.cs" company="BIA">
// Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Analyzer.CodeFixes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CodeActions;
    using Microsoft.CodeAnalysis.CodeFixes;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Code fix when a namespace is used as type (CS0118).
    /// </summary>
    internal sealed class NamespaceUsedAsTypeCodeFix : ICodeFix
    {
        /// <inheritdoc/>
        public string DiagnosticId => "CS0118";

        /// <inheritdoc/>
        public string Title => throw new NotImplementedException();

        /// <inheritdoc/>
        public async Task Register(CodeFixContext codeFixContext)
        {
            var root = await codeFixContext.Document.GetSyntaxRootAsync(codeFixContext.CancellationToken).ConfigureAwait(false);
            if (root == null)
            {
                return;
            }

            var diagnostic = codeFixContext.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            if (!(root.FindNode(diagnosticSpan) is IdentifierNameSyntax node))
            {
                return;
            }

            var semanticModel = await codeFixContext.Document.GetSemanticModelAsync(codeFixContext.CancellationToken).ConfigureAwait(false);
            if (semanticModel == null)
            {
                return;
            }

            var typeName = node.Identifier.Text;
            var typeSymbols = FindTypeCandidates(semanticModel, typeName);

            foreach (var typeSymbol in typeSymbols)
            {
                var title = $"BIA.Net - using {typeSymbol.ContainingNamespace};";
                var codeAction = CodeAction.Create(
                    title,
                    ct => AddUsingDirectiveAsync(codeFixContext.Document, root, typeSymbol),
                    equivalenceKey: title);

                codeFixContext.RegisterCodeFix(codeAction, diagnostic);
            }
        }

        private static IEnumerable<INamedTypeSymbol> FindTypeCandidates(SemanticModel semanticModel, string typeName)
        {
            var typeSymbols = new List<INamedTypeSymbol>();

            var compilation = semanticModel.Compilation;
            foreach (var namespaceSymbol in compilation.GlobalNamespace.GetNamespaceMembers())
            {
                FillMatchingTypeSymbolsRecursively(typeSymbols, namespaceSymbol, typeName);
            }

            return typeSymbols;
        }

        private static void FillMatchingTypeSymbolsRecursively(List<INamedTypeSymbol> typeSymbols, INamespaceSymbol currentNamespaceSymbol, string typeNameToFind)
        {
            foreach (var typeSymbol in currentNamespaceSymbol.GetTypeMembers(typeNameToFind))
            {
                typeSymbols.Add(typeSymbol);
            }

            foreach (var namespaceSymbol in currentNamespaceSymbol.GetNamespaceMembers())
            {
                FillMatchingTypeSymbolsRecursively(typeSymbols, namespaceSymbol, typeNameToFind);
            }
        }

        private static Task<Document> AddUsingDirectiveAsync(Document document, SyntaxNode root, INamedTypeSymbol typeSymbol)
        {
            var newUsingDirective = SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(typeSymbol.ContainingNamespace.ToDisplayString()));
            var existingUsings = root.DescendantNodes()
                .OfType<UsingDirectiveSyntax>()
                .ToList();

            if (!existingUsings.Any())
            {
                if (root is CompilationUnitSyntax compilationUnit)
                {
                    var compilationRoot = compilationUnit.AddUsings(newUsingDirective);
                    return Task.FromResult(document.WithSyntaxRoot(compilationRoot));
                }

                return Task.FromResult(document);
            }

            var systemUsings = existingUsings.Where(u => u.Name.ToString().StartsWith("System", StringComparison.Ordinal));
            var otherUsings = existingUsings.Except(systemUsings);

            if (newUsingDirective.Name.ToString().StartsWith("System", StringComparison.Ordinal))
            {
                systemUsings = systemUsings.Concat(new[] { newUsingDirective }).Distinct().OrderBy(u => u.Name.ToString(), StringComparer.Ordinal);
            }
            else
            {
                otherUsings = otherUsings.Concat(new[] { newUsingDirective }).Distinct().OrderBy(u => u.Name.ToString(), StringComparer.Ordinal);
            }

            var orderedUsings = systemUsings.Concat(otherUsings).ToList();
            var newUsingIndex = orderedUsings.FindIndex(ou => ou.ToFullString().Equals(newUsingDirective.ToFullString()));
            if (newUsingIndex == 0)
            {
                var usingNodeAfterNewUsing = orderedUsings[newUsingIndex + 1];
                root = root.InsertNodesBefore(usingNodeAfterNewUsing, new[] { newUsingDirective });
            }
            else
            {
                var usingNodeBeforeNewUsing = orderedUsings[newUsingIndex - 1];
                root = root.InsertNodesAfter(usingNodeBeforeNewUsing, new[] { newUsingDirective });
            }

            return Task.FromResult(document.WithSyntaxRoot(root));
        }
    }
}
