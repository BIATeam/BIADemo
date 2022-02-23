// BIADemo only
// <copyright file="AircraftMaintenanceCompanyModelBuilder.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.ModelBuilders
{
    using Microsoft.EntityFrameworkCore;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
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
        }

        /// <summary>
        /// Create the model for aircraft maintenance companies.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private static void CreateAircraftMaintenanceCompanyModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AircraftMaintenanceCompany>().ToTable("AircraftMaintenanceCompanies");
            modelBuilder.Entity<AircraftMaintenanceCompany>().Property(s => s.TeamTypeId).HasDefaultValue(TeamTypeId.AircraftMaintenanceCompany);
            modelBuilder.Entity<AircraftMaintenanceCompany>().Property(p => p.Name).IsRequired().HasMaxLength(64);
        }
    }
}