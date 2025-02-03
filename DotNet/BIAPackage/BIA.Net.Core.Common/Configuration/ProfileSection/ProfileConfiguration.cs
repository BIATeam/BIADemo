// <copyright file="ProfileConfiguration.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Configuration.ProfileSection
{
    using BIA.Net.Core.Common.Configuration.AuthenticationSection;

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
        /// Url to access user profile image edit page.
        /// </summary>
        public string EditProfileImageUrl { get; set; }

        /// <summary>
        /// Authentication configuration for profile image.
        /// </summary>
        public AuthenticationConfiguration AuthenticationConfiguration { get; set; }
        public object AuthentMode { get; set; }
    }
}