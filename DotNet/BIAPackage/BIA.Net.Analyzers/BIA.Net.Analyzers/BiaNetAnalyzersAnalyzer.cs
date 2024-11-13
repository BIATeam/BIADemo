// <copyright file="BiaNetAnalyzersAnalyzer.cs" company="BIA">
// Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Analyzers
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Reflection;
    using BIA.Net.Analyzers.Diagnostics;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;

    /// <summary>
    /// Analyzer for BIA Net framework.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class BiaNetAnalyzersAnalyzer : DiagnosticAnalyzer
    {
        private readonly List<DiagnosticBase> diagnostics;

        /// <summary>
        /// Initializes a new instance of the <see cref="BiaNetAnalyzersAnalyzer"/> class.
        /// </summary>
        public BiaNetAnalyzersAnalyzer()
        {
            this.diagnostics = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(type => typeof(DiagnosticBase).IsAssignableFrom(type) && !type.IsAbstract && type.IsClass && type.IsSealed)
                .Select(type => (DiagnosticBase)Activator.CreateInstance(type))
                .ToList();
        }

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
