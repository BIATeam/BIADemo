namespace TheBIADevCompany.BIADemo.Domain.Utilities.DocumentAnalysis
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum.DocumentAnalysis;

    public class DocumentPage
    {
        public int Number { get; set; }
        public int LinesCount => this.Lines.Count;
        public int WordsCount => this.Words.Count();
        public List<DocumentLine> Lines { get; }
        public IEnumerable<DocumentWord> Words => this.Lines.SelectMany(l => l.Words);

        public DocumentPage(int number, IEnumerable<DocumentWord> words)
        {
            Number = number;
            Lines = GetLines(words);
        }

        public static List<DocumentLine> GetLines(IEnumerable<DocumentWord> words)
        {
            var lines = new List<DocumentLine>();

            lines.AddRange(GetHorizontalLines(words));
            lines.AddRange(GetRotated90Lines(words));
            lines.AddRange(GetRotated180Lines(words));
            lines.AddRange(GetRotated270Lines(words));

            return lines;
        }

        public static IEnumerable<DocumentLine> GetHorizontalLines(IEnumerable<DocumentWord> words)
        {
            return words
                .Where(w => w.Orientation == TextOrientation.Horizontal)
                .GroupBy(w => GroupByPosition(w.PositionY, words.Max(x => x.Height)))
                .OrderByDescending(g => g.Key)
                .Select(g => g.OrderBy(w => w.PositionX))
                .Select(g => new DocumentLine(TextOrientation.Horizontal, g));
        }

        public static IEnumerable<DocumentLine> GetRotated180Lines(IEnumerable<DocumentWord> words)
        {
            return words
                .Where(w => w.Orientation == TextOrientation.Rotated180)
                .GroupBy(w => GroupByPosition(w.PositionY, words.Max(x => x.Height)))
                .OrderByDescending(g => g.Key)
                .Select(g => g.OrderByDescending(w => w.PositionX))
                .Select(g => new DocumentLine(TextOrientation.Rotated180, g));
        }

        public static IEnumerable<DocumentLine> GetRotated90Lines(IEnumerable<DocumentWord> words)
        {
            return words
                .Where(w => w.Orientation == TextOrientation.Rotated90)
                .GroupBy(w => GroupByPosition(w.PositionX, words.Max(x => x.Height) + 1))
                .OrderBy(g => g.Key)
                .Select(g => g.OrderByDescending(w => w.PositionY))
                .Select(g => new DocumentLine(TextOrientation.Rotated90, g));
        }

        public static IEnumerable<DocumentLine> GetRotated270Lines(IEnumerable<DocumentWord> words)
        {
            return words
                .Where(w => w.Orientation == TextOrientation.Rotated270)
                .GroupBy(w => GroupByPosition(w.PositionX, words.Max(x => x.Height) + 1))
                .OrderBy(g => g.Key)
                .Select(g => g.OrderBy(w => w.PositionY))
                .Select(g => new DocumentLine(TextOrientation.Rotated270, g));
        }

        private static double GroupByPosition(double position, double tolerance)
        {
            return Math.Round(position / tolerance) * tolerance;
        }
    }
}
