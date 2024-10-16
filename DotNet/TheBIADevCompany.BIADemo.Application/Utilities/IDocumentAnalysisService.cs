namespace TheBIADevCompany.BIADemo.Application.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TheBIADevCompany.BIADemo.Domain.Utilities.DocumentAnalysis;

    public interface IDocumentAnalysisService
    {
        DocumentContent GetContent(string fileName, string fileContentType, Stream fileStream);
    }
}
