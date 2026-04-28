// <copyright file="Constants.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Crosscutting.Common
{
    /// <summary>
    /// The class containing all constants.
    /// </summary>
    public static partial class Constants
    {
        /// <summary>
        /// The application constants.
        /// </summary>
        public static partial class Application
        {
            /// <summary>
            /// The framework version.
            /// </summary>
            public const string FrameworkVersion = "7.0.3";

            /// <summary>
            /// The environment.
            /// </summary>
            public const string Environment = "ASPNETCORE_ENVIRONMENT";
        }

        /// <summary>
        /// The default values.
        /// </summary>
        public static partial class DefaultValues
        {
            /// <summary>
            /// The default theme.
            /// </summary>
            public const string Theme = "Light";
        }

        /// <summary>
        /// Language Id.
        /// </summary>
        public static partial class LanguageId
        {
            /// <summary>
            /// The english language Id.
            /// </summary>
            public const int English = 1;

            /// <summary>
            /// The french language Id.
            /// </summary>
            public const int French = 2;

            /// <summary>
            /// The spanish language Id.
            /// </summary>
            public const int Spanish = 3;
        }

        /// <summary>
        /// Provides constants for database migrations.
        /// </summary>
        public static partial class DatabaseMigrations
        {
            /// <summary>
            /// Assembly names for database migrations SQL Server.
            /// </summary>
            public const string AssemblyNameSqlServer = "TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations.SqlServer";

            /// <summary>
            /// Assembly names for database migrations PostgreSQL.
            /// </summary>
            public const string AssemblyNamePostgreSQL = "TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations.PostgreSQL";
        }
    }
}