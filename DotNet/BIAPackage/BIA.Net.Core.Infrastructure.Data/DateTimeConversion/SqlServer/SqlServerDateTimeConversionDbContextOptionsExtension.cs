// <copyright file="SqlServerDateTimeConversionDbContextOptionsExtension.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.DateTimeConversion.SqlServer
{
    using Microsoft.EntityFrameworkCore.Query;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// SQL Server-specific DateTime conversion EF Core options extension.
    /// </summary>
    public sealed class SqlServerDateTimeConversionDbContextOptionsExtension : DateTimeConversionDbContextOptionsExtensionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlServerDateTimeConversionDbContextOptionsExtension"/> class.
        /// </summary>
        /// <param name="serviceLifetime">The service lifetime for the registered services.</param>
        public SqlServerDateTimeConversionDbContextOptionsExtension(ServiceLifetime serviceLifetime)
            : base(serviceLifetime)
        {
        }

        /// <inheritdoc/>
        public override void ApplyServices(IServiceCollection services)
        {
            if (this.ServiceLifetime == ServiceLifetime.Scoped)
            {
                services.AddScoped<IMethodCallTranslatorPlugin, SqlServerDateTimeConversionTranslator>();
                services.AddScoped<IQuerySqlGeneratorFactory, SqlServerDateTimeConversionQuerySqlGeneratorFactory>();
                services.AddScoped<IRelationalParameterBasedSqlProcessorFactory, SqlServerDateTimeConversionParameterBasedSqlProcessorFactory>();
            }
            else if (this.ServiceLifetime == ServiceLifetime.Transient)
            {
                services.AddTransient<IMethodCallTranslatorPlugin, SqlServerDateTimeConversionTranslator>();
                services.AddTransient<IQuerySqlGeneratorFactory, SqlServerDateTimeConversionQuerySqlGeneratorFactory>();
                services.AddTransient<IRelationalParameterBasedSqlProcessorFactory, SqlServerDateTimeConversionParameterBasedSqlProcessorFactory>();
            }
        }
    }
}
