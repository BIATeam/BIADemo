// <copyright file="DiagnosticBase.cs" company="BIA">
//  Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Analyzers.Diagnostics
{
    using Microsoft.CodeAnalysis;
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
    }
}
