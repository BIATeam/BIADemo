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
            CreatePartModel(modelBuilder);
        }

        /// <summary>
        /// Create the model for planes.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private static void CreatePlaneModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Plane>().Property(p => p.Msn).IsRequired().HasMaxLength(64);
            modelBuilder.Entity<Plane>().Property(p => p.Manufacturer).HasMaxLength(64);
            modelBuilder.Entity<Plane>().Property(p => p.OriginalPrice).HasColumnType("Money");
            modelBuilder.Entity<Plane>().Property(p => p.EstimatedPrice).HasColumnType("Money");
            modelBuilder.Entity<Plane>()
                .HasOne(x => x.CurrentAirport)
                .WithMany()
                .HasForeignKey(x => x.CurrentAirportId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Plane>()
                .HasOne(x => x.PlaneType)
                .WithMany()
                .HasForeignKey(x => x.PlaneTypeId);
            modelBuilder.Entity<Plane>()
                .HasMany(p => p.ConnectingAirports)
                .WithMany()
                .UsingEntity<PlaneAirport>();
            modelBuilder.Entity<Plane>()
               .HasMany(p => p.SimilarTypes)
               .WithMany()
               .UsingEntity<PlanePlaneType>();
            modelBuilder.Entity<Plane>()
               .HasMany(p => p.Engines)
               .WithOne()
               .HasForeignKey(engine => engine.PlaneId);
        }

        /// <summary>
        /// Create the model for planes.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private static void CreatePlaneTypeModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlaneType>().Property(p => p.Title).IsRequired().HasMaxLength(64);
        }

        /// <summary>
        /// Create the model for aiports.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private static void CreateAirportModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Airport>().Property(p => p.Name).IsRequired().HasMaxLength(64);
            modelBuilder.Entity<Airport>().Property(p => p.City).IsRequired().HasMaxLength(64);
        }

        /// <summary>
        /// Create the model for engines.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private static void CreateEngineModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Engine>().Property(p => p.Reference).HasMaxLength(64);
            modelBuilder.Entity<Engine>()
                .HasOne(x => x.Plane)
                .WithMany(x => x.Engines)
                .OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<Engine>().HasOne(x => x.PrincipalPart).WithMany().HasForeignKey(x => x.PrincipalPartId);
            modelBuilder.Entity<Engine>()
               .HasMany(e => e.InstalledParts)
               .WithMany()
               .UsingEntity<EnginePart>();
        }

        /// <summary>
        /// Create the model for parts.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private static void CreatePartModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Part>().ToTable("Parts");
            modelBuilder.Entity<Part>().HasData(
                new Part { Id = 1, SN = "P0001", Family = "N.A", Price = 499.99M },
                new Part { Id = 2, SN = "P0002", Family = "N.A", Price = 250.99M },
                new Part { Id = 3, SN = "P0003", Family = "N.A", Price = 100.99M },
                new Part { Id = 4, SN = "P0004", Family = "N.A", Price = 25.99M });
        }
    }
}