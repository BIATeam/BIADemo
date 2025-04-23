// BIADemo only
// <copyright file="DocumentLine.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.DocumentAnalysis.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum.DocumentAnalysis;

    /// <summary>
    /// Represents a line with words in a document.
    /// </summary>
    public class DocumentLine
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentLine"/> class.
        /// </summary>
        /// <param name="orientation">Line text orientation.</param>
        /// <param name="words">Words in the line.</param>
        public DocumentLine(TextOrientation orientation, IEnumerable<DocumentWord> words)
        {
            var lineWords = words.Where(w => w.Orientation == orientation);

            this.Orientation = orientation;
            this.Content = string.Concat(lineWords.Select(w => w.Text));
            this.Words = new List<DocumentWord>(lineWords);
        }

        /// <summary>
        /// Content of the line.
        /// </summary>
        public string Content { get; }

        /// <summary>
        /// Text orientation.
        /// </summary>
        public TextOrientation Orientation { get; }

        /// <summary>
        /// Words of the line.
        /// </summary>
        public List<DocumentWord> Words { get; }
    }
}
