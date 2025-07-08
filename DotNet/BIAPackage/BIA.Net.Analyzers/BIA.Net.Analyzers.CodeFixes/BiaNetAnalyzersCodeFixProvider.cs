// <copyright file="BiaNetAnalyzersCodeFixProvider.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Analyzer
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Composition;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using BIA.Net.Analyzer.CodeFixes;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CodeFixes;

    /// <summary>
    /// Code fix provider for BIA Net framework.
    /// </summary>
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(BiaNetAnalyzersCodeFixProvider))]
    [Shared]
    public sealed class BiaNetAnalyzersCodeFixProvider : CodeFixProvider
    {
        private readonly List<ICodeFix> codeFixes;

        /// <summary>
        /// Initializes a new instance of the <see cref="BiaNetAnalyzersCodeFixProvider"/> class.
        /// </summary>
        public BiaNetAnalyzersCodeFixProvider()
        {
            this.codeFixes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(type => typeof(ICodeFix).IsAssignableFrom(type) && !type.IsAbstract && type.IsClass && type.IsSealed)
                .Select(type => (ICodeFix)Activator.CreateInstance(type))
                .ToList();
        }

        /// <inheritdoc/>
        public sealed override ImmutableArray<string> FixableDiagnosticIds => this.codeFixes.Select(d => d.DiagnosticId).ToImmutableArray();

        /// <inheritdoc/>
        public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        /// <inheritdoc/>
        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var contextDiagnosticIds = context.Diagnostics.Select(d => d.Id);
            await Task.WhenAll(this.codeFixes.Where(cf => contextDiagnosticIds.Contains(cf.DiagnosticId)).Select(cf => cf.Register(context)));
        }
    }
}
