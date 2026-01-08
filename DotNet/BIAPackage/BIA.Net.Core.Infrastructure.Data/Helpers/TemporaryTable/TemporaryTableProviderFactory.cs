// <copyright file="TemporaryTableProviderFactory.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.Helpers.TemporaryTable
{
    using System;
    using System.Collections.Concurrent;
    using BIA.Net.Core.Common.Enum;

    /// <summary>
    /// Factory for creating and caching database provider-specific temporary table providers.
    /// This factory is a singleton that creates providers once and reuses them.
    /// </summary>
    internal static class TemporaryTableProviderFactory
    {
        private static readonly ConcurrentDictionary<DbProvider, TemporaryTableProvider> Providers = new();

        /// <summary>
        /// Gets the appropriate temporary table provider for the given database provider.
        /// Providers are cached and reused for all subsequent calls.
        /// </summary>
        /// <param name="dbProvider">The database provider.</param>
        /// <returns>An instance of TemporaryTableProvider suitable for the database provider.</returns>
        /// <exception cref="NotSupportedException">If the database provider is not supported.</exception>
        public static TemporaryTableProvider Get(DbProvider dbProvider)
        {
            return Providers.GetOrAdd(dbProvider, provider =>
            {
                return provider switch
                {
                    DbProvider.SqlServer => new SqlServerTemporaryTableProvider(),
                    DbProvider.PostGreSql => new PostgreSqlTemporaryTableProvider(),
                    _ => throw new NotSupportedException($"Database provider {provider} is not supported"),
                };
            });
        }
    }
}
