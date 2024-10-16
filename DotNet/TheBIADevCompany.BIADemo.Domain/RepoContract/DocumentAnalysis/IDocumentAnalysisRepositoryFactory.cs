namespace TheBIADevCompany.BIADemo.Domain.RepoContract.DocumentAnalysis
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum.DocumentAnalysis;

    public interface IDocumentAnalysisRepositoryFactory
    {
        IDocumentAnalysisRepository GetDocumentAnalysisRepository(DocumentType documentType);
    }
}
