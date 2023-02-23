// <copyright file="Rights.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common
{
    /// <summary>
    /// The list of all rights.
    /// </summary>
    public static class BIARights
    {
        /// <summary>
        /// The LDAP domains rights.
        /// </summary>
        public static class LdapDomains
        {
            /// <summary>
            /// The right to get all LDAP domains.
            /// </summary>
            public const string List = "LdapDomains_List";
        }

        /// <summary>
        /// The LDAP domains rights.
        /// </summary>
        public static class Languages
        {
            /// <summary>
            /// The right to get all LDAP domains.
            /// </summary>
            public const string Options = "Languages_Options";
        }
    }
}