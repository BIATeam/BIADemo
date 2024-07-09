// <copyright file="JobException.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// The business exception.
    /// </summary>
    public class JobException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JobException"/> class.
        /// </summary>
        public JobException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JobException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public JobException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JobException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public JobException(string message, Exception exception)
            : base(message, exception)
        {
        }
    }
}