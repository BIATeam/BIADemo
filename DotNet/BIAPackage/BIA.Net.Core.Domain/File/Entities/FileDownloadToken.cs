// <copyright file="FileDownloadToken.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.File.Entities
{
    using System;

    /// <summary>
    /// Represents a token that is generated for a file download request.
    /// </summary>
    public class FileDownloadToken
    {
        /// <summary>
        /// FileGuid is the unique identifier of the file for which the download token is generated.
        /// </summary>
        public required Guid FileGuid { get; set; }

        /// <summary>
        /// Navigation property to the FileDownloadData entity that contains the details of the file download request.
        /// </summary>
        public FileDownloadData FileDownloadData { get; set; }

        /// <summary>
        /// The token is a unique string that is generated for each file download request.
        /// </summary>
        public required string Token { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the object was created.
        /// </summary>
        public required DateTime CreatedAt { get; set; }
    }
}
