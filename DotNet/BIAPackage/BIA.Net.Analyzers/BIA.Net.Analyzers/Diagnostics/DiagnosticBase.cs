// <copyright file="DiagnosticBase.cs" company="BIA">
//  Copyright (c) BIA.Net. All rights reserved.
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
    /// Base class for all diagnostics of BIA Net framework analyzer.
    /// </summary>
    internal abstract class DiagnosticBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DiagnosticBase"/> class.
        /// </summary>
        protected DiagnosticBase()
        {
            this.Rule = new DiagnosticDescriptor(
                this.DiagnosticId,
                this.Title,
                this.MessageFormat,
                this.Category,
                this.Severity,
                this.IsEnabledByDefault,
                this.Description);
        }

        /// <summary>
        /// Diagnostic rule descriptor.
        /// </summary>
        public DiagnosticDescriptor Rule { get; private set; }

        /// <summary>
        /// Diagnostic ID.
        /// </summary>
        protected abstract string DiagnosticId { get; }

        /// <summary>
        /// Diagnostic category.
        /// </summary>
        protected abstract string Category { get; }

        /// <summary>
        /// Diagnostic title.
        /// </summary>
        protected abstract string Title { get; }

        /// <summary>
        /// Diagnostic message formated.
        /// </summary>
        protected abstract string MessageFormat { get; }

        /// <summary>
        /// Diagnostic description.
        /// </summary>
        protected abstract string Description { get; }

        /// <summary>
        /// Diagnostic severity.
        /// </summary>
        protected abstract DiagnosticSeverity Severity { get; }

        /// <summary>
        /// Indicates weither the diagnostic is enabled by default or not.
        /// </summary>
        protected abstract bool IsEnabledByDefault { get; }

        /// <summary>
        /// Register the diagnostic into <see cref="AnalysisContext"/>.
        /// </summary>
        /// <param name="analysisContext">The analysis context.</param>
        public abstract void Register(AnalysisContext analysisContext);

        /// <summary>
        /// Register diagnostics related to forbidden namespaces elements called from specific namespaces.
        /// </summary>
        /// <param name="compilationContext">The <see cref="CompilationStartAnalysisContext"/>.</param>
        /// <param name="fromNamespaces">List of namespaces where references to <paramref name="forbiddenNamespaces"/> must be catch.</param>
        /// <param name="forbiddenNamespaces">List of forbidden namespaces to use in <paramref name="fromNamespaces"/>.</param>
        /// <param name="exceptedNamespaces">List of namespaces that can contains parts of <paramref name="forbiddenNamespaces"/> but allowed in <paramref name="fromNamespaces"/>.</param>
        protected void RegisterForbiddenNamespacesDiagnostics(CompilationStartAnalysisContext compilationContext, IEnumerable<string> fromNamespaces, IEnumerable<string> forbiddenNamespaces, IEnumerable<string> exceptedNamespaces)
        {
            compilationContext.RegisterSyntaxNodeAction(
                syntaxNodeContext =>
                {
                    if (!fromNamespaces.Any(x => syntaxNodeContext.Compilation.Assembly.Name.StartsWith(x)))
                    {
                        return;
                    }

                    switch (syntaxNodeContext.Node)
                    {
                        case UsingDirectiveSyntax usingDirective:
                            var usingDirectiveName = usingDirective.Name.ToString();
                            if (IsForbiddenNamespace(usingDirectiveName, forbiddenNamespaces, exceptedNamespaces))
                            {
                                this.CreateDiagnostic(syntaxNodeContext, usingDirective.GetLocation(), usingDirectiveName);
                            }

                            break;

                        case VariableDeclarationSyntax variableDeclaration:
                            var variableType = syntaxNodeContext.SemanticModel.GetTypeInfo(variableDeclaration.Type).Type;
                            if (variableType != null)
                            {
                                var typeNamespace = variableType.ContainingNamespace?.ToDisplayString();
                                if (IsForbiddenNamespace(typeNamespace, forbiddenNamespaces, exceptedNamespaces))
                                {
                                    this.CreateDiagnostic(syntaxNodeContext, variableDeclaration.GetLocation(), typeNamespace);
                                }
                            }

                            break;

                        case TypeOfExpressionSyntax typeOfExpression:
                            var typeSyntax = typeOfExpression.Type;
                            if (syntaxNodeContext.SemanticModel.GetSymbolInfo(typeSyntax).Symbol is INamedTypeSymbol typeSymbol)
                            {
                                var typeNamespace = typeSymbol.ContainingNamespace?.ToDisplayString();
                                if (IsForbiddenNamespace(typeNamespace, forbiddenNamespaces, exceptedNamespaces))
                                {
                                    this.CreateDiagnostic(syntaxNodeContext, typeOfExpression.GetLocation(), typeNamespace);
                                }
                            }

                            break;

                        case MethodDeclarationSyntax methodDeclaration:
                            var returnTypeSymbol = syntaxNodeContext.SemanticModel.GetTypeInfo(methodDeclaration.ReturnType).Type;
                            if (returnTypeSymbol != null)
                            {
                                var returnTypeNamespace = returnTypeSymbol.ContainingNamespace?.ToDisplayString();
                                if (IsForbiddenNamespace(returnTypeNamespace, forbiddenNamespaces, exceptedNamespaces))
                                {
                                    this.CreateDiagnostic(syntaxNodeContext, methodDeclaration.ReturnType.GetLocation(), returnTypeNamespace);
                                }
                            }

                            foreach (var parameterType in methodDeclaration.ParameterList.Parameters.Select(p => p.Type))
                            {
                                var parameterTypeSymbol = syntaxNodeContext.SemanticModel.GetTypeInfo(parameterType).Type;
                                if (parameterTypeSymbol != null)
                                {
                                    var parameterTypeNamespace = parameterTypeSymbol.ContainingNamespace?.ToDisplayString();
                                    if (IsForbiddenNamespace(parameterTypeNamespace, forbiddenNamespaces, exceptedNamespaces))
                                    {
                                        this.CreateDiagnostic(syntaxNodeContext, parameterType.GetLocation(), parameterTypeNamespace);
                                    }
                                }
                            }

                            break;

                        case ConstructorDeclarationSyntax constructorDeclaration:
                            foreach (var parameterType in constructorDeclaration.ParameterList.Parameters.Select(p => p.Type))
                            {
                                var parameterTypeSymbol = syntaxNodeContext.SemanticModel.GetTypeInfo(parameterType).Type;
                                if (parameterTypeSymbol != null)
                                {
                                    var parameterTypeNamespace = parameterTypeSymbol.ContainingNamespace?.ToDisplayString();
                                    if (IsForbiddenNamespace(parameterTypeNamespace, forbiddenNamespaces, exceptedNamespaces))
                                    {
                                        this.CreateDiagnostic(syntaxNodeContext, parameterType.GetLocation(), parameterTypeNamespace);
                                    }
                                }
                            }

                            break;
                    }
                },
                SyntaxKind.UsingDirective,
                SyntaxKind.VariableDeclaration,
                SyntaxKind.TypeOfExpression,
                SyntaxKind.MethodDeclaration,
                SyntaxKind.ConstructorDeclaration);
        }

        /// <summary>
        /// Create a diagnostic based on current <see cref="Rule"/> and report it into <paramref name="syntaxNodeContext"/>.
        /// </summary>
        /// <param name="syntaxNodeContext">The <see cref="SyntaxNodeAnalysisContext"/>.</param>
        /// <param name="location">The <see cref="Location"/> of the diagnostic.</param>
        /// <param name="messageArgs">The message arguments for the diagnostic.</param>
        protected void CreateDiagnostic(SyntaxNodeAnalysisContext syntaxNodeContext, Location location, params object[] messageArgs)
        {
            var diagnostic = Diagnostic.Create(this.Rule, location, messageArgs);
            syntaxNodeContext.ReportDiagnostic(diagnostic);
        }

        private static bool IsForbiddenNamespace(string ns, IEnumerable<string> forbiddenNamespaces, IEnumerable<string> exceptedNamespaces)
        {
            return !string.IsNullOrWhiteSpace(ns) && forbiddenNamespaces.Any(x => ns.StartsWith(x) && !exceptedNamespaces.Any(y => ns.StartsWith(y)));
        }
    }
}
