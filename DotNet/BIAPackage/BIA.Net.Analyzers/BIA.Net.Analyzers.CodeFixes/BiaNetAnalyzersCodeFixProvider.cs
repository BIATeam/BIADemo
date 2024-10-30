// <copyright file="BiaNetAnalyzersCodeFixProvider.cs" company="BIA">
// Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Analyzers
{
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Composition;
    using System.Linq;
    using System.Threading.Tasks;
    using BIA.Net.Analyzer.CodeFixes;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CodeFixes;

    /// <summary>
    /// Code fix provider for BIA Net framework.
    /// </summary>
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(BiaNetAnalyzersCodeFixProvider))]
    [Shared]
    public class BiaNetAnalyzersCodeFixProvider : CodeFixProvider
    {
        private readonly List<ICodeFixBase> codeFixes = new List<ICodeFixBase>
        {
            new PresentationLayerUsingDomainLayerCodeFix(),
        };

        /// <inheritdoc/>
        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get
            {
                var arrayBuilder = ImmutableArray.CreateBuilder<string>();
                arrayBuilder.AddRange(this.codeFixes.Select(d => d.DiagnosticId));
                return arrayBuilder.MoveToImmutable();
            }
        }

        /// <inheritdoc/>
        public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        /// <inheritdoc/>
        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            await Task.WhenAll(this.codeFixes.Select(cf => cf.Register(context)));
        }
    }
}
