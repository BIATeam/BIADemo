namespace TheBIADevCompany.BIADemo.Domain.Utilities.DocumentAnalysis
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum.DocumentAnalysis;

    public class DocumentLine
    {
        public string Content { get; }
        public TextOrientation Orientation { get; }
        public List<DocumentWord> Words { get; }

        public DocumentLine(TextOrientation orientation, IEnumerable<DocumentWord> words)
        {
            words = words.Where(w => w.Orientation == orientation);

            Orientation = orientation;
            Content = string.Concat(words.Select(w => w.Text));
            Words = new List<DocumentWord>(words);
        }
    }
}
