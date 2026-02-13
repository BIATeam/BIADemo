// <copyright file="CrosscuttingCommonRefersToBusinessLayersDiagnostic.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Analyzers.Diagnostics
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;

    /// <summary>
    /// Diagnostic to detect using of business layers into Crosscutting.Common.
    /// </summary>
    internal sealed class CrosscuttingCommonRefersToBusinessLayersDiagnostic : DiagnosticBase
    {
        private readonly List<string> crosscuttingCommonNamespaces = new List<string>
        {
            "BIA.Net.Core.Common",
        };

        private readonly List<string> forbiddenNamespaces = new List<string>
        {
            "BIA.Net.Core.Domain",
            "BIA.Net.Core.Application",
            "BIA.Net.Core.Infrastructure",
            "BIA.Net.Core.Presentation",
        };

        /// <inheritdoc/>
        protected override string DiagnosticId => "BIA011";

        /// <inheritdoc/>
        protected override string Category => "Architecture";

        /// <inheritdoc/>
        protected override string Title => "Forbidden reference to business layers in Crosscutting.Common";

        /// <inheritdoc/>
        protected override string MessageFormat => "Crosscutting.Common should not reference business layers: '{0}'";

        /// <inheritdoc/>
        protected override string Description => "Ensures that Crosscutting.Common does not reference any business layer to maintain architectural boundaries.";

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

                var projectCrosscuttingCommonNamespace = $"{rootNamespace}.Crosscutting.Common";
                this.crosscuttingCommonNamespaces.Add(projectCrosscuttingCommonNamespace);

                var projectDomainNamespace = $"{rootNamespace}.Domain";
                var projectApplicationNamespace = $"{rootNamespace}.Application";
                var projectInfrastructureNamespace = $"{rootNamespace}.Infrastructure";
                var projectPresentationNamespace = $"{rootNamespace}.Presentation";

                this.forbiddenNamespaces.Add(projectDomainNamespace);
                this.forbiddenNamespaces.Add(projectApplicationNamespace);
                this.forbiddenNamespaces.Add(projectInfrastructureNamespace);
                this.forbiddenNamespaces.Add(projectPresentationNamespace);

                this.RegisterForbiddenNamespacesDiagnostics(compilationContext, this.crosscuttingCommonNamespaces, this.forbiddenNamespaces, System.Linq.Enumerable.Empty<string>());
            });
        }
    }
}
