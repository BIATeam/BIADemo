// <copyright file="ApplicationLayerRefersToPresentationLayerDiagnostic.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Analyzers.Diagnostics
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;

    /// <summary>
    /// Diagnostic to detect using of Presentation layer into Application layer.
    /// </summary>
    internal sealed class ApplicationLayerRefersToPresentationLayerDiagnostic : DiagnosticBase
    {
        private readonly List<string> applicationLayerNamespaces = new List<string>
        {
            "BIA.Net.Core.Application",
        };

        private readonly List<string> presentationLayerNamespaces = new List<string>
        {
            "BIA.Net.Core.Presentation",
        };

        /// <inheritdoc/>
        protected override string DiagnosticId => "BIA004";

        /// <inheritdoc/>
        protected override string Category => "Architecture";

        /// <inheritdoc/>
        protected override string Title => "Forbidden reference to Presentation layer in Application layer";

        /// <inheritdoc/>
        protected override string MessageFormat => "Application layer should not reference Presentation layer: '{0}'";

        /// <inheritdoc/>
        protected override string Description => "Ensures that the Application layer does not reference the Presentation layer to maintain architectural boundaries.";

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

                var projectApplicationNamespace = $"{rootNamespace}.Application";
                this.applicationLayerNamespaces.Add(projectApplicationNamespace);

                var projectPresentationNamespace = $"{rootNamespace}.Presentation";
                this.presentationLayerNamespaces.Add(projectPresentationNamespace);

                this.RegisterForbiddenNamespacesDiagnostics(compilationContext, this.applicationLayerNamespaces, this.presentationLayerNamespaces, System.Linq.Enumerable.Empty<string>());
            });
        }
    }
}
