namespace TheBIADevCompany.BIADemo.Application.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum.DocumentAnalysis;
    using TheBIADevCompany.BIADemo.Domain.RepoContract.DocumentAnalysis;
    using TheBIADevCompany.BIADemo.Domain.Utilities.DocumentAnalysis;

    public class DocumentAnalysisService : IDocumentAnalysisService
    {
        private readonly IDocumentAnalysisRepositoryFactory documentAnalysisRepositoryFactory;

        public DocumentAnalysisService(IDocumentAnalysisRepositoryFactory documentAnalysisRepositoryFactory)
        {
            this.documentAnalysisRepositoryFactory = documentAnalysisRepositoryFactory;
        }

        public DocumentContent GetContent(string fileName, string fileContentType, Stream fileStream)
        {
            var documentType = this.GetDocumentType(fileContentType);
            var documentAnalysisRepository = this.documentAnalysisRepositoryFactory.GetDocumentAnalysisRepository(documentType);
            var documentPages = documentAnalysisRepository.GetPagesContent(fileStream);

            return new DocumentContent
            {
                Name = fileName,
                Type = documentType,
                Pages = documentPages,
            };
        }

        private DocumentType GetDocumentType(string fileContentType)
        {
            return fileContentType switch
            {
                "application/pdf" => DocumentType.Pdf,
                _ => throw new NotImplementedException($"Document analysis for content type {fileContentType} is not implemented yet.")
            };
        }
    }
}
