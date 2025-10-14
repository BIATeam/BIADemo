// <copyright file="CredentialHelper.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Helpers
{
    using System;
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using BIA.Net.Core.Common.Configuration;

    /// <summary>
    /// Credential Helper.
    /// </summary>
#pragma warning disable CA1060 // Move pinvokes to native methods class
    public static class CredentialHelper
#pragma warning restore CA1060 // Move pinvokes to native methods class
    {
        // Windows API declarations
        private const int CREDTYPEGENERIC = 1;

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
                    var credential = ReadWindowsCredential(source.VaultCredentialsKey);
                    login = credential.UserName;
                    password = credential.Password;

                    if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
                    {
                        throw new InvalidOperationException($"Credential {source.VaultCredentialsKey} not found in Vault");
                    }
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

                    if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
                    {
                        throw new InvalidOperationException($"Credential not found in Environment Variable. {source.EnvLoginKey}, {source.EnvPasswordKey}");
                    }
                }
            }

            return (Login: login, Password: password);
        }

        /// <summary>
        /// Retrieves the credentials by vault key.
        /// </summary>
        /// <param name="vaultCredentialsKey">The vault credentials key.</param>
        /// <returns>Credential.</returns>
        public static (string Login, string Password) RetrieveCredentialsByVaultKey(string vaultCredentialsKey)
        {
            return RetrieveCredentials(new CredentialSource() { VaultCredentialsKey = vaultCredentialsKey });
        }

        private static (string UserName, string Password) ReadWindowsCredential(string target)
        {
            if (CredRead(target, CREDTYPEGENERIC, 0, out IntPtr credPtr))
            {
                try
                {
                    var cred = Marshal.PtrToStructure<Credential>(credPtr);
                    var userName = Marshal.PtrToStringUni(cred.UserName);
                    var password = Marshal.PtrToStringUni(cred.CredentialBlob, cred.CredentialBlobSize / 2);
                    return (userName, password);
                }
                finally
                {
                    CredFree(credPtr);
                }
            }
            else
            {
                var errorCode = Marshal.GetLastWin32Error();
                throw new Win32Exception(errorCode, $"Failed to read credential '{target}' from Windows Credential Manager.");
            }
        }

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool CredRead(string target, int type, int reservedFlag, out IntPtr credentialPtr);

        [DllImport("advapi32.dll")]
        private static extern void CredFree(IntPtr cred);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct Credential
        {
            public int Flags;
            public int Type;
            public IntPtr TargetName;
            public IntPtr Comment;
            public System.Runtime.InteropServices.ComTypes.FILETIME LastWritten;
            public int CredentialBlobSize;
            public IntPtr CredentialBlob;
            public int Persist;
            public int AttributeCount;
            public IntPtr Attributes;
            public IntPtr TargetAlias;
            public IntPtr UserName;
        }
    }
}
