namespace TheBIADevCompany.BIADemo.Domain.Utilities.DocumentAnalysis
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum.DocumentAnalysis;

    public class DocumentContent
    {
        public string Name { get; set; }
        public DocumentType Type { get; set; }
        public List<DocumentPage> Pages { get; set; } = [];
    }
}
