// <copyright file="BadBiaFrameworkUsageException.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// The business exception.
    /// </summary>
    public class BadBiaFrameworkUsageException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BadBiaFrameworkUsageException"/> class.
        /// </summary>
        public BadBiaFrameworkUsageException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BadBiaFrameworkUsageException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public BadBiaFrameworkUsageException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BadBiaFrameworkUsageException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public BadBiaFrameworkUsageException(string message, Exception exception)
            : base(message, exception)
        {
        }
    }
}