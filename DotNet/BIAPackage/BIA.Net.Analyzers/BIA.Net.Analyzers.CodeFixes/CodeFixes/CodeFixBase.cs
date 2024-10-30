namespace BIA.Net.Analyzer.CodeFixes
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.CodeFixes;

    internal abstract class CodeFixBase
    {
        public abstract string DiagnosticId { get; }
        public abstract string Title { get; }
        public abstract Task Register(CodeFixContext codeFixContext);
    }
}
