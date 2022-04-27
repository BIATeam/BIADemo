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
#pragma warning disable CA1416 // Validate platform compatibility
        /// <summary>
        /// Runs the specified action as the impersonated Windows identity.
        /// </summary>
        /// <param name="domainName">Name of the domain.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="action">The System.Action to run.</param>
        public static void RunImpersonated(string domainName, string userName, string password, Action action)
        {
            using (SafeAccessTokenHandle safeAccessTokenHandle = GetSafeAccessTokenHandle(domainName, userName, password))
            {

                WindowsIdentity.RunImpersonated(
                    safeAccessTokenHandle,
                    action);
            }
        }

        /// <summary>
        /// Runs the specified function as the impersonated Windows identity.
        /// </summary>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <param name="domainName">Name of the domain.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="func">The System.Func to run.</param>
        /// <returns>The result of the function.</returns>
        public static T RunImpersonated<T>(string domainName, string userName, string password, Func<T> func)
        {
            using (SafeAccessTokenHandle safeAccessTokenHandle = GetSafeAccessTokenHandle(domainName, userName, password))
            {
                return WindowsIdentity.RunImpersonated(
                    safeAccessTokenHandle,
                    func);
            }
        }

        /// <summary>
        /// Runs the specified function as the impersonated Windows identity.
        /// </summary>
        /// <param name="domainName">Name of the domain.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="func">The System.Func to run.</param>
        /// <returns>A task that represents the asynchronous operation of the provided System.Func.</returns>
        public static Task RunImpersonated(string domainName, string userName, string password, Func<Task> func)
        {
            using (SafeAccessTokenHandle safeAccessTokenHandle = GetSafeAccessTokenHandle(domainName, userName, password))
            {
                return WindowsIdentity.RunImpersonatedAsync(
                    safeAccessTokenHandle,
                    func);
            }
        }

        /// <summary>
        /// Runs the specified function as the impersonated Windows identity.
        /// </summary>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <param name="domainName">Name of the domain.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="func">The System.Func to run.</param>
        /// <returns>A task that represents the asynchronous operation of func.</returns>
        public static Task<T> RunImpersonated<T>(string domainName, string userName, string password, Func<Task<T>> func)
        {
            using (SafeAccessTokenHandle safeAccessTokenHandle = GetSafeAccessTokenHandle(domainName, userName, password))
            {
                return WindowsIdentity.RunImpersonatedAsync(
                    safeAccessTokenHandle,
                    func);
            }
        }

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, out SafeAccessTokenHandle phToken);

        private static SafeAccessTokenHandle GetSafeAccessTokenHandle(string domainName, string userName, string password)
        {
            const int LOGON32_PROVIDER_DEFAULT = 0;
            const int LOGON32_TYPE_NEW_CREDENTIALS = 9;

            bool returnValue = LogonUser(userName, domainName, password, LOGON32_TYPE_NEW_CREDENTIALS, LOGON32_PROVIDER_DEFAULT, out SafeAccessTokenHandle safeAccessTokenHandle);

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
