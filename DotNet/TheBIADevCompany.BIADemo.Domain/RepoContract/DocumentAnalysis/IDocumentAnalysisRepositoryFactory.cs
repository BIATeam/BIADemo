namespace TheBIADevCompany.BIADemo.Domain.RepoContract.DocumentAnalysis
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
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
