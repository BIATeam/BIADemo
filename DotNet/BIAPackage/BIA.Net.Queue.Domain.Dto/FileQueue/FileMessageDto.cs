// <copyright file="FileMessageDto.cs" company="BIA">
//  Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Queue.Domain.Dto.FileQueue
{
    using System;

    /// <summary>
    /// DTO of File Message.
    /// </summary>
    [Serializable]
    public class FileMessageDto
    {
        /// <summary>
        /// Gets or Sets the filename.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or Sets the output path folder to put the file.
        /// </summary>
        public string OutputPath { get; set; }

        /// <summary>
        /// Gets or sets the data of the files en bytes.
        /// </summary>
        public byte[] Data { get; set; }
    }
}
