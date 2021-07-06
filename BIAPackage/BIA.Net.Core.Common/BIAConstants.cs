// <copyright file="Constants.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common
{
    /// <summary>
    /// The class containing all constants.
    /// </summary>
    public static class BIAConstants
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
        /// The class containing HTTP headers constants.
        /// </summary>
        public static class HttpHeaders
        {
            /// <summary>
            /// The total count returned on getAll methods.
            /// </summary>
            public const string TotalCount = "X-Total-Count";
        }
    }
}