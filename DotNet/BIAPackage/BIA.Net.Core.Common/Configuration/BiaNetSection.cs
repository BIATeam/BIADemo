// <copyright file="BiaNetSection.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Configuration
{
    using System.Collections.Generic;
    using BIA.Net.Core.Common.Configuration.ApiFeature;
    using BIA.Net.Core.Common.Configuration.CommonFeature;
    using BIA.Net.Core.Common.Configuration.ProfileSection;
    using BIA.Net.Core.Common.Configuration.WorkerFeature;

    /// <summary>
    /// The BIA Net section.
    /// </summary>
    public class BiaNetSection
    {
        /// <summary>
        /// List of database configurations.
        /// </summary>
        public List<DatabaseConfiguration> DatabaseConfigurations { get; set; }

        /// <summary>
        /// Configure the activation of common feature (worker and webApi).
        /// </summary>
        public CommonFeatures CommonFeatures { get; set; }

        /// <summary>
        /// Configure the activation of feature for worker.
        /// </summary>
        public WorkerFeatures WorkerFeatures { get; set; }

        /// <summary>
        /// Configure the activation of feature for webApi.
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
        public Security Security { get; set; }

        /// <summary>
        /// Gets or sets the authentication configuration.
        /// </summary>
        public Jwt Jwt { get; set; }

        /// <summary>
        /// Gets or sets the profile configuration.
        /// </summary>
        public ProfileConfiguration ProfileConfiguration { get; set; }

        /// <summary>
        /// Gets or sets the policies.
        /// </summary>
        public IEnumerable<Policy> Policies { get; set; }

        /// <summary>
        /// Gets or sets the Roles configuration.
        /// </summary>
        public IEnumerable<Role> Roles { get; set; }

        /// <summary>
        /// Gets or sets the permissions configuration.
        /// </summary>
        public IEnumerable<Permission> PermissionsByEnv { get; set; }

        /// <summary>
        /// Gets or sets the permissions configuration.
        /// </summary>
        public IEnumerable<Permission> Permissions { get; set; }

        /// <summary>
        /// Gets or sets the cultures configuration.
        /// </summary>
        public IEnumerable<Culture> Cultures { get; set; }
    }
}