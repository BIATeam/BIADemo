// <copyright file="DataContextPostGre.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Data
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// DataContex tPostGre.
    /// </summary>
    public class DataContextPostGre : DataContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataContextPostGre"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="configuration">The configuration.</param>
        public DataContextPostGre(DbContextOptions<DataContext> options, ILogger<DataContextPostGre> logger, IConfiguration configuration)
            : base(options, logger, configuration)
        {
        }

        /// <inheritdoc/>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql();
    }
}
