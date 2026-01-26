// <copyright file="DateTimeConversionDbContextOptionsExtension.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.QueryExpression
{
    using System;
    using System.Collections.Generic;
    using BIA.Net.Core.Common.Enum;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.EntityFrameworkCore.Query;
    using Microsoft.EntityFrameworkCore.Storage;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// EF Core options extension for DateTime conversion support.
    /// </summary>
    public class DateTimeConversionDbContextOptionsExtension : IDbContextOptionsExtension
    {
        private DbContextOptionsExtensionInfo info;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeConversionDbContextOptionsExtension"/> class.
        /// </summary>
        /// <param name="dbProvider">The database provider.</param>
        public DateTimeConversionDbContextOptionsExtension(DbProvider dbProvider)
        {
            this.DbProvider = dbProvider;
        }

        /// <summary>
        /// Gets the database provider.
        /// </summary>
        public DbProvider DbProvider { get; }

        /// <inheritdoc/>
        public DbContextOptionsExtensionInfo Info => this.info ??= new ExtensionInfo(this);

        /// <inheritdoc/>
        public void ApplyServices(IServiceCollection services)
        {
            // Use factory lambda to defer dependency resolution until EF Core's service provider is built
            // The lambda captures this.DbProvider in its closure
            services.AddScoped<IMethodCallTranslatorPlugin>(serviceProvider =>
            {
                return this.DbProvider == DbProvider.SqlServer
                    ? new SqlServerDateTimeConversionTranslator(serviceProvider)
                    : new PostgreSqlDateTimeConversionTranslator(serviceProvider);
            });
        }

        /// <inheritdoc/>
        public void Validate(IDbContextOptions options)
        {
            // No validation needed
        }

        private sealed class ExtensionInfo : DbContextOptionsExtensionInfo
        {
            public ExtensionInfo(IDbContextOptionsExtension extension)
                : base(extension)
            {
            }

            public override bool IsDatabaseProvider => false;

            public override string LogFragment => "using DateTimeConversion";

            public override int GetServiceProviderHashCode() => this.Extension.GetType().GetHashCode();

            public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other)
                => other is ExtensionInfo;

            public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
            {
                debugInfo["DateTimeConversion"] = "1";
            }
        }
    }
}
