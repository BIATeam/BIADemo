// BIADemo only
// <copyright file="AircraftMaintenanceCompanyModelBuilder.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.ModelBuilders
{
    using Microsoft.EntityFrameworkCore;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Domain.Maintenance.Entities;

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
            CreateCountryModel(modelBuilder);
            CreateMaintenanceContractModel(modelBuilder);
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

            modelBuilder.Entity<MaintenanceTeam>().Property(p => p.Code).HasMaxLength(64);
            modelBuilder.Entity<MaintenanceTeam>().Property(p => p.TotalOperationCost).HasColumnType("Money");
            modelBuilder.Entity<MaintenanceTeam>().Property(p => p.AverageOperationCost).HasColumnType("Money");
            modelBuilder.Entity<MaintenanceTeam>()
                .HasOne(x => x.CurrentAirport)
                .WithMany()
                .HasForeignKey(x => x.CurrentAirportId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<MaintenanceTeam>()
                .HasOne(x => x.CurrentCountry)
                .WithMany()
                .HasForeignKey(x => x.CurrentCountryId);
            modelBuilder.Entity<MaintenanceTeam>()
                .HasMany(p => p.OperationAirports)
                .WithMany()
                .UsingEntity<MaintenanceTeamAirport>();
            modelBuilder.Entity<MaintenanceTeam>()
               .HasMany(p => p.OperationCountries)
               .WithMany()
               .UsingEntity<MaintenanceTeamCountry>();
        }

        /// <summary>
        /// Create the model for countries.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private static void CreateCountryModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>().ToTable("Countries");
            modelBuilder.Entity<Country>().Property(p => p.Name).IsRequired().HasMaxLength(64);
            modelBuilder.Entity<Country>().HasData(new Country { Id = (int)CountryId.France, Name = "France" });
            modelBuilder.Entity<Country>().HasData(new Country { Id = (int)CountryId.Mexico, Name = "Mexico" });
            modelBuilder.Entity<Country>().HasData(new Country { Id = (int)CountryId.China, Name = "China" });
            modelBuilder.Entity<Country>().HasData(new Country { Id = (int)CountryId.Spain, Name = "Spain" });
        }

        /// <summary>
        /// Create the model for countries.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private static void CreateMaintenanceContractModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MaintenanceContract>()
                .HasMany(p => p.Planes)
                .WithMany()
                .UsingEntity<MaintenanceContractPlane>();
            modelBuilder.Entity<MaintenanceContract>().Property(p => p.ContractNumber).IsRequired().HasMaxLength(64)/*.UseCollation("SQL_Latin1_General_CP1_CS_AS")*/;
            modelBuilder.Entity<MaintenanceContract>()
                .HasOne(x => x.Site)
                .WithMany(x => x.MaintenanceContracts)
                .HasForeignKey(x => x.SiteId)
                .OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<MaintenanceContract>()
                .HasOne(x => x.AircraftMaintenanceCompany)
                .WithMany(x => x.MaintenanceContracts)
                .HasForeignKey(x => x.AircraftMaintenanceCompanyId)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}