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
            public const string FrameworkVersion = "4.0.0-beta";

            /// <summary>
            /// The environment.
            /// </summary>
            public const string Environment = "ASPNETCORE_ENVIRONMENT";
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