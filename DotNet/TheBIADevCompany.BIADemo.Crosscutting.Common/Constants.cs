// <copyright file="Constants.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Crosscutting.Common
{
    /// <summary>
    /// The class containing all constants.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// The application constants.
        /// </summary>
        public static class Application
        {
            /// <summary>
            /// The back end version.
            /// </summary>
            public const string BackEndVersion = "0.0.0";

            /// <summary>
            /// The front end version.
            /// </summary>
            public const string FrontEndVersion = "0.0.0";

            /// <summary>
            /// The framework version.
            /// </summary>
            public const string FrameworkVersion = "3.6.5";
        }

        /// <summary>
        /// The default values.
        /// </summary>
        public static class DefaultValues
        {
            /// <summary>
            /// The default theme.
            /// </summary>
            public const string Theme = "Light";

            /// <summary>
            /// The default language.
            /// </summary>
            public const string Language = "en-US";
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
            /// The site member role code.
            /// </summary>
            public const string SiteMember = "Site_Member";

            // Begin BIADemo

            /// <summary>
            /// The Aircraft Maintenance Company member role code.
            /// </summary>
            public const string AircraftMaintenanceCompanyMember = "AircraftMaintenanceCompany_Member";

            /// <summary>
            /// The Maintenance Team member role code.
            /// </summary>
            public const string MaintenanceTeamMember = "MaintenanceTeam_Member";

            // End BIADemo
        }

        /// <summary>
        /// Language Id.
        /// </summary>
        public static class LanguageId
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

            /// <summary>
            /// The german language Id.
            /// </summary>
            public const int German = 4;
        }
    }
}