// BIADemo only
// <copyright file="PlaneModelBuilder.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.ModelBuilders
{
    using Microsoft.EntityFrameworkCore;
    using TheBIADevCompany.BIADemo.Domain.PlaneModule.Aggregate;

    /// <summary>
    /// Class used to update the model builder for plane domain.
    /// </summary>
    public static class PlaneModelBuilder
    {
        /// <summary>
        /// Create the model for projects.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        public static void CreateModel(ModelBuilder modelBuilder)
        {
            CreatePlaneModel(modelBuilder);
            CreatePlaneTypeModel(modelBuilder);
            CreateAirportModel(modelBuilder);
            CreateEngineModel(modelBuilder);
        }

        /// <summary>
        /// Create the model for planes.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private static void CreatePlaneModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Plane>().HasKey(p => p.Id);
            modelBuilder.Entity<Plane>().Property(p => p.SiteId).IsRequired(); // relationship 1-*
            modelBuilder.Entity<Plane>().Property(p => p.PlaneTypeId).IsRequired(false); // relationship 0..1-*
            modelBuilder.Entity<Plane>().Property(p => p.CurrentAirportId).IsRequired(false); // relationship 0..1-*
            modelBuilder.Entity<Plane>().Property(p => p.Msn).IsRequired().HasMaxLength(64);
            modelBuilder.Entity<Plane>().Property(p => p.Manufacturer).IsRequired(false).HasMaxLength(64);
            modelBuilder.Entity<Plane>().Property(p => p.IsActive).IsRequired();
            modelBuilder.Entity<Plane>().Property(p => p.IsMaintenance).IsRequired(false);
            modelBuilder.Entity<Plane>().Property(p => p.FirstFlightDate).IsRequired();
            modelBuilder.Entity<Plane>().Property(p => p.LastFlightDate).IsRequired(false);
            modelBuilder.Entity<Plane>().Property(p => p.DeliveryDate).IsRequired(false);
            modelBuilder.Entity<Plane>().Property(p => p.NextMaintenanceDate).IsRequired();
            modelBuilder.Entity<Plane>().Property(p => p.SyncTime).IsRequired(false);
            modelBuilder.Entity<Plane>().Property(p => p.SyncFlightDataTime).IsRequired();
            modelBuilder.Entity<Plane>().Property(p => p.Capacity).IsRequired();
            modelBuilder.Entity<Plane>().Property(p => p.MotorsCount).IsRequired(false);
            modelBuilder.Entity<Plane>().Property(p => p.TotalFlightHours).IsRequired();
            modelBuilder.Entity<Plane>().Property(p => p.Probability).IsRequired(false);
            modelBuilder.Entity<Plane>().Property(p => p.FuelCapacity).IsRequired();
            modelBuilder.Entity<Plane>().Property(p => p.FuelLevel).IsRequired(false);
            modelBuilder.Entity<Plane>().Property(p => p.OriginalPrice).IsRequired().HasColumnType("Money");
            modelBuilder.Entity<Plane>().Property(p => p.EstimatedPrice).IsRequired(false).HasColumnType("Money");
            modelBuilder.Entity<Plane>().HasOne(x => x.CurrentAirport).WithMany().HasForeignKey(x => x.CurrentAirportId);
            modelBuilder.Entity<Plane>().HasOne(x => x.PlaneType).WithMany().HasForeignKey(x => x.PlaneTypeId);
            modelBuilder.Entity<Plane>()
                .HasMany(p => p.ConnectingAirports)
                .WithMany(a => a.ClientPlanes)
                .UsingEntity<PlaneAirport>();
            modelBuilder.Entity<Plane>()
               .HasMany(p => p.SimilarTypes)
               .WithMany(a => a.ClientPlanes)
               .UsingEntity<PlanePlaneType>();
        }

        /// <summary>
        /// Create the model for planes.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private static void CreatePlaneTypeModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlaneType>().HasKey(p => p.Id);
            modelBuilder.Entity<PlaneType>().Property(p => p.Title).IsRequired().HasMaxLength(64);
            modelBuilder.Entity<PlaneType>().Property(p => p.CertificationDate).IsRequired(false);
        }

        /// <summary>
        /// Create the model for aiports.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private static void CreateAirportModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Airport>().HasKey(p => p.Id);
            modelBuilder.Entity<Airport>().Property(p => p.Name).IsRequired().HasMaxLength(64);
            modelBuilder.Entity<Airport>().Property(p => p.City).IsRequired().HasMaxLength(64);
        }

        /// <summary>
        /// Create the model for engines.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private static void CreateEngineModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Engine>().HasKey(p => p.Id);
            modelBuilder.Entity<Engine>().Property(p => p.PlaneId).IsRequired(); // relationship 1-*
            modelBuilder.Entity<Engine>().Property(p => p.Reference).HasMaxLength(64);
            modelBuilder.Entity<Engine>().Property(p => p.LastMaintenanceDate).IsRequired();
            modelBuilder.Entity<Engine>().Property(p => p.SyncTime).IsRequired();
            modelBuilder.Entity<Engine>().Property(p => p.Power).IsRequired(false);
        }
    }
}