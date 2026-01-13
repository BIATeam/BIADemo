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
#if BIA_FRONT_FEATURE
    using TheBIADevCompany.BIADemo.Domain.User.Entities;
#endif

    /// <summary>
    /// Class used to update the model builder for audits.
    /// </summary>
    public class AuditModelBuilder : BaseAuditModelBuilder
    {
        /// <inheritdoc/>
        public override void CreateModel(ModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);
#if BIA_FRONT_FEATURE
            this.CreateUserAuditModel<UserAudit, User>(modelBuilder);
#endif

            // Add here the project specific audit model creation.
            // Begin BIADemo
            CreateEngineAuditModel(modelBuilder);
            CreatePlaneAirportAuditModel(modelBuilder);

            // End BIADemo
        }

        // Begin BIADemo

        /// <summary>
        /// Create the model for <see cref="EngineAudit"/>.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private static void CreateEngineAuditModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EngineAudit>().Property(p => p.PlaneId).IsRequired();
            modelBuilder.Entity<EngineAudit>().Property(p => p.Reference).IsRequired();
        }

        /// <summary>
        /// Create the model for <see cref="PlaneAirportAudit"/>.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private static void CreatePlaneAirportAuditModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlaneAirportAudit>().Property(p => p.AirportId).IsRequired();
            modelBuilder.Entity<PlaneAirportAudit>().Property(p => p.PlaneId).IsRequired();
            modelBuilder.Entity<PlaneAirportAudit>().Property(p => p.AirportName).IsRequired();
        }

        // End BIADemo
    }
}