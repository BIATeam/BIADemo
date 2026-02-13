// <copyright file="DomainLayerRefersToPresentationLayerDiagnostic.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Analyzers.Diagnostics
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;

    /// <summary>
    /// Diagnostic to detect using of Presentation layer into Domain layer.
    /// </summary>
    internal sealed class DomainLayerRefersToPresentationLayerDiagnostic : DiagnosticBase
    {
        private readonly List<string> domainLayerNamespaces = new List<string>
        {
            "BIA.Net.Core.Domain",
        };

        private readonly List<string> presentationLayerNamespaces = new List<string>
        {
            "BIA.Net.Core.Presentation",
        };

        /// <inheritdoc/>
        protected override string DiagnosticId => "BIA007";

        /// <inheritdoc/>
        protected override string Category => "Architecture";

        /// <inheritdoc/>
        protected override string Title => "Forbidden reference to Presentation layer in Domain layer";

        /// <inheritdoc/>
        protected override string MessageFormat => "Domain layer should not reference Presentation layer: '{0}'";

        /// <inheritdoc/>
        protected override string Description => "Ensures that the Domain layer does not reference the Presentation layer to maintain architectural boundaries.";

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

                var projectDomainNamespace = $"{rootNamespace}.Domain";
                this.domainLayerNamespaces.Add(projectDomainNamespace);

                var projectPresentationNamespace = $"{rootNamespace}.Presentation";
                this.presentationLayerNamespaces.Add(projectPresentationNamespace);

                var domainDtoLayerNamespaces = this.domainLayerNamespaces.Select(x => $"{x}.Dto").ToList();

                this.RegisterForbiddenNamespacesDiagnostics(compilationContext, this.domainLayerNamespaces, this.presentationLayerNamespaces, domainDtoLayerNamespaces);
            });
        }
    }
}
