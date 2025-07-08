// BIADemo only
// <copyright file="DocumentPage.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.DocumentAnalysis.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum.DocumentAnalysis;

    /// <summary>
    /// Represents a page in a document.
    /// </summary>
    public class DocumentPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentPage"/> class.
        /// </summary>
        /// <param name="number">Page number.</param>
        /// <param name="words">Words in the page.</param>
        public DocumentPage(int number, IEnumerable<DocumentWord> words)
        {
            this.Number = number;
            this.Lines = ExtractLines(words);
        }

        /// <summary>
        /// Page number.
        /// </summary>
        public int Number { get; }

        /// <summary>
        /// Count of lines with words in the page.
        /// </summary>
        public int LinesCount => this.Lines.Count;

        /// <summary>
        /// Count of words in the page.
        /// </summary>
        public int WordsCount => this.Words.Count();

        /// <summary>
        /// Lines with words in the page.
        /// </summary>
        public List<DocumentLine> Lines { get; }

        /// <summary>
        /// Words in the page.
        /// </summary>
        public IEnumerable<DocumentWord> Words => this.Lines.SelectMany(l => l.Words);

        /// <summary>
        /// Extract lines from words.
        /// </summary>
        /// <param name="words">Collection of <see cref="DocumentWord"/>.</param>
        /// <returns>Collection of <see cref="DocumentLine"/>.</returns>
        public static List<DocumentLine> ExtractLines(IEnumerable<DocumentWord> words)
        {
            var lines = new List<DocumentLine>();

            lines.AddRange(ExtractHorizontalLines(words));
            lines.AddRange(ExtractRotated90Lines(words));
            lines.AddRange(ExtractRotated180Lines(words));
            lines.AddRange(ExtractRotated270Lines(words));

            return lines;
        }

        /// <summary>
        /// Extract lines from horizontal words.
        /// </summary>
        /// <param name="words">Collection of <see cref="DocumentWord"/>.</param>
        /// <returns>Collection of <see cref="DocumentLine"/>.</returns>
        public static IEnumerable<DocumentLine> ExtractHorizontalLines(IEnumerable<DocumentWord> words)
        {
            return words
                .Where(w => w.Orientation == TextOrientation.Horizontal)
                .GroupBy(w => GroupByPosition(w.PositionY, words.Max(x => x.Height) + 1))
                .OrderByDescending(g => g.Key)
                .Select(g => g.OrderBy(w => w.PositionX))
                .Select(g => new DocumentLine(TextOrientation.Horizontal, g));
        }

        /// <summary>
        /// Extract lines from horizontal upside down words.
        /// </summary>
        /// <param name="words">Collection of <see cref="DocumentWord"/>.</param>
        /// <returns>Collection of <see cref="DocumentLine"/>.</returns>
        public static IEnumerable<DocumentLine> ExtractRotated180Lines(IEnumerable<DocumentWord> words)
        {
            return words
                .Where(w => w.Orientation == TextOrientation.Rotated180)
                .GroupBy(w => GroupByPosition(w.PositionY, words.Max(x => x.Height) + 1))
                .OrderByDescending(g => g.Key)
                .Select(g => g.OrderByDescending(w => w.PositionX))
                .Select(g => new DocumentLine(TextOrientation.Rotated180, g));
        }

        /// <summary>
        /// Extract lines from vertical going down words.
        /// </summary>
        /// <param name="words">Collection of <see cref="DocumentWord"/>.</param>
        /// <returns>Collection of <see cref="DocumentLine"/>.</returns>
        public static IEnumerable<DocumentLine> ExtractRotated90Lines(IEnumerable<DocumentWord> words)
        {
            return words
                .Where(w => w.Orientation == TextOrientation.Rotated90)
                .GroupBy(w => GroupByPosition(w.PositionX, words.Max(x => x.Height) + 1))
                .OrderBy(g => g.Key)
                .Select(g => g.OrderByDescending(w => w.PositionY))
                .Select(g => new DocumentLine(TextOrientation.Rotated90, g));
        }

        /// <summary>
        /// Extract lines from vertical going up words.
        /// </summary>
        /// <param name="words">Collection of <see cref="DocumentWord"/>.</param>
        /// <returns>Collection of <see cref="DocumentLine"/>.</returns>
        public static IEnumerable<DocumentLine> ExtractRotated270Lines(IEnumerable<DocumentWord> words)
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
