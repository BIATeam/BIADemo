// <copyright file="NativeMethods.cs" company="BIA">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>
namespace BIA.Net.Core.Infrastructure.Service.Repositories.Ldap
{
    using System;
    using System.ComponentModel;
    using System.Runtime.InteropServices;

    /// <summary>
    /// LDAP Native Methods.
    /// </summary>
    internal static class NativeMethods
    {
        private const int ERROR_SUCCESS = 0;

        [Flags]
        private enum DSGETDCNAME : uint
        {
            DS_FORCE_REDISCOVERY = 0x00000001,
            DS_DIRECTORY_SERVICE_REQUIRED = 0x00000010,
            DS_DIRECTORY_SERVICE_PREFERRED = 0x00000020,
            DS_GC_SERVER_REQUIRED = 0x00000040,
            DS_PDC_REQUIRED = 0x00000080,
            DS_BACKGROUND_ONLY = 0x00000100,
            DS_IP_REQUIRED = 0x00000200,
            DS_KDC_REQUIRED = 0x00000400,
            DS_TIMESERV_REQUIRED = 0x00000800,
            DS_WRITABLE_REQUIRED = 0x00001000,
            DS_GOOD_TIMESERV_PREFERRED = 0x00002000,
            DS_AVOID_SELF = 0x00004000,
            DS_ONLY_LDAP_NEEDED = 0x00008000,
            DS_IS_FLAT_NAME = 0x00010000,
            DS_IS_DNS_NAME = 0x00020000,
            DS_RETURN_DNS_NAME = 0x40000000,
            DS_RETURN_FLAT_NAME = 0x80000000,
        }

        /// <summary>
        /// Gets the netbios name for domain.
        /// </summary>
        /// <param name="dns">The DNS.</param>
        /// <returns>the netbios name domain.</returns>
        /// <exception cref="System.ComponentModel.Win32Exception">if error.</exception>
        internal static string GetNetbiosNameForDomain(string dns)
        {
            IntPtr pDomainInfo;
            int result = DsGetDcName(
                null,
                dns,
                IntPtr.Zero,
                null,
                DSGETDCNAME.DS_IS_DNS_NAME | DSGETDCNAME.DS_RETURN_FLAT_NAME,
                out pDomainInfo);
            try
            {
                if (result != ERROR_SUCCESS)
                {
                    throw new Win32Exception(result);
                }

                var dcinfo = new DomainControllerInfo();
                Marshal.PtrToStructure(pDomainInfo, dcinfo);

                return dcinfo.DomainName;
            }
            finally
            {
                if (pDomainInfo != IntPtr.Zero)
                {
                    NetApiBufferFree(pDomainInfo);
                }
            }
        }

        [DllImport("Netapi32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "DsGetDcNameW", CharSet = CharSet.Unicode)]
        private static extern int DsGetDcName(
            [In] string computerName,
            [In] string domainName,
            [In] IntPtr domainGuid,
            [In] string siteName,
            [In] DSGETDCNAME flags,
            [Out] out IntPtr domainControllerInfo);

        [DllImport("Netapi32.dll")]
        private static extern int NetApiBufferFree(
            [In] IntPtr buffer);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private sealed class DomainControllerInfo
        {
#pragma warning disable S1144 // Unused private types or members should be removed
            public string DomainControllerName { get; set; }

            public string DomainControllerAddress { get; set; }

            public int DomainControllerAddressType { get; set; }

            public Guid DomainGuid { get; set; }

            public string DomainName { get; set; }

            public string DnsForestName { get; set; }

            public int Flags { get; set; }

            public string DcSiteName { get; set; }

            public string ClientSiteName { get; set; }
#pragma warning restore S1144 // Unused private types or members should be removed
        }
    }
}
