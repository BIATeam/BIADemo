// <copyright file="InfrastructureServiceRefersToInfrastructureDataDiagnostic.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Analyzers.Diagnostics
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;

    /// <summary>
    /// Diagnostic to detect using of Infrastructure.Data sublayer into Infrastructure.Service sublayer.
    /// </summary>
    internal sealed class InfrastructureServiceRefersToInfrastructureDataDiagnostic : DiagnosticBase
    {
        private readonly List<string> infrastructureServiceNamespaces = new List<string>
        {
            "BIA.Net.Core.Infrastructure.Service",
        };

        private readonly List<string> infrastructureDataNamespaces = new List<string>
        {
            "BIA.Net.Core.Infrastructure.Data",
        };

        /// <inheritdoc/>
        protected override string DiagnosticId => "BIA010";

        /// <inheritdoc/>
        protected override string Category => "Architecture";

        /// <inheritdoc/>
        protected override string Title => "Forbidden reference to Infrastructure.Data in Infrastructure.Service";

        /// <inheritdoc/>
        protected override string MessageFormat => "Infrastructure.Service should not reference Infrastructure.Data: '{0}'";

        /// <inheritdoc/>
        protected override string Description => "Ensures that Infrastructure.Service does not reference Infrastructure.Data to maintain sublayer boundaries.";

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

                var projectInfrastructureServiceNamespace = $"{rootNamespace}.Infrastructure.Service";
                this.infrastructureServiceNamespaces.Add(projectInfrastructureServiceNamespace);

                var projectInfrastructureDataNamespace = $"{rootNamespace}.Infrastructure.Data";
                this.infrastructureDataNamespaces.Add(projectInfrastructureDataNamespace);

                this.RegisterForbiddenNamespacesDiagnostics(compilationContext, this.infrastructureServiceNamespaces, this.infrastructureDataNamespaces, System.Linq.Enumerable.Empty<string>());
            });
        }
    }
}
