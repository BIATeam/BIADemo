namespace TheBIADevCompany.BIADemo.Domain.Utilities.DocumentAnalysis
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class DocumentPage
    {
        public int PageNumber { get; set; }
        public List<DocumentWord> Words { get; } = [];
    }
}
