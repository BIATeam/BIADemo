// <copyright file="AuditModelBuilder.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.ModelBuilders
{
    using BIA.Net.Core.Infrastructure.Data.ModelBuilders;
    using Microsoft.EntityFrameworkCore;

    // Begin BIADemo
    using TheBIADevCompany.BIADemo.Domain.Fleet.Entities;

    // End BIADemo
    using TheBIADevCompany.BIADemo.Domain.User.Entities;

    /// <summary>
    /// Class used to update the model builder for audits.
    /// </summary>
    public class AuditModelBuilder : BaseAuditModelBuilder
    {
        /// <summary>
        /// Create the audit models.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        public void CreateModel(ModelBuilder modelBuilder)
        {
            this.CreateAuditModel(modelBuilder);
            this.CreateUserAuditModel<UserAudit, User>(modelBuilder);

            // Add here the project specific audit model creation.
            // Begin BIADemo
            modelBuilder.Entity<EngineAudit>().Property(p => p.PlaneId).IsRequired();
            modelBuilder.Entity<EngineAudit>().Property(p => p.Reference).IsRequired();

            modelBuilder.Entity<PlaneAirportAudit>().Property(p => p.AirportId).IsRequired();
            modelBuilder.Entity<PlaneAirportAudit>().Property(p => p.PlaneId).IsRequired();
            modelBuilder.Entity<PlaneAirportAudit>().Property(p => p.AirportName).IsRequired();

            // End BIADemo
        }
    }
}