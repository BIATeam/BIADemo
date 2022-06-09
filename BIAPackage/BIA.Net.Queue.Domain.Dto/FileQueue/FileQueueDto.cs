// <copyright file="FileQueueDto.cs" company="BIA">
//  Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Queue.Domain.Dto.FileQueue
{
    using System;

    /// <summary>
    /// DTO of File Queue
    /// </summary>
    [Serializable]
    public class FileQueueDto
    {
        /// <summary>
        /// Gets or Sets the filename.
        /// </summary>
        public string FileName { get; set; }

        public string? OutputPath { get; set; }

        public byte[] Data { get; set; }
    }
}
