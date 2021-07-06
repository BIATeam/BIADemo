// <copyright file="LogDto.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto
{
    using System.Collections.Generic;
    using BIA.Net.Core.Common.Enum;

    /// <summary>
    /// The DTO used to transfer logs coming from front.
    /// </summary>
    public class LogDto
    {
        /// <summary>
        /// Gets or sets the level of log.
        /// </summary>
        public NgxLogLevel Level { get; set; }

        /// <summary>
        /// Gets or sets the timestamp.
        /// </summary>
        public string Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the file name associated with the log.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the line number in the file name.
        /// </summary>
        public string LineNumber { get; set; }

        /// <summary>
        /// Gets or sets the message to log.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets additional information.
        /// </summary>
        public IEnumerable<object> Additional { get; set; }
    }
}