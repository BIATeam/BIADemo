namespace TheBIADevCompany.BIADemo.Infrastructure.Service.Repositories.DocumentAnalysis
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

    public class PdfAnalysisRepository : IDocumentAnalysisRepository
    {
        public DocumentType DocumentType => DocumentType.Pdf;

        public List<DocumentPage> GetPagesContent(Stream stream)
        {
            return [];
        }
    }
}
