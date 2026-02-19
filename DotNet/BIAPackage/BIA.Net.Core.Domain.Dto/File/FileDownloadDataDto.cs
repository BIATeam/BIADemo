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
    }
}
