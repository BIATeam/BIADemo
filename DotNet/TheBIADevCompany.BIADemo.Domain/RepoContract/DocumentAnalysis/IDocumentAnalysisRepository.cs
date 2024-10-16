namespace TheBIADevCompany.BIADemo.Domain.RepoContract.DocumentAnalysis
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum.DocumentAnalysis;
    using TheBIADevCompany.BIADemo.Domain.Utilities.DocumentAnalysis;

    public interface IDocumentAnalysisRepository
    {
        DocumentType DocumentType { get; }
        List<DocumentPage> GetPagesContent(Stream stream);
    }
}
