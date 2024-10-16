namespace TheBIADevCompany.BIADemo.Infrastructure.Service.Repositories.DocumentAnalysis
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum.DocumentAnalysis;
    using TheBIADevCompany.BIADemo.Domain.RepoContract.DocumentAnalysis;

    public class DocumentAnalysisRepositoryFactory : IDocumentAnalysisRepositoryFactory
    {
        private readonly List<IDocumentAnalysisRepository> documentAnalysisRepositories = [];

        public DocumentAnalysisRepositoryFactory(IServiceProvider serviceProvider)
        {
            this.documentAnalysisRepositories.Add(serviceProvider.GetRequiredService<PdfAnalysisRepository>());
        }

        public IDocumentAnalysisRepository GetDocumentAnalysisRepository(DocumentType documentType)
        {
            return this.documentAnalysisRepositories.FirstOrDefault(x => x.DocumentType == documentType)
                ?? throw new NotImplementedException($"Document analysis repository for document type {documentType} is not implemented yet.");
        }
    }
}
