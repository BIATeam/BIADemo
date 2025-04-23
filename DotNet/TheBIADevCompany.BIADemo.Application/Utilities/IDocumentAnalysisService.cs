// BIADemo only
// <copyright file="IDocumentAnalysisService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Utilities
{
    using System.IO;
    using TheBIADevCompany.BIADemo.Domain.DocumentAnalysis.Models;

    /// <summary>
    /// Public interface for document analysis services.
    /// </summary>
    public interface IDocumentAnalysisService
    {
        /// <summary>
        /// Analyse and retrieve document content.
        /// </summary>
        /// <param name="fileName">The document name.</param>
        /// <param name="fileContentType">The document content type.</param>
        /// <param name="fileStream">The document conntat as <see cref="Stream"/>.</param>
        /// <returns>The document content as <see cref="DocumentContent"/>.</returns>
        DocumentContent GetContent(string fileName, string fileContentType, Stream fileStream);
    }
}
