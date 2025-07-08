// <copyright file="FrontUserException.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>
namespace BIA.Net.Core.Common.Exceptions
{
    using System;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Common.Helpers;

    /// <summary>
    /// Exception to return to the front user.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="FrontUserException"/> class.
    /// </remarks>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The inner exception if exists.</param>
    public class FrontUserException(string message, Exception innerException = null)
        : Exception(message, innerException)
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FrontUserException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The inner exception if exists.</param>
        /// <param name="messageParameters">The parameters to format into the error message.</param>
        public FrontUserException(string message, Exception innerException = null, params string[] messageParameters)
            : this(message, innerException)
        {
            this.ErrorMessageParameters = messageParameters;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrontUserException"/> class.
        /// </summary>
        /// <param name="innerException">The inner exception if exists.</param>
        public FrontUserException(Exception innerException)
            : this(string.Empty, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrontUserException"/> class.
        /// </summary>
        /// <param name="messageKey">The error message key used to get the corresponding error message to return with <see cref="GetFormatedErrorMessage"/>.</param>
        /// <param name="innerException">The inner exception if exists.</param>
        public FrontUserException(FrontUserExceptionErrorMessageKey messageKey, Exception innerException = null)
            : this(ExceptionHelper.GetErrorMessage(messageKey), innerException)
        {
            this.ErrorMessageKey = messageKey;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrontUserException"/> class.
        /// </summary>
        /// <param name="messageKey">The error message key used to get the corresponding error message to return with <see cref="GetFormatedErrorMessage"/>.</param>
        /// <param name="innerException">The inner exception if exists.</param>
        /// <param name="messageParameters">The parameters to format into the error message with the function <see cref="GetFormatedErrorMessage"/>.</param>
        public FrontUserException(FrontUserExceptionErrorMessageKey messageKey, Exception innerException = null, params string[] messageParameters)
            : this(messageKey, innerException)
        {
            this.ErrorMessageParameters = messageParameters;
        }

        /// <summary>
        /// The error message key.
        /// </summary>
        public FrontUserExceptionErrorMessageKey ErrorMessageKey { get; } = FrontUserExceptionErrorMessageKey.Unknown;

        /// <summary>
        /// The parameters to format into the current <see cref="Exception.Message"/>.
        /// </summary>
        public string[] ErrorMessageParameters { get; } = Array.Empty<string>();
    }
}
