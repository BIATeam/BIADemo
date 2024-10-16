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
