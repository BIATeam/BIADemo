// <copyright file="DataContextPostGreSql.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Data
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// DataContextPostGre.
    /// </summary>
    public class DataContextPostGreSql : DataContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataContextPostGreSql"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="configuration">The configuration.</param>
        public DataContextPostGreSql(DbContextOptions<DataContext> options, ILogger<DataContextPostGreSql> logger, IConfiguration configuration)
            : base(options, logger, configuration)
        {
        }

        /// <inheritdoc/>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql();
    }
}
