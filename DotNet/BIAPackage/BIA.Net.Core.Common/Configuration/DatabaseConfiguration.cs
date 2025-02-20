// <copyright file="DatabaseConfiguration.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Configuration
{
    /// <summary>
    /// A database configuration.
    /// </summary>
    public class DatabaseConfiguration
    {
        /// <summary>
        /// Key of the configuration.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// The database provider kind.
        /// </summary>
        public string Provider { get; set; }

        /// <summary>
        /// The database connection string.
        /// </summary>
        public string ConnectionString { get; set; }

        public bool SQLDataBroker { get; set; }
    }
}
