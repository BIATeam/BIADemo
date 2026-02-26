// <copyright file="FileDownloadDataDto.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.File
{
    using System;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Option;

    /// <summary>
    /// The DTO used for file download data.
    /// </summary>
    public class FileDownloadDataDto : BaseDto<Guid>
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
        /// Gets or sets the date and time of the request.
        /// </summary>
        public DateTime RequestDateTime { get; set; }

        /// <summary>
        /// Gets or sets the user that requested the download.
        /// </summary>
        public OptionDto RequestByUser { get; set; }

        /// <summary>
        /// Gets or sets the duration for which the file will be available for download..
        /// </summary>
        public TimeSpan? AvailabilityDuration { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="FileDownloadDataDto"/> class with the specified file information and optional availability duration.
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <param name="fileContentType">File content type.</param>
        /// <param name="filePath">File path.</param>
        /// <param name="availabilityDuration">Availability duration.</param>
        /// <returns>A new instance of the <see cref="FileDownloadDataDto"/> class.</returns>
        public static FileDownloadDataDto Create(string fileName, string fileContentType, string filePath, TimeSpan? availabilityDuration = null)
        {
            return new FileDownloadDataDto
            {
                FileName = fileName,
                FileContentType = fileContentType,
                FilePath = filePath,
                AvailabilityDuration = availabilityDuration,
            };
        }
    }
}
