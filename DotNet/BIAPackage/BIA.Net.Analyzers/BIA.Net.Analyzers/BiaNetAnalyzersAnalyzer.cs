// <copyright file="BiaNetAnalyzersAnalyzer.cs" company="BIA">
// Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Analyzers
{
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using BIA.Net.Analyzers.Diagnostics;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;

    /// <summary>
    /// Analyzer for BIA Net framework.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class BiaNetAnalyzersAnalyzer : DiagnosticAnalyzer
    {
        private readonly List<DiagnosticBase> diagnostics = new List<DiagnosticBase>
        {
            new PresentationLayerUsingDomainLayerDiagnostic(),
        };

        /// <inheritdoc/>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => this.diagnostics.Select(d => d.Rule).ToImmutableArray();

        /// <inheritdoc/>
        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            this.diagnostics.ForEach(d => d.Register(context));
        }
    }
}
