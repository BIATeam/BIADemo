// <copyright file="FrontUserException.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>
namespace BIA.Net.Core.Common.Exceptions
{
    using System;
    using BIA.Net.Core.Common.Error;

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
            : this(null, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrontUserException"/> class.
        /// </summary>
        /// <param name="errorId">The error id.</param>
        /// <param name="innerException">The inner exception if exists.</param>
        public FrontUserException(int errorId, Exception innerException = null)
            : this(null, innerException)
        {
            this.ErrorId = errorId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrontUserException"/> class.
        /// </summary>
        /// <param name="errorId">The error id.</param> 
        /// <param name="innerException">The inner exception if exists.</param>
        /// <param name="messageParameters">The parameters to format into the error message with the function <see cref="GetFormatedErrorMessage"/>.</param>
        public FrontUserException(int errorId, Exception innerException = null, params string[] messageParameters)
            : this(errorId, innerException)
        {
            this.ErrorMessageParameters = messageParameters;
        }

        /// <summary>
        /// The error message key.
        /// </summary>
        public int ErrorId { get; } = (int)BiaErrorId.Unknown;

        /// <summary>
        /// The parameters to format into the current <see cref="Exception.Message"/>.
        /// </summary>
        public string[] ErrorMessageParameters { get; } = [];

        /// <summary>
        /// Create  a new instance of <see cref="FrontUserException"/> class.
        /// </summary>
        /// <typeparam name="TEnum">Error id enum type.</typeparam>
        /// <param name="errorId">Error id.</param>
        /// <param name="innerException">Inner exception if exists.</param>
        /// <param name="messageParameters">Error message parameters.</param>
        /// <returns><see cref="FrontUserException"/>.</returns>
        public static FrontUserException Create<TEnum>(TEnum errorId, Exception innerException = null, params string[] messageParameters)
            where TEnum : Enum
        {
            return new FrontUserException(Convert.ToInt32(errorId), innerException, messageParameters);
        }

        /// <summary>
        /// Create  a new instance of <see cref="FrontUserException"/> class.
        /// </summary>
        /// <typeparam name="TEnum">Error id enum type.</typeparam>
        /// <param name="errorId">Error id.</param>
        /// <param name="innerException">Inner exception if exists.</param>
        /// <returns><see cref="FrontUserException"/>.</returns>
        public static FrontUserException Create<TEnum>(TEnum errorId, Exception innerException = null)
            where TEnum : Enum
        {
            return new FrontUserException(Convert.ToInt32(errorId), innerException);
        }
    }
}
