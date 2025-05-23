// <copyright file="AuditModelBuilder.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.ModelBuilders.Bia
{
    using Microsoft.EntityFrameworkCore;
    using TheBIADevCompany.BIADemo.Domain.Bia.Audit.Entities;
    using TheBIADevCompany.BIADemo.Domain.Bia.User.Entities;

    // Begin BIADemo
    using TheBIADevCompany.BIADemo.Domain.Fleet.Entities;

    // End BIADemo

    /// <summary>
    /// Class used to update the model builder for user domain.
    /// </summary>
    public static class AuditModelBuilder
    {
        /// <summary>
        /// Create the user model.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        public static void CreateModel(ModelBuilder modelBuilder)
        {
            CreateUserAuditModel(modelBuilder);

            // Begin BIADemo
            CreateAirportAuditModel(modelBuilder);

            // End BIADemo
            CreateAuditModel(modelBuilder);
        }

        /// <summary>
        /// Create the model for users.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private static void CreateAuditModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuditLog>().HasKey(u => new { u.Id });
            modelBuilder.Entity<AuditLog>().Property(u => u.Table).IsRequired().HasMaxLength(50);
        }

        /// <summary>
        /// Create the model for users.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private static void CreateUserAuditModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserAudit>().HasKey(u => new { u.AuditId });
            modelBuilder.Entity<UserAudit>().Property(u => u.FirstName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<UserAudit>().Property(u => u.LastName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<UserAudit>().Property(u => u.Login).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<UserAudit>().Property(u => u.Domain).IsRequired().HasDefaultValue("--");
        }

        // Begin BIADemo

        /// <summary>
        /// Create the model for aiports.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private static void CreateAirportAuditModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AirportAudit>().HasKey(p => new { p.AuditId });
            modelBuilder.Entity<AirportAudit>().Property(p => p.Name).IsRequired().HasMaxLength(64);
            modelBuilder.Entity<AirportAudit>().Property(p => p.City).IsRequired().HasMaxLength(64);
        }

        // End BIADemo
    }
}