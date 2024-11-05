// <copyright file="ProfileConfiguration.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Configuration
{
    using System.Collections.Generic;

    /// <summary>
    /// The user profile configuration.
    /// </summary>
    public class ProfileConfiguration
    {
        /// <summary>
        /// Url to access user profile image.
        /// </summary>
        public string UrlProfileImage { get; set; }

        /// <summary>
        /// Url to access user profile image edit page.
        /// </summary>
        public string UrlEditProfileImage { get; set; }
    }
}