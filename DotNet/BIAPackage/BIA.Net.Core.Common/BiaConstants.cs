// <copyright file="BiaConstants.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common
{
    /// <summary>
    /// The class containing all constants.
    /// </summary>
    public static class BiaConstants
    {
        /// <summary>
        /// CSV parameters.
        /// </summary>
        public static class Csv
        {
            /// <summary>
            /// the separator for a csv file.
            /// </summary>
            public const string Separator = ",";

            /// <summary>
            /// the extension of a csv file.
            /// </summary>
            public const string Extension = ".csv";

            /// <summary>
            /// the extension of a csv file.
            /// </summary>
            public const string ContentType = "text/csv";
        }

        /// <summary>
        /// Eclipse.
        /// </summary>
        public static class Eclipse
        {
            /// <summary>
            /// the extension of a eclipse file.
            /// </summary>
            public const string Extension = ".ecl";

            /// <summary>
            /// the extension of a eclipse file.
            /// </summary>
            public const string ContentType = "text/x-ecl";
        }

        /// <summary>
        /// PDF.
        /// </summary>
        public static class Pdf
        {
            /// <summary>
            /// the extension of a eclipse file.
            /// </summary>
            public const string Extension = ".pdf";

            /// <summary>
            /// the extension of a eclipse file.
            /// </summary>
            public const string ContentType = "application/pdf";
        }

        /// <summary>
        /// Role.
        /// </summary>
        public static class Role
        {
            /// <summary>
            /// The admin role code.
            /// </summary>
            public const string Admin = "Admin";

            /// <summary>
            /// The user role code.
            /// </summary>
            public const string User = "User";

            /// <summary>
            /// The right to access to the list of members.
            /// </summary>
            public const string TeamMemberSuffix = "_Member";

            /// <summary>
            /// The right to access to the list of members.
            /// </summary>
            public const string TeamMemberOfOneSuffix = "_MemberOfOne";
        }

        /// <summary>
        /// The class containing HTTP headers constants.
        /// </summary>
        public static class HttpHeaders
        {
            /// <summary>
            /// The total count returned on getAll methods.
            /// </summary>
            public const string TotalCount = "X-Total-Count";
        }

        /// <summary>
        /// Role Type.
        /// </summary>
        public static class RoleType
        {
            /// <summary>
            /// Fake.
            /// </summary>
            public const string Fake = "Fake";

            /// <summary>
            /// User in database.
            /// </summary>
            public const string UserInDB = "UserInDB";

            /// <summary>
            /// The claims to role.
            /// </summary>
            public const string ClaimsToRole = "ClaimsToRole";

            /// <summary>
            /// From an LDAP group.
            /// </summary>
            public const string Ldap = "Ldap";

            /// <summary>
            /// From an LDAP group provided by the sid history provider.
            /// </summary>
            public const string LdapWithSidHistory = "LdapWithSidHistory";

            /// <summary>
            /// Synchro.
            /// </summary>
            public const string Synchro = "Synchro";
        }

        /// <summary>
        /// Policy.
        /// </summary>
        public static class Policy
        {
            /// <summary>
            /// Policy for  service API RW.
            /// </summary>
            public const string ServiceApiRW = "ServiceApiRW";
        }

        /// <summary>
        /// Database Configuration.
        /// </summary>
        public static class DatabaseConfiguration
        {
            /// <summary>
            /// The default key.
            /// </summary>
            public const string DefaultKey = "ProjectDatabase";

            /// <summary>
            /// The default key read only.
            /// </summary>
            public const string DefaultKeyReadOnly = "ProjectDatabaseReadOnly";
        }

        /// <summary>
        /// Constants for database provider specific AppContext switches.
        /// </summary>
        public static class AppContextSwitch
        {
            /// <summary>
            /// PostgreSQL switches.
            /// </summary>
            public static class Npgsql
            {
                /// <summary>
                /// Enables legacy timestamp behavior in Npgsql to maintain UTC handling compatibility.
                /// This switch prevents automatic timezone conversion that was introduced in Npgsql 6.0+.
                /// </summary>
                public const string EnableLegacyTimestampBehavior = "Npgsql.EnableLegacyTimestampBehavior";
            }
        }

        /// <summary>
        /// Audit.
        /// </summary>
        public static class Audit
        {
            /// <summary>
            /// Audit update action.
            /// </summary>
            public const string UpdateAction = "Update";

            /// <summary>
            /// Audit insert action.
            /// </summary>
            public const string InsertAction = "Insert";

            /// <summary>
            /// Audit delete action.
            /// </summary>
            public const string DeleteAction = "Delete";

            /// <summary>
            /// Custom field user login.
            /// </summary>
            public const string UserLoginCustomField = "UserLogin";
        }
    }
}
