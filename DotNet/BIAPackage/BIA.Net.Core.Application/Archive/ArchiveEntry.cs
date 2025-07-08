// <copyright file="ArchiveEntry.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Archive
{
    using System.IO;

    /// <summary>
    /// Represents an entry to add to an archive.
    /// </summary>
    public class ArchiveEntry
    {
        /// <summary>
        /// Name of the entry in the archive.
        /// </summary>
        required public string EntryName { get; set; }

        /// <summary>
        /// Stream content of the entry.
        /// </summary>
        required public Stream ContentStream { get; set; }
    }
}
