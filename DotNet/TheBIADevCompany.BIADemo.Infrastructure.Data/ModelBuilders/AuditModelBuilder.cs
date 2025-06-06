// <copyright file="AuditModelBuilder.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.ModelBuilders
{
    using System.Diagnostics;
    using Microsoft.EntityFrameworkCore;

    // Begin BIADemo
    using TheBIADevCompany.BIADemo.Domain.Fleet.Entities;
    using TheBIADevCompany.BIADemo.Infrastructure.Data.ModelBuilders.Bia;

    // End BIADemo

    /// <summary>
    /// Class used to update the model builder for user domain.
    /// </summary>
    public class AuditModelBuilder : BaseAuditModelBuilder
    {
        /// <summary>
        /// Create the user model.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        public override void CreateModel(ModelBuilder modelBuilder)
        {
            Debug.Assert(modelBuilder != null, "Line to avoid warning empty method");
            base.CreateModel(modelBuilder);

            // Add here the project specific audit model creation.
            // Begin BIADemo
            CreateAirportAuditModel(modelBuilder);

            // End BIADem
        }

        // Begin BIADemo

        /// <summary>
        /// Create the model for aiports.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected static void CreateAirportAuditModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AirportAudit>().HasKey(p => new { p.AuditId });
            modelBuilder.Entity<AirportAudit>().Property(p => p.Name).IsRequired().HasMaxLength(64);
            modelBuilder.Entity<AirportAudit>().Property(p => p.City).IsRequired().HasMaxLength(64);
        }

        // End BIADemo
    }
}