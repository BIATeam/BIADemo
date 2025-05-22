// BIADemo only
// <copyright file="IDocumentAnalysisRepositoryFactory.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Bia.RepoContract.DocumentAnalysis
{
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum.DocumentAnalysis;

    /// <summary>
    /// Public interface for document analysis repository factories.
    /// </summary>
    public interface IDocumentAnalysisRepositoryFactory
    {
        /// <summary>
        /// Retrieve the corresponding document analysis repository by document type.
        /// </summary>
        /// <param name="documentType">Document type handled by document analysis repository.</param>
        /// <returns>Implementation of corresponding <see cref="IDocumentAnalysisRepository"/>.</returns>
        IDocumentAnalysisRepository GetDocumentAnalysisRepository(DocumentType documentType);
    }
}
