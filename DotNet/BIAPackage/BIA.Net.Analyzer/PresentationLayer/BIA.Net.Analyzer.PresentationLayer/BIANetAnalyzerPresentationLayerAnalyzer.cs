namespace BIA.Net.Analyzer.PresentationLayer
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
    public class BIANetAnalyzerPresentationLayerAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "BIA001";

        // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
        // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Localizing%20Analyzers.md for more on localization
        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Architecture";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);
        private readonly List<string> forbidenNamespaces = new List<string>
        {
            "BIA.Net.Core.Domain",
        };

        private readonly string domainDtoNamespacePart = "Dto";
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

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
