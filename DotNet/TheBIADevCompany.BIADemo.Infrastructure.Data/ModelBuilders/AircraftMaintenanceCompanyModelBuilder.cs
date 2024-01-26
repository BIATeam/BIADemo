// BIADemo only
// <copyright file="AircraftMaintenanceCompanyModelBuilder.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.ModelBuilders
{
    using Microsoft.EntityFrameworkCore;
    using TheBIADevCompany.BIADemo.Domain.AircraftMaintenanceCompanyModule.Aggregate;

    /// <summary>
    /// Class used to update the model builder for aircraft maintenance company domain.
    /// </summary>
    public static class AircraftMaintenanceCompanyModelBuilder
    {
        /// <summary>
        /// Create the model for projects.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        public static void CreateModel(ModelBuilder modelBuilder)
        {
            CreateAircraftMaintenanceCompanyModel(modelBuilder);
            CreateMaintenanceTeamModel(modelBuilder);
        }

        /// <summary>
        /// Create the model for aircraft maintenance companies.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private static void CreateAircraftMaintenanceCompanyModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AircraftMaintenanceCompany>().ToTable("AircraftMaintenanceCompanies");
            modelBuilder.Entity<AircraftMaintenanceCompany>().Property(p => p.Title).IsRequired().HasMaxLength(64);
        }

        /// <summary>
        /// Create the model for aircraft maintenance companies.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private static void CreateMaintenanceTeamModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MaintenanceTeam>().ToTable("MaintenanceTeams");
            modelBuilder.Entity<MaintenanceTeam>().Property(p => p.Title).IsRequired().HasMaxLength(64);
            modelBuilder.Entity<MaintenanceTeam>().Property(p => p.AircraftMaintenanceCompanyId).IsRequired(); // relationship 1-*
            modelBuilder.Entity<MaintenanceTeam>().HasOne(x => x.AircraftMaintenanceCompany).WithMany(x => x.MaintenanceTeams).OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}