namespace BIA.Net.Analyzers.Diagnostics
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;

    internal abstract class DiagnosticBase
    {
        public DiagnosticDescriptor Rule { get; private set; }
        protected abstract string DiagnosticId { get; }
        protected abstract string Category { get; }
        protected abstract string Title { get; }
        protected abstract string MessageFormat { get; }
        protected abstract string Description { get; }
        protected abstract DiagnosticSeverity Severity { get; }
        protected abstract bool IsEnabledByDefault { get; }
        public abstract void Register(AnalysisContext analysisContext);

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
    }
}
