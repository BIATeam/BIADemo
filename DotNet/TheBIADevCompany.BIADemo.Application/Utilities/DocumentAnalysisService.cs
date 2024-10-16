// BIADemo only
// <copyright file="DocumentAnalysisService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Utilities
{
    using System;
    using System.IO;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum.DocumentAnalysis;
    using TheBIADevCompany.BIADemo.Domain.RepoContract.DocumentAnalysis;
    using TheBIADevCompany.BIADemo.Domain.Utilities.DocumentAnalysis;

    /// <summary>
    /// Service for document analysis.
    /// </summary>
    public class DocumentAnalysisService : IDocumentAnalysisService
    {
        private readonly IDocumentAnalysisRepositoryFactory documentAnalysisRepositoryFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentAnalysisService"/> class.
        /// </summary>
        /// <param name="documentAnalysisRepositoryFactory">The document analysys repositories factory <see cref="IDocumentAnalysisRepositoryFactory"/>.</param>
        public DocumentAnalysisService(IDocumentAnalysisRepositoryFactory documentAnalysisRepositoryFactory)
        {
            this.documentAnalysisRepositoryFactory = documentAnalysisRepositoryFactory;
        }

        /// <inheritdoc/>
        public DocumentContent GetContent(string fileName, string fileContentType, Stream fileStream)
        {
            // Retrieve the document type from his content type
            var documentType = GetDocumentType(fileContentType);

            // Get the corresponding implementation of IDocumentAnalysisRepository for the document type
            var documentAnalysisRepository = this.documentAnalysisRepositoryFactory.GetDocumentAnalysisRepository(documentType);

            // Get document pages from repository
            var documentPages = documentAnalysisRepository.GetPagesContent(fileStream);

            return new DocumentContent
            {
                Name = fileName,
                Type = documentType,
                Pages = documentPages,
            };
        }

        private static DocumentType GetDocumentType(string fileContentType)
        {
            return fileContentType switch
            {
                "application/pdf" => DocumentType.Pdf,
                _ => throw new NotImplementedException($"Document analysis for content type {fileContentType} is not implemented yet.")
            };
        }
    }
}
