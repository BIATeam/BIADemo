// <copyright file="SystemException.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Exceptions
{
    using System;

    /// <summary>
    /// The system exception.
    /// </summary>
    public class SystemException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemException"/> class.
        /// </summary>
        public SystemException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public SystemException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public SystemException(string message, Exception exception)
            : base(message, exception)
        {
        }
    }
}