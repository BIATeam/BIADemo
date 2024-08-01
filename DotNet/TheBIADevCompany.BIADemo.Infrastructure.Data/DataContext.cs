// <copyright file="DataContext.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Data
{
    using Audit.EntityFramework;
    using BIA.Net.Core.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    // Begin BIADemo
    using TheBIADevCompany.BIADemo.Domain.AircraftMaintenanceCompanyModule.Aggregate;

    // End BIADemo
    using TheBIADevCompany.BIADemo.Domain.Audit.Aggregate;
    using TheBIADevCompany.BIADemo.Domain.NotificationModule.Aggregate;

    // Begin BIADemo
    using TheBIADevCompany.BIADemo.Domain.PlaneModule.Aggregate;

    // End BIADemo
    using TheBIADevCompany.BIADemo.Domain.SiteModule.Aggregate;
    using TheBIADevCompany.BIADemo.Domain.TranslationModule.Aggregate;
    using TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate;
    using TheBIADevCompany.BIADemo.Domain.ViewModule.Aggregate;
    using TheBIADevCompany.BIADemo.Infrastructure.Data.ModelBuilders;

    // BIAToolKit - Begin ModelFirst

    /// <summary>
    /// The database context.
    /// </summary>
    [AuditDbContext(Mode = AuditOptionMode.OptIn, IncludeEntityObjects = false, AuditEventType = "{database}_{context}")]

    // BIAToolKit - End ModelFirst
    public class DataContext : BiaDataContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="logger">The logger.</param>
        public DataContext(DbContextOptions<DataContext> options, ILogger<DataContext> logger)
            : base(options, logger)
        {
        }

        // BIAToolKit - Begin ModelFirst

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

        // BIAToolKit - End ModelFirst

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

        // End BIADemo

        /// <inheritdoc cref="DbContext.OnModelCreating"/>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.HasDefaultSchema("dbo")
            base.OnModelCreating(modelBuilder);

            // BIAToolKit - Begin ModelFirst
            TranslationModelBuilder.CreateModel(modelBuilder);
            SiteModelBuilder.CreateSiteModel(modelBuilder);
            UserModelBuilder.CreateModel(modelBuilder);
            ViewModelBuilder.CreateModel(modelBuilder);
            NotificationModelBuilder.CreateModel(modelBuilder);
            AuditModelBuilder.CreateModel(modelBuilder);

            // BIAToolKit - End ModelFirst

            // Begin BIADemo
            PlaneModelBuilder.CreateModel(modelBuilder);
            AircraftMaintenanceCompanyModelBuilder.CreateModel(modelBuilder);

            // End BIADemo
            this.OnEndModelCreating(modelBuilder);
        }
    }
}