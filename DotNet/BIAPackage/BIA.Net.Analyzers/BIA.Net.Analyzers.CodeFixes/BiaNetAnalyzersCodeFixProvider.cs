namespace BIA.Net.Analyzers
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Composition;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using BIA.Net.Analyzer.CodeFixes;
    using BIA.Net.Analyzers.Diagnostics;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CodeActions;
    using Microsoft.CodeAnalysis.CodeFixes;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Rename;
    using Microsoft.CodeAnalysis.Text;

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(BiaNetAnalyzersCodeFixProvider)), Shared]
    public class BiaNetAnalyzersCodeFixProvider : CodeFixProvider
    {
        private readonly List<CodeFixBase> codeFixes = new List<CodeFixBase>
        {
            new PresentationLayerUsingDomainLayerCodeFix()
        };

        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get
            {
                var arrayBuilder = ImmutableArray.CreateBuilder<string>();
                arrayBuilder.AddRange(this.codeFixes.Select(d => d.DiagnosticId));
                return arrayBuilder.MoveToImmutable();
            }
        }

        public sealed override FixAllProvider GetFixAllProvider()
            => WellKnownFixAllProviders.BatchFixer;

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            await Task.WhenAll(this.codeFixes.Select(cf => cf.Register(context)));
        }
    }
}
