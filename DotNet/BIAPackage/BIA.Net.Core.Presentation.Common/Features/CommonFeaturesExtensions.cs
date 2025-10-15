// <copyright file="CommonFeaturesExtensions.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>
namespace BIA.Net.Core.Presentation.Common.Features
{
    using System.Diagnostics.CodeAnalysis;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Common.Configuration.CommonFeature;
    using BIA.Net.Core.Common.Enum;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Add the standard service.
    /// </summary>
    public static class CommonFeaturesExtensions
    {
        /// <summary>
        /// Start the Worker service features.
        /// </summary>
        /// <param name="services">the service collection.</param>
        /// <param name="commonFeatures">the common Features.</param>
        /// <param name="configuration">the application configuration.</param>
        /// <returns>the services collection.</returns>
        public static IServiceCollection AddBiaCommonFeatures(
            [NotNull] this IServiceCollection services,
            CommonFeatures commonFeatures,
            IConfiguration configuration)
        {
            // Distributed Cache
            if (commonFeatures?.DistributedCache?.IsActive == true)
            {
                DbProvider provider = configuration.GetProvider(commonFeatures.DistributedCache.ConnectionStringName);
                if (provider == DbProvider.SqlServer)
                {
                    services.AddDistributedSqlServerCache(config =>
                    {
                        config.ConnectionString = configuration.GetDatabaseConnectionString(commonFeatures.DistributedCache.ConnectionStringName);
                        config.TableName = "DistCache";
                        config.SchemaName = "dbo";
                    });
                }
                else if (provider == DbProvider.PostGreSql)
                {
                    services.AddDistributedMemoryCache();
                }
            }

            services.AddMemoryCache();

            return services;
        }
    }
}
