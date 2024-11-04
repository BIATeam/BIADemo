// <copyright file="PresentationLayerUsingDomainLayerDiagnostic.cs" company="BIA">
// Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Analyzers.Diagnostics
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;

    /// <summary>
    /// Diagnostic to detect using of Domain layer into Presentation layer.
    /// </summary>
    internal sealed class PresentationLayerUsingDomainLayerDiagnostic : DiagnosticBase
    {
        private readonly List<string> presentationLayerNamespaces = new List<string>
        {
            "BIA.Net.Core.Presentation",
        };

        private readonly List<string> domainLayerNamespaces = new List<string>
        {
            "BIA.Net.Core.Domain",
        };

        /// <inheritdoc/>
        protected override string DiagnosticId => "BIA001";

        /// <inheritdoc/>
        protected override string Category => "Architecture";

        /// <inheritdoc/>
        protected override string Title => "Forbidden reference to Domain layer in Presentation layer";

        /// <inheritdoc/>
        protected override string MessageFormat => "Presentation layer should not reference Domain layer: '{0}'";

        /// <inheritdoc/>
        protected override string Description => "Ensures that the Presentation layer does not reference the Domain layer to maintain architectural boundaries.";

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

                var projectPresentationNamespace = $"{rootNamespace}.Presentation";
                this.presentationLayerNamespaces.Add(projectPresentationNamespace);

                var projectDomainNamespace = $"{rootNamespace}.Domain";
                this.domainLayerNamespaces.Add(projectDomainNamespace);

                var domainDtoLayerNamespaces = this.domainLayerNamespaces.Select(x => $"{x}.Dto").ToList();

                this.RegisterForbiddenNamespacesDiagnostics(compilationContext, this.presentationLayerNamespaces, this.domainLayerNamespaces, domainDtoLayerNamespaces);
            });
        }
    }
}
