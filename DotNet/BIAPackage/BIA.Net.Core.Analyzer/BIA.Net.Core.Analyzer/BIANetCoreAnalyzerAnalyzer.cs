namespace BIA.Net.Core.Analyzer
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Threading;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Diagnostics;

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class BIANetCoreAnalyzerAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "BIA001";
        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            "Forbidden reference to Domain layer",
            "Presentation layer should not reference Domain layer: '{0}'",
            "Architecture",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: "Ensures that the Presentation layer does not reference the Domain layer to maintain architectural boundaries.");

        private readonly List<string> forbidenNamespaces = new List<string>
        {
            "BIA.Net.Core.Domain",
        };

        private readonly string domainDtoNamespacePart = "Dto";

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            // Register a syntax node action to analyze using directives in the Presentation Layer
            context.RegisterCompilationStartAction(compilationContext =>
            {
                // Determine the root namespace dynamically from the assembly name
                var assemblyName = compilationContext.Compilation.Assembly.Name;
                var rootNamespace = string.Join(".", assemblyName.Split('.').Take(2)); // Assume root namespace is the first segment (e.g., "Company")
                var projectDomainNamespace = $"{rootNamespace}.Domain"; // Define Domain Layer namespace
                this.forbidenNamespaces.Add(projectDomainNamespace);

                compilationContext.RegisterSyntaxNodeAction(
                    syntaxNodeContext =>
                {
                    var usingDirective = (UsingDirectiveSyntax)syntaxNodeContext.Node;
                    var usingDirectiveName = usingDirective.Name.ToString();

                    // Check if the using directive references the forbidden Domain namespace
                    if (this.forbidenNamespaces.Exists(ns => usingDirectiveName.StartsWith(ns) && !usingDirectiveName.StartsWith(ns + $".{this.domainDtoNamespacePart}")))
                    {
                        // Verify that the containing assembly is a Presentation layer project
                        var containingAssembly = syntaxNodeContext.Compilation.Assembly;
                        if (containingAssembly.Name.Contains("Presentation"))
                        {
                            // Report diagnostic error if a forbidden namespace is used
                            var diagnostic = Diagnostic.Create(Rule, usingDirective.GetLocation(), usingDirective.Name.ToString());
                            syntaxNodeContext.ReportDiagnostic(diagnostic);
                        }
                    }
                }, SyntaxKind.UsingDirective);
            });
        }
    }
}
