// BIADemo only
// <copyright file="DocumentAnalysisRepositoryFactory.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Service.Repositories.DocumentAnalysis
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.DependencyInjection;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum.DocumentAnalysis;
    using TheBIADevCompany.BIADemo.Domain.Bia.RepoContract.DocumentAnalysis;

    /// <summary>
    /// The <see cref="IDocumentAnalysisRepository"/> factory.
    /// </summary>
    public class DocumentAnalysisRepositoryFactory : IDocumentAnalysisRepositoryFactory
    {
        private readonly List<IDocumentAnalysisRepository> documentAnalysisRepositories = new ();

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentAnalysisRepositoryFactory"/> class.
        /// </summary>
        /// <param name="serviceProvider">Services provider.</param>
        public DocumentAnalysisRepositoryFactory(IServiceProvider serviceProvider)
        {
            this.documentAnalysisRepositories.Add(serviceProvider.GetRequiredService<PdfAnalysisRepository>());
        }

        /// <inheritdoc/>
        public IDocumentAnalysisRepository GetDocumentAnalysisRepository(DocumentType documentType)
        {
            return this.documentAnalysisRepositories.Find(x => x.DocumentType == documentType)
                ?? throw new NotImplementedException($"Document analysis repository for document type {documentType} is not implemented yet.");
        }
    }
}
