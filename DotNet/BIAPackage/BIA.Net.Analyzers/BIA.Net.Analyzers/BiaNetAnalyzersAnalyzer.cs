namespace BIA.Net.Analyzers
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Threading;
    using BIA.Net.Analyzers.Diagnostics;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Diagnostics;

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class BiaNetAnalyzersAnalyzer : DiagnosticAnalyzer
    {
        private readonly List<DiagnosticBase> diagnostics = new List<DiagnosticBase>
        {
            new PresentationLayerUsingDomainLayerDiagnostic()
        };

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get
            {
                var arrayBuilder = ImmutableArray.CreateBuilder<DiagnosticDescriptor>();
                arrayBuilder.AddRange(this.diagnostics.Select(d => d.Rule));
                return arrayBuilder.MoveToImmutable();
            }
        }

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            this.diagnostics.ForEach(d => d.Register(context));
        }
    }
}
