// <copyright file="CredentialRepository.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Service.Repositories
{
    using System;
    using System.Runtime.InteropServices;
    using BIA.Net.Core.Common.Configuration;
    using Meziantou.Framework.Win32;

    /// <summary>
    /// Credential Repository.
    /// </summary>
    public static class CredentialRepository
    {
        /// <summary>
        /// Retrieves the credentials.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>Credential.</returns>
        public static (string Login, string Password) RetrieveCredentials(CredentialSource source)
        {
            string login = string.Empty;
            string password = string.Empty;

            if (source != default)
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && !string.IsNullOrWhiteSpace(source.VaultCredentialsKey))
                {
#pragma warning disable CA1416 // Validate platform compatibility
                    Credential cred = CredentialManager.ReadCredential(applicationName: source.VaultCredentialsKey);
#pragma warning restore CA1416 // Validate platform compatibility
                    login = cred?.UserName;
                    password = cred?.Password;
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    if (!string.IsNullOrWhiteSpace(source.EnvLoginKey))
                    {
                        login = Environment.GetEnvironmentVariable(variable: source.EnvLoginKey);
                    }

                    if (!string.IsNullOrWhiteSpace(source.EnvPasswordKey))
                    {
                        password = Environment.GetEnvironmentVariable(variable: source.EnvPasswordKey);
                    }
                }
            }

            return (Login: login, Password: password);
        }
    }
}
