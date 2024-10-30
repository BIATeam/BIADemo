// <copyright file="ICodeFix.cs" company="BIA">
// Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Analyzer.CodeFixes
{
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.CodeFixes;

    /// <summary>
    /// Interface for all code fixes of BIA Net framework analyzer.
    /// </summary>
    internal interface ICodeFix
    {
        /// <summary>
        /// The diagnostic ID associated to code fix.
        /// </summary>
        string DiagnosticId { get; }

        /// <summary>
        /// The code fix title.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Register the code fix into <see cref="CodeFixContext"/>.
        /// </summary>
        /// <param name="codeFixContext">The code fix context.</param>
        /// <returns><see cref="Task"/>.</returns>
        Task Register(CodeFixContext codeFixContext);
    }
}
