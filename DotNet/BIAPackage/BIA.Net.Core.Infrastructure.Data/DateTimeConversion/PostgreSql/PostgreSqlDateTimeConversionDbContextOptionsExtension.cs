// <copyright file="PostgreSqlDateTimeConversionDbContextOptionsExtension.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.DateTimeConversion.PostgreSql
{
    using Microsoft.EntityFrameworkCore.Query;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// PostgreSQL-specific DateTime conversion EF Core options extension.
    /// </summary>
    public sealed class PostgreSqlDateTimeConversionDbContextOptionsExtension : DateTimeConversionDbContextOptionsExtensionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PostgreSqlDateTimeConversionDbContextOptionsExtension"/> class.
        /// </summary>
        /// <param name="serviceLifetime">The service lifetime for the registered services.</param>
        public PostgreSqlDateTimeConversionDbContextOptionsExtension(ServiceLifetime serviceLifetime)
            : base(serviceLifetime)
        {
        }

        /// <inheritdoc/>
        public override void ApplyServices(IServiceCollection services)
        {
            if (this.ServiceLifetime == ServiceLifetime.Scoped)
            {
                services.AddScoped<IMethodCallTranslatorPlugin, PostgreSqlDateTimeConversionTranslator>();
                services.AddScoped<IQuerySqlGeneratorFactory, PostgreSqlDateTimeConversionQuerySqlGeneratorFactory>();
                services.AddScoped<IRelationalParameterBasedSqlProcessorFactory, PostgreSqlDateTimeConversionParameterBasedSqlProcessorFactory>();
            }
            else if (this.ServiceLifetime == ServiceLifetime.Transient)
            {
                services.AddTransient<IMethodCallTranslatorPlugin, PostgreSqlDateTimeConversionTranslator>();
                services.AddTransient<IQuerySqlGeneratorFactory, PostgreSqlDateTimeConversionQuerySqlGeneratorFactory>();
                services.AddTransient<IRelationalParameterBasedSqlProcessorFactory, PostgreSqlDateTimeConversionParameterBasedSqlProcessorFactory>();
            }
        }
    }
}
