// <copyright file="FileDownloadData.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.File.Entities
{
    using System;
    using BIA.Net.Core.Domain.Entity;
    using BIA.Net.Core.Domain.User.Entities;

    /// <summary>
    /// The FileDownloadData entity.
    /// </summary>
    public class FileDownloadData : BaseEntity<Guid>
    {
        /// <summary>
        /// Gets or sets the file name.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the file content type.
        /// </summary>
        public string FileContentType { get; set; }

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the request was made.
        /// </summary>
        public DateTime RequestDateTime { get; set; }

        /// <summary>
        /// Gets or sets the user id that requested the download.
        /// </summary>
        public int RequestByUserId { get; set; }

        /// <summary>
        /// Gets or sets the user that requested the download.
        /// </summary>
        public virtual BaseEntityUser RequestByUser { get; set; }
    }
}
