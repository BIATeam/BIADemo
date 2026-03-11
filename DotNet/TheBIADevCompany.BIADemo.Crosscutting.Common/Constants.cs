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
            /// The back end version.
            /// </summary>
            // Except BIADemo public const string BackEndVersion = "0.0.0";
            // Begin BIADemo
            public const string BackEndVersion = "7.0.0";

            // End BIADemo

            /// <summary>
            /// The front end version.
            /// </summary>
            // Except BIADemo public const string FrontEndVersion = "0.0.0";
            // Begin BIADemo
            public const string FrontEndVersion = "7.0.0";

            // End BIADemo
        }

        /// <summary>
        /// Language Id.
        /// </summary>
        public static partial class LanguageId
        {
            // Begin BIADemo

            /// <summary>
            /// The german language Id.
            /// </summary>
            public const int German = 4;

            // End BIADemo
        }
    }
}