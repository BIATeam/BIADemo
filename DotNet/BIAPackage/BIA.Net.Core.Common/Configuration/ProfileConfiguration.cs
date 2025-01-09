// <copyright file="ProfileConfiguration.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Configuration
{
    /// <summary>
    /// The user profile configuration.
    /// </summary>
    public class ProfileConfiguration
    {
        /// <summary>
        /// When true, the client will acces the url of the image by itself.
        /// When false, the client will call the api that will make the call to the image url or path.
        /// </summary>
        public bool ClientProfileImageGetMode { get; set; }

        /// <summary>
        /// Url to access user profile image.
        /// </summary>
        public string ProfileImageUrlOrPath { get; set; }

        /// <summary>
        /// Authentication mode for access to ProfileImageUrl.
        /// </summary>
        public AuthentMode AuthentMode { get; set; }

        /// <summary>
        /// Authentication credentials used to access profile image when in AuthentMode Standard = 1.
        /// </summary>
        public AuthentCredentials AuthentCredentials { get; set; }

        /// <summary>
        /// Api key used to access profile image when in AuthenMode ApiKey = 3.
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// Url to access user profile image edit page.
        /// </summary>
        public string EditProfileImageUrl { get; set; }
    }
}