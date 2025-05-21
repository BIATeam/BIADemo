// BIADemo only
// <copyright file="IDocumentAnalysisRepository.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Bia.RepoContract.DocumentAnalysis
{
    using System.Collections.Generic;
    using System.IO;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum.DocumentAnalysis;
    using TheBIADevCompany.BIADemo.Domain.DocumentAnalysis.Models;

    /// <summary>
    /// Public interface for all document analysis repositories.
    /// </summary>
    public interface IDocumentAnalysisRepository
    {
        /// <summary>
        /// Document type handled by the repository.
        /// </summary>
        DocumentType DocumentType { get; }

        /// <summary>
        /// Analyze the document's pages content.
        /// </summary>
        /// <param name="stream">Document content.</param>
        /// <returns>Collection of <see cref="DocumentPage"/>.</returns>
        List<DocumentPage> GetPagesContent(Stream stream);
    }
}
