// <copyright file="InfrastructureLayerRefersToApplicationLayerDiagnostic.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Analyzers.Diagnostics
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;

    /// <summary>
    /// Diagnostic to detect using of Application layer into Infrastructure layer.
    /// </summary>
    internal sealed class InfrastructureLayerRefersToApplicationLayerDiagnostic : DiagnosticBase
    {
        private readonly List<string> infrastructureLayerNamespaces = new List<string>
        {
            "BIA.Net.Core.Infrastructure",
        };

        private readonly List<string> applicationLayerNamespaces = new List<string>
        {
            "BIA.Net.Core.Application",
        };

        /// <inheritdoc/>
        protected override string DiagnosticId => "BIA008";

        /// <inheritdoc/>
        protected override string Category => "Architecture";

        /// <inheritdoc/>
        protected override string Title => "Forbidden reference to Application layer in Infrastructure layer";

        /// <inheritdoc/>
        protected override string MessageFormat => "Infrastructure layer should not reference Application layer: '{0}'";

        /// <inheritdoc/>
        protected override string Description => "Ensures that the Infrastructure layer does not reference the Application layer to maintain architectural boundaries.";

        /// <inheritdoc/>
        protected override DiagnosticSeverity Severity => DiagnosticSeverity.Warning;

        /// <inheritdoc/>
        protected override bool IsEnabledByDefault => true;

        /// <inheritdoc/>
        public override void Register(AnalysisContext analysisContext)
        {
            analysisContext.RegisterCompilationStartAction(compilationContext =>
            {
                var assemblyName = compilationContext.Compilation.Assembly.Name;
                var rootNamespace = string.Join(".", assemblyName.Split('.').Take(2));

                var projectInfrastructureNamespace = $"{rootNamespace}.Infrastructure";
                this.infrastructureLayerNamespaces.Add(projectInfrastructureNamespace);

                var projectApplicationNamespace = $"{rootNamespace}.Application";
                this.applicationLayerNamespaces.Add(projectApplicationNamespace);

                this.RegisterForbiddenNamespacesDiagnostics(compilationContext, this.infrastructureLayerNamespaces, this.applicationLayerNamespaces, System.Linq.Enumerable.Empty<string>());
            });
        }
    }
}
