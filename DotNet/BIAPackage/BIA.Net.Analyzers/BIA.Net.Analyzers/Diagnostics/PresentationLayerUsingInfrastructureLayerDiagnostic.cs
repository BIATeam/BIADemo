// <copyright file="PresentationLayerUsingInfrastructureLayerDiagnostic.cs" company="BIA">
// Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Analyzers.Diagnostics
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Diagnostics;

    /// <summary>
    /// Diagnostic to detect using of Infrastructure layer into Presentation layer.
    /// </summary>
    internal sealed class PresentationLayerUsingInfrastructureLayerDiagnostic : DiagnosticBase
    {
        private readonly List<string> infrastructureLayerNamespaces = new List<string>
        {
            "BIA.Net.Core.Infrastructure",
        };

        /// <inheritdoc/>
        protected override string DiagnosticId => "BIA002";

        /// <inheritdoc/>
        protected override string Category => "Architecture";

        /// <inheritdoc/>
        protected override string Title => "Forbidden reference to Infrastructure layer in Presentation layer";

        /// <inheritdoc/>
        protected override string MessageFormat => "Presentation layer should not reference Infrastructure layer: '{0}'";

        /// <inheritdoc/>
        protected override string Description => "Ensures that the Presentation layer does not reference the Infrastructure layer to maintain architectural boundaries.";

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

                compilationContext.RegisterSyntaxNodeAction(
                    syntaxNodeContext =>
                    {
                        var usingDirective = (UsingDirectiveSyntax)syntaxNodeContext.Node;
                        var usingDirectiveName = usingDirective.Name.ToString();

                        if (this.infrastructureLayerNamespaces.Exists(ns => usingDirectiveName.StartsWith(ns)))
                        {
                            var containingAssembly = syntaxNodeContext.Compilation.Assembly;
                            if (containingAssembly.Name.Contains("Presentation"))
                            {
                                var diagnostic = Diagnostic.Create(this.Rule, usingDirective.GetLocation(), usingDirective.Name.ToString());
                                syntaxNodeContext.ReportDiagnostic(diagnostic);
                            }
                        }
                    }, SyntaxKind.UsingDirective);
            });
        }
    }
}
