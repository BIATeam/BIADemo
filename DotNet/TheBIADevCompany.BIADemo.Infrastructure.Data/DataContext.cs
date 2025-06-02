// <copyright file="DataContext.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Data
{
    using System.Threading.Tasks;
#if BIA_FRONT_FEATURE
    using Audit.EntityFramework;
    using BIA.Net.Core.Domain.Audit;
    using BIA.Net.Core.Domain.Notification.Entities;
    using BIA.Net.Core.Domain.Translation.Entities;
    using BIA.Net.Core.Domain.User.Entities;
    using BIA.Net.Core.Domain.View.Entities;
#endif
    using BIA.Net.Core.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
#if BIA_FRONT_FEATURE

    // Begin BIADemo
    using TheBIADevCompany.BIADemo.Domain.Fleet.Entities;

    // End BIADemo
    // Begin BIADemo
    using TheBIADevCompany.BIADemo.Domain.Maintenance.Entities;

    // End BIADemo
    using TheBIADevCompany.BIADemo.Domain.Site.Entities;
    using TheBIADevCompany.BIADemo.Domain.User.Entities;
    using TheBIADevCompany.BIADemo.Infrastructure.Data.ModelBuilders;
    using TheBIADevCompany.BIADemo.Infrastructure.Data.ModelBuilders.Bia;
#endif

    /// <summary>
    /// The database context.
    /// </summary>
#if BIA_FRONT_FEATURE
    [AuditDbContext(Mode = AuditOptionMode.OptIn, IncludeEntityObjects = false, AuditEventType = "{database}_{context}")]
#endif
    public class DataContext : BiaDataContext
    {
        /// <summary>
        /// The current logger.
        /// </summary>
        private readonly ILogger<BiaDataContext> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="configuration">The configuration.</param>
        public DataContext(DbContextOptions<DataContext> options, ILogger<DataContext> logger, IConfiguration configuration)
            : base(options, logger, configuration)
        {
            this.logger = logger;
            this.logger.LogDebug("----------------Create Context--------------");
        }

#if BIA_FRONT_FEATURE
        /// <summary>
        /// Gets or sets the Plane DBSet.
        /// </summary>
        public DbSet<AuditLog> AuditLogs { get; set; }

        /// <summary>
        /// Gets or sets the Site DBSet.
        /// </summary>
        public DbSet<Site> Sites { get; set; }

        /// <summary>
        /// Gets or sets the User DBSet.
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Gets or sets the User DBSet.
        /// </summary>
        public DbSet<UserAudit> UsersAudit { get; set; }

        /// <summary>
        /// Gets or sets the type of team DBSet.
        /// </summary>
        public DbSet<Team> Teams { get; set; }

        /// <summary>
        /// Gets or sets the type of team DBSet.
        /// </summary>
        public DbSet<TeamType> TeamTypes { get; set; }

        /// <summary>
        /// Gets or sets the Role DBSet.
        /// </summary>
        public DbSet<Role> Roles { get; set; }

        /// <summary>
        /// Gets or sets the Role DBSet.
        /// </summary>
        public DbSet<RoleTranslation> RoleTranslations { get; set; }

        /// <summary>
        /// Gets or sets the Member DBSet.
        /// </summary>
        public DbSet<Member> Members { get; set; }

        /// <summary>
        /// Gets or sets the views.
        /// </summary>
        public DbSet<View> Views { get; set; }

        /// <summary>
        /// Gets or sets the notification DBSet.
        /// </summary>
        public DbSet<Notification> Notifications { get; set; }

        /// <summary>
        /// Gets or sets the notification translation DBSet.
        /// </summary>
        public DbSet<NotificationTranslation> NotificationTranslations { get; set; }

        /// <summary>
        /// Gets or sets the notification type DBSet.
        /// </summary>
        public DbSet<NotificationType> NotificationTypes { get; set; }

        /// <summary>
        /// Gets or sets the notification type DBSet.
        /// </summary>
        public DbSet<NotificationTypeTranslation> NotificationTypeTranslations { get; set; }

        // Begin BIADemo

        /// <summary>
        /// Gets or sets the Aircraft Maintenance Company DBSet.
        /// </summary>
        public DbSet<AircraftMaintenanceCompany> AircraftMaintenanceCompanies { get; set; }

        /// <summary>
        /// Gets or sets the Plane DBSet.
        /// </summary>
        public DbSet<Plane> Planes { get; set; }

        /// <summary>
        /// Gets or sets the Airport DBSet.
        /// </summary>
        public DbSet<Airport> Airports { get; set; }

        /// <summary>
        /// Gets or sets the Airport Audit DBSet.
        /// </summary>
        public DbSet<AirportAudit> AirportsAudit { get; set; }

        /// <summary>
        /// Gets or sets the Plane DBSet.
        /// </summary>
        public DbSet<PlaneType> PlanesTypes { get; set; }

        /// <summary>
        /// Gets or sets the engines.
        /// </summary>
        public DbSet<Engine> Engines { get; set; }

        /// <summary>
        /// Gets or sets the parts.
        /// </summary>
        public DbSet<Part> Parts { get; set; }

        /// <summary>
        /// Gets or sets the user team defaults.
        /// </summary>
        public DbSet<UserDefaultTeam> UserDefaultTeams { get; set; }

        // End BIADemo
#endif

        /// <summary>
        /// Releases the allocated resources for this context.
        /// </summary>
        /// <returns>Task.</returns>
        public override ValueTask DisposeAsync()
        {
            this.logger.LogDebug("----------------Dispose Context--------------");
            return base.DisposeAsync();
        }

        /// <inheritdoc cref="DbContext.OnModelCreating"/>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.HasDefaultSchema("dbo")
            base.OnModelCreating(modelBuilder);
#if BIA_FRONT_FEATURE

            TranslationModelBuilder.CreateModel(modelBuilder);
            SiteModelBuilder.CreateSiteModel(modelBuilder);
            new UserModelBuilder().CreateModel(modelBuilder);
            ViewModelBuilder.CreateModel(modelBuilder);
            NotificationModelBuilder.CreateModel(modelBuilder);
            AuditModelBuilder.CreateModel(modelBuilder);

            // Begin BIADemo
            PlaneModelBuilder.CreateModel(modelBuilder);
            AircraftMaintenanceCompanyModelBuilder.CreateModel(modelBuilder);

            // End BIADemo
#endif
            this.OnEndModelCreating(modelBuilder);
        }
    }
}