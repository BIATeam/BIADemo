// <copyright file="WindowsIdentityHelper.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Helpers
{
    using System;
    using System.Runtime.InteropServices;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using Microsoft.Win32.SafeHandles;

    /// <summary>
    /// Helper for the class <see cref="WindowsIdentity"/>.
    /// </summary>
    public static class WindowsIdentityHelper
    {
        /// <summary>
        /// The type of logon operation to perform.
        /// </summary>
        public enum LogonType
        {
            /// <summary>
            /// This logon type is intended for users who will be interactively using the computer,
            /// such as a user being logged on by a terminal server, remote shell, or similar process.
            /// This logon type has the additional expense of caching logon information for disconnected operations; therefore,
            /// it is inappropriate for some client/server applications, such as a mail server.
            /// </summary>
            LOGON32_LOGON_INTERACTIVE = 2,

            /// <summary>
            /// This logon type is intended for high performance servers to authenticate plaintext passwords.
            /// The LogonUserExExW function does not cache credentials for this logon type.
            /// </summary>
            LOGON32_LOGON_NETWORK = 3,

            /// <summary>
            /// This logon type is intended for batch servers, where processes may be executing on behalf of a user without their direct intervention.
            /// This type is also for higher performance servers that process many plaintext authentication attempts at a time, such as mail or web servers.
            /// The LogonUserExExW function does not cache credentials for this logon type.
            /// </summary>
            LOGON32_LOGON_BATCH = 4,

            /// <summary>
            /// Indicates a service-type logon. The account provided must have the service privilege enabled.
            /// </summary>
            LOGON32_LOGON_SERVICE = 5,

            /// <summary>
            /// This logon type is for GINA DLLs that log on users who will be interactively using the computer.
            /// This logon type can generate a unique audit record that shows when the workstation was unlocked.
            /// </summary>
            LOGON32_LOGON_UNLOCK = 7,

            /// <summary>
            /// This logon type preserves the name and password in the authentication package,
            /// which allows the server to make connections to other network servers while impersonating the client.
            /// A server can accept plaintext credentials from a client, call LogonUserExExW, verify that the user can access the system across the network,
            /// and still communicate with other servers.
            /// </summary>
            LOGON32_LOGON_NETWORK_CLEARTEXT = 8,

            /// <summary>
            /// This logon type allows the caller to clone its current token and specify new credentials for outbound connections.
            /// The new logon session has the same local identifier but uses different credentials for other network connections.
            /// This logon type is supported only by the LOGON32_PROVIDER_WINNT50 logon provider.
            /// </summary>
            LOGON32_LOGON_NEW_CREDENTIALS = 9,
        }

        /// <summary>
        /// The logon provider.
        /// </summary>
        public enum LogonProvider
        {
            /// <summary>
            /// The logon provider default.
            /// </summary>
            LOGON32_PROVIDER_DEFAULT = 0,

            /// <summary>
            /// The logon provider WINNT35.
            /// </summary>
            LOGON32_PROVIDER_WINNT35 = 1,

            /// <summary>
            /// The logon provider WINNT40.
            /// </summary>
            LOGON32_PROVIDER_WINNT40 = 2,

            /// <summary>
            /// The logon provider WINNT50.
            /// </summary>
            LOGON32_PROVIDER_WINNT50 = 3,
        }

#pragma warning disable CA1416 // Validate platform compatibility
        /// <summary>
        /// Runs the specified action as the impersonated Windows identity.
        /// </summary>
        /// <param name="domainName">Name of the domain.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="action">The System.Action to run.</param>
        /// <param name="logonType">The type of logon operation to perform (LOGON32_LOGON_NEW_CREDENTIALS by default).</param>
        /// <param name="logonProvider">The logon provider (LOGON32_PROVIDER_DEFAULT by default).</param>
        public static void RunImpersonated(
            string domainName,
            string userName,
            string password,
            Action action,
            LogonType logonType = LogonType.LOGON32_LOGON_NEW_CREDENTIALS,
            LogonProvider logonProvider = LogonProvider.LOGON32_PROVIDER_DEFAULT)
        {
            using SafeAccessTokenHandle safeAccessTokenHandle = GetSafeAccessTokenHandle(domainName, userName, password, logonType, logonProvider);
            WindowsIdentity.RunImpersonated(
                safeAccessTokenHandle,
                action);
        }

        /// <summary>
        /// Runs the specified function as the impersonated Windows identity.
        /// </summary>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <param name="domainName">Name of the domain.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="func">The System.Func to run.</param>
        /// <param name="logonType">The type of logon operation to perform (LOGON32_LOGON_NEW_CREDENTIALS by default).</param>
        /// <param name="logonProvider">The logon provider (LOGON32_PROVIDER_DEFAULT by default).</param>
        /// <returns>The result of the function.</returns>
        public static T RunImpersonated<T>(
            string domainName,
            string userName,
            string password,
            Func<T> func,
            LogonType logonType = LogonType.LOGON32_LOGON_NEW_CREDENTIALS,
            LogonProvider logonProvider = LogonProvider.LOGON32_PROVIDER_DEFAULT)
        {
            using SafeAccessTokenHandle safeAccessTokenHandle = GetSafeAccessTokenHandle(domainName, userName, password, logonType, logonProvider);
            return WindowsIdentity.RunImpersonated(
                safeAccessTokenHandle,
                func);
        }

        /// <summary>
        /// Runs the specified function as the impersonated Windows identity.
        /// </summary>
        /// <param name="domainName">Name of the domain.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="func">The System.Func to run.</param>
        /// <param name="logonType">The type of logon operation to perform (LOGON32_LOGON_NEW_CREDENTIALS by default).</param>
        /// <param name="logonProvider">The logon provider (LOGON32_PROVIDER_DEFAULT by default).</param>
        /// <returns>A task that represents the asynchronous operation of the provided System.Func.</returns>
        public static Task RunImpersonated(
            string domainName,
            string userName,
            string password,
            Func<Task> func,
            LogonType logonType = LogonType.LOGON32_LOGON_NEW_CREDENTIALS,
            LogonProvider logonProvider = LogonProvider.LOGON32_PROVIDER_DEFAULT)
        {
            using SafeAccessTokenHandle safeAccessTokenHandle = GetSafeAccessTokenHandle(domainName, userName, password, logonType, logonProvider);
            return WindowsIdentity.RunImpersonatedAsync(
                safeAccessTokenHandle,
                func);
        }

        /// <summary>
        /// Runs the specified function as the impersonated Windows identity.
        /// </summary>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <param name="domainName">Name of the domain.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="func">The System.Func to run.</param>
        /// <param name="logonType">The type of logon operation to perform (LOGON32_LOGON_NEW_CREDENTIALS by default).</param>
        /// <param name="logonProvider">The logon provider (LOGON32_PROVIDER_DEFAULT by default).</param>
        /// <returns>A task that represents the asynchronous operation of func.</returns>
        public static Task<T> RunImpersonated<T>(
            string domainName,
            string userName,
            string password,
            Func<Task<T>> func,
            LogonType logonType = LogonType.LOGON32_LOGON_NEW_CREDENTIALS,
            LogonProvider logonProvider = LogonProvider.LOGON32_PROVIDER_DEFAULT)
        {
            using SafeAccessTokenHandle safeAccessTokenHandle = GetSafeAccessTokenHandle(domainName, userName, password, logonType, logonProvider);
            return WindowsIdentity.RunImpersonatedAsync(
                safeAccessTokenHandle,
                func);
        }

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int logonType, int dwLogonProvider, out SafeAccessTokenHandle phToken);

        private static SafeAccessTokenHandle GetSafeAccessTokenHandle(string domainName, string userName, string password, LogonType logonType, LogonProvider logonProvider)
        {
            bool returnValue = LogonUser(userName, domainName, password, (int)logonType, (int)logonProvider, out SafeAccessTokenHandle safeAccessTokenHandle);

            if (!returnValue)
            {
                int ret = Marshal.GetLastWin32Error();
                throw new System.ComponentModel.Win32Exception(ret);
            }

            return safeAccessTokenHandle;
        }
#pragma warning restore CA1416 // Validate platform compatibility
    }
}
