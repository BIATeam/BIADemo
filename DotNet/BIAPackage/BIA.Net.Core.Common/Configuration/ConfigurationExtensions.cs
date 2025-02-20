// <copyright file="ConfigurationExtensions.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Configuration
{
    using System.Linq;
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
        /// <param name="key">The database key.</param>
        /// <returns>The database engine.</returns>
        public static string GetDBEngine(this IConfiguration configuration, string key)
        {
            var appSettingsValue = configuration["DBEngine:" + key];
            if (!string.IsNullOrWhiteSpace(appSettingsValue))
            {
                return appSettingsValue;
            }

            var bianetSection = new BiaNetSection();
            configuration.GetSection("BiaNet").Bind(bianetSection);
            return bianetSection.DatabaseConfigurations?.FirstOrDefault(x => x.Key == key)?.Provider;
        }

        /// <summary>
        /// Gets the connection string of a database.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="key">The database key.</param>
        /// <returns>The database engine.</returns>
        public static string GetDatabaseConnectionString(this IConfiguration configuration, string key)
        {
            var appSettingsValue = configuration["ConnectionStrings:" + key];
            if (!string.IsNullOrWhiteSpace(appSettingsValue))
            {
                return appSettingsValue;
            }

            var bianetSection = new BiaNetSection();
            configuration.GetSection("BiaNet").Bind(bianetSection);
            return bianetSection.DatabaseConfigurations?.FirstOrDefault(x => x.Key == key)?.ConnectionString;
        }

        /// <summary>
        /// Gets the SQL data broker mode.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="key">The name.</param>
        /// <returns>The database engine.</returns>
        public static bool? GetSqlDataBroker(this IConfiguration configuration, string key)
        {
            var appSettingsValue = configuration["SQLDataBroker:" + key];
            if (!string.IsNullOrWhiteSpace(appSettingsValue))
            {
                return bool.Parse(appSettingsValue);
            }

            var bianetSection = new BiaNetSection();
            configuration.GetSection("BiaNet").Bind(bianetSection);
            return bianetSection.DatabaseConfigurations?.FirstOrDefault(x => x.Key == key)?.SQLDataBroker;
        }

        /// <summary>
        /// Gets the entity model state validation mode.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns>The entity model state valide mode as <see cref="bool"/>.</returns>
        public static bool GetEntityModelStateValidation(this IConfiguration configuration)
        {
            var value = configuration["EntityModelStateValidation"];
            return !string.IsNullOrEmpty(value) && bool.Parse(configuration["EntityModelStateValidation"]);
        }
    }
}
