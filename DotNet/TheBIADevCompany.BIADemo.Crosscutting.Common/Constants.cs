// <copyright file="Constants.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
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
            // Except BIADemo public const string BackEndVersion = "0.0.0";
            // Begin BIADemo
            public const string BackEndVersion = "5.0.3";

            // End BIADemo

            /// <summary>
            /// The front end version.
            /// </summary>
            // Except BIADemo public const string FrontEndVersion = "0.0.0";
            // Begin BIADemo
            public const string FrontEndVersion = "5.0.3";

            // End BIADemo

            /// <summary>
            /// The framework version.
            /// </summary>
            public const string FrameworkVersion = "next";

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

            // Begin BIADemo

            /// <summary>
            /// The german language Id.
            /// </summary>
            public const int German = 4;

            // End BIADemo
        }
    }
}