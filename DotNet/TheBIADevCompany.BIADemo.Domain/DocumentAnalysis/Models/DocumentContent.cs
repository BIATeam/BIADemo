// BIADemo only
// <copyright file="DocumentContent.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.DocumentAnalysis.Models
{
    using System.Collections.Generic;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum.DocumentAnalysis;

    /// <summary>
    /// Represents the content of a document.
    /// </summary>
    public class DocumentContent
    {
        /// <summary>
        /// Document name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Document type.
        /// </summary>
        public DocumentType Type { get; set; }

        /// <summary>
        /// Count of pages.
        /// </summary>
        public int PagesCount => this.Pages.Count;

        /// <summary>
        /// Pages of the document.
        /// </summary>
        public List<DocumentPage> Pages { get; set; } = new ();
    }
}
