// <copyright file="BiaNetSection.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Configuration
{
    using BIA.Net.Core.Common.Configuration.ApiFeature;
    using BIA.Net.Core.Common.Configuration.WorkerFeature;
    using System.Collections.Generic;

    /// <summary>
    /// The BIA Net section.
    /// </summary>
    public class BiaNetSection
    {
        /// <summary>
        /// Configure the activation of feature for worker.
        /// </summary>
        public WorkerFeatures WorkerFeatures { get; set; }

        /// <summary>
        /// Configure the activation of feature for worker.
        /// </summary>
        public ApiFeatures ApiFeatures { get; set; }

        /// <summary>
        /// Gets or sets the authentication configuration.
        /// </summary>
        public EnvironmentConfiguration Environment { get; set; }

        /// <summary>
        /// Gets or sets the email configuration.
        /// </summary>
        public EmailConfiguration EmailConfiguration { get; set; }

        /// <summary>
        /// Gets or sets the authentication configuration.
        /// </summary>
        public Authentication Authentication { get; set; }

        /// <summary>
        /// Gets or sets the authentication configuration.
        /// </summary>
        public Jwt Jwt { get; set; }

        /// <summary>
        /// Gets or sets the Roles configuration.
        /// </summary>
        public IEnumerable<Role> Roles { get; set; }

        /// <summary>
        /// Gets or sets the permissions configuration.
        /// </summary>
        public IEnumerable<Permission> Permissions { get; set; }

        /// <summary>
        /// Gets or sets the user profile configuration.
        /// </summary>
        public UserProfile UserProfile { get; set; }

        /// <summary>
        /// Gets or sets the languages configuration.
        /// </summary>
        public IEnumerable<Language> Languages { get; set; }
    }
}