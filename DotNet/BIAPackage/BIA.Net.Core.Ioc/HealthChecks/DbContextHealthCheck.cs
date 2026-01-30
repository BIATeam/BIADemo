// <copyright file="DbContextHealthCheck.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Ioc.HealthChecks
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Diagnostics.HealthChecks;

    /// <summary>
    /// DbContext Health Check.
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    public class DbContextHealthCheck<TContext> : IHealthCheck
        where TContext : DbContext
    {
        private readonly IServiceScopeFactory serviceScopeFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbContextHealthCheck{TContext}"/> class.
        /// </summary>
        /// <param name="serviceScopeFactory">The service scope factory.</param>
        public DbContextHealthCheck(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        /// <inheritdoc />
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            using IServiceScope serviceScope = this.serviceScopeFactory.CreateScope();
            TContext db = serviceScope.ServiceProvider.GetRequiredService<TContext>();
            try
            {
                bool canConnect = await db.Database.CanConnectAsync(cancellationToken);
                return canConnect ? HealthCheckResult.Healthy() : HealthCheckResult.Unhealthy("Database unreachable");
            }
            catch
            {
                return HealthCheckResult.Unhealthy("Database check failed");
            }
        }
    }
}
