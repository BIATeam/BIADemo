// <copyright file="ConfigurationExtensions.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Configuration
{
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// ConfigurationExtensions.
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Gets the database engine.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="name">The name.</param>
        /// <returns>The database engine.</returns>
        public static string GetDBEngine(this IConfiguration configuration, string name)
        {
            return configuration["DBEngine:" + name];
        }

        /// <summary>
        /// Gets the SQL data broker mode.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="name">The name.</param>
        /// <returns>The database engine.</returns>
        public static bool GetSqlDataBroker(this IConfiguration configuration, string name)
        {
            var value = configuration["SQLDataBroker:" + name];
            return !string.IsNullOrEmpty(value) && bool.Parse(configuration["SQLDataBroker:" + name]);
        }
    }
}
