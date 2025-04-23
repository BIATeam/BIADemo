// <copyright file="OutdateException.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Exceptions
{
    using System;

    /// <summary>
    /// The business exception.
    /// </summary>
    public class OutdateException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OutdateException"/> class.
        /// </summary>
        public OutdateException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OutdateException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public OutdateException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OutdateException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public OutdateException(string message, Exception exception)
            : base(message, exception)
        {
        }
    }
}
