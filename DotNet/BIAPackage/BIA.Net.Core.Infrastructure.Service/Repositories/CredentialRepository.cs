// <copyright file="CredentialRepository.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Service.Repositories
{
    using System;
    using BIA.Net.Core.Common.Configuration;

    /// <summary>
    /// Credential Repository.
    /// </summary>
    [Obsolete(message: "CredentialRepository is deprecated, please use BIA.Net.Core.Common.Helpers.CredentialHelper", error: true)]
    public static class CredentialRepository
    {
        /// <summary>
        /// Retrieves the credentials.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>Credential.</returns>
        public static (string Login, string Password) RetrieveCredentials(CredentialSource source)
        {
            throw new NotImplementedException();
        }
    }
}
