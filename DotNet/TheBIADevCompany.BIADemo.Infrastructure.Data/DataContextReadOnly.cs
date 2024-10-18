// <copyright file="DataContextReadOnly.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Data
{
    using BIA.Net.Core.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The database context.
    /// </summary>
    public class DataContextReadOnly : DataContext, IQueryableUnitOfWorkReadOnly
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataContextReadOnly"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="configuration">The configuration.</param>
        public DataContextReadOnly(DbContextOptions<DataContext> options, ILogger<DataContextReadOnly> logger, IConfiguration configuration)
            : base(options, logger, configuration)
        {
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            this.ChangeTracker.LazyLoadingEnabled = false;
            this.ChangeTracker.AutoDetectChangesEnabled = false;
        }
    }
}